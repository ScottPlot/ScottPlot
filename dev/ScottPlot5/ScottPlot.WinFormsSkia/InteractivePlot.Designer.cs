namespace ScottPlot.WinFormsSkia
{
    partial class InteractivePlot
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
            this.skglControl1 = new SkiaSharp.Views.Desktop.SKGLControl();
            this.SuspendLayout();
            // 
            // skglControl1
            // 
            this.skglControl1.BackColor = System.Drawing.Color.Black;
            this.skglControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skglControl1.Location = new System.Drawing.Point(0, 0);
            this.skglControl1.Name = "skglControl1";
            this.skglControl1.Size = new System.Drawing.Size(450, 300);
            this.skglControl1.TabIndex = 0;
            this.skglControl1.VSync = false;
            this.skglControl1.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs>(this.skglControl1_PaintSurface);
            this.skglControl1.DoubleClick += new System.EventHandler(this.skglControl1_DoubleClick);
            this.skglControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.skglControl1_MouseDown);
            this.skglControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.skglControl1_MouseMove);
            this.skglControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.skglControl1_MouseUp);
            // 
            // InteractivePlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Navy;
            this.Controls.Add(this.skglControl1);
            this.Name = "InteractivePlot";
            this.Size = new System.Drawing.Size(450, 300);
            this.ResumeLayout(false);

        }

        #endregion

        private SkiaSharp.Views.Desktop.SKGLControl skglControl1;
    }
}
