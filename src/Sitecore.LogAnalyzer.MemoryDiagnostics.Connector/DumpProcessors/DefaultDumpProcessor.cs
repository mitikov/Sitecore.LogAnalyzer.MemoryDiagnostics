namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DumpProcessors
{
  using Sitecore.LogAnalyzer;
  using Sitecore.LogAnalyzer.Contexts;
  using Sitecore.LogAnalyzer.Managers;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.LogAnalyzer.Settings;
  using Sitecore.MemoryDiagnostics.ClrObjectEnumerators;
  using Sitecore.MemoryDiagnostics.ClrObjectsProviders;
  using Sitecore.MemoryDiagnostics.Facades.ObjectEnumeration;
  using Sitecore.MemoryDiagnostics.ModelFactory;
  using Sitecore.MemoryDiagnostics.ModelFactory.Abstracts;
  using Sitecore.MemoryDiagnostics.ModelFilters;
  using Sitecore.MemoryDiagnostics.SourceFactories;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.ObjectEnumeration;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;

  /// <summary>
  /// Default bindings for dump processor.
  /// </summary>
  /// <seealso cref="AbstractDumpProcessor" />
  public class DefaultDumpProcessor : AbstractDumpProcessor
  {
    /// <summary>
    /// Groups <see cref="ClrObjLogEntry" /> by caption.
    /// </summary>
    protected override ICaptionManager CaptionManager { get; } = new CaptionManager();

    /// <summary>
    /// Provides <see cref="IEnumerable{T}" /> of <see cref="ClrObject" /> from provided <see cref="IConnectionSettings" />
    /// </summary>
    protected override IEnumeratiorConnection ClrObjectEnumerator { get; } = new BaseObjectEnumerationConnectionDetails(
      new MDClrRuntimeFactory(),
      new DefaultObjectEnumerationFacade(
     new ManagedTheadPoolWorkersThreadEnumerator(),
     NullFilterClrObjectProvider.Instance));

    /// <summary>
    /// The _ context factory. TODO: Understand why is it here and write meaningful comments.
    /// </summary>
    protected override IContextFactory ContextFactory { get; } = EmptyContextFactory.Instance;

    /// <summary>
    /// Gets an object that transforms <see cref="ClrObject" /> (Memory snapshot based) to <see cref="LogEntry"/> (SCLA) representation.
    /// </summary>
    protected override IClrObjectTransformator ClrObjToLogEntryTransformProvider { get; } = new ClrObjectTranformProvider(
      Context.GetOrDefault<IModelMapperFactory>(new LazyLoadModelMapperFactory()),
      Context.GetOrDefault<IInitLogEntryFields>(LogEntryFieldInitializer.Singleton),
      Context.GetOrDefault<IModelMappingFilter>(EmptyModelMappingFilter.Instance));
  }
}
