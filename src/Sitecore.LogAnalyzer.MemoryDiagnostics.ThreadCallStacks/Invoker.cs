namespace Sitecore.LogAnalyzer.MemoryDiagnostics.ThreadCallStacks
{
  using System.Drawing;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector;

  public class Invoker : AbstractDumpInvoker
  {
    public override Image Image => RunningRequests.Resources.Image;

    public override string Name => "Thread Callstacks";
  }
}