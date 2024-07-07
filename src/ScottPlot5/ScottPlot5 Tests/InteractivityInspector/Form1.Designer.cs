namespace Interactivity_Inspector;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(12, 12);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(980, 682);
        formsPlot1.TabIndex = 2;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1004, 706);
        Controls.Add(formsPlot1);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Form1";
        ResumeLayout(false);
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
}
