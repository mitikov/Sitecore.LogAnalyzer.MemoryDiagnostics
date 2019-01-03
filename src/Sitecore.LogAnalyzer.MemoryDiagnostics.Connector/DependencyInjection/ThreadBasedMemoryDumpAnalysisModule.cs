using Microsoft.Diagnostics.Runtime;
using Sitecore.LogAnalyzer.Managers;
using Sitecore.LogAnalyzer.Parsing;
using Sitecore.MemoryDiagnostics.Interfaces;
using Sitecore.MemoryDiagnostics.ModelFilters;
using Sitecore.MemoryDiagnostics.ThreadFilters;
using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders;
using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DumpProcessors;
using SitecoreMemoryInspectionKit.Core.ClrHelpers;

namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DependencyInjection
{
  public class ThreadBasedMemoryDumpAnalysisModule : BaseMemoryDumpAnalysisModule
  {
    protected override void RegisterDumpProcessorDependencies()
    {
      base.RegisterDumpProcessorDependencies();

      Rebind<IFilter<ClrThread>>().To<NoThreadFiltering>().InSingletonScope();

      Rebind<IFilter<ClrObject>>().To<BlacklistObjectFilter>().InSingletonScope();

      Rebind<ILogProcessor>().To<ClrThreadDumpProcessor>().InSingletonScope();
    }

    protected override void RegisterLogAnalyzerSpecific()
    {
      base.RegisterLogAnalyzerSpecific();
      Rebind<ICaptionManager>().To<ClrObjCaptionManager>().InSingletonScope();
    }

    protected override void RegisterClrModelToLogEntryDependencies()
    {
      base.RegisterClrModelToLogEntryDependencies();
      Rebind<IClrObjectTransformator>().To<ExtractThreadStaticObjects>().InSingletonScope();
    }
  }
  }
