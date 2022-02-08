namespace ScottPlot.WinForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.skglControl1 = new SkiaSharp.Views.Desktop.SKGLControl();
            this.btnScatterBasic = new System.Windows.Forms.Button();
            this.btnScatter100k = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // skglControl1
            // 
            this.skglControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skglControl1.BackColor = System.Drawing.Color.Black;
            this.skglControl1.Location = new System.Drawing.Point(12, 66);
            this.skglControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.skglControl1.Name = "skglControl1";
            this.skglControl1.Size = new System.Drawing.Size(895, 425);
            this.skglControl1.TabIndex = 0;
            this.skglControl1.VSync = true;
            this.skglControl1.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs>(this.skglControl1_PaintSurface);
            this.skglControl1.SizeChanged += new System.EventHandler(this.skglControl1_SizeChanged);
            this.skglControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.skglControl1_MouseDown);
            this.skglControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.skglControl1_MouseMove);
            this.skglControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.skglControl1_MouseUp);
            // 
            // btnScatterBasic
            // 
            this.btnScatterBasic.Location = new System.Drawing.Point(12, 12);
            this.btnScatterBasic.Name = "btnScatterBasic";
            this.btnScatterBasic.Size = new System.Drawing.Size(79, 48);
            this.btnScatterBasic.TabIndex = 1;
            this.btnScatterBasic.Text = "Scatter with 51 points";
            this.btnScatterBasic.UseVisualStyleBackColor = true;
            this.btnScatterBasic.Click += new System.EventHandler(this.btnScatterBasic_Click);
            // 
            // btnScatter100k
            // 
            this.btnScatter100k.Location = new System.Drawing.Point(97, 12);
            this.btnScatter100k.Name = "btnScatter100k";
            this.btnScatter100k.Size = new System.Drawing.Size(79, 48);
            this.btnScatter100k.TabIndex = 2;
            this.btnScatter100k.Text = "Scatter with 10k points";
            this.btnScatter100k.UseVisualStyleBackColor = true;
            this.btnScatter100k.Click += new System.EventHandler(this.btnScatter100k_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 503);
            this.Controls.Add(this.btnScatterBasic);
            this.Controls.Add(this.btnScatter100k);
            this.Controls.Add(this.skglControl1);
            this.Name = "Form1";
            this.Text = "Maui.Graphics model with SkiaSharp OpenGL rendering";
            this.ResumeLayout(false);

        }

        #endregion

        private SkiaSharp.Views.Desktop.SKGLControl skglControl1;
        private Button btnScatterBasic;
        private Button btnScatter100k;
    }
}