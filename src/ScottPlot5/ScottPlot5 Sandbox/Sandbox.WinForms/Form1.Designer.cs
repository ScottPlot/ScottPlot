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
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        button1 = new Button();
        button2 = new Button();
        trackBar1 = new TrackBar();
        ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        formsPlot1.DisplayScale = 1F;
        formsPlot1.Location = new Point(12, 41);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new Size(776, 397);
        formsPlot1.TabIndex = 0;
        // 
        // button1
        // 
        button1.Location = new Point(12, 12);
        button1.Name = "button1";
        button1.Size = new Size(75, 23);
        button1.TabIndex = 1;
        button1.Text = "button1";
        button1.UseVisualStyleBackColor = true;
        // 
        // button2
        // 
        button2.Location = new Point(93, 12);
        button2.Name = "button2";
        button2.Size = new Size(75, 23);
        button2.TabIndex = 2;
        button2.Text = "button2";
        button2.UseVisualStyleBackColor = true;
        // 
        // trackBar1
        // 
        trackBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        trackBar1.Location = new Point(174, 12);
        trackBar1.Maximum = 100;
        trackBar1.Name = "trackBar1";
        trackBar1.Size = new Size(614, 45);
        trackBar1.TabIndex = 3;
        trackBar1.Value = 10;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(trackBar1);
        Controls.Add(button2);
        Controls.Add(button1);
        Controls.Add(formsPlot1);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ScottPlot 5 - Windows Forms Sandbox";
        ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ScottPlot.WinForms.FormsPlot formsPlot1;
    private Button button1;
    private Button button2;
    private TrackBar trackBar1;
}
