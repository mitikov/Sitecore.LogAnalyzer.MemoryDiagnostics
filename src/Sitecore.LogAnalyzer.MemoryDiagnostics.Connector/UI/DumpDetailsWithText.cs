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
      this.InitializeComponent();
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
      Assert.IsNotNullOrEmpty(this.TextMask, "TextMask is not set.");
      this.DialogResult = DialogResult.OK;

      var inputText = this.additionalTxtInput.Text;

      var textForConnection = this.FormatTextByMask(inputText);

      this.FileConnection = new MemoryDumpFileBasedConnectionWithName(textForConnection, this.dumpPathTxt.Text, this.mscordPathTxt.Text);

      PickDumpDetails.prevConnection = this.FileConnection;

      this.Close();
    }

    protected virtual string FormatTextByMask(string inputText) => string.Format(this.TextMask, inputText);

  }                                                                          
}