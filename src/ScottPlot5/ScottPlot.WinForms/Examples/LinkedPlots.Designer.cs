namespace ScottPlot.WinForms.Examples
{
    partial class LinkedPlots
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.interactivePlot1 = new ScottPlot.WinForms.InteractivePlot();
            this.interactivePlot2 = new ScottPlot.WinForms.InteractivePlot();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.interactivePlot2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.interactivePlot1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1292, 459);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // interactivePlot1
            // 
            this.interactivePlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.interactivePlot1.Location = new System.Drawing.Point(3, 3);
            this.interactivePlot1.Name = "interactivePlot1";
            this.interactivePlot1.Size = new System.Drawing.Size(640, 453);
            this.interactivePlot1.TabIndex = 0;
            // 
            // interactivePlot2
            // 
            this.interactivePlot2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.interactivePlot2.Location = new System.Drawing.Point(649, 3);
            this.interactivePlot2.Name = "interactivePlot2";
            this.interactivePlot2.Size = new System.Drawing.Size(640, 453);
            this.interactivePlot2.TabIndex = 1;
            // 
            // LinkedPlots
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1292, 459);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LinkedPlots";
            this.Text = "LinkedPlots";
            this.Load += new System.EventHandler(this.LinkedPlots_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private InteractivePlot interactivePlot2;
        private InteractivePlot interactivePlot1;
    }
}