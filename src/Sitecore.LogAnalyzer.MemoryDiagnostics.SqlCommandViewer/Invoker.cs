namespace Sitecore.LogAnalyzer.MemoryDiagnostics.SqlCommandViewer
{
  using System.Drawing;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector;

  public class Invoker : AbstractDumpInvoker
  {
    public override Image Image => Properties.Resources.Picture;

    public override string Name => "SQL commands viewer";
  }
}