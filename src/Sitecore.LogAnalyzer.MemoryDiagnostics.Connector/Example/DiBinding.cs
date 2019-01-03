using Sitecore.MemoryDiagnostics.ClrObjectEnumerators;
using Sitecore.MemoryDiagnostics.ClrObjectsProviders.Base;
using Sitecore.LogAnalyzer.Managers;

namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector
{
  using Sitecore.MemoryDiagnostics.ClrObjectsProviders;
  using Sitecore.MemoryDiagnostics.Facades.ObjectEnumeration;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DependencyInjection;

  /// <summary>
  /// Bindings for memory dump analysis.
  /// </summary>
  public class DiBindings : BaseMemoryDumpAnalysisModule
  {
    protected override void RegisterLogAnalyzerSpecific()
    {
      base.RegisterLogAnalyzerSpecific();

      Rebind<ICaptionManager>().To<ClrObjCaptionManager>().InSingletonScope();
    }

    protected override void RegisterObjectEnumeration()
    {
      base.RegisterObjectEnumeration();

      Rebind<IEnumerateClrObjectsFromClrRuntime>().To<HeapBasedClrObjectEnumerator>().InSingletonScope();

      Rebind<IFilteredObjectsProvider>()
        .To<FilteredObjectProviderByTypeName>()
        .InSingletonScope()
        .WithConstructorArgument("typeName", "Sitecore.Caching.Cache");

      Rebind<IObjectEnumerationFacade>().To<DefaultObjectEnumerationFacade>().InSingletonScope();
    }
  }
}