namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders
{  
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;
  using System;

  /// <summary>
  /// Does nothing, returns <c>null</c> always.
  /// </summary>
  /// <seealso cref="IClrObjectTransformator" />
  public class EmptyClrObjectTransformator : IClrObjectTransformator
  {
    public LogEntry BuildCandidate(ClrRuntime clrRuntime, ClrObject clrObject, out LogEntry[] nested)
    {
      nested = Array.Empty<LogEntry>();
      return null;
    }
  }
}
