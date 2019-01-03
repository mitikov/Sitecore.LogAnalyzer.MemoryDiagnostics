namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector
{
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;

  /// <summary>
  /// Allows to apply custom logic on <see cref="ClrObjLogEntry"/> via manipulations with <see cref="ClrObjLogEntry.Model"/> entry. 
  /// <para>Example: Set other text content.</para>
  /// <para>Remarks: Stock implementation does nothing by default.</para>
  /// </summary>
  public class LogEntryFieldInitializer : IInitLogEntryFields
  {
    /// <summary>
    /// The singleton of <see cref="LogEntryFieldInitializer"/> type, that does not do any object adjustments.
    /// </summary>
    public static readonly LogEntryFieldInitializer Singleton = new LogEntryFieldInitializer();

    public virtual void ApplyCustomLogicOnLogEntry(ClrObjLogEntry entry) { }
  }
}
