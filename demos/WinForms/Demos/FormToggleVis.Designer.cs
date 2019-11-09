namespace ScottPlotDemos
{
    partial class FormToggleVis
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
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.cbSin = new System.Windows.Forms.CheckBox();
            this.cbCos = new System.Windows.Forms.CheckBox();
            this.cbRandom = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(12, 35);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(776, 403);
            this.formsPlot1.TabIndex = 0;
            // 
            // cbSin
            // 
            this.cbSin.AutoSize = true;
            this.cbSin.Checked = true;
            this.cbSin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSin.Location = new System.Drawing.Point(12, 12);
            this.cbSin.Name = "cbSin";
            this.cbSin.Size = new System.Drawing.Size(39, 17);
            this.cbSin.TabIndex = 1;
            this.cbSin.Text = "sin";
            this.cbSin.UseVisualStyleBackColor = true;
            this.cbSin.CheckedChanged += new System.EventHandler(this.cbSin_CheckedChanged);
            // 
            // cbCos
            // 
            this.cbCos.AutoSize = true;
            this.cbCos.Checked = true;
            this.cbCos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCos.Location = new System.Drawing.Point(57, 12);
            this.cbCos.Name = "cbCos";
            this.cbCos.Size = new System.Drawing.Size(43, 17);
            this.cbCos.TabIndex = 2;
            this.cbCos.Text = "cos";
            this.cbCos.UseVisualStyleBackColor = true;
            this.cbCos.CheckedChanged += new System.EventHandler(this.cbCos_CheckedChanged);
            // 
            // cbRandom
            // 
            this.cbRandom.AutoSize = true;
            this.cbRandom.Checked = true;
            this.cbRandom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRandom.Location = new System.Drawing.Point(106, 12);
            this.cbRandom.Name = "cbRandom";
            this.cbRandom.Size = new System.Drawing.Size(61, 17);
            this.cbRandom.TabIndex = 3;
            this.cbRandom.Text = "random";
            this.cbRandom.UseVisualStyleBackColor = true;
            this.cbRandom.CheckedChanged += new System.EventHandler(this.cbRandom_CheckedChanged);
            // 
            // FormToggleVis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cbRandom);
            this.Controls.Add(this.cbCos);
            this.Controls.Add(this.cbSin);
            this.Controls.Add(this.formsPlot1);
            this.Name = "FormToggleVis";
            this.Text = "FormToggleVis";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.CheckBox cbSin;
        private System.Windows.Forms.CheckBox cbCos;
        private System.Windows.Forms.CheckBox cbRandom;
    }
}