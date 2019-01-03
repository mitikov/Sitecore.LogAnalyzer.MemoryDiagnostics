namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.UI
{
  partial class DumpDetailsWithText
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
      additionalTxtInput = new System.Windows.Forms.TextBox();
      additionalInputComments = new System.Windows.Forms.Label();
      SuspendLayout();
      // 
      // okBtn
      // 
      okBtn.Click += new System.EventHandler(okBtn_Click);
      // 
      // additionalTxtInput
      // 
      additionalTxtInput.Font = new System.Drawing.Font("Times New Roman", 12F);
      additionalTxtInput.Location = new System.Drawing.Point(54, 270);
      additionalTxtInput.Name = "additionalTxtInput";
      additionalTxtInput.Size = new System.Drawing.Size(100, 26);
      additionalTxtInput.TabIndex = 8;
      additionalTxtInput.Text = "web";
      // 
      // additionalInputComments
      // 
      additionalInputComments.AutoSize = true;
      additionalInputComments.Font = new System.Drawing.Font("Times New Roman", 12F);
      additionalInputComments.Location = new System.Drawing.Point(16, 239);
      additionalInputComments.Name = "additionalInputComments";
      additionalInputComments.Size = new System.Drawing.Size(233, 19);
      additionalInputComments.TabIndex = 9;
      additionalInputComments.Text = "Database name to load prefetch from";
      // 
      // DumpDetailsWithText
      // 
      AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      ClientSize = new System.Drawing.Size(697, 325);
      Controls.Add(additionalInputComments);
      Controls.Add(additionalTxtInput);
      Name = "DumpDetailsWithText";
      Text = "Name";
      Controls.SetChildIndex(okBtn, 0);
      Controls.SetChildIndex(CancelBtn, 0);
      Controls.SetChildIndex(dumpPathTxt, 0);
      Controls.SetChildIndex(mscordPathTxt, 0);
      Controls.SetChildIndex(additionalTxtInput, 0);
      Controls.SetChildIndex(additionalInputComments, 0);
      ResumeLayout(false);
      PerformLayout();

    }

    #endregion

    public System.Windows.Forms.TextBox additionalTxtInput;

    public System.Windows.Forms.Label additionalInputComments;
  }
}