namespace Sitecore.LogAnalyzer.MemoryDiagnostics.ModelViewer
{
  using System.Drawing;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector;

  public class Invoker : AbstractDumpInvoker
  {
    #region Fields

    public static readonly string DuplicateObjectFinderModuleName = "Model viewer";

    #endregion

    #region Constructors

    public Invoker()
    {
      PickDumpDetailsForm = new PickObjectToPrint();
    }

    #endregion

    #region Public properties

    public override Image Image => Properties.Resources.Emblem;

    public override string Name => DuplicateObjectFinderModuleName;

    #endregion
  }
}