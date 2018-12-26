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

      this.Rebind<IFilter<ClrThread>>().To<NoThreadFiltering>().InSingletonScope();

      this.Rebind<IFilter<ClrObject>>().To<BlacklistObjectFilter>().InSingletonScope();

      this.Rebind<ILogProcessor>().To<ClrThreadDumpProcessor>().InSingletonScope();
    }

    protected override void RegisterLogAnalyzerSpecific()
    {
      base.RegisterLogAnalyzerSpecific();
      this.Rebind<ICaptionManager>().To<ClrObjCaptionManager>().InSingletonScope();
    }

    protected override void RegisterClrModelToLogEntryDependencies()
    {
      base.RegisterClrModelToLogEntryDependencies();
      this.Rebind<IClrObjectTransformator>().To<ExtractThreadStaticObjects>().InSingletonScope();
    }
  }
  }
