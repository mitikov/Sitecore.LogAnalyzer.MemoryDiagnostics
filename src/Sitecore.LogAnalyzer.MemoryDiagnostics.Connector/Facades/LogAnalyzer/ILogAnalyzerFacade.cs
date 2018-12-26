namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades
{
  using Sitecore.LogAnalyzer.Contexts;
  using Sitecore.LogAnalyzer.Managers;

  /// <summary>
  ///  Provides <see cref="ICaptionManager"/> and <see cref="IContextFactory"/> pair.
  /// <para>Objects are needed for producing, grouping, and locating entries within Log Analyzer UI.</para> 
  /// </summary>
  public interface ILogAnalyzerFacade
  {
    /// <summary>
    /// Gets the caption manager.
    /// </summary>
    /// <value>
    /// The caption manager.
    /// </value>
    ICaptionManager CaptionManager { get; }

    /// <summary>
    /// Gets the context factory.
    /// </summary>
    /// <value>
    /// The context factory.
    /// </value>
    IContextFactory ContextFactory { get; }
  }
}
