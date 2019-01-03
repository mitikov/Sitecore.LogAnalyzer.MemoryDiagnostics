namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.UI
{
  using System;
  using System.Windows.Forms;
  using Sitecore.MemoryDiagnostics.ConnectionDetails;
  using Sitecore.DumpModule.Common.UI;
  using Sitecore.LogAnalyzer;

  /// <summary>
  ///   The cache name.
  /// </summary>
  public partial class DumpDetailsWithText : PickDumpDetails
  {

    public DumpDetailsWithText()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Gets the name of the cache.
    /// </summary>
    /// <value>
    /// The name of the cache.
    /// </value>
    public virtual string TextMask { get; set; }

    protected override void okBtn_Click(object sender, EventArgs e)
    {
      Assert.IsNotNullOrEmpty(TextMask, "TextMask is not set.");
      DialogResult = DialogResult.OK;

      var inputText = additionalTxtInput.Text;

      var textForConnection = FormatTextByMask(inputText);

      FileConnection = new MemoryDumpFileBasedConnectionWithName(textForConnection, dumpPathTxt.Text, mscordPathTxt.Text);

      PickDumpDetails.prevConnection = FileConnection;

      Close();
    }

    protected virtual string FormatTextByMask(string inputText) => string.Format(TextMask, inputText);

  }                                                                          
}