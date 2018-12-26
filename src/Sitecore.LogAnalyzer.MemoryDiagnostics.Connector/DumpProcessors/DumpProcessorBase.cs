namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DumpProcessors
{
  using System;
  using System.Collections.Generic;
  using Microsoft.Diagnostics.Runtime;
  using Ninject;
  using Sitecore.LogAnalyzer;
  using Sitecore.LogAnalyzer.Contexts;
  using Sitecore.LogAnalyzer.Managers;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.LogAnalyzer.Parsing;
  using Sitecore.LogAnalyzer.Settings;
  using Sitecore.MemoryDiagnostics.ClrObjectEnumerators;
  using Sitecore.MemoryDiagnostics.ClrObjectsProviders;
  using Sitecore.MemoryDiagnostics.ClrObjectsProviders.Base;
  using Sitecore.MemoryDiagnostics.Facades.ObjectEnumeration;
  using Sitecore.MemoryDiagnostics.ModelFactory;
  using Sitecore.MemoryDiagnostics.ModelFactory.Abstracts;
  using Sitecore.MemoryDiagnostics.ModelFilters;
  using Sitecore.MemoryDiagnostics.SourceFactories;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.ObjectEnumeration;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;

  /// <summary>
  ///   <para>Use <see cref="Context.CurrentContainer" /> to get dependencies.</para>
  ///   <see cref="ILogProcessor" /> code flow:
  ///   <para>
  ///     1. Use <see cref="IClrRuntimeFactory" /> (fallback to <see cref="MDClrRuntimeFactory" />) to get
  ///     <see cref="ClrRuntime" />.
  ///   </para>
  ///   <para>
  ///     2. Use <see cref="IEnumerateClrObjectsFromClrRuntime" /> (
  ///     <see cref="ManagedTheadPoolWorkersThreadEnumerator" /> fallback) to enumerate objects <see cref="ClrObject" />
  ///     suitable for parsing.
  ///   </para>
  ///   <para>
  ///     <p>
  ///       2.1. Can specify <see cref="IFilteredObjectsProvider" /> (fallback to <see cref="NullFilterClrObjectProvider" />
  ///       ) to filter <see cref="ClrObject" /> returned.
  ///     </p>
  ///   </para>
  ///   <para>
  ///     2.2. Call <see cref="AbstractDumpProcessor.BuildCandidate" /> (<see cref="IClrObjectTransformator" />, fallback to
  ///     <see cref="ClrObjectTranformProvider" />) to transform <see cref="ClrObject" /> into <see cref="LogEntry" /> with
  ///     related objects.
  ///   </para>
  ///   <para>
  ///     2.2.1. <see cref="IClrObjectTransformator" /> can use <see cref="IModelMappingFilter" /> (fallback to
  ///     <see cref="EmptyModelMappingFilter" />).
  ///   </para>
  ///   <para>
  ///     2.3. Applies <see cref="LogProcessorSettings.ReaderSettings" /> filter on resulting <see cref="LogEntry" />
  ///   </para>
  ///   <para>2.4. <see cref="LogEntry" /> is added to <see cref="ParsingResult.All" /></para>
  ///   <para>2.5. Sorting, reorganizing, and indexing is performed</para>
  ///   <para>3. <see cref="LogGroupsCollection" /> is build using <see cref="AbstractDumpProcessor.BuildCaptions" /> method.</para>
  ///   <para>
  ///     4. <see cref="GeneralContext" /> is build by call to <see cref="AbstractDumpProcessor.BuildGeneralContext" /> API ( call to
  ///     <see cref="IContextFactory" /> - fallback to <see cref="EmptyContextFactory.Instance" />)
  ///   </para>
  /// </summary>
  [Obsolete("Use DiDumpProcessor instead.")]
  public class DumpProcessorBase : AbstractDumpProcessor
  {
    /// <summary>
    ///   Groups <see cref="ClrObjLogEntry" /> by caption.
    /// </summary>
    protected override ICaptionManager CaptionManager { get; }

    /// <summary>
    /// Provides a filtered by <see cref="IFilteredObjectsProvider" /> sequence <see cref="IEnumerable{T}" /> of <see cref="ClrObject" /> from provided <see cref="IConnectionSettings" />
    /// </summary>
    protected override IEnumeratiorConnection ClrObjectEnumerator { get; }

    /// <summary>
    ///   The _ context factory. TODO: Understand why is it here and write meaningful comments.
    /// </summary>
    protected override IContextFactory ContextFactory { get; }

    /// <summary>
    ///   <see cref="ClrObject" /> to <see cref="LogEntry" /> transform provider
    /// </summary>
    protected override IClrObjectTransformator ClrObjToLogEntryTransformProvider { get; }

    /// <summary>
    ///   Initializes a new instance of the <see cref="DumpProcessorBase" /> class.
    /// </summary>
    [Inject]
    public DumpProcessorBase()
    {
      this.ClrObjectEnumerator =
        Context.GetOrDefault<IEnumeratiorConnection>(
          new MdBasedEnumerationConnection(
          new DefaultObjectEnumerationFacade(
            Context.GetOrDefault<IEnumerateClrObjectsFromClrRuntime>(ManagedTheadPoolWorkersThreadEnumerator.Instance),
            Context.GetOrDefault<IFilteredObjectsProvider>(NullFilterClrObjectProvider.Instance))));


      this.ContextFactory = Context.GetOrDefault(EmptyContextFactory.Instance);

      this.CaptionManager = Context.GetOrDefault<ICaptionManager>(new CaptionManager());

      this.ClrObjToLogEntryTransformProvider = Context.GetOrDefault<IClrObjectTransformator>(
        new ClrObjectTranformProvider(
      Context.GetOrDefault<IModelMapperFactory>(new LazyLoadModelMapperFactory()),
      Context.GetOrDefault<IInitLogEntryFields>(LogEntryFieldInitializer.Singleton),
      Context.GetOrDefault<IModelMappingFilter>(EmptyModelMappingFilter.Instance)));
    }  
  }
}