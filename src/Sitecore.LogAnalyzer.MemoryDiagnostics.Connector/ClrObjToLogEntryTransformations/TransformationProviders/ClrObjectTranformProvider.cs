namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders
{  
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.MemoryDiagnostics.ModelFactory;
  using Sitecore.MemoryDiagnostics.ModelFactory.Abstracts;
  using Sitecore.MemoryDiagnostics.ModelFilters;
  using Sitecore.MemoryDiagnostics.Models.BaseMappingModel;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;

  /// <summary>
  /// Transforms <see cref="ClrObject"/> into <see cref="ClrObjLogEntry"/> with an array of supporting <see cref="ClrObjLogEntry"/> nested elements.
  /// <para>Code flow</para>
  /// <para>1. Uses <see cref="IModelMapperFactory"/> (fallback to <see cref="LazyLoadModelMapperFactory"/>) to build <see cref="IClrObjMappingModel"/></para>
  /// <para>2. Applies <see cref="IModelMappingFilter"/> filter (fallback to <see cref="EmptyModelMappingFilter"/>) afterwards.</para>
  /// <para>3. Initiates new instance of <see cref="ClrObjLogEntry"/> with <see cref="IClrObjMappingModel"/> and calls <see cref="ClrObjLogEntry.InitFldsFromModel"/> API.</para>
  /// <para>4. Calls <see cref="IInitLogEntryFields"/> which provides possibility for custom logic entry initialization. </para>
  /// <para>5. Calls <see cref="BaseClrObjectTransformProvider.GetNestedObjects"/> to compose an array of nested objects.</para>
  /// </summary>
  public class ClrObjectTranformProvider : BaseClrObjectTransformProvider
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ClrObjectTranformProvider"/> class.
    /// </summary>
    /// <param name="modelMapperFactory">
    /// The model mapper factory.
    /// </param>
    /// <param name="logEntryFieldsInitializer">
    /// The log entry fields initializer to apply case-specific initialization.
    /// </param>
    /// <param name="filter">
    /// The filter to be applied on resulting entities for filtering.
    /// </param>
    public ClrObjectTranformProvider(IModelMapperFactory modelMapperFactory, IInitLogEntryFields logEntryFieldsInitializer, IModelMappingFilter filter)
    {
      ModelMapperFactory = modelMapperFactory;
      Filter = filter;
      LogEntryFieldsInitializer = logEntryFieldsInitializer;
      Storage = new TextStorage();
    }

    #region fields

    protected override IModelMapperFactory ModelMapperFactory { get; }

    /// <summary>
    /// Defines additional logic/control for filtering <see cref="IClrObjMappingModel" /> instances.
    /// <para><example>Show caches that are not empty. </example></para>
    /// </summary>
    protected override IModelMappingFilter Filter { get; }

    /// <summary>
    /// Provides additional control for setting <see cref="ClrObjLogEntry" /> fields after <see cref="ClrObjLogEntry.InitFldsFromModel" /> API execution.
    /// </summary>
    protected override IInitLogEntryFields LogEntryFieldsInitializer { get; }

    protected override TextStorage Storage { get; }
    #endregion

  }
}
