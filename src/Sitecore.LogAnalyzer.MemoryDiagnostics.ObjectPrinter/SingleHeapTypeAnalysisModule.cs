namespace Sitecore.LogAnalyzer.MemoryDiagnostics.ObjectPrinter
{
  using Sitecore.LogAnalyzer.Parsing;
  using DumpProcessors;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DependencyInjection;

  /// <summary>
  /// Configures <see cref="ILogProcessor"/> to be of <see cref="SingleTypeDumpProcessor"/>.
  /// <para>Will process objects of only one type taken from selected model.</para>
  /// </summary>
  public class SingleHeapTypeAnalysisModule : HeapBasedMemoryDumpAnalysisModule
  {
    protected override void RegisterDumpProcessorDependencies()
    {
      base.RegisterDumpProcessorDependencies();
      Rebind<ILogProcessor>().To<SingleTypeDumpProcessor>().InSingletonScope();
    }      
  }
}