namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DumpProcessors
{

  using Sitecore.MemoryDiagnostics.Interfaces;
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.LogAnalyzer;
  using Sitecore.LogAnalyzer.Managers;
  using Sitecore.LogAnalyzer.Settings;
  using Sitecore.LogAnalyzer.States;
  using Sitecore.MemoryDiagnostics.ClrObjectsProviders.Base;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.ObjectEnumeration;

  /// <summary>
  /// Extracts text filter from <see cref="IConnectionSettings"/> via <see cref="INameProvider"/>,
  /// <para>and forwards it to <see cref="IFilteredObjectsProvider"/> in case it implements <see cref="INameProvider"/>.</para> 
  /// </summary>
  /// <seealso cref="MD.JulyPowered.DumpProcessors.DiDumpProcessor" />
  public class NamedFilterDrivenProcessor : DiDumpProcessor
  {
    #region fields    

    #endregion

    #region constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="NamedFilterDrivenProcessor"/> class.
    /// </summary>
    /// <param name="logAnalyzerFacade">The Log Analyzer Facade (pair of <see cref="ICaptionManager" />, and <see cref="IContextFactory" />).</param>
    /// <param name="objectEnumerationConnection">Capable of transforming <see cref="IConnectionSettings" /> into enumeration of matching objects.</param>
    /// <param name="clrObjToLogEntryTransformProvider">The object to log entry transform provider.</param>
    public NamedFilterDrivenProcessor(ILogAnalyzerFacade logAnalyzerFacade, IEnumeratiorConnection objectEnumerationConnection, IClrObjectTransformator clrObjToLogEntryTransformProvider) : base(logAnalyzerFacade, objectEnumerationConnection, clrObjToLogEntryTransformProvider)
    {
      ClrObjectEnumerator = objectEnumerationConnection;
    }

    #endregion

    #region Properties    

    protected override IEnumeratiorConnection ClrObjectEnumerator { get; }

    #endregion

    protected override void OnPreBuildParsingResult(ClrRuntime clrRuntime, ProcessContext context)
    {
      var connection = context.Settings.ConnectionSettings;

      Assert.OfType<INameProvider>(connection, typeof(INameProvider), nameof(context.Settings.ConnectionSettings));

      var filterName = GetfilterName(connection);

      var filterObjProvider = ClrObjectEnumerator.FilteredObjectsProvider;

      Assert.OfType<INameProvider>(filterObjProvider, typeof(INameProvider), nameof(filterObjProvider));

      var filterObjNameProvider = filterObjProvider as INameProvider;

      filterObjNameProvider.Name = filterName;
    }

    /// <summary>
    /// Gets the name of the cache from provided connection.
    /// </summary>
    /// <param name="connection">The connection.</param>
    /// <returns>name extracted from connection details.</returns>
    protected virtual string GetfilterName(IConnectionSettings connection)
    {
      return ((INameProvider)connection).Name;
    }
  }
}
