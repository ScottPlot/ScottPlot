namespace Benchmark
{
    partial class FormGui
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
            this.RunCheckbox = new System.Windows.Forms.CheckBox();
            this.AntiAliasCheckbox = new System.Windows.Forms.CheckBox();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // RunCheckbox
            // 
            this.RunCheckbox.AutoSize = true;
            this.RunCheckbox.Location = new System.Drawing.Point(12, 12);
            this.RunCheckbox.Name = "RunCheckbox";
            this.RunCheckbox.Size = new System.Drawing.Size(46, 17);
            this.RunCheckbox.TabIndex = 0;
            this.RunCheckbox.Text = "Run";
            this.RunCheckbox.UseVisualStyleBackColor = true;
            this.RunCheckbox.CheckedChanged += new System.EventHandler(this.RunCheckbox_CheckedChanged);
            // 
            // AntiAliasCheckbox
            // 
            this.AntiAliasCheckbox.AutoSize = true;
            this.AntiAliasCheckbox.Checked = true;
            this.AntiAliasCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AntiAliasCheckbox.Location = new System.Drawing.Point(64, 12);
            this.AntiAliasCheckbox.Name = "AntiAliasCheckbox";
            this.AntiAliasCheckbox.Size = new System.Drawing.Size(69, 17);
            this.AntiAliasCheckbox.TabIndex = 1;
            this.AntiAliasCheckbox.Text = "Anti-Alias";
            this.AntiAliasCheckbox.UseVisualStyleBackColor = true;
            this.AntiAliasCheckbox.CheckedChanged += new System.EventHandler(this.AntiAliasCheckbox_CheckedChanged);
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(12, 35);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(628, 298);
            this.formsPlot1.TabIndex = 3;
            // 
            // timer1
            // 
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(425, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Cycle time:";
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.Enabled = false;
            this.TimeLabel.Location = new System.Drawing.Point(489, 13);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(83, 13);
            this.TimeLabel.TabIndex = 5;
            this.TimeLabel.Text = "benchmarking...";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(139, 6);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(280, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // FormGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 345);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.TimeLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.formsPlot1);
            this.Controls.Add(this.AntiAliasCheckbox);
            this.Controls.Add(this.RunCheckbox);
            this.Name = "FormGui";
            this.Text = "FormsPlot Stress-Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox RunCheckbox;
        private System.Windows.Forms.CheckBox AntiAliasCheckbox;
        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}