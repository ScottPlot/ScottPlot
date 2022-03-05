
namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    partial class MultiAxisLock
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
            this.cbPrimary = new System.Windows.Forms.CheckBox();
            this.cbSecondary = new System.Windows.Forms.CheckBox();
            this.cbTertiary = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.BackColor = System.Drawing.Color.Transparent;
            this.formsPlot1.Location = new System.Drawing.Point(12, 43);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(594, 326);
            this.formsPlot1.TabIndex = 0;
            // 
            // cbPrimary
            // 
            this.cbPrimary.AutoSize = true;
            this.cbPrimary.Checked = true;
            this.cbPrimary.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPrimary.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPrimary.ForeColor = System.Drawing.Color.Magenta;
            this.cbPrimary.Location = new System.Drawing.Point(12, 12);
            this.cbPrimary.Name = "cbPrimary";
            this.cbPrimary.Size = new System.Drawing.Size(85, 25);
            this.cbPrimary.TabIndex = 7;
            this.cbPrimary.Text = "Primary";
            this.cbPrimary.UseVisualStyleBackColor = true;
            this.cbPrimary.CheckedChanged += new System.EventHandler(this.cbPrimary_CheckedChanged);
            // 
            // cbSecondary
            // 
            this.cbSecondary.AutoSize = true;
            this.cbSecondary.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSecondary.ForeColor = System.Drawing.Color.Green;
            this.cbSecondary.Location = new System.Drawing.Point(103, 12);
            this.cbSecondary.Name = "cbSecondary";
            this.cbSecondary.Size = new System.Drawing.Size(107, 25);
            this.cbSecondary.TabIndex = 8;
            this.cbSecondary.Text = "Secondary";
            this.cbSecondary.UseVisualStyleBackColor = true;
            this.cbSecondary.CheckedChanged += new System.EventHandler(this.cbSecondary_CheckedChanged);
            // 
            // cbTertiary
            // 
            this.cbTertiary.AutoSize = true;
            this.cbTertiary.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTertiary.ForeColor = System.Drawing.Color.Navy;
            this.cbTertiary.Location = new System.Drawing.Point(216, 12);
            this.cbTertiary.Name = "cbTertiary";
            this.cbTertiary.Size = new System.Drawing.Size(84, 25);
            this.cbTertiary.TabIndex = 9;
            this.cbTertiary.Text = "Tertiary";
            this.cbTertiary.UseVisualStyleBackColor = true;
            this.cbTertiary.CheckedChanged += new System.EventHandler(this.cbTertiary_CheckedChanged);
            // 
            // MultiAxis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 381);
            this.Controls.Add(this.cbTertiary);
            this.Controls.Add(this.cbSecondary);
            this.Controls.Add(this.cbPrimary);
            this.Controls.Add(this.formsPlot1);
            this.Name = "MultiAxis";
            this.Text = "MultiAxis";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormsPlot formsPlot1;
        private System.Windows.Forms.CheckBox cbPrimary;
        private System.Windows.Forms.CheckBox cbSecondary;
        private System.Windows.Forms.CheckBox cbTertiary;
    }
}