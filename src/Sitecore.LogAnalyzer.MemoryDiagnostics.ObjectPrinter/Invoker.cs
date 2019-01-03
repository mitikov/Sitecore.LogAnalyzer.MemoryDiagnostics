namespace Sitecore.LogAnalyzer.MemoryDiagnostics.ObjectPrinter
{
  using System.Drawing;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector;

  public class Invoker : AbstractDumpInvoker
  {
    public static readonly string ObjectPrinterModuleName = "Object printer";

    public Invoker() => PickDumpDetailsForm = new PickObjectToPrint();

    public override Image Image => Properties.Resources.Emblem;

    public override string Name => ObjectPrinterModuleName; 
  }
}