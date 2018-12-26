using Sitecore.LogAnalyzer;
using Sitecore.LogAnalyzer.Contexts;
using Sitecore.LogAnalyzer.Managers;
using Sitecore.LogAnalyzer.Models;

namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector
{
  /// <summary>
  /// Shrub. TODO: change one day.
  /// </summary>
  public sealed class EmptyContextFactory : IContextFactory
  {
    /// <summary>
    /// Gets the context from <paramref name="parsingResult" /> and <paramref name="captions" />.
    /// </summary>
    /// <param name="parsingResult">The parsing result.</param>
    /// <param name="captions">The captions.</param>
    /// <returns></returns>
    public GeneralContext GetContext(ParsingResult parsingResult, LogGroupsCollection captions)
    {
      var result = new GeneralContext
      {
        AuditContext = new AuditContext { Logs = parsingResult.Audits },
        Captions = captions,
        HealthMonitorContext =
          new HealthMonitorContext(
          new LogGroups(),
          LogAnalyzer.Configuration.DateTimeFormat),
        LogContext = new LogContext(),
        ParsingResult = parsingResult
      };

      return result;
    }
    /// <summary>
    /// Class instance.
    /// </summary>
    public static IContextFactory Instance = new EmptyContextFactory();
  }
}
