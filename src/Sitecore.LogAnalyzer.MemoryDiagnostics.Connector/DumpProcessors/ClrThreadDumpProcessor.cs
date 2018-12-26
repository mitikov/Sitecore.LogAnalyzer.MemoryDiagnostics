namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DumpProcessors
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.LogAnalyzer;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.LogAnalyzer.Settings;
  using Sitecore.MemoryDiagnostics.ClrThreadTransformators;
  using Sitecore.MemoryDiagnostics.Interfaces;
  using Sitecore.MemoryDiagnostics.Models.InternalProcessing;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DumpProcessors;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.ObjectEnumeration;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;

  /// <summary>
  /// Enumerates threads from <see cref="ClrRuntime"/>.
  /// </summary>
  /// <seealso cref="DiDumpProcessor" />
  public class ClrThreadDumpProcessor : DiDumpProcessor
  {
    #region Constructors
    public ClrThreadDumpProcessor(ILogAnalyzerFacade logAnalyzerFacade, IClrObjectTransformator clrObjToLogEntryTransformProvider, [NotNull] IFilter<ClrThread> threadFilter)
        : base(logAnalyzerFacade, new MdConnectionNoEnumerator(), clrObjToLogEntryTransformProvider)
    {
      Assert.ArgumentNotNull(threadFilter, nameof(threadFilter));

      this.ThreadFilter = threadFilter;
    }
    #endregion
    #region Properties
    protected IFilter<ClrThread> ThreadFilter { get; private set; }
    #endregion

    protected override ParsingResult BuildParsingResult(ClrRuntime clrRuntime, LogProcessorSettings processContext)
    {
      var parsingResult = new ParsingResult();

      var filter = processContext.ReaderSettings;
      int i = 0;
      var heap = clrRuntime.Heap;
      var correctModels = new List<LogEntry>();

      foreach (var thread in clrRuntime.Threads.Where(ThreadFilter.Matches))
      {
        LogEntry candidate;
        LogEntry[] nested;
        var threadClrObj = new ClrObject(thread.Address, heap);

        using (new LongRunningOperationWatcher($"Thread {thread.OSThreadId} processed", 1000))
        {
          if (ClrObjToLogEntryTransformProvider is IClrThreadTransformator)
          {
            candidate = ((IClrThreadTransformator)ClrObjToLogEntryTransformProvider).BuildCandidate(clrRuntime, thread, out nested);
          }
          else
          {
            candidate = this.BuildCandidate(clrRuntime, threadClrObj, out nested);
          }
        }

        if (candidate == null || (filter != null && !filter.Matches(candidate)))
        {
          continue;
        }

        candidate.Index = ++i;
        correctModels.Add(candidate);
        if ((nested == null) || (nested.Length == 0))
        {
          continue;
        }

        var filtered = this.FilterChildren(nested).ToArray();

        Array.ForEach(filtered, t => t.Index = ++i);
        correctModels.AddRange(filtered);
      }

      parsingResult.AddToGroupAll(correctModels);
      return parsingResult;
    }

    protected virtual IEnumerable<LogEntry> FilterChildren(LogEntry[] nested)
    {
      return from model in nested.OfType<ClrObjLogEntry>()
             where (model.HasMappingModel)
             let isGeneral = model.Model is GeneralMapping
             where !isGeneral
             let clrObject = model.ClrObject
             where clrObject.HasValue             
             select model;
    }
  }
}
