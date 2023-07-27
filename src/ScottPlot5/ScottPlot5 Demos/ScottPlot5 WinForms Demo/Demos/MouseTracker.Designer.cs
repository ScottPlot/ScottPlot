namespace WinForms_Demo.Demos;

partial class MouseTracker
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
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Dock = DockStyle.Fill;
        formsPlot1.Location = new Point(0, 0);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(800, 450);
        formsPlot1.TabIndex = 0;
        // 
        // MouseTracker
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(formsPlot1);
        Name = "MouseTracker";
        Text = "MouseTracker";
        ResumeLayout(false);
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
}