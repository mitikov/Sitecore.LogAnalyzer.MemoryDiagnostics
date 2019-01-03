namespace Sitecore.LogAnalyzer.MemoryDiagnostics.SqlCommandViewer
{
  using System.Data.SqlClient;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DependencyInjection;
  using Sitecore.MemoryDiagnostics.ClrObjectsProviders;
  using Sitecore.MemoryDiagnostics.ClrObjectsProviders.Base;

  /// <summary>
  /// Extracts <see cref="SqlCommand"/> objects from heap and magically prints them!
  /// </summary>
  public class ExistingSqlCommandDiBindings : HeapBasedMemoryDumpAnalysisModule
  { 
    protected override void RegisterObjectEnumeration()
    {
      base.RegisterObjectEnumeration();
      Rebind<IFilteredObjectsProvider>().To<FilteredObjectProviderByTypeName>().InSingletonScope()
        .WithConstructorArgument("typeName", typeof(SqlCommand).FullName);      
    }
  }
}