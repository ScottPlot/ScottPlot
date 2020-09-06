namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    partial class ShowValueOnHover2
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
            this.cbTooltip = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(12, 33);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(582, 305);
            this.formsPlot1.TabIndex = 0;
            this.formsPlot1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.formsPlot1_MouseMove);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Message";
            // 
            // cbTooltip
            // 
            this.cbTooltip.AutoSize = true;
            this.cbTooltip.Location = new System.Drawing.Point(536, 14);
            this.cbTooltip.Name = "cbTooltip";
            this.cbTooltip.Size = new System.Drawing.Size(58, 17);
            this.cbTooltip.TabIndex = 2;
            this.cbTooltip.Text = "Tooltip";
            this.cbTooltip.UseVisualStyleBackColor = true;
            // 
            // ShowValueOnHover2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 350);
            this.Controls.Add(this.cbTooltip);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.formsPlot1);
            this.Name = "ShowValueOnHover2";
            this.Text = "Highlightable Scatter Demo";
            this.Load += new System.EventHandler(this.ShowValueOnHover2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormsPlot formsPlot1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbTooltip;
    }
}