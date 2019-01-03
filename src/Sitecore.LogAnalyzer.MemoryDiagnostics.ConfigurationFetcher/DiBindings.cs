namespace Sitecore.LogAnalyzer.MemoryDiagnostics.ConfigurationFetcher
{
  using Sitecore.LogAnalyzer.Parsing;
  using Sitecore.MemoryDiagnostics.ClrObjectEnumerators;
  using Sitecore.MemoryDiagnostics.ClrObjectEnumerators.Samples;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DependencyInjection;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DumpProcessors;

  public class DiBindings : BaseMemoryDumpAnalysisModule
  {
    public override void Load()
    {
      base.Load();
      Rebind<ILogProcessor>().To<DiDumpProcessor>().InSingletonScope();      
    }

    protected override void RegisterObjectEnumeration()
    {
      base.RegisterObjectEnumeration();
      Rebind<IEnumerateClrObjectsFromClrRuntime>()
          .To<ConfigurationProvider>()
          .InSingletonScope();
    }
  }
}