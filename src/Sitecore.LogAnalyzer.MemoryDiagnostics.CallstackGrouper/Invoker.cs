namespace Sitecore.LogAnalyzer.MemoryDiagnostics.CallstackGrouper
{
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector;

  public class Invoker : AbstractDumpInvoker
  {
    public override string Name => "Callstack grouper";
  }
}
