using System.Collections.Generic;

namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders
{
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.MemoryDiagnostics.ModelFactory.Abstracts;
  using Sitecore.MemoryDiagnostics.ModelFilters;
  using Sitecore.MemoryDiagnostics.Models.BaseMappingModel;
  using Sitecore.MemoryDiagnostics.Models.FallBack.MongoRelated;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;

  public class MongoConnectionPoolTransformProvider : ClrObjectTranformProvider
  {
    public MongoConnectionPoolTransformProvider(IModelMapperFactory modelMapperFactory, IInitLogEntryFields logEntryFieldsInitializer, IModelMappingFilter filter) : base(modelMapperFactory, logEntryFieldsInitializer, filter)
    {
    }

    public override LogEntry[] GetNestedObjects(ClrRuntime clrRuntime, ClrObject clrObject, IClrObjMappingModel mapping, ClrObjLogEntry parentEntry)
    {
      var poolMapping = mapping as MongoConnectionPoolMappingModel;
      if ((poolMapping == null) || poolMapping._availableConnections.IsEmpty)
      {
        return null;
      }

      var result = new List<LogEntry>();

      foreach (IClrObjMappingModel availableConnection in poolMapping._availableConnections)
      {
        var entry = this.ModelToClrObjLogEntry(parentEntry, availableConnection);
        if (entry != null)
        {
          result.Add(entry);
        }
      }

      return result.ToArray();
    }

  }
}
