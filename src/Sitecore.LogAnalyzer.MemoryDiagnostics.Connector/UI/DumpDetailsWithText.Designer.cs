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
      if (disposing && (this.components != null))
      {
        this.components.Dispose();
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
      this.additionalTxtInput = new System.Windows.Forms.TextBox();
      this.additionalInputComments = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // okBtn
      // 
      this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
      // 
      // additionalTxtInput
      // 
      this.additionalTxtInput.Font = new System.Drawing.Font("Times New Roman", 12F);
      this.additionalTxtInput.Location = new System.Drawing.Point(54, 270);
      this.additionalTxtInput.Name = "additionalTxtInput";
      this.additionalTxtInput.Size = new System.Drawing.Size(100, 26);
      this.additionalTxtInput.TabIndex = 8;
      this.additionalTxtInput.Text = "web";
      // 
      // additionalInputComments
      // 
      this.additionalInputComments.AutoSize = true;
      this.additionalInputComments.Font = new System.Drawing.Font("Times New Roman", 12F);
      this.additionalInputComments.Location = new System.Drawing.Point(16, 239);
      this.additionalInputComments.Name = "additionalInputComments";
      this.additionalInputComments.Size = new System.Drawing.Size(233, 19);
      this.additionalInputComments.TabIndex = 9;
      this.additionalInputComments.Text = "Database name to load prefetch from";
      // 
      // DumpDetailsWithText
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(697, 325);
      this.Controls.Add(this.additionalInputComments);
      this.Controls.Add(this.additionalTxtInput);
      this.Name = "DumpDetailsWithText";
      this.Text = "Name";
      this.Controls.SetChildIndex(this.okBtn, 0);
      this.Controls.SetChildIndex(this.CancelBtn, 0);
      this.Controls.SetChildIndex(this.dumpPathTxt, 0);
      this.Controls.SetChildIndex(this.mscordPathTxt, 0);
      this.Controls.SetChildIndex(this.additionalTxtInput, 0);
      this.Controls.SetChildIndex(this.additionalInputComments, 0);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    public System.Windows.Forms.TextBox additionalTxtInput;

    public System.Windows.Forms.Label additionalInputComments;
  }
}