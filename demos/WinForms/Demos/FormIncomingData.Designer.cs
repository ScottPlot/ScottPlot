namespace ScottPlotDemos
{
    partial class FormIncomingData
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RunCheckbox = new System.Windows.Forms.CheckBox();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.AdjustAxisButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.LatestValueLabel = new System.Windows.Forms.Label();
            this.NewDataTimer = new System.Windows.Forms.Timer(this.components);
            this.RenderTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "This demo simulates a measurement device which makes 100 new readings per second." +
    "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 49);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Description";
            // 
            // RunCheckbox
            // 
            this.RunCheckbox.AutoSize = true;
            this.RunCheckbox.Checked = true;
            this.RunCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RunCheckbox.Location = new System.Drawing.Point(494, 35);
            this.RunCheckbox.Name = "RunCheckbox";
            this.RunCheckbox.Size = new System.Drawing.Size(46, 17);
            this.RunCheckbox.TabIndex = 3;
            this.RunCheckbox.Text = "Run";
            this.RunCheckbox.UseVisualStyleBackColor = true;
            this.RunCheckbox.CheckedChanged += new System.EventHandler(this.RunCheckbox_CheckedChanged);
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(12, 67);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(776, 371);
            this.formsPlot1.TabIndex = 4;
            // 
            // AdjustAxisButton
            // 
            this.AdjustAxisButton.Location = new System.Drawing.Point(413, 28);
            this.AdjustAxisButton.Name = "AdjustAxisButton";
            this.AdjustAxisButton.Size = new System.Drawing.Size(75, 28);
            this.AdjustAxisButton.TabIndex = 5;
            this.AdjustAxisButton.Text = "Adjust Axis";
            this.AdjustAxisButton.UseVisualStyleBackColor = true;
            this.AdjustAxisButton.Click += new System.EventHandler(this.AdjustAxisButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.LatestValueLabel);
            this.groupBox2.Location = new System.Drawing.Point(253, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(154, 49);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Latest Reading";
            // 
            // LatestValueLabel
            // 
            this.LatestValueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LatestValueLabel.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LatestValueLabel.Location = new System.Drawing.Point(3, 16);
            this.LatestValueLabel.Name = "LatestValueLabel";
            this.LatestValueLabel.Size = new System.Drawing.Size(148, 30);
            this.LatestValueLabel.TabIndex = 0;
            this.LatestValueLabel.Text = "label2";
            this.LatestValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NewDataTimer
            // 
            this.NewDataTimer.Enabled = true;
            this.NewDataTimer.Interval = 10;
            this.NewDataTimer.Tick += new System.EventHandler(this.NewDataTimer_Tick);
            // 
            // RenderTimer
            // 
            this.RenderTimer.Enabled = true;
            this.RenderTimer.Interval = 20;
            this.RenderTimer.Tick += new System.EventHandler(this.RenderTimer_Tick);
            // 
            // FormIncomingData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.AdjustAxisButton);
            this.Controls.Add(this.formsPlot1);
            this.Controls.Add(this.RunCheckbox);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormIncomingData";
            this.Text = "Incoming Data";
            this.Load += new System.EventHandler(this.FormIncomingData_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox RunCheckbox;
        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.Button AdjustAxisButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label LatestValueLabel;
        private System.Windows.Forms.Timer NewDataTimer;
        private System.Windows.Forms.Timer RenderTimer;
    }
}