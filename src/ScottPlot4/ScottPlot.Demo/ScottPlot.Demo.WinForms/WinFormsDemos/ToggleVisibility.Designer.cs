namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    partial class ToggleVisibility
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
            this.cbSin = new System.Windows.Forms.CheckBox();
            this.cbCos = new System.Windows.Forms.CheckBox();
            this.cbLines = new System.Windows.Forms.CheckBox();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.SuspendLayout();
            // 
            // cbSin
            // 
            this.cbSin.AutoSize = true;
            this.cbSin.Checked = true;
            this.cbSin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSin.Location = new System.Drawing.Point(12, 12);
            this.cbSin.Name = "cbSin";
            this.cbSin.Size = new System.Drawing.Size(41, 17);
            this.cbSin.TabIndex = 0;
            this.cbSin.Text = "Sin";
            this.cbSin.UseVisualStyleBackColor = true;
            this.cbSin.CheckedChanged += new System.EventHandler(this.cbSin_CheckedChanged);
            // 
            // cbCos
            // 
            this.cbCos.AutoSize = true;
            this.cbCos.Checked = true;
            this.cbCos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCos.Location = new System.Drawing.Point(59, 12);
            this.cbCos.Name = "cbCos";
            this.cbCos.Size = new System.Drawing.Size(44, 17);
            this.cbCos.TabIndex = 1;
            this.cbCos.Text = "Cos";
            this.cbCos.UseVisualStyleBackColor = true;
            this.cbCos.CheckedChanged += new System.EventHandler(this.cbCos_CheckedChanged);
            // 
            // cbLines
            // 
            this.cbLines.AutoSize = true;
            this.cbLines.Checked = true;
            this.cbLines.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLines.Location = new System.Drawing.Point(109, 12);
            this.cbLines.Name = "cbLines";
            this.cbLines.Size = new System.Drawing.Size(47, 17);
            this.cbLines.TabIndex = 2;
            this.cbLines.Text = "lines";
            this.cbLines.UseVisualStyleBackColor = true;
            this.cbLines.CheckedChanged += new System.EventHandler(this.cbLines_CheckedChanged);
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(12, 35);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(449, 255);
            this.formsPlot1.TabIndex = 3;
            // 
            // ToggleVisibility
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 302);
            this.Controls.Add(this.formsPlot1);
            this.Controls.Add(this.cbLines);
            this.Controls.Add(this.cbCos);
            this.Controls.Add(this.cbSin);
            this.Name = "ToggleVisibility";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ToggleVisibility";
            this.Load += new System.EventHandler(this.ToggleVisibility_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbSin;
        private System.Windows.Forms.CheckBox cbCos;
        private System.Windows.Forms.CheckBox cbLines;
        private FormsPlot formsPlot1;
    }
}