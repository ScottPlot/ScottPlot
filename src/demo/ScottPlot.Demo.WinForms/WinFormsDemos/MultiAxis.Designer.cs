
namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    partial class MultiAxis
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
            this.rbPrimary = new System.Windows.Forms.RadioButton();
            this.rbSecondary = new System.Windows.Forms.RadioButton();
            this.rbTertiary = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
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
            this.formsPlot1.Size = new System.Drawing.Size(739, 370);
            this.formsPlot1.TabIndex = 0;
            // 
            // rbPrimary
            // 
            this.rbPrimary.AutoSize = true;
            this.rbPrimary.Checked = true;
            this.rbPrimary.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbPrimary.ForeColor = System.Drawing.Color.Red;
            this.rbPrimary.Location = new System.Drawing.Point(12, 12);
            this.rbPrimary.Name = "rbPrimary";
            this.rbPrimary.Size = new System.Drawing.Size(84, 25);
            this.rbPrimary.TabIndex = 1;
            this.rbPrimary.TabStop = true;
            this.rbPrimary.Text = "Primary";
            this.rbPrimary.UseVisualStyleBackColor = true;
            this.rbPrimary.CheckedChanged += new System.EventHandler(this.rbPrimary_CheckedChanged);
            // 
            // rbSecondary
            // 
            this.rbSecondary.AutoSize = true;
            this.rbSecondary.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSecondary.ForeColor = System.Drawing.Color.Green;
            this.rbSecondary.Location = new System.Drawing.Point(102, 12);
            this.rbSecondary.Name = "rbSecondary";
            this.rbSecondary.Size = new System.Drawing.Size(106, 25);
            this.rbSecondary.TabIndex = 2;
            this.rbSecondary.Text = "Secondary";
            this.rbSecondary.UseVisualStyleBackColor = true;
            this.rbSecondary.CheckedChanged += new System.EventHandler(this.rbSecondary_CheckedChanged);
            // 
            // rbTertiary
            // 
            this.rbTertiary.AutoSize = true;
            this.rbTertiary.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbTertiary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.rbTertiary.Location = new System.Drawing.Point(214, 12);
            this.rbTertiary.Name = "rbTertiary";
            this.rbTertiary.Size = new System.Drawing.Size(83, 25);
            this.rbTertiary.TabIndex = 3;
            this.rbTertiary.Text = "Tertiary";
            this.rbTertiary.UseVisualStyleBackColor = true;
            this.rbTertiary.CheckedChanged += new System.EventHandler(this.rbTertiary_CheckedChanged);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbAll.Location = new System.Drawing.Point(303, 12);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(85, 25);
            this.rbAll.TabIndex = 4;
            this.rbAll.Text = "Lock All";
            this.rbAll.UseVisualStyleBackColor = true;
            this.rbAll.CheckedChanged += new System.EventHandler(this.rbAll_CheckedChanged);
            // 
            // MultiAxis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 425);
            this.Controls.Add(this.rbAll);
            this.Controls.Add(this.rbTertiary);
            this.Controls.Add(this.rbSecondary);
            this.Controls.Add(this.rbPrimary);
            this.Controls.Add(this.formsPlot1);
            this.Name = "MultiAxis";
            this.Text = "MultiAxis";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormsPlot formsPlot1;
        private System.Windows.Forms.RadioButton rbPrimary;
        private System.Windows.Forms.RadioButton rbSecondary;
        private System.Windows.Forms.RadioButton rbTertiary;
        private System.Windows.Forms.RadioButton rbAll;
    }
}