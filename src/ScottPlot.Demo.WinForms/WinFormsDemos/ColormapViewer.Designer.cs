namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    partial class ColormapViewer
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbColormapNames = new System.Windows.Forms.ListBox();
            this.lblColormap = new System.Windows.Forms.Label();
            this.pbColormap = new System.Windows.Forms.PictureBox();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.formsPlot2 = new ScottPlot.FormsPlot();
            this.rbData = new System.Windows.Forms.RadioButton();
            this.rbImage = new System.Windows.Forms.RadioButton();
            this.formsPlot3 = new ScottPlot.FormsPlot();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbColormap)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.Controls.Add(this.lbColormapNames);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(148, 633);
            this.panel1.TabIndex = 0;
            // 
            // lbColormapNames
            // 
            this.lbColormapNames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbColormapNames.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbColormapNames.FormattingEnabled = true;
            this.lbColormapNames.ItemHeight = 21;
            this.lbColormapNames.Location = new System.Drawing.Point(0, 0);
            this.lbColormapNames.Name = "lbColormapNames";
            this.lbColormapNames.Size = new System.Drawing.Size(148, 633);
            this.lbColormapNames.TabIndex = 0;
            this.lbColormapNames.SelectedIndexChanged += new System.EventHandler(this.lbColormapNames_SelectedIndexChanged);
            // 
            // lblColormap
            // 
            this.lblColormap.AutoSize = true;
            this.lblColormap.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColormap.Location = new System.Drawing.Point(166, 12);
            this.lblColormap.Name = "lblColormap";
            this.lblColormap.Size = new System.Drawing.Size(68, 30);
            this.lblColormap.TabIndex = 1;
            this.lblColormap.Text = "label1";
            // 
            // pbColormap
            // 
            this.pbColormap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbColormap.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pbColormap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbColormap.Location = new System.Drawing.Point(171, 45);
            this.pbColormap.Name = "pbColormap";
            this.pbColormap.Size = new System.Drawing.Size(432, 37);
            this.pbColormap.TabIndex = 2;
            this.pbColormap.TabStop = false;
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.formsPlot1.Location = new System.Drawing.Point(171, 88);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(432, 336);
            this.formsPlot1.TabIndex = 3;
            // 
            // formsPlot2
            // 
            this.formsPlot2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot2.Location = new System.Drawing.Point(609, 88);
            this.formsPlot2.Name = "formsPlot2";
            this.formsPlot2.Size = new System.Drawing.Size(671, 336);
            this.formsPlot2.TabIndex = 4;
            // 
            // rbData
            // 
            this.rbData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbData.AutoSize = true;
            this.rbData.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbData.Location = new System.Drawing.Point(1011, 14);
            this.rbData.Name = "rbData";
            this.rbData.Size = new System.Drawing.Size(116, 25);
            this.rbData.TabIndex = 5;
            this.rbData.Text = "Sample Data";
            this.rbData.UseVisualStyleBackColor = true;
            this.rbData.CheckedChanged += new System.EventHandler(this.rbData_CheckedChanged);
            // 
            // rbImage
            // 
            this.rbImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbImage.AutoSize = true;
            this.rbImage.Checked = true;
            this.rbImage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbImage.Location = new System.Drawing.Point(1153, 14);
            this.rbImage.Name = "rbImage";
            this.rbImage.Size = new System.Drawing.Size(127, 25);
            this.rbImage.TabIndex = 6;
            this.rbImage.TabStop = true;
            this.rbImage.Text = "Sample Image";
            this.rbImage.UseVisualStyleBackColor = true;
            this.rbImage.CheckedChanged += new System.EventHandler(this.rbImage_CheckedChanged);
            // 
            // formsPlot3
            // 
            this.formsPlot3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot3.Location = new System.Drawing.Point(171, 430);
            this.formsPlot3.Name = "formsPlot3";
            this.formsPlot3.Size = new System.Drawing.Size(1109, 215);
            this.formsPlot3.TabIndex = 7;
            // 
            // ColormapViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1292, 657);
            this.Controls.Add(this.formsPlot3);
            this.Controls.Add(this.rbImage);
            this.Controls.Add(this.rbData);
            this.Controls.Add(this.formsPlot2);
            this.Controls.Add(this.formsPlot1);
            this.Controls.Add(this.pbColormap);
            this.Controls.Add(this.lblColormap);
            this.Controls.Add(this.panel1);
            this.Name = "ColormapViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Colormap Viewer";
            this.Load += new System.EventHandler(this.ColormapViewer_Load);
            this.SizeChanged += new System.EventHandler(this.ColormapViewer_SizeChanged);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbColormap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox lbColormapNames;
        private System.Windows.Forms.Label lblColormap;
        private System.Windows.Forms.PictureBox pbColormap;
        private FormsPlot formsPlot1;
        private FormsPlot formsPlot2;
        private System.Windows.Forms.RadioButton rbData;
        private System.Windows.Forms.RadioButton rbImage;
        private FormsPlot formsPlot3;
    }
}