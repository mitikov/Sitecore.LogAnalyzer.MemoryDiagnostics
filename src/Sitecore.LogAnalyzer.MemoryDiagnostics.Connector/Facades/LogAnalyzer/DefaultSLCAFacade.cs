namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.LogAnalyzer
{
  using Sitecore.LogAnalyzer.Managers;

  /// <summary>
  /// A default implementation with <see cref="CaptionManager"/> and <see cref="EmptyContextFactory"/> types.
  /// </summary>
  /// <seealso cref="Facades.LogAnalyzer.BaseSCLAFacade" />
  public class DefaultSLCAFacade : BaseSCLAFacade
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultSLCAFacade"/> class.
    /// </summary>
    public DefaultSLCAFacade() : base(new CaptionManager(), EmptyContextFactory.Instance)
    {
    }
  }
}
