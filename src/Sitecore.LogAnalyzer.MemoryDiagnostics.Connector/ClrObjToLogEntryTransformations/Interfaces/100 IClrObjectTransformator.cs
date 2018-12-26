namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces
{
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.LogAnalyzer.Attributes;
  using Sitecore.LogAnalyzer.Models;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;

  /// <summary>
  /// Transforms <see cref="ClrObject"/> into <see cref="LogEntry"/> with an array of supporting <see cref="LogEntry"/> nested elements. 
  /// </summary>
  public interface IClrObjectTransformator
  {
    /// <summary>
    /// Builds the <see cref="LogEntry"/> candidate from <see cref="ClrObject"/>.
    /// </summary>
    /// <param name="clrRuntime">The runtime</param>
    /// <param name="clrObject">The object to be transformed into SCLA representation</param>
    /// <param name="nested">The array of supporting nested entries for processed candidate..</param>
    /// <returns>A SCLA log entry model to be shown in UI.</returns>
    [CanBeNull]
    LogEntry BuildCandidate([NotNull]ClrRuntime clrRuntime, ClrObject clrObject, out LogEntry[] nested);
  }
}