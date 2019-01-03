namespace Sitecore.LogAnalyzer.MemoryDiagnostics.RunningRequests
{
  using System.Drawing;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector;

  public class Invoker : AbstractDumpInvoker
  {
    public override Image Image => Resources.Image;

    public override string Name => "Running Requests";
  }
}