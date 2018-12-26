namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ConnectionDetails
{
  using Sitecore.LogAnalyzer.Settings;
  using Sitecore.MemoryDiagnostics.ConnectionDetails;

  /// <summary>
  /// A composition of <see cref="IConnectionSettings"/> (Log Analyzer), and <see cref="MDFileConnection"/> (Memory Diagnostic) connections.
  /// <para>Allows to use <see cref="Sitecore.MemoryDiagnostics"/> engine, and <see cref="Sitecore.LogAnalyzer"/> UI.</para>
  /// </summary>
  /// <seealso cref="Sitecore.MemoryDiagnostics.ConnectionDetails.MDFileConnection" />
  /// <seealso cref="Sitecore.LogAnalyzer.Settings.IConnectionSettings" />
  public class MemoryDumpConnectionDetails : MDFileConnection, IConnectionSettings
  {
    public MemoryDumpConnectionDetails([NotNull]string pathToDump, string pathToMsCorDacwks) : base(pathToDump, pathToMsCorDacwks)
    {
    }
  }
}
