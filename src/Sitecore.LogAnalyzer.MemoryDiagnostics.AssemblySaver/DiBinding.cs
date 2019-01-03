namespace Sitecore.LogAnalyzer.MemoryDiagnostics.AssemblySaver
{
  using Sitecore.LogAnalyzer.Parsing;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DependencyInjection;

  /// <summary>
  /// Bindings for memory dump analysis.
  /// </summary>
  public class DiBindings : BaseMemoryDumpAnalysisModule
  {
    protected override void RegisterDumpProcessorDependencies()
    {
      base.RegisterDumpProcessorDependencies();
      Rebind<ILogProcessor>().To<AssemblyFetchProcessor>().InSingletonScope();
    }
  }
}