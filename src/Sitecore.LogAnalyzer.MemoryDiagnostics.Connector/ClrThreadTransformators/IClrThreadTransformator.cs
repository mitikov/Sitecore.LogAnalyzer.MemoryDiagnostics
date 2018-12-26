using Microsoft.Diagnostics.Runtime;
using Sitecore.LogAnalyzer.Models;

namespace Sitecore.MemoryDiagnostics.ClrThreadTransformators
{
  public interface IClrThreadTransformator
  {
    /// <summary>
    /// Builds the <see cref="LogEntry"/> candidate from <see cref="ClrThread"/>.
    /// </summary>
    /// <param name="clrRuntime">The runtime</param>
    /// <param name="clrObject">The object to be transformed into SCLA representation</param>
    /// <param name="nested">The array of supporting nested entries for processed candidate..</param>
    /// <returns>A SCLA log entry model to be shown in UI.</returns>
    [CanBeNull]
    LogEntry BuildCandidate([NotNull]ClrRuntime clrRuntime, ClrThread clrThread, out LogEntry[] nested);
  }
}
