namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.HeapStat
{
  using Sitecore.LogAnalyzer.Models;

  /// <summary> 
  /// Holds the logic on how to set <see cref="LogLevel"/> for entry.
  /// </summary>
  public interface ISetStatsLevel
  {
    LogLevel SetLevel(LiveDeadHeapStatEntry entry);
  }
}