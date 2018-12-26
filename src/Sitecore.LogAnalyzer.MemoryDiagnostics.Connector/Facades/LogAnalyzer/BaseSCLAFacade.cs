namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.LogAnalyzer
{
  using JetBrains.Annotations;
  using Sitecore.LogAnalyzer.Contexts;
  using Sitecore.LogAnalyzer.Managers;

  /// <summary>
  ///  Provides DI-based facade for Log Analyzer view.
  /// <para>A pair of <see cref="ICaptionManager"/> and <see cref="IContextFactory"/>.</para> 
  /// </summary>
  /// <seealso cref="Facades.ILogAnalyzerFacade" />
  public class BaseSCLAFacade : ILogAnalyzerFacade
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSCLAFacade"/> class.
    /// </summary>
    /// <param name="captionManager">The caption manager.</param>
    /// <param name="contextFactory">The context factory.</param>
    public BaseSCLAFacade([NotNull]ICaptionManager captionManager, [NotNull]IContextFactory contextFactory)
    {
      this.CaptionManager = captionManager;
      this.ContextFactory = contextFactory;
    }

    public virtual ICaptionManager CaptionManager { get; }

    public virtual IContextFactory ContextFactory { get; }
  }
}
