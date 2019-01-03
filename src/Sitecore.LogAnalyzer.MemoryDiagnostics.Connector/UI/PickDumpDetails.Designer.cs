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
      DumpDlg = new System.Windows.Forms.OpenFileDialog();
      mscordDlg = new System.Windows.Forms.OpenFileDialog();
      dumpPathTxt = new System.Windows.Forms.TextBox();
      mscordPathTxt = new System.Windows.Forms.TextBox();
      okBtn = new System.Windows.Forms.Button();
      CancelBtn = new System.Windows.Forms.Button();
      label1 = new System.Windows.Forms.Label();
      label2 = new System.Windows.Forms.Label();
      showDumpDlg = new System.Windows.Forms.Button();
      ShowMscord = new System.Windows.Forms.Button();
      SuspendLayout();
      // 
      // DumpDlg
      // 
      DumpDlg.DefaultExt = "*.dmp";
      DumpDlg.Filter = "Dump files | *.dmp";
      // 
      // mscordDlg
      // 
      mscordDlg.Filter = " System assemblies | *.dll";
      // 
      // dumpPathTxt
      // 
      dumpPathTxt.Location = new System.Drawing.Point(19, 71);
      dumpPathTxt.Name = "dumpPathTxt";
      dumpPathTxt.Size = new System.Drawing.Size(599, 20);
      dumpPathTxt.TabIndex = 0;
      dumpPathTxt.Text = "C:\\Users\\nmi\\Downloads\\7_OCT_2015_Memory usage Rule_d1\\w3wp.dmp";
      dumpPathTxt.TextChanged += new System.EventHandler(EnsureSelectedDataExists);
      // 
      // mscordPathTxt
      // 
      mscordPathTxt.Location = new System.Drawing.Point(19, 136);
      mscordPathTxt.Name = "mscordPathTxt";
      mscordPathTxt.Size = new System.Drawing.Size(599, 20);
      mscordPathTxt.TabIndex = 1;
      mscordPathTxt.Text = "C:\\Users\\nmi\\Downloads\\7_OCT_2015_Memory usage Rule_d1\\mscordacwks.dll";
      mscordPathTxt.TextChanged += new System.EventHandler(EnsureSelectedDataExists);
      // 
      // okBtn
      // 
      okBtn.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      okBtn.Location = new System.Drawing.Point(54, 178);
      okBtn.Name = "okBtn";
      okBtn.Size = new System.Drawing.Size(249, 41);
      okBtn.TabIndex = 2;
      okBtn.Text = "Continue";
      okBtn.UseVisualStyleBackColor = true;
      okBtn.Click += new System.EventHandler(okBtn_Click);
      // 
      // CancelBtn
      // 
      CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      CancelBtn.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      CancelBtn.Location = new System.Drawing.Point(365, 178);
      CancelBtn.Name = "CancelBtn";
      CancelBtn.Size = new System.Drawing.Size(227, 41);
      CancelBtn.TabIndex = 3;
      CancelBtn.Text = "Cancel";
      CancelBtn.UseVisualStyleBackColor = true;
      CancelBtn.Click += new System.EventHandler(CancelBtn_Click);
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      label1.Location = new System.Drawing.Point(16, 43);
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size(287, 19);
      label1.TabIndex = 4;
      label1.Text = "Please select memory dump file for processing";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      label2.Location = new System.Drawing.Point(16, 108);
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size(539, 19);
      label2.TabIndex = 5;
      label2.Text = "Please pick path to mscordacwks assembly which enables parsing of memory shanpsho" +
    "t";
      // 
      // showDumpDlg
      // 
      showDumpDlg.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      showDumpDlg.Location = new System.Drawing.Point(624, 64);
      showDumpDlg.Name = "showDumpDlg";
      showDumpDlg.Size = new System.Drawing.Size(43, 30);
      showDumpDlg.TabIndex = 6;
      showDumpDlg.Text = "...";
      showDumpDlg.UseVisualStyleBackColor = true;
      showDumpDlg.Click += new System.EventHandler(showDumpDlg_Click);
      // 
      // ShowMscord
      // 
      ShowMscord.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      ShowMscord.Location = new System.Drawing.Point(624, 129);
      ShowMscord.Name = "ShowMscord";
      ShowMscord.Size = new System.Drawing.Size(43, 31);
      ShowMscord.TabIndex = 7;
      ShowMscord.Text = "...";
      ShowMscord.UseVisualStyleBackColor = true;
      ShowMscord.Click += new System.EventHandler(ShowMscord_Click);
      // 
      // PickDumpDetails
      // 
      AcceptButton = okBtn;
      AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      BackColor = System.Drawing.SystemColors.Window;
      CancelButton = CancelBtn;
      ClientSize = new System.Drawing.Size(697, 274);
      Controls.Add(ShowMscord);
      Controls.Add(showDumpDlg);
      Controls.Add(label2);
      Controls.Add(label1);
      Controls.Add(CancelBtn);
      Controls.Add(okBtn);
      Controls.Add(mscordPathTxt);
      Controls.Add(dumpPathTxt);
      Name = "PickDumpDetails";
      StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      Text = "Select Memory dump and assemblies needed for processing";
      Load += new System.EventHandler(PickDumpDetails_Load);
      ResumeLayout(false);
      PerformLayout();

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