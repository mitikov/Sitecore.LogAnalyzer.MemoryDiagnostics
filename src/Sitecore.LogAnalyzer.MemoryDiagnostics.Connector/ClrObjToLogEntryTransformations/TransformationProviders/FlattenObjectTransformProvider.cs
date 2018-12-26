using System.Collections.Generic;
using Microsoft.Diagnostics.Runtime;
using Sitecore.LogAnalyzer.Models;
using Sitecore.MemoryDiagnostics.ModelFactory.Abstracts;
using Sitecore.MemoryDiagnostics.ModelFilters;
using Sitecore.MemoryDiagnostics.Models.BaseMappingModel;
using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
using SitecoreMemoryInspectionKit.Core.ClrHelpers;

namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders
{
  public class FlattenObjectTransformProvider : ClrObjectTranformProvider
  {
    public FlattenObjectTransformProvider(IModelMapperFactory modelMapperFactory, IInitLogEntryFields logEntryFieldsInitializer, IModelMappingFilter filter) : base(modelMapperFactory, logEntryFieldsInitializer, filter)
    {
    }

    public override LogEntry[] GetNestedObjects([LogAnalyzer.Attributes.NotNull] ClrRuntime clrRuntime, ClrObject clrObject, IClrObjMappingModel mapping, ClrObjLogEntry parentEntry)
    {
      var objectStream = mapping as IEnumerable<IClrObjMappingModel>;
      if (objectStream ==null)
      {
        return null;
      }

      var children = new List<ClrObjLogEntry>();
      foreach (var @obj in objectStream)
      {
        if (@obj.Obj.IsNullObj)
          continue;

        var entry = this.ModelToClrObjLogEntry(parentEntry, @obj);
        children.Add(entry);
      }

      return children.ToArray();
    }
  }
}
