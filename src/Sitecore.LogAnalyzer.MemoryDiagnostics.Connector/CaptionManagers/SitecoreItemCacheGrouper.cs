namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Example.ItemCacheStats
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.MemoryDiagnostics.Models.FallBack.SitecoreRelated;

  //TODO: Extract parent - child interface

  /// <summary>
  /// Enables grouping of <see cref="PrefetchDataModel"/> by setting mappings between <see cref="PrefetchDataModel.ItemID"/> and <see cref="PrefetchDataModel.ParentID"/> where possible.  
  /// </summary>
  public class PrefetchModelCaptionManager : ClrObjCaptionManager
  {
    protected override void ProcessClrObjectEntries(List<ClrObjLogEntry> items, LogGroups result, Func<LogEntry, string> selecter)
    {
      using (new LongRunningOperationWatcher("Setting parent child elements", 200))
      {
        SetParentWherePossible(items);
      }

      base.ProcessClrObjectEntries(items, result, selecter);
    }


    private void SetParentWherePossible([NotNull]List<ClrObjLogEntry> items)
    {
      //select entries with  models as PrefetchDataModel
      var itemPrefetchDataModels = (from item in items
                                    where (item != null) // extra safety.
                                    let prefetch = item.Model as PrefetchDataModel
                                    where (prefetch != null)
                                    where prefetch.HasItemDefinition
                                    orderby item.Index
        select item).ToArray();

      Parallel.ForEach(itemPrefetchDataModels, (t) =>
      {
        var searchForParent = t;
        var searchedForParentPrefetchModel = searchForParent.Model as PrefetchDataModel;
        if (searchedForParentPrefetchModel == null)
        {
          return;
        }

        var searchedDadId = (searchedForParentPrefetchModel).ParentID;

        //cannot find dad, anyway..
        if (searchedDadId.IsGlobalNullId)
        {
          return;
        }

        // try find father
        var father = (from model in itemPrefetchDataModels
                      let potentialDad = model.Model as PrefetchDataModel
                      where potentialDad != null && potentialDad.HasItemDefinition
                      where potentialDad.ItemID == searchedDadId
                      select model).FirstOrDefault();

        // father was not found.
        if (ReferenceEquals(father, null))
        {
          return;
        }

        searchForParent.Parent = father;
        });
    }
  }
}
