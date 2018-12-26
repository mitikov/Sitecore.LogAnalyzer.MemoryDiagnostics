using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector;

namespace Sitecore.LogAnalyzer.MemoryDiagnostics.CallstackGrouper
{
  public class Invoker : AbstractDumpInvoker
  {
    public override string Name => "Callstack grouper";
  }
}
