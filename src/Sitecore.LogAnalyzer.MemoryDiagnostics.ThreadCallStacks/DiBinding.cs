namespace Sitecore.LogAnalyzer.MemoryDiagnostics.ThreadCallStacks
{
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.MemoryDiagnostics.Helpers;
  using Sitecore.MemoryDiagnostics.Interfaces;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DependencyInjection;

  public class DiBindings : ThreadBasedMemoryDumpAnalysisModule
  {
    protected override void RegisterDumpProcessorDependencies()
    {
      base.RegisterDumpProcessorDependencies();

      Rebind<IFilter<ClrThread>>().To<UserThreadsFilter>().InSingletonScope();
    }
  }
}