namespace Sitecore.LogAnalyzer.MemoryDiagnostics.AssemblySaver
{
  partial class DumpDetailsWithFolder
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      saveFolderSelectBtn = new System.Windows.Forms.Button();
      fldPath = new System.Windows.Forms.TextBox();
      FldBrowseDlg = new System.Windows.Forms.FolderBrowserDialog();
      IncludeDotNet = new System.Windows.Forms.CheckBox();
      SuspendLayout();
      // 
      // dumpPathTxt
      // 
      dumpPathTxt.Margin = new System.Windows.Forms.Padding(2);
      // 
      // mscordPathTxt
      // 
      mscordPathTxt.Margin = new System.Windows.Forms.Padding(2);
      // 
      // saveFolderSelectBtn
      // 
      saveFolderSelectBtn.Location = new System.Drawing.Point(597, 186);
      saveFolderSelectBtn.Margin = new System.Windows.Forms.Padding(2);
      saveFolderSelectBtn.Name = "saveFolderSelectBtn";
      saveFolderSelectBtn.Size = new System.Drawing.Size(89, 53);
      saveFolderSelectBtn.TabIndex = 8;
      saveFolderSelectBtn.Text = "SelectFolder";
      saveFolderSelectBtn.UseVisualStyleBackColor = true;
      saveFolderSelectBtn.Click += new System.EventHandler(button1_Click);
      // 
      // fldPath
      // 
      fldPath.Location = new System.Drawing.Point(54, 241);
      fldPath.Margin = new System.Windows.Forms.Padding(2);
      fldPath.Name = "fldPath";
      fldPath.Size = new System.Drawing.Size(495, 20);
      fldPath.TabIndex = 9;
      fldPath.Text = "C:\\Dumps\\";
      // 
      // FldBrowseDlg
      // 
      FldBrowseDlg.RootFolder = System.Environment.SpecialFolder.MyComputer;
      FldBrowseDlg.ShowNewFolderButton = false;
      // 
      // IncludeDotNet
      // 
      IncludeDotNet.AutoSize = true;
      IncludeDotNet.Location = new System.Drawing.Point(576, 245);
      IncludeDotNet.Name = "IncludeDotNet";
      IncludeDotNet.Size = new System.Drawing.Size(95, 17);
      IncludeDotNet.TabIndex = 10;
      IncludeDotNet.Text = "IncludeDotNet";
      IncludeDotNet.UseVisualStyleBackColor = true;
      // 
      // DumpDetailsWithFolder
      // 
      AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      ClientSize = new System.Drawing.Size(697, 274);
      Controls.Add(IncludeDotNet);
      Controls.Add(fldPath);
      Controls.Add(saveFolderSelectBtn);
      Margin = new System.Windows.Forms.Padding(2);
      Name = "DumpDetailsWithFolder";
      Text = "DumpDetailsWithFolder";
      Controls.SetChildIndex(okBtn, 0);
      Controls.SetChildIndex(CancelBtn, 0);
      Controls.SetChildIndex(dumpPathTxt, 0);
      Controls.SetChildIndex(mscordPathTxt, 0);
      Controls.SetChildIndex(saveFolderSelectBtn, 0);
      Controls.SetChildIndex(fldPath, 0);
      Controls.SetChildIndex(IncludeDotNet, 0);
      ResumeLayout(false);
      PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button saveFolderSelectBtn;
    private System.Windows.Forms.TextBox fldPath;
    protected System.Windows.Forms.FolderBrowserDialog FldBrowseDlg;
    private System.Windows.Forms.CheckBox IncludeDotNet;
  }
}