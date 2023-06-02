namespace ScottPlot.Demo.WinForms.WinFormsDemos;

partial class DataStreamer
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
        formsPlot1 = new FormsPlot();
        comboBox1 = new System.Windows.Forms.ComboBox();
        label1 = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // formsPlot1
        // 
        formsPlot1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        formsPlot1.Location = new System.Drawing.Point(13, 41);
        formsPlot1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        formsPlot1.Name = "formsPlot1";
        formsPlot1.Size = new System.Drawing.Size(774, 397);
        formsPlot1.TabIndex = 0;
        // 
        // comboBox1
        // 
        comboBox1.FormattingEnabled = true;
        comboBox1.Location = new System.Drawing.Point(88, 12);
        comboBox1.Name = "comboBox1";
        comboBox1.Size = new System.Drawing.Size(121, 23);
        comboBox1.TabIndex = 1;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(13, 15);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(69, 15);
        label1.TabIndex = 2;
        label1.Text = "View Mode:";
        // 
        // DataStreamer
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(label1);
        Controls.Add(comboBox1);
        Controls.Add(formsPlot1);
        Name = "DataStreamer";
        Text = "DataStreamer Demo";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private FormsPlot formsPlot1;
    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.Label label1;
}