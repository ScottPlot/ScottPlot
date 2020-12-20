
namespace ControlBackEndDev
{
    partial class Form1
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
            this.spControl1 = new ControlBackEndDev.SPControl();
            this.SuspendLayout();
            // 
            // spControl1
            // 
            this.spControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spControl1.Location = new System.Drawing.Point(0, 0);
            this.spControl1.Name = "spControl1";
            this.spControl1.Size = new System.Drawing.Size(584, 361);
            this.spControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.spControl1);
            this.Name = "Form1";
            this.Text = "ScottPlot Control Development";
            this.ResumeLayout(false);

        }

        #endregion

        private SPControl spControl1;
    }
}

