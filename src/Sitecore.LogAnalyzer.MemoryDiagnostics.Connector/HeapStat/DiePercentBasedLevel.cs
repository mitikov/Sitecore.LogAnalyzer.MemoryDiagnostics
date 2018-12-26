namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.HeapStat
{
  using Sitecore.LogAnalyzer.Models;

  /// <summary>
  /// Sets <see cref="LogEntry.Level"/> depending on <see cref="LiveDeadHeapStatEntry.AlivePercent"/>.
  /// <para>Lower percent likely indicates objects allocations can be optimized.</para>  
  /// </summary>
  /// <seealso cref="Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.HeapStat.ISetStatsLevel" />
  public class DiePercentBasedLevel : ISetStatsLevel
  {
    public virtual LogLevel SetLevel(LiveDeadHeapStatEntry entry)
    {
      if (entry.Count < this.MinimumObjectCount)
      {
        return LogLevel.DEBUG;
      }

      var percent = (int)(entry.AlivePercent * 100);

      if (percent <= 20)
      {
        return LogLevel.ERROR;
      }

      if (percent <= 40)
      {
        return LogLevel.WARN;
      }

      return percent >= 60 ? LogLevel.DEBUG : LogLevel.INFO;
    }

    protected virtual int MinimumObjectCount => 500;


  }
}