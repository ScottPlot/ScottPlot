namespace ScottPlot.Demo.WinForms.WinFormsDemos;

partial class HistogramDemo
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
            this.components = new System.ComponentModel.Container();
            this.btnReset = new System.Windows.Forms.Button();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cbRun = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(12, 12);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 34);
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(12, 52);
            this.formsPlot1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(718, 398);
            this.formsPlot1.TabIndex = 2;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cbRun
            // 
            this.cbRun.AutoSize = true;
            this.cbRun.Checked = true;
            this.cbRun.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRun.Location = new System.Drawing.Point(93, 21);
            this.cbRun.Name = "cbRun";
            this.cbRun.Size = new System.Drawing.Size(47, 19);
            this.cbRun.TabIndex = 3;
            this.cbRun.Text = "Run";
            this.cbRun.UseVisualStyleBackColor = true;
            // 
            // HistogramDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 462);
            this.Controls.Add(this.cbRun);
            this.Controls.Add(this.formsPlot1);
            this.Controls.Add(this.btnReset);
            this.Name = "HistogramDemo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Histogram Demo";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnReset;
    private FormsPlot formsPlot1;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.CheckBox cbRun;
}