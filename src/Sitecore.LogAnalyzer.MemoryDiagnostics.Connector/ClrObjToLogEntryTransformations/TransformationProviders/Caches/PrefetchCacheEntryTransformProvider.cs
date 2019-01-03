using Sitecore.MemoryDiagnostics.Models.FallBack.SitecoreRelated;

namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders.Caches
{
  using System;
  using System.Data.SqlClient;
  using Sitecore.MemoryDiagnostics.Helpers;
  using Sitecore.MemoryDiagnostics.ModelFactory.Abstracts;
  using Sitecore.MemoryDiagnostics.ModelFilters;
  using Sitecore.MemoryDiagnostics.Models.BaseMappingModel;  
  using Sitecore.Data;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;

  /// <summary>
  /// Extracts <see cref="PrefetchDataModel"/> from given <see cref="CacheEntryModel"/>.
  /// </summary>
  public class PrefetchCacheEntryTransformProvider : CacheEntryTransformProvider
  {
    private string connection = @"Data Source=.\SQL14;Initial Catalog=sc80rev160115Sitecore_master;Integrated Security=False;User ID=sa;Password=Bl0shenk0";
    public PrefetchCacheEntryTransformProvider(IModelMapperFactory modelMapperFactory, IModelMappingFilter filter, IInitLogEntryFields logEntryFieldsInitializer) : base(modelMapperFactory, filter, logEntryFieldsInitializer)
    {
      var query = "SELECT ID, Name from Items";
      using (var cmd = new SqlCommand(query, new SqlConnection(connection)))
      {
        cmd.Connection.Open();
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
          IDsNameHelper.AddMapping(reader[1] as string, new ID((Guid)reader[0]));
        }

        cmd.Connection.Close();
      }
    }

    /// <summary>
    /// Transforms <paramref name="casted" /> to <see cref="ClrObjLogEntry" /> without any transformation.
    /// </summary>
    /// <param name="parentEntry">The parent entry.</param>
    /// <param name="casted">The casted.</param>
    /// <returns></returns>
    protected override ClrObjLogEntry ModelToClrObjLogEntry(ClrObjLogEntry parentEntry, IClrObjMappingModel casted)
    {
      var target = casted as CacheEntryModel;
      if (target == null)
      {
        return null;
      }

      var dt = target.data as IClrObjMappingModel;

      var entry = new ClrObjLogEntry(dt, Storage, parentEntry);

      entry.InitFldsFromModel();

      return entry;
    }
  }
}
