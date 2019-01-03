namespace Sitecore.LogAnalyzer.MemoryDiagnostics.AssemblySaver
{
  using System;
  using System.IO;
  using System.Windows.Forms;
  using Sitecore.DumpModule.Common.UI;

  public partial class DumpDetailsWithFolder : PickDumpDetails
  {
    public DumpDetailsWithFolder()
    {
      InitializeComponent();
      okBtn.Enabled = false;
    }

    protected override bool SelectedDataExists()
    {
      return base.SelectedDataExists() && Directory.Exists(fldPath.Text);
    }

    protected virtual void button1_Click(object sender, System.EventArgs e)
    {
      if (FldBrowseDlg.ShowDialog() == DialogResult.OK)
      {
        fldPath.Text = FldBrowseDlg.SelectedPath;
      }

      EnsureSelectedDataExists(sender, e);
    }

    protected override void okBtn_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;

      FileConnection = new AssemblyFetchConnection(fldPath.Text, IncludeDotNet.Checked, dumpPathTxt.Text, mscordPathTxt.Text);
      prevConnection = FileConnection;
      Close();
    }
  }
}
