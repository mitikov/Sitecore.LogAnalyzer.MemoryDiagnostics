using System.Collections.Generic;

namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders
{
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.MemoryDiagnostics.ModelFactory;
  using Sitecore.MemoryDiagnostics.ModelFactory.Abstracts;
  using Sitecore.MemoryDiagnostics.ModelFilters;

  using Sitecore.MemoryDiagnostics.Models.BaseMappingModel;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;

  /// <summary>
  /// <see cref="ClrObjectTranformProvider"/> that respects <see cref="IHaveChildEntries"/> implementation for <see cref="IClrObjMappingModel"/>.  
  /// <para>Ip</para>
  /// </summary>
  /// <seealso cref="ClrObjectTranformProvider" />
  public class HaveChildEntriesTransformationProvider : ClrObjectTranformProvider
  {
    public HaveChildEntriesTransformationProvider(IModelMapperFactory modelMapperFactory, IInitLogEntryFields logEntryFieldsInitializer, IModelMappingFilter filter) 
      : base(modelMapperFactory, logEntryFieldsInitializer, filter)
    {
    }

    public override LogEntry[] GetNestedObjects(ClrRuntime clrRuntime, ClrObject clrObject, IClrObjMappingModel mapping, ClrObjLogEntry parentEntry)
    {
      var mappingWithKids = mapping as IHaveChildEntries;
      if (mappingWithKids == null)
      {
        return null;
      }

      var result = new List<LogEntry>();

      foreach (IClrObjMappingModel child in mappingWithKids.ChildrenEntries)
      {
        var entry = ModelToClrObjLogEntry(parentEntry, child);
        if (entry != null)
        {
          result.Add(entry);
        }
      }
      return result.ToArray();
    }


  }
}
