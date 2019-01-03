namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.LogEntryFieldInitialization
{
  using System;
  using Interfaces;
  using Sitecore.MemoryDiagnostics.Models.FallBack.SitecoreRelated;
  using Sitecore.LogAnalyzer.Attributes;
  using Sitecore.LogAnalyzer.Models;

  /// <summary>
  /// Sorts entries by fill level, and changes <see cref="Models.LogLevel"/> according to cache filled percent.
  /// </summary>
  public class ScCacheInitLogEntryFields : IInitLogEntryFields
  {
    /// <summary>
    /// Applies the custom logic on log entry after <see cref="ClrObjLogEntry.InitFldsFromModel" /> API called.
    /// </summary>
    /// <param name="entry">The entry.</param>
    public void ApplyCustomLogicOnLogEntry([NotNull]ClrObjLogEntry entry)
    {
      Assert.ArgumentNotNull(entry, nameof(entry));

      if (!(entry.Model is ScCache model))
      {
        return;
      }

      entry.LogDateTime = DateTime.Today.AddTicks(-(long)(TimeSpan.FromDays(1).Ticks * model.FilledPercent));
      var filled = model.FilledPercent * 100;
      if (filled > 90)
      {
        entry.Level = LogLevel.ERROR;
        return;
      }

      if (filled > 75)
      {
        entry.Level = LogLevel.WARN;
        return;
      }

      if (filled < 20)
      {
        entry.Level = LogLevel.DEBUG;
      }
    }
  }
}
