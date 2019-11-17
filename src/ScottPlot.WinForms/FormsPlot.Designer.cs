namespace ScottPlot
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
            this.pbPlot = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlot)).BeginInit();
            this.SuspendLayout();
            // 
            // pbPlot
            // 
            this.pbPlot.BackColor = System.Drawing.Color.Navy;
            this.pbPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbPlot.Location = new System.Drawing.Point(0, 0);
            this.pbPlot.Name = "pbPlot";
            this.pbPlot.Size = new System.Drawing.Size(500, 350);
            this.pbPlot.TabIndex = 0;
            this.pbPlot.TabStop = false;
            this.pbPlot.SizeChanged += new System.EventHandler(this.PbPlot_SizeChanged);
            this.pbPlot.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PbPlot_MouseDoubleClick);
            this.pbPlot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbPlot_MouseDown);
            this.pbPlot.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PbPlot_MouseMove);
            this.pbPlot.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PbPlot_MouseUp);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Navy;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(3, 2);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(137, 37);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "ScottPlot";
            // 
            // lblVersion
            // 
            this.lblVersion.BackColor = System.Drawing.Color.Navy;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.White;
            this.lblVersion.Location = new System.Drawing.Point(10, 39);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(130, 37);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "0.0.0.0";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FormsPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pbPlot);
            this.Name = "FormsPlot";
            this.Size = new System.Drawing.Size(500, 350);
            ((System.ComponentModel.ISupportInitialize)(this.pbPlot)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPlot;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblVersion;
    }
}
