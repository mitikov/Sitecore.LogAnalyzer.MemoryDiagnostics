namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DumpProcessors
{
  using System;
  using System.Collections.Generic;
  using System.Runtime;
  using System.Runtime.CompilerServices;
  using Sitecore.MemoryDiagnostics.Attributes;
  using Sitecore.MemoryDiagnostics.SourceFactories;
  using Microsoft.Diagnostics.Runtime;
  
  using Sitecore.LogAnalyzer.Attributes;
  using Sitecore.LogAnalyzer.Managers;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.LogAnalyzer.Parsing;
  using Sitecore.LogAnalyzer.Settings;
  using Sitecore.MemoryDiagnostics.ClrObjectsProviders.Base;
  using Sitecore.MemoryDiagnostics.Facades.ObjectEnumeration;
  using Sitecore.MemoryDiagnostics.ModelFilters;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ConnectionDetails;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.ObjectEnumeration;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;
  using Sitecore.LogAnalyzer.Contexts;

  /// <summary>  
  ///   <see cref="ILogProcessor" /> code flow:
  ///   <para><see cref="BaseDumpProcessor.Initialize"/> method called to perform initialization.</para>
  ///   <para>
  ///     1. Use <see cref="IClrRuntimeFactory" /> to get <see cref="ClrRuntime"/>.
  ///   </para>
  ///   <para>
  ///    1.1. <see cref="BaseDumpProcessor.OnPreBuildParsingResult"/> placeholder for doing custom initialization with configured <see cref="ClrRuntime"/>.
  ///  </para>
  ///   <para>
  ///     2. Use <see cref="IObjectEnumerationFacade" /> to enumerate objects <see cref="ClrObject"/>.
  ///     suitable for parsing.
  ///   </para>
  ///   <para>
  ///     <p>
  ///       2.1. Can specify <see cref="IFilteredObjectsProvider" /> to filter <see cref="ClrObject" /> returned.
  ///     </p>
  ///   </para>
  ///   <para>
  ///     3. Call <see cref="BuildCandidate"/> (<see cref="IClrObjectTransformator"/>, to transform <see cref="ClrObject" /> into <see cref="LogEntry" /> with related objects.
  ///   </para>
  ///   <para>
  /// 3.1. <see cref="IClrObjectTransformator" /> can use <see cref="IInitLogEntryFields"/> to initiate model fields in a given way.
  ///   </para>
  /// <para>
  ///     3.2. <see cref="IClrObjectTransformator" /> can use <see cref="IModelMappingFilter"/> to filter some model mappings.
  /// </para>
  ///   <para>
  ///     2.3. Applies <see cref="LogProcessorSettings.ReaderSettings"/> filter on resulting <see cref="LogEntry" />
  ///   </para>
  ///   <para>2.4. <see cref="LogEntry" /> is added to <see cref="ParsingResult.All" /></para>
  ///   <para>2.5. Sorting, reorganizing, and indexing is performed</para>
  ///   <para>3. <see cref="LogGroupsCollection" /> is build using <see cref="BuildCaptions" /> method.</para>
  ///   <para>
  ///     4. <see cref="GeneralContext" /> is build by call to <see cref="IContextFactory"/>.
  ///   </para>
  /// </summary>
  public abstract class AbstractDumpProcessor : BaseDumpProcessor
  {
    #region Fields
    /// <summary>
    ///  Gets manager that groups <see cref="ClrObjLogEntry" /> by caption.
    /// </summary>
    protected abstract ICaptionManager CaptionManager { get; }

    /// <summary>
    ///  Gets an a filtered by <see cref="IFilteredObjectsProvider"/> sequence <see cref="IEnumerable{T}" /> of <see cref="ClrObject" /> from provided <see cref="IConnectionSettings" />.
    /// </summary>
    protected abstract IEnumeratiorConnection ClrObjectEnumerator { get; }

    /// <summary>
    /// Gets context factory. TODO: Understand why is it here and write meaningful comments.
    /// </summary>
    /// <value>
    /// The context factory.
    /// </value>
    protected abstract IContextFactory ContextFactory { get; }

    /// <summary>
    ///   Gets an object that transforms <see cref="ClrObject"/> (Memory snapshot based) to <see cref="LogEntry" /> (SCLA) representation.
    /// </summary>
    protected abstract IClrObjectTransformator ClrObjToLogEntryTransformProvider { get; }
    #endregion


    /// <summary>
    ///   Builds the <see cref="LogEntry" /> from <paramref name="clrObject" /> with supporting <see cref="LogEntry" /> array
    ///   in case needed.
    /// <para>Uses <see cref="ClrObjToLogEntryTransformProvider"/>.</para>
    /// </summary>
    /// <param name="clrRuntime">The runtime that stores data fetched.</param>
    /// <param name="clrObject">The object in memory that is to be transformed.</param>
    /// <param name="nested">The supporting <see cref="LogEntry" /> created during main one computing.</param>
    /// <returns>object from memory transformed into SCLA representation.</returns>
    [CanBeNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [TargetedPatchingOptOut("Performace critical")]
    protected LogEntry BuildCandidate([NotNull] ClrRuntime clrRuntime, [ClrObjAndTypeNotEmpty] ClrObject clrObject, [CanBeNull] out LogEntry[] nested)
    {
      return this.ClrObjToLogEntryTransformProvider.BuildCandidate(clrRuntime, clrObject, out nested);
    }


    /// <summary>
    /// Builds the captions to be shown in UI (wrapped in try-catch).
    /// <para>Uses <see cref="CaptionManager"/> to get groups by log levels.</para>
    /// </summary>
    /// <param name="parsingResult">The parsing result.</param>
    /// <returns>
    /// A collection of groups per LogLevel.
    /// </returns>
    protected override LogGroupsCollection BuildCaptions([NotNull] ParsingResult parsingResult)
    {
      Sitecore.Diagnostics.Assert.ArgumentNotNull(parsingResult, "parsingResult");
      var collection = new LogGroupsCollection
      {
        {
          LogLevel.DEBUG, this.CaptionManager.GetGroups(parsingResult.Debugs, LogLevel.DEBUG, x => x.Caption)
        },
        {
          LogLevel.FATAL, this.CaptionManager.GetGroups(parsingResult.Fatals, LogLevel.FATAL, x => x.Caption)
        },
        {
          LogLevel.WARN, this.CaptionManager.GetGroups(parsingResult.Warns, LogLevel.WARN, x => x.Caption)
        },
        {
          LogLevel.ERROR, this.CaptionManager.GetGroups(parsingResult.Errors, LogLevel.ERROR, x => x.Caption)
        },
        {
          LogLevel.INFO, this.CaptionManager.GetGroups(parsingResult.Infos, LogLevel.INFO, x => x.Caption)
        }
      };
      return collection;
    }

    /// <summary>
    ///   Builds the general context.
    ///   <para>TODO: Write understandable comments.</para>
    /// </summary>
    /// <param name="parsingResult">The parsing result.</param>
    /// <param name="captions">The captions.</param>
    /// <returns></returns>
    protected override GeneralContext BuildGeneralContext(ParsingResult parsingResult, LogGroupsCollection captions)
    {
      return this.ContextFactory.GetContext(parsingResult, captions);
    }

    /// <summary>
    ///   Builds the parsing result wrapped in try-catch, so must raise exceptions.
    /// </summary>
    /// <param name="clrRuntime">The data source.</param>
    /// <param name="processContext">The process context.</param>
    /// <returns></returns>
    protected override ParsingResult BuildParsingResult([NotNull] ClrRuntime clrRuntime, [NotNull] LogProcessorSettings processContext)
    {
      var parsingResult = new ParsingResult();

      var filter = processContext.ReaderSettings;

      var i = 0;

      var correctModels = new List<LogEntry>();

      foreach (var clrObject in this.ClrObjectEnumerator.ExtractFromRuntime(clrRuntime))
      {
        // Context.Message("Processing " + clrObject.Address.ToString("X") + " of " + clrObject.Type);
        LogEntry[] nested;
        var candidate = this.BuildCandidate(clrRuntime, clrObject, out nested);

        if ((candidate == null) || ((filter != null) && !filter.Matches(candidate)))
        {
          continue;
        }

        candidate.Index = ++i;
        correctModels.Add(candidate);
        if ((nested == null) || (nested.Length == 0))
        {
          continue;
        }

        Array.ForEach(nested, t => t.Index = ++i);
        correctModels.AddRange(nested);
      }

      parsingResult.AddToGroupAll(correctModels);

      return parsingResult;
    }

    /// <summary>
    ///   Reorganizes the parsing result by
    ///   <para>Sorting all entries by <see cref="LogEntry.LogDateTime" /></para>
    ///   <para>
    ///     Reorganizes <see cref="ParsingResult.All" /> by appropriate <see cref="LogLevel" /> into
    ///     <see cref="ParsingResult.Infos" />, <see cref="ParsingResult.Warns" />, and so on.
    ///   </para>
    ///   <para>indexes <see cref="ParsingResult.All" /> entries by giving sequential numbers.</para>
    /// </summary>
    /// <param name="parsingResult">The parsing result.</param>
    protected override void ReorganizeParsingResult([NotNull]ParsingResult parsingResult)
    {
      parsingResult.SortAll().ReorganizeAllByLevels().IndexAllGroup();
    }

    /// <summary>
    ///   Builds the <see cref="ClrRuntime" /> from provided <see cref="MemoryDumpConnectionDetails" /> connection details.
    ///   <para>
    ///     Uses <see cref="IClrRuntimeFactory" /> from <see cref="ClrObjectEnumerator" />.
    ///   </para>
    /// </summary>
    /// <param name="connection">The connection.</param>
    /// <returns>Initialized <see cref="ClrRuntime"/> instance from provided connection details.</returns>
    /// <exception cref="RequiredObjectIsNullException">
    ///   in case <see cref="IClrRuntimeFactory" /> attempted to return null
    ///   <see cref="ClrRuntime" />.
    /// </exception>
    [NotNull]
    protected override ClrRuntime BuildRuntime([NotNull] MemoryDumpConnectionDetails connection)
    {
      var factory = this.ClrObjectEnumerator.RuntimeFactory;

      Sitecore.Diagnostics.Assert.IsNotNull(factory, "{0} is not set.", typeof(IClrRuntimeFactory).Name);

      return Sitecore.Diagnostics.Assert.ResultNotNull(factory.BuildClrRuntime(connection), $"{factory.GetType().Name} returned null object from {connection}");
    }
  }
}
