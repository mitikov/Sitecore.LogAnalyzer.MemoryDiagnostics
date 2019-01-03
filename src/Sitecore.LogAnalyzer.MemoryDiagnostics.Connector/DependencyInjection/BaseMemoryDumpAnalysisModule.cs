namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DependencyInjection
{
  using Ninject.Modules;
  using Sitecore.LogAnalyzer.Contexts;
  using Sitecore.LogAnalyzer.Managers;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.LogAnalyzer.Parsing;
  using Sitecore.MemoryDiagnostics.ClrObjectEnumerators;
  using Sitecore.MemoryDiagnostics.ClrObjectsProviders;
  using Sitecore.MemoryDiagnostics.ClrObjectsProviders.Base;
  using Sitecore.MemoryDiagnostics.Facades.ObjectEnumeration;
  using Sitecore.MemoryDiagnostics.ModelFactory;
  using Sitecore.MemoryDiagnostics.ModelFactory.Abstracts;
  using Sitecore.MemoryDiagnostics.ModelFilters;
  using Sitecore.MemoryDiagnostics.Models.BaseMappingModel;
  using Sitecore.MemoryDiagnostics.SourceFactories;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DumpProcessors;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.LogAnalyzer;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.ObjectEnumeration;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;

  /// <summary>
  /// Defines default OOB bindings for memory analysis dependencies.
  /// <para><see cref="Load"/> method gives following binding order:</para>
  /// <para><see cref="RegisterLogAnalyzerSpecific"/> is first. Registers <see cref="ILogAnalyzerFacade"/>, <see cref="ICaptionManager"/>, and <see cref="IContextFactory"/>.</para>
  /// <para><see cref="RegisterObjectEnumeration"/> is second. Registers <see cref="IEnumeratiorConnection"/>, <see cref="IClrRuntimeFactory"/>, <see cref="IEnumerateClrObjectsFromClrRuntime"/>, <see cref="IFilteredObjectsProvider"/>, <see cref="IObjectEnumerationFacade"/>).</para>
  /// <para><see cref="RegisterClrModelToLogEntryDependencies"/> is third. Registers <see cref="IInitLogEntryFields"/>, <see cref="IModelMappingFilter"/>, <see cref="IModelMapperFactory"/>, and <see cref="IClrObjectTransformator"/>.</para>
  /// <para><see cref="RegisterDumpProcessorDependencies"/> is forth. Registers <see cref="ILogProcessor"/>.</para>
  /// </summary>
  /// <seealso cref="Ninject.Modules.NinjectModule" />
  public abstract class BaseMemoryDumpAnalysisModule : NinjectModule
  {
    /// <summary>
    /// Loads the module into the kernel, and registers needed dependencies. Defines following binding order:
    /// <para><see cref="RegisterLogAnalyzerSpecific"/> is first. Registers <see cref="ILogAnalyzerFacade"/>, <see cref="ICaptionManager"/>, and <see cref="IContextFactory"/>.</para>
    /// <para><see cref="RegisterObjectEnumeration"/> is second. Registers <see cref="IEnumeratiorConnection"/>, <see cref="IClrRuntimeFactory"/>, <see cref="IEnumerateClrObjectsFromClrRuntime"/>, <see cref="IFilteredObjectsProvider"/>, <see cref="IObjectEnumerationFacade"/>).</para>
    /// <para><see cref="RegisterClrModelToLogEntryDependencies"/> is third. Registers <see cref="IInitLogEntryFields"/>, <see cref="IModelMappingFilter"/>, <see cref="IModelMapperFactory"/>, and <see cref="IClrObjectTransformator"/>.</para>
    /// <para><see cref="RegisterDumpProcessorDependencies"/> is forth. Registers <see cref="ILogProcessor"/>.</para>
    /// <para>Violating binding order would cause your dependencies to be overridden.</para>
    /// </summary>
    public override void Load()
    {
      RegisterLogAnalyzerSpecific();

      RegisterObjectEnumeration();

      RegisterClrModelToLogEntryDependencies();

      RegisterDumpProcessorDependencies();             
    }

    /// <summary>
    /// Registers the dump processor that does all the magic.
    /// <para>Constructs factory source, that will extract/enumerate data from connection; transforms/filters factory stream into corresponding models (<see cref="ClrObjLogEntry"/>).</para>
    /// <para>Default <see cref="DiDumpProcessor"/>.</para>
    /// </summary>
    protected virtual void RegisterDumpProcessorDependencies()
    {      
      Rebind<ILogProcessor>().To<DiDumpProcessor>().InSingletonScope();      
    }

    /// <summary>
    /// Registers the dependencies needed to transform <see cref="ClrObject"/> to <see cref="LogEntry"/> (Usually <see cref="ClrObjLogEntry"/>).    
    /// <para>Uses <see cref="IClrObjectTransformator"/> - a wrapper for following operations: </para>
    /// <para><see cref="IModelMapperFactory"/> transforms <see cref="ClrObject"/> from input stream into <see cref="IClrObjMappingModel"/>.</para>
    /// <para><see cref="IModelMapperFactory"/> will use <see cref="IInitLogEntryFields"/> to perform task-specific model initialization.</para>
    /// <para><see cref="IModelMappingFilter"/> allows to filter objects according to task-specific criteria. Example - SQL queries created less than 2 seconds ago.</para>
    /// </summary>
    protected virtual void RegisterClrModelToLogEntryDependencies()
    {
      Rebind<IInitLogEntryFields>().ToConstant(LogEntryFieldInitializer.Singleton);

      Rebind<IModelMappingFilter>().ToConstant(EmptyModelMappingFilter.Instance).InSingletonScope();      

      Rebind<IModelMapperFactory>().To<LazyLoadModelMapperFactory>().InSingletonScope();

      Rebind<IClrObjectTransformator>().To<ClrObjectTranformProvider>();
    }

    /// <summary>
    /// Registers the <see cref="ClrObject"/> enumeration part. <see cref="IEnumerateClrObjectsFromClrRuntime"/> and <see cref="IFilteredObjectsProvider"/>.    
    /// <para>Groups these into <see cref="IObjectEnumerationFacade"/>. </para>
    /// </summary>
    protected virtual void RegisterObjectEnumeration()
    {
      Rebind<IEnumeratiorConnection>().To<BaseObjectEnumerationConnectionDetails>();

      Rebind<IClrRuntimeFactory>().To<MDClrRuntimeFactory>().InSingletonScope();

      Rebind<IEnumerateClrObjectsFromClrRuntime>().ToConstant(ManagedTheadPoolWorkersThreadEnumerator.Instance).InSingletonScope();

      Rebind<IFilteredObjectsProvider>().ToConstant(NullFilterClrObjectProvider.Instance).InSingletonScope();

      Rebind<IObjectEnumerationFacade>().To<DefaultObjectEnumerationFacade>().InSingletonScope();
    }

    /// <summary>
    /// Registers <see cref="ICaptionManager"/>, and <see cref="IContextFactory"/>.
    /// <para>Unions them into <see cref="ILogAnalyzerFacade"/>.</para>
    /// </summary>                                            
    protected virtual void RegisterLogAnalyzerSpecific()
    {
      Rebind<ILogAnalyzerFacade>().To<BaseSCLAFacade>().InSingletonScope();

      // TODO: think if we need 
      Rebind<IContextFactory>().ToConstant(EmptyContextFactory.Instance).InSingletonScope();
      Rebind<ICaptionManager>().To<CaptionManager>().InSingletonScope();
    }
  }
}
