namespace ScottPlot.WinForms.Examples
{
    partial class QuickStart
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
            this.SuspendLayout();
            // 
            // interactivePlot1
            // 
            this.interactivePlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.interactivePlot1.Location = new System.Drawing.Point(0, 0);
            this.interactivePlot1.Name = "interactivePlot1";
            this.interactivePlot1.Size = new System.Drawing.Size(800, 450);
            this.interactivePlot1.TabIndex = 0;
            // 
            // QuickStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.interactivePlot1);
            this.Name = "QuickStart";
            this.Text = "QuickStart";
            this.Load += new System.EventHandler(this.QuickStart_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private InteractivePlot interactivePlot1;
    }
}