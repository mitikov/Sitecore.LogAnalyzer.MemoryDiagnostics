namespace Sitecore.LogAnalyzer.MemoryDiagnostics.ModelViewer.DumpProcessors
{
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.LogAnalyzer;
  using Sitecore.MemoryDiagnostics.Attributes;
  using Sitecore.MemoryDiagnostics.ClrObjectsProviders;
  using Sitecore.MemoryDiagnostics.Facades.ObjectEnumeration;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ConnectionDetails;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DumpProcessors;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.ObjectEnumeration;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.ModelViewer.ConnectionDetails;

  /// <summary>
  /// Outputs clrObjects of only one type.
  /// <para><see cref="MemoryDumpConnectionWithType"/> carries model type that will be picked, while source type is taken from model class <see cref="ModelMappingAttribute"/> value.</para>
  /// </summary>
  public class SingleTypeDumpProcessor : DiDumpProcessor
  {
    private IEnumeratiorConnection clrObjectEnumerationFacade;

    public SingleTypeDumpProcessor(ILogAnalyzerFacade logAnalyzerFacade, IClrObjectTransformator clrObjToLogEntryTransformProvider) : base(logAnalyzerFacade, new MdConnectionNoEnumerator(), clrObjToLogEntryTransformProvider)
    {
    }

    protected override IEnumeratiorConnection ClrObjectEnumerator => clrObjectEnumerationFacade;

    /// <summary>
    /// Arrange
    /// </summary>
    /// <param name="connection"></param>
    /// <returns></returns>
    protected override ClrRuntime BuildRuntime(MemoryDumpConnectionDetails connection)
    {
      Assert.OfType<MemoryDumpConnectionWithType>(connection, typeof(MemoryDumpConnectionWithType), nameof(connection));

      var connectionDetails = connection as MemoryDumpConnectionWithType;

      clrObjectEnumerationFacade = ConstructFilteredObjectsProvider(connectionDetails);

      return base.BuildRuntime(connection);
    }

    /// <summary>
    /// Locates <see cref="ModelMappingAttribute.TypeToMapOn"/> from given model in <paramref name="connectionDetails"/> and setups heap enumeration for located type only.
    /// </summary>
    /// <param name="connectionDetails">Carry model type decorated with <see cref="ModelMappingAttribute"/> that defines objects of which type to be loaded from memory.</param>
    /// <returns></returns>
    protected virtual IEnumeratiorConnection ConstructFilteredObjectsProvider(MemoryDumpConnectionWithType connectionDetails)
    {
      var modelOfTypeToEnumerate = connectionDetails.ModelsType;
      var targetTypeName = ModelMappingAttribute.GetTypeToMapOn(modelOfTypeToEnumerate, assert: true);
      return 
        new MdBasedEnumerationConnection(
        new HeapBasedFacadeObjectEnumerator(new FilteredObjectProviderByTypeName(targetTypeName)));
    }

  }
}
