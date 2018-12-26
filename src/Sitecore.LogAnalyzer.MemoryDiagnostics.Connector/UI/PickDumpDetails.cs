namespace Sitecore.DumpModule.Common.UI
{
  using System;
  using System.IO;
  using System.Windows.Forms;
  using Sitecore.MemoryDiagnostics.ConnectionDetails;
  using Sitecore.MemoryDiagnostics.SourceFactories;
  using Sitecore.Diagnostics;
  using Sitecore.LogAnalyzer.Settings;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ConnectionDetails;

  public partial class PickDumpDetails : Form
  {
    protected static IConnectionSettings prevConnection;

    public IConnectionSettings FileConnection { get; protected set; }

    protected bool msCordSetManually;

    public PickDumpDetails()
    {
      this.InitializeComponent();
    }

    protected virtual void okBtn_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;

      this.FileConnection = new MemoryDumpConnectionDetails(dumpPathTxt.Text, mscordPathTxt.Text);
      prevConnection = this.FileConnection;
      this.Close();
    }


    protected virtual void EnsureSelectedDataExists(object sender, EventArgs e)
    {
      this.okBtn.Enabled = this.SelectedDataExists();
    }

    protected virtual bool SelectedDataExists()
    {
      return File.Exists(this.mscordPathTxt.Text) && File.Exists(this.dumpPathTxt.Text);      
    }


    private void CancelBtn_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }


    private void showDumpDlg_Click(object sender, EventArgs e)
    {
      if (this.DumpDlg.ShowDialog() == DialogResult.OK)
      {
        this.dumpPathTxt.Text = this.DumpDlg.FileName;
        string pathToMscord;
        if (MDClrRuntimeFactory.TryGetMscordacPath(this.dumpPathTxt.Text, out pathToMscord))
        {
          this.mscordPathTxt.Text = pathToMscord;
        }
        else
        {
          if (this.msCordSetManually)
          {
            return;
          }

          try
          {
            this.mscordPathTxt.Text = this.FindMsCordInSameFolder(new FileInfo(this.DumpDlg.FileName).Directory);
          }
          catch (Exception)
          {
            // Silent catch.  
          }
        }
      }
    }

    [NotNull]
    protected virtual string FindMsCordInSameFolder([NotNull] DirectoryInfo directory)
    {
      Assert.ArgumentNotNull(directory, "directory");
      var files = directory.GetFiles("mscordacwks*.dll");
      if (files.Length == 0)
      {
        return directory.FullName + "\\mscordacwks.dll";
      }
      else
      {
        return files[0].FullName;
      }
    }
    private void ShowMscord_Click(object sender, EventArgs e)
    {
      if (this.mscordDlg.ShowDialog() == DialogResult.OK)
      {
        this.mscordPathTxt.Text = this.mscordDlg.FileName;
        this.msCordSetManually = true;
      }
    }

    private void PickDumpDetails_Load(object sender, EventArgs e)
    {
      var connection = prevConnection as MDFileConnection;
      if (connection == null)
      {
        return;
      }

      this.dumpPathTxt.Text = connection.PathToDump;
      this.mscordPathTxt.Text = connection.PathToMsCorDacwks;
    }
  }
}