namespace WinForms_Demo.Demos;

partial class SignalXY_ZoomBug
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
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        formsPlot2 = new ScottPlot.WinForms.FormsPlot();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Dock = DockStyle.Left;
        formsPlot1.Location = new Point(0, 0);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(800, 450);
        formsPlot1.TabIndex = 0;
        // 
        // formsPlot2
        // 
        formsPlot2.DisplayScale = 1F;
        formsPlot2.Dock = DockStyle.Left;
        formsPlot2.Location = new Point(800, 0);
        formsPlot2.Name = "formsPlot2";
        formsPlot2.Size = new Size(772, 450);
        formsPlot2.TabIndex = 1;
        // 
        // SignalXY_ZoomBug
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1584, 450);
        Controls.Add(formsPlot2);
        Controls.Add(formsPlot1);
        Name = "SignalXY_ZoomBug";
        Text = "SignalXYDrag";
        ResumeLayout(false);
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private ScottPlot.WinForms.FormsPlot formsPlot2;
}