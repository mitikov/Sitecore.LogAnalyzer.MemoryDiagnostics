namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.MemoryDiagnostics.ModelFactory;
  using Sitecore.MemoryDiagnostics.ModelFactory.Abstracts;
  using Sitecore.MemoryDiagnostics.ModelFilters;
  using Sitecore.MemoryDiagnostics.Models.BaseMappingModel;
  using Sitecore.MemoryDiagnostics.Models.FallBack.SitecoreRelated;
  using Sitecore.MemoryDiagnostics.Models.InternalProcessing;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;

  /// <summary>
  /// Gets nested cache content as <see cref="CacheEntryModel"/>, and builds <see cref="ClrObjLogEntry"/> using object.
  /// </summary>
  public class CacheEntryTransformProvider : ClrObjectTranformProvider
  {
    public override LogEntry[] GetNestedObjects(ClrRuntime clrRuntime, ClrObject clrObject, IClrObjMappingModel mapping,
      ClrObjLogEntry parentEntry)
    {
      var sitecoreCache = mapping as ScCache;
      if (sitecoreCache == null)
      {
        return null;
      }

      if (sitecoreCache.currentSize == 0)
      {
        LogAnalyzer.Context.Message("{0} cache is empty.", sitecoreCache.name);
        return null;
      }

      var result = new List<LogEntry>();
      var box = sitecoreCache.Obj.GetRefFld("box").GetRefFld("Data");

      foreach (DictionaryEntry candidate in (HashtableMappingModel)ModelMapperFactory.BuildModel(box))
      {
        var casted = candidate.Value as CacheEntryModel;
        if ((casted == null) || casted.KeyIsNull)
        {
          continue;
        }

        var entry = this.ModelToClrObjLogEntry(parentEntry, casted);
        if (entry != null)
        {
          result.Add(entry);
        }
      }

      return result.Count > 0 ? result.ToArray() : null;
    }

    public CacheEntryTransformProvider(IModelMapperFactory modelMapperFactory, IModelMappingFilter filter, IInitLogEntryFields logEntryFieldsInitializer) : base(modelMapperFactory, logEntryFieldsInitializer, filter)
    {
    }
  }
}
