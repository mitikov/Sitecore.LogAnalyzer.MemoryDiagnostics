namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces
{
  using Sitecore.LogAnalyzer.Attributes;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.LogEntryFieldInitialization;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders;

  /// <summary>
  /// Allows to apply custom logic on <see cref="ClrObjLogEntry"/> via manipulations with <see cref="ClrObjLogEntry.Model"/> entry. 
  /// <para>Called after <see cref="ClrObjectTranformProvider"/> resolved instance and <see cref="ClrObjLogEntry.InitFldsFromModel"/> execution.</para>
  /// <para>Used by <see cref="ClrObjectTranformProvider"/></para>
  /// <para>
  ///   <example>Example: Set different <see cref="LogLevel"/> depending on cache fill level (see <see cref="ScCacheInitLogEntryFields"/> )</example>
  /// </para>  
  /// </summary>  
  public interface IInitLogEntryFields
  {
    /// <summary>
    /// Applies the custom logic on log entry after <see cref="ClrObjLogEntry.InitFldsFromModel"/> API called.
    /// </summary>
    /// <param name="entry">The entry.</param>
    void ApplyCustomLogicOnLogEntry([NotNull]ClrObjLogEntry entry);
  }
}