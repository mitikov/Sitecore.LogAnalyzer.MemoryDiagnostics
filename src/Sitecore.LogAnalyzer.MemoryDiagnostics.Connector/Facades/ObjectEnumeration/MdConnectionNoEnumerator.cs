namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.ObjectEnumeration
{
  using Sitecore.MemoryDiagnostics.Facades.ObjectEnumeration;

  /// <summary>
  /// Provides file-based connection to memory snapshot only.
  /// <para>Carries empty enumerator.</para>
  /// </summary>
  /// <seealso cref="ObjectEnumeration.MdBasedEnumerationConnection" />
  public class MdConnectionNoEnumerator : MdBasedEnumerationConnection
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MdConnectionNoEnumerator"/> class.
    /// <para>Carries no </para>
    /// </summary>
    public MdConnectionNoEnumerator() : base(new EmptyEnumeratorFacade())
    {
    }
  }
}
