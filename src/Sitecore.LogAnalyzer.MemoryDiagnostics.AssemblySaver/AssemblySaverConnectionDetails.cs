namespace Sitecore.LogAnalyzer.MemoryDiagnostics.AssemblySaver
{ 
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ConnectionDetails;

  /// <summary>
  /// Connection string to memory dump file with extra folder to save exported assemblies to.
  /// </summary>
  public class AssemblySaverConnectionDetails : MemoryDumpConnectionDetails
  {
    #region Fields
    public readonly string FolderToSave;

    #endregion

    public AssemblySaverConnectionDetails(string pathToDump, string pathToMscord, string folderToSave) : base(pathToDump, pathToMscord)
    {
      FolderToSave = folderToSave;
    }
  }
}
