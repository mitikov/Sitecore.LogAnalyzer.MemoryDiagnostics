namespace Sitecore.LogAnalyzer.MemoryDiagnostics.RunningRequests
{
  using System.Web;
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.MemoryDiagnostics.ClrObjectsProviders;
  using Sitecore.MemoryDiagnostics.Interfaces;
  using Sitecore.MemoryDiagnostics.ThreadFilters;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DependencyInjection;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;

  public class DiBindings : ThreadBasedMemoryDumpAnalysisModule
  {
    protected override void RegisterDumpProcessorDependencies()
    {
      base.RegisterDumpProcessorDependencies();

      Rebind<IFilter<ClrThread>>().To<ThreadsWithHttpRequests>().InSingletonScope();

      Rebind<IFilter<ClrObject>>().To<FilteredObjectProviderByTypeName>().InSingletonScope().WithConstructorArgument("type", typeof(HttpContext));
    }

    protected override void RegisterClrModelToLogEntryDependencies()
    {
      base.RegisterClrModelToLogEntryDependencies();
      Rebind<IInitLogEntryFields>().To<AddRequestInfoToParentText>().InSingletonScope();
    }

  }
}