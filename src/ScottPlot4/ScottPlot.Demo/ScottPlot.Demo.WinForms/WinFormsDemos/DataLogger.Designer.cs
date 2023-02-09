namespace ScottPlot.Demo.WinForms.WinFormsDemos;

partial class DataLogger
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
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.cbAutoscale = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbRun = new System.Windows.Forms.CheckBox();
            this.lblReads = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblCh1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblCh2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblCh3 = new System.Windows.Forms.Label();
            this.newDataTimer = new System.Windows.Forms.Timer(this.components);
            this.plotUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(155, 12);
            this.formsPlot1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(802, 463);
            this.formsPlot1.TabIndex = 0;
            // 
            // cbAutoscale
            // 
            this.cbAutoscale.AutoSize = true;
            this.cbAutoscale.Checked = true;
            this.cbAutoscale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoscale.Location = new System.Drawing.Point(6, 47);
            this.cbAutoscale.Name = "cbAutoscale";
            this.cbAutoscale.Size = new System.Drawing.Size(78, 19);
            this.cbAutoscale.TabIndex = 6;
            this.cbAutoscale.Text = "Autoscale";
            this.cbAutoscale.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbRun);
            this.groupBox1.Controls.Add(this.lblReads);
            this.groupBox1.Controls.Add(this.cbAutoscale);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(136, 97);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fake Sensor";
            // 
            // cbRun
            // 
            this.cbRun.AutoSize = true;
            this.cbRun.Checked = true;
            this.cbRun.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRun.Location = new System.Drawing.Point(6, 22);
            this.cbRun.Name = "cbRun";
            this.cbRun.Size = new System.Drawing.Size(47, 19);
            this.cbRun.TabIndex = 8;
            this.cbRun.Text = "Run";
            this.cbRun.UseVisualStyleBackColor = true;
            // 
            // lblReads
            // 
            this.lblReads.AutoSize = true;
            this.lblReads.Location = new System.Drawing.Point(3, 72);
            this.lblReads.Name = "lblReads";
            this.lblReads.Size = new System.Drawing.Size(51, 15);
            this.lblReads.TabIndex = 7;
            this.lblReads.Text = "lblReads";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblCh1);
            this.groupBox3.Location = new System.Drawing.Point(12, 123);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(136, 55);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Channel 1";
            // 
            // lblCh1
            // 
            this.lblCh1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCh1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCh1.ForeColor = System.Drawing.Color.Red;
            this.lblCh1.Location = new System.Drawing.Point(3, 19);
            this.lblCh1.Name = "lblCh1";
            this.lblCh1.Size = new System.Drawing.Size(130, 33);
            this.lblCh1.TabIndex = 0;
            this.lblCh1.Text = "lblCh1";
            this.lblCh1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblCh2);
            this.groupBox2.Location = new System.Drawing.Point(12, 197);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(136, 55);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Channel 2";
            // 
            // lblCh2
            // 
            this.lblCh2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCh2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCh2.ForeColor = System.Drawing.Color.Green;
            this.lblCh2.Location = new System.Drawing.Point(3, 19);
            this.lblCh2.Name = "lblCh2";
            this.lblCh2.Size = new System.Drawing.Size(130, 33);
            this.lblCh2.TabIndex = 0;
            this.lblCh2.Text = "lblCh2";
            this.lblCh2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblCh3);
            this.groupBox4.Location = new System.Drawing.Point(12, 271);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(136, 55);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Channel 3";
            // 
            // lblCh3
            // 
            this.lblCh3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCh3.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCh3.ForeColor = System.Drawing.Color.Blue;
            this.lblCh3.Location = new System.Drawing.Point(3, 19);
            this.lblCh3.Name = "lblCh3";
            this.lblCh3.Size = new System.Drawing.Size(130, 33);
            this.lblCh3.TabIndex = 0;
            this.lblCh3.Text = "lblCh3";
            this.lblCh3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // newDataTimer
            // 
            this.newDataTimer.Enabled = true;
            this.newDataTimer.Interval = 10;
            this.newDataTimer.Tick += new System.EventHandler(this.newDataTimer_Tick);
            // 
            // plotUpdateTimer
            // 
            this.plotUpdateTimer.Enabled = true;
            this.plotUpdateTimer.Tick += new System.EventHandler(this.plotUpdateTimer_Tick);
            // 
            // DataLogger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 487);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.formsPlot1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "DataLogger";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ScottPlot Data Logger Demo";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private FormsPlot formsPlot1;
    private System.Windows.Forms.CheckBox cbAutoscale;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.Label lblCh1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Label lblCh2;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.Label lblCh3;
    private System.Windows.Forms.Label lblReads;
    private System.Windows.Forms.CheckBox cbRun;
    private System.Windows.Forms.Timer newDataTimer;
    private System.Windows.Forms.Timer plotUpdateTimer;
}