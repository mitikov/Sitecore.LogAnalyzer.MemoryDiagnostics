namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.HeapStat
{
  using System.Collections.Generic;
  using System.Linq;
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.MemoryDiagnostics.Facades.ObjectEnumeration;

  public class LiveDeadHeapStat : DumpHeapStat
  {
    public override IOrderedEnumerable<HeapStatEntry> GetObjectTypeStats(ClrRuntime runtime, IObjectEnumerationFacade liveObjectEnumerator)
    {
      // Avoid further moves in memory
      Dictionary<ClrType, HeapStatEntry> stats = new Dictionary<ClrType, HeapStatEntry>(1000 * 1000);      

      foreach (var clrObj in liveObjectEnumerator.ClrObjectEnumerator.EnumerateObjectsFromSource(runtime))
      {
        ulong obj = clrObj.Address;
        ClrType type = clrObj.Type;

        if (type == null)
        {
          continue;
        }

        ulong size = type.GetSize(obj);

        // Add an entry to the dictionary, if one doesn't already exist.
        HeapStatEntry entry = null;
        if (!stats.TryGetValue(type, out entry))
        {
          entry = new HeapStatEntry(type.Name);
          stats.Add(type, entry);
        }

        // Update the statistics for this object.
        entry.Count++;
        entry.Size += size;
      }
      return stats.Values.OrderBy(t => t.Count);
    }
  }    
  
}
