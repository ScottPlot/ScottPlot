namespace Sandbox.WinForms3D;

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
        formsPlot3d1 = new FormsPlot3D();
        SuspendLayout();
        // 
        // formsPlot3d1
        // 
        formsPlot3d1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot3d1.Location = new Point(12, 12);
        formsPlot3d1.Name = "formsPlot3d1";
        formsPlot3d1.Size = new Size(758, 495);
        formsPlot3d1.TabIndex = 0;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(782, 519);
        Controls.Add(formsPlot3d1);
        Name = "Form1";
        Text = "ScottPlot 3D Test";
        ResumeLayout(false);
    }

    #endregion

    private FormsPlot3D formsPlot3d1;
}
