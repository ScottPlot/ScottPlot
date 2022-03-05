namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    partial class LiveDataIncoming
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbLatestValue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbLastValue = new System.Windows.Forms.TextBox();
            this.dataTimer = new System.Windows.Forms.Timer(this.components);
            this.renderTimer = new System.Windows.Forms.Timer(this.components);
            this.cbAutoAxis = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(12, 58);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(776, 380);
            this.formsPlot1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(268, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "This example simulates live display of a growing dataset";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "readings: ";
            // 
            // tbLatestValue
            // 
            this.tbLatestValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLatestValue.Location = new System.Drawing.Point(68, 29);
            this.tbLatestValue.Name = "tbLatestValue";
            this.tbLatestValue.ReadOnly = true;
            this.tbLatestValue.Size = new System.Drawing.Size(61, 23);
            this.tbLatestValue.TabIndex = 3;
            this.tbLatestValue.Text = "123";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(163, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "latest value:";
            // 
            // tbLastValue
            // 
            this.tbLastValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLastValue.Location = new System.Drawing.Point(233, 29);
            this.tbLastValue.Name = "tbLastValue";
            this.tbLastValue.ReadOnly = true;
            this.tbLastValue.Size = new System.Drawing.Size(74, 23);
            this.tbLastValue.TabIndex = 5;
            this.tbLastValue.Text = "+123.4";
            // 
            // dataTimer
            // 
            this.dataTimer.Enabled = true;
            this.dataTimer.Interval = 1;
            this.dataTimer.Tick += new System.EventHandler(this.dataTimer_Tick);
            // 
            // renderTimer
            // 
            this.renderTimer.Enabled = true;
            this.renderTimer.Interval = 20;
            this.renderTimer.Tick += new System.EventHandler(this.renderTimer_Tick);
            // 
            // cbAutoAxis
            // 
            this.cbAutoAxis.AutoSize = true;
            this.cbAutoAxis.Checked = true;
            this.cbAutoAxis.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoAxis.Location = new System.Drawing.Point(342, 32);
            this.cbAutoAxis.Name = "cbAutoAxis";
            this.cbAutoAxis.Size = new System.Drawing.Size(146, 17);
            this.cbAutoAxis.TabIndex = 6;
            this.cbAutoAxis.Text = "auto-axis on each update";
            this.cbAutoAxis.UseVisualStyleBackColor = true;
            this.cbAutoAxis.CheckedChanged += new System.EventHandler(this.cbAutoAxis_CheckedChanged);
            // 
            // LiveDataIncoming
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cbAutoAxis);
            this.Controls.Add(this.tbLastValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbLatestValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.formsPlot1);
            this.Name = "LiveDataIncoming";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Live Data (growing)";
            this.Load += new System.EventHandler(this.LiveDataIncoming_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormsPlot formsPlot1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbLatestValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbLastValue;
        private System.Windows.Forms.Timer dataTimer;
        private System.Windows.Forms.Timer renderTimer;
        private System.Windows.Forms.CheckBox cbAutoAxis;
    }
}