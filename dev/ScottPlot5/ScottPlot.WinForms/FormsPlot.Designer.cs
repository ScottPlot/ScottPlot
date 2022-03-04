namespace ScottPlot.WinForms
{
    partial class FormsPlot
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
            this.skglControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.skglControl1.Name = "skglControl1";
            this.skglControl1.Size = new System.Drawing.Size(600, 400);
            this.skglControl1.TabIndex = 0;
            this.skglControl1.VSync = true;
            // 
            // FormsPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.skglControl1);
            this.Name = "FormsPlot";
            this.Size = new System.Drawing.Size(600, 400);
            this.ResumeLayout(false);

        }

        #endregion

        private SkiaSharp.Views.Desktop.SKGLControl skglControl1;
    }
}
