namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DumpProcessors
{
  using System.Collections.Generic;
  using System.Linq;
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.LogAnalyzer;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.LogAnalyzer.Settings;
  using Sitecore.MemoryDiagnostics.ClrObjectEnumerators.LiveObjects;
  using Sitecore.MemoryDiagnostics.Facades.ObjectEnumeration;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.ObjectEnumeration;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.HeapStat;

  /// <summary>
  /// An ordered statistics for live-dead ratio per type basic.
  /// </summary>
  /// <seealso cref="DiDumpProcessor" />
  public class LiveDeadHeapStatsProcessor : DiDumpProcessor
  {
    #region Fields
    /// <summary>
    /// Sets the resulting <see cref="LogLevel"/> for <see cref="LogEntry"/>.
    /// </summary>
    public readonly ISetStatsLevel StatsLevelSetter;
    
    protected readonly IGetAliveObjects LiveObjectsEnumerator;
    #endregion
    #region Constructors
    public LiveDeadHeapStatsProcessor(ILogAnalyzerFacade logAnalyzerFacade, IGetAliveObjects liveObjectsEnumerator, ISetStatsLevel statsLevelSetter) 
      : base(logAnalyzerFacade, new MdBasedEnumerationConnection(new AllHeapObjects()), new EmptyClrObjectTransformator())
    {
      this.LiveObjectsEnumerator = liveObjectsEnumerator;
      this.StatsLevelSetter = statsLevelSetter;
    }
    #endregion
    protected override ParsingResult BuildParsingResult(ClrRuntime clrRuntime, LogProcessorSettings processContext)
    {
      Context.Message("Locating of alive objects started (can take a while).");

      var alive = this.LiveObjectsEnumerator.GetAliveObjects(clrRuntime);

      Context.Message("Locating of alive objects ended.");
      
      var parsingResult = new ParsingResult();
      var filter = processContext.ReaderSettings;
      var list = new List<LogEntry>(1000 * 10);
      var index = 0;

      list.AddRange(from statEntry in this.GetObjectTypeStats(clrRuntime, alive)
                    let result = new LogEntry
                    {
                      LogDateTime = statEntry.Datetime,
                      Text = statEntry.ToString(),
                      Level = this.StatsLevelSetter.SetLevel(statEntry),
                      LinesCount = 1,
                      Caption = statEntry.Caption,
                      Index = ++index
                    }
                    where filter.Matches(result)
                    select result);

      parsingResult.AddToGroupAll(list);
      return parsingResult;
    }

    /// <summary>
    ///   Gets the object type stats live-dead distribution.
    /// </summary>
    /// <param name="runtime">The runtime.</param>
    /// <param name="alive">The alive.</param>
    /// <returns>An ordered statistics of live-dead ratio per type basic.</returns>
    protected virtual IOrderedEnumerable<LiveDeadHeapStatEntry> GetObjectTypeStats(ClrRuntime runtime, HashSet<ulong> alive)
    {
      // Avoid further moves in memory
      var stats = new Dictionary<ClrType, LiveDeadHeapStatEntry>(1000 * 1000);

      var notificationLimit = 50 * 1000;

      var processed = 0;

      foreach (var clrObj in this.ClrObjectEnumerator.ExtractFromRuntime(runtime))
      {
        var obj = clrObj.Address;
        var type = clrObj.Type;
        ++processed;
        if (type == null)
        {
          continue;
        }

        if (processed % notificationLimit == 0)
        {
          Context.Message($"{processed} objects processed.");
        }

        var size = type.GetSize(obj);

        // Add an entry to the dictionary, if one doesn't already exist.
        LiveDeadHeapStatEntry entry = null;
        if (!stats.TryGetValue(type, out entry))
        {
          entry = new LiveDeadHeapStatEntry(type.Name);
          stats.Add(type, entry);
        }

        entry.Count++;
        entry.Size += size;

        if (!alive.Contains(obj))
        {
          continue;
        }

        ++entry.LiveCount;
        entry.LiveSize += size;
        alive.Remove(obj);
      }

      return stats.Values.OrderByDescending(t => t.Datetime);
    }
  }
}