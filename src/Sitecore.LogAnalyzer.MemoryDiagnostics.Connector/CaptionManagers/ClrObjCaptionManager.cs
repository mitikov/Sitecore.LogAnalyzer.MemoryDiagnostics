namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.LogAnalyzer.Managers;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.MemoryDiagnostics.Attributes;

  /// <summary>
  /// Converts <see cref="LogEntry"/>s into <see cref="LogGroups"/> using <see cref="LogEntry.Caption"/>s. Finds given <see cref="LogEntry"/> or caption in grouping.
  /// <para>Composes <see cref="LogGroups"/></para>
  /// </summary>
  public class ClrObjCaptionManager : CaptionManager
  {
    protected const string DefaultIntent = "     ";

    /// <summary>
    /// Gets the groups.
    /// </summary>
    /// <param name="logs">The logs.</param>
    /// <param name="logLevel">The log level.</param>
    /// <param name="selector">The selector.</param>
    /// <returns>Log Entries grouped by captions.</returns>
    public override sealed LogGroups GetGroups(List<LogEntry> logs, LogLevel logLevel, [NotUsed]Func<LogEntry, string> selector)
    {
      var result = new LogGroups();

      List<ClrObjLogEntry> items = new List<ClrObjLogEntry>(logs.OfType<ClrObjLogEntry>());
      if (items.Count == 0)
      {
        return result;
      }

      this.ProcessClrObjectEntries(items, result, selector);

      return result;
    }

    protected virtual void ProcessClrObjectEntries(List<ClrObjLogEntry> items, LogGroups result, Func<LogEntry, string> selecter)
    {
      var groups = items.GroupBy(t => t.Parent).ToArray();
      var groupsWithoutParent = from t in groups
                                let parent = t.Key
                                where parent == null
                                select t;

      foreach (var groupCandidate in groupsWithoutParent)
      {
        var values = groupCandidate.OrderBy(t => t.Index).ToArray();
        foreach (var rootElem in values)
        {
          var lg = new LogGroup
          {
            Name = rootElem.Caption,
          };
          List<LogEntry> descendants = new List<LogEntry>() { rootElem };
          lg.SubGroups = this.GetSubGroups(groups, rootElem, descendants, selecter);
          lg.Values = descendants.ToArray();
          result.Add(lg.DisplayName, lg);
        }
      }
    }

    private LogGroups GetSubGroups(IGrouping<ClrObjLogEntry, ClrObjLogEntry>[] groups, ClrObjLogEntry rootElem, List<LogEntry> kids, Func<LogEntry, string> selecter, string intent = DefaultIntent)
    {
      var matchedGroup = groups.FirstOrDefault(t => t.Key == rootElem);
      if (matchedGroup == null)
      {
        return null;
      }

      var lgs = new LogGroups();
      var childGroup = matchedGroup.OrderBy(t => t.Index).ToArray();
      foreach (var childEntry in childGroup)
      {
        if (childEntry == rootElem)
        {
          continue;
        }

        var lg = new LogGroup
        {
          Name = childEntry.Caption,
          DisplayNameSelector = @group => intent + selecter(@group.Values[0])

        };

        List<LogEntry> innerKids = new List<LogEntry> { childEntry };

        lg.SubGroups = this.GetSubGroups(groups, childEntry, innerKids, selecter, intent + DefaultIntent);

        kids.AddRange(innerKids);
        var descendants = new LogEntry[innerKids.Count + 1];
        descendants[0] = childEntry; // to show current root as first among children grouping.
        innerKids.ToArray().CopyTo(descendants, 1);
        lg.Values = descendants;
        if (!lgs.ContainsKey(lg.Name))
        {
          lgs.Add(lg.Name, lg);
        }
        else
        {
          var existing = lgs[lg.Name];
          var expanded = existing.Values.ToList();
          expanded.AddRange(lg.Values);
          existing.Values = expanded.ToArray();
        }

      }
      return lgs;
    }
  }
}