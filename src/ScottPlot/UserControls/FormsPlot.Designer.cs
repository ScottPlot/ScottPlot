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
            this.lblStatus = new System.Windows.Forms.Label();
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
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Red;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Yellow;
            this.lblStatus.Location = new System.Drawing.Point(10, 10);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(110, 17);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "ERROR MESSAGE";
            this.lblStatus.Visible = false;
            // 
            // FormsPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pbPlot);
            this.Name = "FormsPlot";
            this.Size = new System.Drawing.Size(500, 350);
            ((System.ComponentModel.ISupportInitialize)(this.pbPlot)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPlot;
        private System.Windows.Forms.Label lblStatus;
    }
}
