namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders.Caches.Statistics
{
  using System;
  using Interfaces;
  using LogAnalyzer.Models;
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.MemoryDiagnostics.ModelFactory.Abstracts;
  using Sitecore.MemoryDiagnostics.ModelFilters;
  using Sitecore.MemoryDiagnostics.Models.BaseMappingModel;
  using Sitecore.MemoryDiagnostics.Models.FallBack.SitecoreRelated;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;

  /// <summary>
  ///  Measures how much smaller cache gonna be in case non-used langauge is removed from database.
  /// </summary>
  /// <seealso cref="PrefetchCacheEntryTransformProvider" />
  public class ImpactOfRemovingLangfromPrefetchCacheTransformProvider : PrefetchCacheEntryTransformProvider
  {  
    protected string KeyInListName;

    protected long AccumulatedSize;

    protected long TotalSize;

    public ImpactOfRemovingLangfromPrefetchCacheTransformProvider(string stringPart, IModelMapperFactory modelMapperFactory, IModelMappingFilter filter, IInitLogEntryFields logEntryFieldsInitializer) : base(modelMapperFactory, filter, logEntryFieldsInitializer)
    {
      this.KeyInListName = stringPart;
    }

    /// <summary>
    /// Gets the nested objects.
    /// </summary>
    /// <param name="clrRuntime">The color runtime.</param>
    /// <param name="clrObject">The color object.</param>
    /// <param name="mapping">The mapping.</param>
    /// <param name="parentEntry">The parent entry.</param>
    /// <returns></returns>
    public override LogEntry[] GetNestedObjects(ClrRuntime clrRuntime, ClrObject clrObject, IClrObjMappingModel mapping,
      ClrObjLogEntry parentEntry)
    {
      this.AccumulatedSize = this.TotalSize = 0;
      var sitecoreCacheMapping = mapping as ScCache;

      if (sitecoreCacheMapping == null)
      {
        return null;
      }

      // Execute base logic to ensure ModelToClrObjLogEntry gets called.
      base.GetNestedObjects(clrRuntime, clrObject, mapping, parentEntry);

      var ratio = this.TotalSize == 0 ? 0 : ((double)this.AccumulatedSize / this.TotalSize);

      var resultingEntry = new ClrObjLogEntry(null, this.Storage, parentEntry)
      {
        Caption = $"Removing {this.KeyInListName} key {ratio:P} impact",
        Text =
          $"{this.KeyInListName} key from {sitecoreCacheMapping.name} cache would save {ratio:P}",
        Level = parentEntry.Level,
        LogDateTime = parentEntry.LogDateTime + TimeSpan.FromSeconds(1)
      };

      return new LogEntry[]
      {
        resultingEntry
      };
    }
  }
}
