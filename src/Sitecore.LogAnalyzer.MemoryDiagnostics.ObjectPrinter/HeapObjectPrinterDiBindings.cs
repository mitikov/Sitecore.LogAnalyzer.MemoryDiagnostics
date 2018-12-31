namespace Sitecore.LogAnalyzer.MemoryDiagnostics.ModelViewer
{
  using Sitecore.LogAnalyzer.Parsing;
  using DumpProcessors;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DependencyInjection;

  public class HeapObjectPrinterDiBindings : HeapBasedMemoryDumpAnalysisModule
  {
    protected override void RegisterDumpProcessorDependencies()
    {
      base.RegisterDumpProcessorDependencies();
      Rebind<ILogProcessor>().To<SingleTypeDumpProcessor>().InSingletonScope();
    }      
  }
}