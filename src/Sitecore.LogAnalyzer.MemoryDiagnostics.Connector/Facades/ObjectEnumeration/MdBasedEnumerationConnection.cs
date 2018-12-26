namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.ObjectEnumeration
{
  using Sitecore.MemoryDiagnostics.Facades.ObjectEnumeration;
  using Sitecore.MemoryDiagnostics.SourceFactories;

  public class MdBasedEnumerationConnection : BaseObjectEnumerationConnectionDetails
  {
    public MdBasedEnumerationConnection(IObjectEnumerationFacade objectEnumeration) : base(MDClrRuntimeFactory.Instance, objectEnumeration)
    {
    }
  }
}
