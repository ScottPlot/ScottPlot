namespace ScottPlot.WinForms.Examples
{
    partial class MultipleAxes2
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
            this.Reset = new System.Windows.Forms.Button();
            this.AddYLeft = new System.Windows.Forms.Button();
            this.AddYRight = new System.Windows.Forms.Button();
            this.AddXTop = new System.Windows.Forms.Button();
            this.AddXBottom = new System.Windows.Forms.Button();
            this.interactivePlot1 = new ScottPlot.WinForms.InteractivePlot();
            this.SuspendLayout();
            // 
            // Reset
            // 
            this.Reset.Location = new System.Drawing.Point(93, 41);
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(75, 23);
            this.Reset.TabIndex = 0;
            this.Reset.Text = "Reset";
            this.Reset.UseVisualStyleBackColor = true;
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // AddYLeft
            // 
            this.AddYLeft.Location = new System.Drawing.Point(12, 41);
            this.AddYLeft.Name = "AddYLeft";
            this.AddYLeft.Size = new System.Drawing.Size(75, 23);
            this.AddYLeft.TabIndex = 0;
            this.AddYLeft.Text = "Y Left";
            this.AddYLeft.UseVisualStyleBackColor = true;
            this.AddYLeft.Click += new System.EventHandler(this.AddYLeft_Click);
            // 
            // AddYRight
            // 
            this.AddYRight.Location = new System.Drawing.Point(174, 41);
            this.AddYRight.Name = "AddYRight";
            this.AddYRight.Size = new System.Drawing.Size(75, 23);
            this.AddYRight.TabIndex = 0;
            this.AddYRight.Text = "Y Right";
            this.AddYRight.UseVisualStyleBackColor = true;
            this.AddYRight.Click += new System.EventHandler(this.AddYRight_Click);
            // 
            // AddXTop
            // 
            this.AddXTop.Location = new System.Drawing.Point(93, 12);
            this.AddXTop.Name = "AddXTop";
            this.AddXTop.Size = new System.Drawing.Size(75, 23);
            this.AddXTop.TabIndex = 0;
            this.AddXTop.Text = "X Top";
            this.AddXTop.UseVisualStyleBackColor = true;
            this.AddXTop.Click += new System.EventHandler(this.AddXTop_Click);
            // 
            // AddXBottom
            // 
            this.AddXBottom.Location = new System.Drawing.Point(93, 70);
            this.AddXBottom.Name = "AddXBottom";
            this.AddXBottom.Size = new System.Drawing.Size(75, 23);
            this.AddXBottom.TabIndex = 0;
            this.AddXBottom.Text = "X Bottom";
            this.AddXBottom.UseVisualStyleBackColor = true;
            this.AddXBottom.Click += new System.EventHandler(this.AddXBottom_Click);
            // 
            // interactivePlot1
            // 
            this.interactivePlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.interactivePlot1.Location = new System.Drawing.Point(12, 99);
            this.interactivePlot1.Name = "interactivePlot1";
            this.interactivePlot1.Size = new System.Drawing.Size(776, 339);
            this.interactivePlot1.TabIndex = 1;
            // 
            // MultipleAxes2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.interactivePlot1);
            this.Controls.Add(this.AddXBottom);
            this.Controls.Add(this.AddXTop);
            this.Controls.Add(this.AddYRight);
            this.Controls.Add(this.AddYLeft);
            this.Controls.Add(this.Reset);
            this.Name = "MultipleAxes2";
            this.Text = "MultipleAxes";
            this.Load += new System.EventHandler(this.MultipleAxes2_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Reset;
        private System.Windows.Forms.Button AddYLeft;
        private System.Windows.Forms.Button AddYRight;
        private System.Windows.Forms.Button AddXTop;
        private System.Windows.Forms.Button AddXBottom;
        private InteractivePlot interactivePlot1;
    }
}