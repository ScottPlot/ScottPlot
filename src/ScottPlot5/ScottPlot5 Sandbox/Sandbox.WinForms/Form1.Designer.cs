namespace Sandbox.WinForms;

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
        multiFormsPlot1 = new ScottPlot.WinForms.MultiFormsPlot();
        SuspendLayout();
        // 
        // multiFormsPlot1
        // 
        multiFormsPlot1.Dock = DockStyle.Fill;
        multiFormsPlot1.Location = new Point(0, 0);
        multiFormsPlot1.Name = "multiFormsPlot1";
        multiFormsPlot1.Size = new Size(800, 450);
        multiFormsPlot1.TabIndex = 0;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(multiFormsPlot1);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ScottPlot 5 - Windows Forms Sandbox";
        ResumeLayout(false);
    }

    #endregion

    private ScottPlot.WinForms.MultiFormsPlot multiFormsPlot1;
}
