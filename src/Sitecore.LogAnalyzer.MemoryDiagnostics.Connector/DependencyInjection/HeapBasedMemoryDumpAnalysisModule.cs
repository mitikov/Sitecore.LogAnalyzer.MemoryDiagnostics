namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DependencyInjection
{
  using Sitecore.MemoryDiagnostics.ClrObjectEnumerators;
  using Sitecore.MemoryDiagnostics.ClrObjectsProviders.Base;
  using Sitecore.MemoryDiagnostics.Facades.ObjectEnumeration;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;

  /// <summary>
  /// <para>Registers heap as object enumerator.</para>
  /// </summary>
  /// <seealso cref="BaseMemoryDumpAnalysisModule" />
  public abstract class HeapBasedMemoryDumpAnalysisModule : BaseMemoryDumpAnalysisModule
  {
    /// <summary>
    /// <para>Registers heap as object enumerator.</para>
    /// Registers the <see cref="ClrObject" /> enumeration part. <see cref="IEnumerateClrObjectsFromClrRuntime" /> and <see cref="IFilteredObjectsProvider" />.
    /// <para>Groups these into <see cref="IObjectEnumerationFacade" />. </para>
    /// </summary>
    protected override void RegisterObjectEnumeration()
    {
      base.RegisterObjectEnumeration();

      this.Rebind<IEnumerateClrObjectsFromClrRuntime>().To<HeapBasedClrObjectEnumerator>().InSingletonScope();
    }
  }
}
