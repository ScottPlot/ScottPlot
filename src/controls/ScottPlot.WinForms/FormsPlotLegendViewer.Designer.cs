
namespace ScottPlot
{
    partial class FormsPlotLegendViewer
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
            this.PanelPictureBox = new System.Windows.Forms.Panel();
            this.PictureBoxLegend = new System.Windows.Forms.PictureBox();
            this.PanelPictureBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxLegend)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelPictureBox
            // 
            this.PanelPictureBox.AutoScroll = true;
            this.PanelPictureBox.AutoSize = true;
            this.PanelPictureBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PanelPictureBox.BackColor = System.Drawing.Color.White;
            this.PanelPictureBox.Controls.Add(this.PictureBoxLegend);
            this.PanelPictureBox.Location = new System.Drawing.Point(0, 0);
            this.PanelPictureBox.Name = "PanelPictureBox";
            this.PanelPictureBox.Size = new System.Drawing.Size(1440, 328);
            this.PanelPictureBox.TabIndex = 0;
            // 
            // PictureBoxLegend
            // 
            this.PictureBoxLegend.BackColor = System.Drawing.Color.Transparent;
            this.PictureBoxLegend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PictureBoxLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBoxLegend.Location = new System.Drawing.Point(0, 0);
            this.PictureBoxLegend.Name = "PictureBoxLegend";
            this.PictureBoxLegend.Size = new System.Drawing.Size(1440, 328);
            this.PictureBoxLegend.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PictureBoxLegend.TabIndex = 1;
            this.PictureBoxLegend.TabStop = false;
            this.PictureBoxLegend.Click += new System.EventHandler(this.PictureBoxLegend_MouseClick);
            // 
            // FormsPlotLegendViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(248, 327);
            this.Controls.Add(this.PanelPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormsPlotLegendViewer";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Legend";
            this.PanelPictureBox.ResumeLayout(false);
            this.PanelPictureBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxLegend)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PanelPictureBox;
        private System.Windows.Forms.PictureBox PictureBoxLegend;
    }
}