namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DumpProcessors
{
  using System.Collections.Generic;
  using System.Linq;

  using Microsoft.Diagnostics.Runtime;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.LogAnalyzer.Settings;
  using Sitecore.MemoryDiagnostics.ClrObjectEnumerators;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.ObjectEnumeration;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.HeapStat;

  /// <summary>
  /// Provides an ordered statistics (<see cref="HeapStatEntry"/>) of objects per type.
  /// <para>Gets objects to enumerate from <see cref="IEnumerateClrObjectsFromClrRuntime"/>.</para>  
  /// </summary>
  public class DumpHeapStatProcessor : DiDumpProcessor
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DumpHeapStatProcessor"/> class.
    /// </summary>
    /// <param name="logAnalyzerFacade">The log Analyzer Facade.</param>
    /// <param name="objectEnumerationConnection">The object Enumeration Facade.</param>    
    public DumpHeapStatProcessor(ILogAnalyzerFacade logAnalyzerFacade, IEnumeratiorConnection objectEnumerationConnection, DumpHeapStat statsCollector) : base(logAnalyzerFacade, objectEnumerationConnection, new EmptyClrObjectTransformator())
    {
    }

    /// <summary>
    /// Builds the parsing result wrapped in try-catch, so must raise exceptions.
    /// </summary>
    /// <param name="clrRuntime">The data source.</param>
    /// <param name="processContext">The process context.</param>
    /// <returns>Constructed <see cref="ParsingResult"/> with entries in All log group.</returns>
    protected override ParsingResult BuildParsingResult(ClrRuntime clrRuntime, LogProcessorSettings processContext)
    {
      var heapStatProcessor = new DumpHeapStat();
      var parsingResult = new ParsingResult();

      var filter = processContext.ReaderSettings;
      var list = new List<LogEntry>(1000 * 10);
      int index = 0;
      list.AddRange(from statEntry in heapStatProcessor.GetObjectTypeStats(clrRuntime, ClrObjectEnumerator)
                    let result = new LogEntry
                    {
                      LogDateTime = statEntry.Datetime,
                      Text = statEntry.ToString(),
                      Level = LogLevel.INFO,
                      LinesCount = 1,
                      Caption = statEntry.Caption,
                      Index = ++index
                    }
                    where filter.Matches(result)
                    select result);

      parsingResult.AddToGroupAll(list);
      return parsingResult;
    }
  }
}

