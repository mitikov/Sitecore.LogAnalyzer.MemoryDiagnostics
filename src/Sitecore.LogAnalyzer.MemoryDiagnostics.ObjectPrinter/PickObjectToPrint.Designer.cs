namespace Sitecore.LogAnalyzer.MemoryDiagnostics.ModelViewer
{
  partial class PickObjectToPrint
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
      typeTreeView = new System.Windows.Forms.TreeView();
      SuspendLayout();
      // 
      // typeTreeView
      // 
      typeTreeView.Location = new System.Drawing.Point(104, 225);
      typeTreeView.Name = "typeTreeView";
      typeTreeView.Size = new System.Drawing.Size(465, 400);
      typeTreeView.TabIndex = 8;
      typeTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(typeTreeView_AfterSelect);
      // 
      // PickObjectToPrint
      // 
      AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      ClientSize = new System.Drawing.Size(697, 637);
      Controls.Add(typeTreeView);
      Name = "PickObjectToPrint";
      Text = "ObjectsToPrintDetails";
      Load += new System.EventHandler(PickObjectToPrint_Load);
      Controls.SetChildIndex(dumpPathTxt, 0);
      Controls.SetChildIndex(mscordPathTxt, 0);
      Controls.SetChildIndex(typeTreeView, 0);
      ResumeLayout(false);
      PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TreeView typeTreeView;
  }
}