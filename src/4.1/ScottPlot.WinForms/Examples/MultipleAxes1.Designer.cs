namespace ScottPlot.WinForms.Examples
{
    partial class MultipleAxes1
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
            this.interactivePlot1 = new ScottPlot.WinForms.InteractivePlot();
            this.MouseControlY1 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MouseControlX = new System.Windows.Forms.CheckBox();
            this.MouseControlY2 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // interactivePlot1
            // 
            this.interactivePlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.interactivePlot1.Location = new System.Drawing.Point(12, 66);
            this.interactivePlot1.Name = "interactivePlot1";
            this.interactivePlot1.Size = new System.Drawing.Size(776, 372);
            this.interactivePlot1.TabIndex = 0;
            // 
            // MouseControlY1
            // 
            this.MouseControlY1.AutoSize = true;
            this.MouseControlY1.Checked = true;
            this.MouseControlY1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MouseControlY1.ForeColor = System.Drawing.Color.Red;
            this.MouseControlY1.Location = new System.Drawing.Point(6, 22);
            this.MouseControlY1.Name = "MouseControlY1";
            this.MouseControlY1.Size = new System.Drawing.Size(39, 19);
            this.MouseControlY1.TabIndex = 1;
            this.MouseControlY1.Text = "Y1";
            this.MouseControlY1.UseVisualStyleBackColor = true;
            this.MouseControlY1.CheckedChanged += new System.EventHandler(this.MouseControlY1_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.MouseControlX);
            this.groupBox1.Controls.Add(this.MouseControlY2);
            this.groupBox1.Controls.Add(this.MouseControlY1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(130, 48);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mouse Control";
            // 
            // MouseControlX
            // 
            this.MouseControlX.AutoSize = true;
            this.MouseControlX.Checked = true;
            this.MouseControlX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MouseControlX.Location = new System.Drawing.Point(96, 22);
            this.MouseControlX.Name = "MouseControlX";
            this.MouseControlX.Size = new System.Drawing.Size(33, 19);
            this.MouseControlX.TabIndex = 1;
            this.MouseControlX.Text = "X";
            this.MouseControlX.UseVisualStyleBackColor = true;
            this.MouseControlX.CheckedChanged += new System.EventHandler(this.MouseControlX_CheckedChanged);
            // 
            // MouseControlY2
            // 
            this.MouseControlY2.AutoSize = true;
            this.MouseControlY2.ForeColor = System.Drawing.Color.Blue;
            this.MouseControlY2.Location = new System.Drawing.Point(51, 22);
            this.MouseControlY2.Name = "MouseControlY2";
            this.MouseControlY2.Size = new System.Drawing.Size(39, 19);
            this.MouseControlY2.TabIndex = 1;
            this.MouseControlY2.Text = "Y2";
            this.MouseControlY2.UseVisualStyleBackColor = true;
            this.MouseControlY2.CheckedChanged += new System.EventHandler(this.MouseControlY2_CheckedChanged);
            // 
            // MultipleAxes1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.interactivePlot1);
            this.Name = "MultipleAxes1";
            this.Text = "MultipleAxes1";
            this.Load += new System.EventHandler(this.MultipleAxes1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private InteractivePlot interactivePlot1;
        private System.Windows.Forms.CheckBox MouseControlY1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox MouseControlX;
        private System.Windows.Forms.CheckBox MouseControlY2;
    }
}