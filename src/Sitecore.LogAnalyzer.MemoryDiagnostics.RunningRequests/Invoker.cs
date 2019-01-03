namespace Sitecore.LogAnalyzer.MemoryDiagnostics.RunningRequests
{
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector;

  public class Invoker : AbstractDumpInvoker
  {
    public override string Name => "Running Requests";
  }
}