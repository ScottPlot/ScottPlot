namespace ScottPlot
{
    partial class ScottPlotUC
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbPlot = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlot)).BeginInit();
            this.SuspendLayout();
            // 
            // pbPlot
            // 
            this.pbPlot.BackColor = System.Drawing.Color.White;
            this.pbPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbPlot.Location = new System.Drawing.Point(0, 0);
            this.pbPlot.Name = "pbPlot";
            this.pbPlot.Size = new System.Drawing.Size(500, 350);
            this.pbPlot.TabIndex = 0;
            this.pbPlot.TabStop = false;
            this.pbPlot.SizeChanged += new System.EventHandler(this.PbPlot_SizeChanged);
            this.pbPlot.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PbPlot_MouseClick);
            this.pbPlot.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PbPlot_MouseDoubleClick);
            this.pbPlot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbPlot_MouseDown);
            this.pbPlot.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PbPlot_MouseMove);
            this.pbPlot.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PbPlot_MouseUp);
            // 
            // ScottPlotUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbPlot);
            this.Name = "ScottPlotUC";
            this.Size = new System.Drawing.Size(500, 350);
            ((System.ComponentModel.ISupportInitialize)(this.pbPlot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPlot;
    }
}
