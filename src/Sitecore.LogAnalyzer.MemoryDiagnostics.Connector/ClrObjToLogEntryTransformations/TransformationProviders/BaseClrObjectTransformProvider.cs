namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders
{
  using Microsoft.Diagnostics.Runtime;

  using Sitecore.LogAnalyzer.Attributes;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.MemoryDiagnostics.ModelFactory.Abstracts;
  using Sitecore.MemoryDiagnostics.ModelFilters;
  using Sitecore.MemoryDiagnostics.Models.BaseMappingModel;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;

  /// <summary>
  /// Transforms <see cref="ClrObject"/> into <see cref="LogEntry"/>.
  /// <para>Uses <see cref="IModelMappingFilter"/> to map fields.</para>
  /// <para>Uses <see cref="IInitLogEntryFields"/> to perform custom initialization.</para>
  /// <para>Uses <see cref="IModelMappingFilter"/> to perform filtering.</para>
  /// </summary>
  /// <seealso cref="IClrObjectTransformator" />
  public abstract class BaseClrObjectTransformProvider : IClrObjectTransformator
  {
    #region fields

    /// <summary>
    /// Gets the model mapper factor that transforms <see cref="ClrObject" /> into corresponding <see cref="IClrObjMappingModel" />.
    /// <para>Injects data from <see cref="ClrObject"/> to <see cref="IClrObjMappingModel"/> fields.</para>
    /// </summary>
    /// <value>
    /// The model mapper factory.
    /// </value>
    protected virtual IModelMapperFactory ModelMapperFactory => EmptyModelMapperFactory.Instance;

    /// <summary>
    /// Defines additional logic/control for filtering <see cref="IClrObjMappingModel"/> instances. 
    /// <para><example>Show caches that are not empty. </example></para>
    /// </summary>
    protected virtual IModelMappingFilter Filter => EmptyModelMappingFilter.Instance;
   

    /// <summary>
    /// Provides additional control for setting <see cref="ClrObjLogEntry"/> fields after <see cref="ClrObjLogEntry.InitFldsFromModel"/> API execution.
    /// </summary>
    protected virtual IInitLogEntryFields LogEntryFieldsInitializer => LogEntryFieldInitializer.Singleton;

    protected virtual TextStorage Storage { get; }

    #endregion 

    #region public methods
    /// <summary>
    /// Builds the <see cref="LogEntry" /> candidate from <see cref="ClrObject" />.
    /// </summary>
    /// <param name="clrRuntime">The runtime</param>
    /// <param name="clrObject">The object to be transformed into SCLA representation</param>
    /// <param name="nested">The array of supporting nested entries for processed candidate would be output</param>
    /// <returns><value>null</value> in case candidate was not built.</returns>
    [CanBeNull]
    public virtual LogEntry BuildCandidate([NotNull]ClrRuntime clrRuntime, ClrObject clrObject, out LogEntry[] nested)
    {
      var mapping = this.ModelMapperFactory.BuildModel(clrObject);

      if (!this.Filter.Matches(mapping))
      {
        nested = null;
        return null;
      }

      var result = new ClrObjLogEntry(mapping, this.Storage);
      result.InitFldsFromModel();

      this.LogEntryFieldsInitializer.ApplyCustomLogicOnLogEntry(result);
      nested = this.GetNestedObjects(clrRuntime, clrObject, mapping, result);
      if (nested != null)
      {
        foreach (var logEntry in nested)
        {
          var casted = logEntry as ClrObjLogEntry;
          if (casted == null)
          {
            continue;
          }
          this.LogEntryFieldsInitializer.ApplyCustomLogicOnLogEntry(casted);
        }
      }

      return result;
    }
    #endregion

    #region protected virtual methods
    /// <summary>
    /// Gets the nested objects for <paramref name="clrObject"/> represented as <paramref name="mapping"/>.
    /// <example>
    ///   <para>Example: For thread - could be thread stack objects.</para>
    ///   <para>For Cache - could be cache content.</para>
    /// </example>
    /// </summary>
    /// <param name="clrRuntime">The runtime.</param>
    /// <param name="clrObject">The ClrObject whose nested elements are to be fetched.</param>
    /// <param name="mapping">The converted representation of <paramref name="clrObject"/>.</param>
    /// <param name="parentEntry">The <paramref name="clrObject"/> resulting SCLA representation.</param>
    /// <returns>An array of elements nested inside clrObject.<value>Null</value>  is ok.</returns>
    [CanBeNull]
    public virtual LogEntry[] GetNestedObjects([NotNull]ClrRuntime clrRuntime, ClrObject clrObject, IClrObjMappingModel mapping, ClrObjLogEntry parentEntry)
    {
      return null;
    }

    /// <summary>
    /// Transforms <paramref name="casted"/> to <see cref="ClrObjLogEntry"/> without any transformation.
    /// </summary>
    /// <param name="parentEntry">The parent entry.</param>
    /// <param name="casted">The casted.</param>
    /// <returns></returns>
    [CanBeNull]
    protected virtual ClrObjLogEntry ModelToClrObjLogEntry(ClrObjLogEntry parentEntry, IClrObjMappingModel casted)
    {
      var entry = new ClrObjLogEntry(casted, this.Storage, parentEntry);

      entry.InitFldsFromModel();

      return entry;
    }

    #endregion

  }
}
