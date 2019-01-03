namespace Sitecore.DumpModule.Common.UI
{
  using System;
  using System.IO;
  using System.Windows.Forms;
  using Sitecore.MemoryDiagnostics.ConnectionDetails;
  using Sitecore.MemoryDiagnostics.SourceFactories;
  using Sitecore.LogAnalyzer.Settings;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ConnectionDetails;
  using Sitecore.LogAnalyzer;

  public partial class PickDumpDetails : Form
  {
    protected static IConnectionSettings prevConnection;

    public IConnectionSettings FileConnection { get; protected set; }

    protected bool msCordSetManually;

    public PickDumpDetails()
    {
      InitializeComponent();
    }

    protected virtual void okBtn_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;

      FileConnection = new MemoryDumpConnectionDetails(dumpPathTxt.Text, mscordPathTxt.Text);
      prevConnection = FileConnection;
      Close();
    }


    protected virtual void EnsureSelectedDataExists(object sender, EventArgs e)
    {
      okBtn.Enabled = SelectedDataExists();
    }

    protected virtual bool SelectedDataExists()
    {
      return File.Exists(mscordPathTxt.Text) && File.Exists(dumpPathTxt.Text);      
    }


    private void CancelBtn_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }


    private void showDumpDlg_Click(object sender, EventArgs e)
    {
      if (DumpDlg.ShowDialog() == DialogResult.OK)
      {
        dumpPathTxt.Text = DumpDlg.FileName;
        string pathToMscord;
        if (MDClrRuntimeFactory.TryGetMscordacPath(dumpPathTxt.Text, out pathToMscord))
        {
          mscordPathTxt.Text = pathToMscord;
        }
        else
        {
          if (msCordSetManually)
          {
            return;
          }

          try
          {
            mscordPathTxt.Text = FindMsCordInSameFolder(new FileInfo(DumpDlg.FileName).Directory);
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
      Assert.ArgumentNotNull(directory, nameof(directory));
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
      if (mscordDlg.ShowDialog() == DialogResult.OK)
      {
        mscordPathTxt.Text = mscordDlg.FileName;
        msCordSetManually = true;
      }
    }

    private void PickDumpDetails_Load(object sender, EventArgs e)
    {
      var connection = prevConnection as MDFileConnection;
      if (connection == null)
      {
        return;
      }

      dumpPathTxt.Text = connection.PathToDump;
      mscordPathTxt.Text = connection.PathToMsCorDacwks;
    }
  }
}