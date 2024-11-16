
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
            PanelPictureBox = new System.Windows.Forms.Panel();
            PictureBoxLegend = new System.Windows.Forms.PictureBox();
            ColorDialog = new System.Windows.Forms.ColorDialog();
            PanelPictureBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxLegend).BeginInit();
            SuspendLayout();
            // 
            // PanelPictureBox
            // 
            PanelPictureBox.AutoScroll = true;
            PanelPictureBox.AutoSize = true;
            PanelPictureBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            PanelPictureBox.BackColor = System.Drawing.Color.White;
            PanelPictureBox.Controls.Add(PictureBoxLegend);
            PanelPictureBox.Location = new System.Drawing.Point(0, 0);
            PanelPictureBox.Name = "PanelPictureBox";
            PanelPictureBox.Size = new System.Drawing.Size(1440, 328);
            PanelPictureBox.TabIndex = 0;
            // 
            // PictureBoxLegend
            // 
            PictureBoxLegend.BackColor = System.Drawing.Color.Transparent;
            PictureBoxLegend.Cursor = System.Windows.Forms.Cursors.Hand;
            PictureBoxLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            PictureBoxLegend.Location = new System.Drawing.Point(0, 0);
            PictureBoxLegend.Name = "PictureBoxLegend";
            PictureBoxLegend.Size = new System.Drawing.Size(1440, 328);
            PictureBoxLegend.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            PictureBoxLegend.TabIndex = 1;
            PictureBoxLegend.TabStop = false;
            PictureBoxLegend.Click += PictureBoxLegend_MouseClick;
            // 
            // FormsPlotLegendViewer
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(248, 327);
            Controls.Add(PanelPictureBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormsPlotLegendViewer";
            ShowIcon = false;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Legend";
            PanelPictureBox.ResumeLayout(false);
            PanelPictureBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxLegend).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel PanelPictureBox;
        private System.Windows.Forms.PictureBox PictureBoxLegend;
        private System.Windows.Forms.ColorDialog ColorDialog;
    }
}