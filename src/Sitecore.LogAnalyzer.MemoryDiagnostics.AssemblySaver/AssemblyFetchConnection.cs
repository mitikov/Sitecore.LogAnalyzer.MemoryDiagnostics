namespace Sitecore.LogAnalyzer.MemoryDiagnostics.AssemblySaver
{
  using Sitecore.LogAnalyzer;

  public class AssemblyFetchConnection : AssemblySaverConnectionDetails
  {
    public AssemblyFetchConnection(string folderToSave, bool includeDotNet, string pathToDump, string pathToMscord) : base(pathToDump, pathToMscord, folderToSave)
    {
      Assert.ArgumentNotNullOrEmpty(folderToSave, nameof(folderToSave));
      IncludeDotNet = includeDotNet;
    }

    public virtual bool IncludeDotNet { get; }
  }
}