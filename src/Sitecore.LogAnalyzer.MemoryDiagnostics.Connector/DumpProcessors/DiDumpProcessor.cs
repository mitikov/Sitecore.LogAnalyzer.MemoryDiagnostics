namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DumpProcessors
{

  using Sitecore;
  using Sitecore.LogAnalyzer.Contexts;
  using Sitecore.LogAnalyzer.Managers;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.Interfaces;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.ObjectEnumeration;

  /// <summary>
  /// <see cref="AbstractDumpProcessor"/> implementation that gets all the dependencies using constructor injection.
  /// <para>Needs <see cref="ILogAnalyzerFacade"/>, <see cref="IEnumeratiorConnection"/>, <see cref="IClrObjectTransformator"/>.</para>
  /// <para>Based on <seealso cref="DumpProcessors.AbstractDumpProcessor"/></para>
  /// </summary>  
  public class DiDumpProcessor : AbstractDumpProcessor
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="DiDumpProcessor"/> class using constructor injection.
    /// </summary>
    /// <param name="logAnalyzerFacade">
    /// The Log Analyzer Facade (pair of <see cref="ICaptionManager"/>, and <see cref="IContextFactory"/>).
    /// </param>
    /// <param name="objectEnumerationConnection">
    /// Capable of transforming <see cref="IConnectionSettings"/> into enumeration of matching objects.
    /// </param>
    /// <param name="clrObjToLogEntryTransformProvider">
    /// The object to log entry transform provider.
    /// </param>
    public DiDumpProcessor(
      [NotNull] ILogAnalyzerFacade logAnalyzerFacade,
      IEnumeratiorConnection objectEnumerationConnection, 
      IClrObjectTransformator clrObjToLogEntryTransformProvider)
    {
      this.LogAnalyzerFacade = logAnalyzerFacade;

      this.ClrObjectEnumerator = objectEnumerationConnection;

      this.ClrObjToLogEntryTransformProvider = clrObjToLogEntryTransformProvider;
    }

    /// <summary>
    /// Gets the Log Analyzer Facade ( pair of <see cref="ICaptionManager"/>, and <see cref="IContextFactory"/>).
    /// </summary>
    /// <value>
    /// The log analyzer facade.
    /// </value>
    protected virtual ILogAnalyzerFacade LogAnalyzerFacade { get; }

    protected override sealed ICaptionManager CaptionManager => this.LogAnalyzerFacade.CaptionManager;

    protected override IEnumeratiorConnection ClrObjectEnumerator { get; }

    protected override sealed IContextFactory ContextFactory => this.LogAnalyzerFacade.ContextFactory;

    protected override IClrObjectTransformator ClrObjToLogEntryTransformProvider { get; }
  }
}
