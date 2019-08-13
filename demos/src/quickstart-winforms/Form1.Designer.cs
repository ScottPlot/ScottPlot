namespace ScottPlotQuickstartForms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.interactivePlot1 = new ScottPlot.FormsPlot();
            this.SuspendLayout();
            // 
            // scottPlotUC1
            // 
            this.interactivePlot1.BackColor = System.Drawing.Color.White;
            this.interactivePlot1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("scottPlotUC1.BackgroundImage")));
            this.interactivePlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.interactivePlot1.Location = new System.Drawing.Point(0, 0);
            this.interactivePlot1.Name = "scottPlotUC1";
            this.interactivePlot1.Size = new System.Drawing.Size(550, 286);
            this.interactivePlot1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 286);
            this.Controls.Add(this.interactivePlot1);
            this.Name = "Form1";
            this.Text = "ScottPlot Quickstart (Forms)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ScottPlot.FormsPlot interactivePlot1;
    }
}

