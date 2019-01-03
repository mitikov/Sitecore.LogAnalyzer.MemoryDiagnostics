namespace Sitecore.LogAnalyzer.MemoryDiagnostics.AssemblySaver
{
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector;

  /// <summary>
  /// Saves assemblies from memory snapshot into file system.
  /// </summary>
  public class Invoker : AbstractDumpInvoker
  {
    public override string Name => "Save assemblies";

    public Invoker()
    {
      PickDumpDetailsForm = new DumpDetailsWithFolder();
    }
  }
}
