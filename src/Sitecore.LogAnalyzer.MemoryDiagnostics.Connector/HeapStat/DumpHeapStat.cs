namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.HeapStat
{
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.MemoryDiagnostics.Facades.ObjectEnumeration;
  using Microsoft.Diagnostics.Runtime;
  using System;

  /// <summary>
  /// Provides an ordered statistics (<see cref="HeapStatEntry"/>) of objects per type.
  /// </summary>
  public class DumpHeapStat
  {
    /// <summary>
    /// Gets the object type stats.
    /// </summary>
    /// <param name="runtime">The runtime.</param>
    /// <param name="liveObjectEnumerator">The objects LiveObjectsEnumerator.</param>
    /// <returns>An ordered statistics of per-type usage.</returns>
    public virtual IOrderedEnumerable<HeapStatEntry> GetObjectTypeStats(ClrRuntime runtime, IObjectEnumerationFacade liveObjectEnumerator)
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

        if (this.WrongSize(size))
        {
          continue;
        }


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

      return this.SetOrder(stats.Values);
    }

    protected virtual bool WrongSize(ulong size) => size < 85000 ;
    
    /// <summary>
    /// Sets the order.
    /// </summary>
    /// <param name="unorderedCollection">The unordered collection.</param>
    /// <returns>Ordered collection by some rules.</returns>
    protected virtual IOrderedEnumerable<HeapStatEntry> SetOrder(IEnumerable<HeapStatEntry> unorderedCollection) => unorderedCollection.OrderByDescending(element => element.Count);
  }
}
