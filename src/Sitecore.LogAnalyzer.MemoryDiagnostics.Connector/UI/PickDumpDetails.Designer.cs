namespace Sitecore.DumpModule.Common.UI
{
  partial class PickDumpDetails
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
      this.DumpDlg = new System.Windows.Forms.OpenFileDialog();
      this.mscordDlg = new System.Windows.Forms.OpenFileDialog();
      this.dumpPathTxt = new System.Windows.Forms.TextBox();
      this.mscordPathTxt = new System.Windows.Forms.TextBox();
      this.okBtn = new System.Windows.Forms.Button();
      this.CancelBtn = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.showDumpDlg = new System.Windows.Forms.Button();
      this.ShowMscord = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // DumpDlg
      // 
      this.DumpDlg.DefaultExt = "*.dmp";
      this.DumpDlg.Filter = "Dump files | *.dmp";
      // 
      // mscordDlg
      // 
      this.mscordDlg.Filter = " System assemblies | *.dll";
      // 
      // dumpPathTxt
      // 
      this.dumpPathTxt.Location = new System.Drawing.Point(19, 71);
      this.dumpPathTxt.Name = "dumpPathTxt";
      this.dumpPathTxt.Size = new System.Drawing.Size(599, 20);
      this.dumpPathTxt.TabIndex = 0;
      this.dumpPathTxt.Text = "C:\\Users\\nmi\\Downloads\\7_OCT_2015_Memory usage Rule_d1\\w3wp.dmp";
      this.dumpPathTxt.TextChanged += new System.EventHandler(this.EnsureSelectedDataExists);
      // 
      // mscordPathTxt
      // 
      this.mscordPathTxt.Location = new System.Drawing.Point(19, 136);
      this.mscordPathTxt.Name = "mscordPathTxt";
      this.mscordPathTxt.Size = new System.Drawing.Size(599, 20);
      this.mscordPathTxt.TabIndex = 1;
      this.mscordPathTxt.Text = "C:\\Users\\nmi\\Downloads\\7_OCT_2015_Memory usage Rule_d1\\mscordacwks.dll";
      this.mscordPathTxt.TextChanged += new System.EventHandler(this.EnsureSelectedDataExists);
      // 
      // okBtn
      // 
      this.okBtn.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.okBtn.Location = new System.Drawing.Point(54, 178);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new System.Drawing.Size(249, 41);
      this.okBtn.TabIndex = 2;
      this.okBtn.Text = "Continue";
      this.okBtn.UseVisualStyleBackColor = true;
      this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
      // 
      // CancelBtn
      // 
      this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.CancelBtn.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CancelBtn.Location = new System.Drawing.Point(365, 178);
      this.CancelBtn.Name = "CancelBtn";
      this.CancelBtn.Size = new System.Drawing.Size(227, 41);
      this.CancelBtn.TabIndex = 3;
      this.CancelBtn.Text = "Cancel";
      this.CancelBtn.UseVisualStyleBackColor = true;
      this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(16, 43);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(287, 19);
      this.label1.TabIndex = 4;
      this.label1.Text = "Please select memory dump file for processing";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(16, 108);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(539, 19);
      this.label2.TabIndex = 5;
      this.label2.Text = "Please pick path to mscordacwks assembly which enables parsing of memory shanpsho" +
    "t";
      // 
      // showDumpDlg
      // 
      this.showDumpDlg.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.showDumpDlg.Location = new System.Drawing.Point(624, 64);
      this.showDumpDlg.Name = "showDumpDlg";
      this.showDumpDlg.Size = new System.Drawing.Size(43, 30);
      this.showDumpDlg.TabIndex = 6;
      this.showDumpDlg.Text = "...";
      this.showDumpDlg.UseVisualStyleBackColor = true;
      this.showDumpDlg.Click += new System.EventHandler(this.showDumpDlg_Click);
      // 
      // ShowMscord
      // 
      this.ShowMscord.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ShowMscord.Location = new System.Drawing.Point(624, 129);
      this.ShowMscord.Name = "ShowMscord";
      this.ShowMscord.Size = new System.Drawing.Size(43, 31);
      this.ShowMscord.TabIndex = 7;
      this.ShowMscord.Text = "...";
      this.ShowMscord.UseVisualStyleBackColor = true;
      this.ShowMscord.Click += new System.EventHandler(this.ShowMscord_Click);
      // 
      // PickDumpDetails
      // 
      this.AcceptButton = this.okBtn;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.CancelButton = this.CancelBtn;
      this.ClientSize = new System.Drawing.Size(697, 274);
      this.Controls.Add(this.ShowMscord);
      this.Controls.Add(this.showDumpDlg);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.CancelBtn);
      this.Controls.Add(this.okBtn);
      this.Controls.Add(this.mscordPathTxt);
      this.Controls.Add(this.dumpPathTxt);
      this.Name = "PickDumpDetails";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Select Memory dump and assemblies needed for processing";
      this.Load += new System.EventHandler(this.PickDumpDetails_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.OpenFileDialog DumpDlg;
    private System.Windows.Forms.OpenFileDialog mscordDlg;
    protected System.Windows.Forms.TextBox dumpPathTxt;
    protected System.Windows.Forms.TextBox mscordPathTxt;
    protected System.Windows.Forms.Button okBtn;
    protected System.Windows.Forms.Button CancelBtn;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button showDumpDlg;
    private System.Windows.Forms.Button ShowMscord;
  }
}