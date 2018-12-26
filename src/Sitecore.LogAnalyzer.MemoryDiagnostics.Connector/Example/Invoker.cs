namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector
{
  using System.Drawing;  

  public class Invoker : AbstractDumpInvoker
  {
    public override Image Image => Resources.Image;

    public override string Name => "Sample Invoker";
  }
}