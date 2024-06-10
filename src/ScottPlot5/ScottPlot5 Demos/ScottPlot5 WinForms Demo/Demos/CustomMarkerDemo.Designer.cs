namespace WinForms_Demo.Demos;

partial class CustomMarkerDemo
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
        trackBar1 = new TrackBar();
        label1 = new Label();
        ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(12, 42);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(776, 396);
        formsPlot1.TabIndex = 1;
        // 
        // trackBar1
        // 
        trackBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        trackBar1.Location = new Point(103, 9);
        trackBar1.Maximum = 50;
        trackBar1.Minimum = -50;
        trackBar1.Name = "trackBar1";
        trackBar1.Size = new Size(685, 45);
        trackBar1.TabIndex = 2;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        label1.Location = new Point(12, 9);
        label1.Name = "label1";
        label1.Size = new Size(85, 21);
        label1.TabIndex = 3;
        label1.Text = "Happiness:";
        // 
        // CustomMarkerDemo
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(label1);
        Controls.Add(trackBar1);
        Controls.Add(formsPlot1);
        Name = "CustomMarkerDemo";
        Text = "CustomMarkerDemo";
        ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private TrackBar trackBar1;
    private Label label1;
}