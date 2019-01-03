using System;
using System.Linq;
using Sitecore.LogAnalyzer.Attributes;
using Sitecore.MemoryDiagnostics;
using Sitecore.MemoryDiagnostics.Models.FallBack.AspNetRelated;
using Sitecore.MemoryDiagnostics.Models.InternalProcessing;
using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector;

namespace Sitecore.LogAnalyzer.MemoryDiagnostics.RunningRequests
{
  /// <summary>
  /// Appends additional details from <see cref="HttpContextMappingModel"/> to <see cref="ClrObjLogEntry"/>.
  /// <para>Adds requested url, duration, cookies (analytics, asp.net).</para>
  /// </summary>
  public class AddRequestInfoToParentText : IInitLogEntryFields
  {
    public virtual void ApplyCustomLogicOnLogEntry([NotNull] ClrObjLogEntry entry)
    {
      if (!(entry.Model is HttpContextMappingModel mapping))
      {
        return;
      }

      var requestInfo = from context in new[] { mapping }
                        where context.HasURL
                        let request = context._request as HttpRequestMappingModel
                        let cookies = request?._cookies?.Value as HashtableMappingModel
                        where cookies != null

                        let analyticsCookie = cookies?[TextConstants.CookieNames.SitecoreAnalyticsGlobal] as HttpCookieModel

                        let sessionCookie = cookies?[TextConstants.CookieNames.AspNetSession] as HttpCookieModel

                        let analyticsId = analyticsCookie?.Value ?? "[No_Analytics_Cookie]"
                        let aspSession = sessionCookie?.Value ?? "[No_Session_Cookie]"

                        let metadata = new
                        {
                          context.URL,
                          TotalSeconds = Math.Round(context.ExecutionDuration.TotalSeconds, digits: 2),
                          ExecutionStarted = context.ContextCreationTime,
                          analyticsId = analyticsId,
                          aspSession = aspSession,
                          context = context.HexAddress,
                          request = request.HexAddress,
                        }
                        select metadata;

      var groupedRequestInfo = requestInfo.FirstOrDefault();

      string message = string.Empty;
      if (groupedRequestInfo != null)
      {
        message = string.Join(
          Environment.NewLine,
          $"URL: {groupedRequestInfo.URL}",
          $"Executed for: {groupedRequestInfo.TotalSeconds} seconds",
          $"Asp.Net session: {groupedRequestInfo.aspSession}",
          $"Analytics: {groupedRequestInfo.analyticsId}",
          $"Started: {groupedRequestInfo.ExecutionStarted}",
          $"HttpRequest: {groupedRequestInfo.request}",
          Environment.NewLine);
      }

      if (string.IsNullOrEmpty(message))
      {
        return;
      }

      if (entry.Parent != null && !entry.Parent.Text.StartsWith(message))
      {
        var existingText = entry.Parent.Text;

        entry.Parent.Text = string.Join(Environment.NewLine, message, entry.Parent.Text);
      }
    }
  }
}
