namespace ScottPlot5_WinForms_Demo
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.skiaButton1 = new ScottPlot5_WinForms_Demo.SkiaButton();
            this.skiaButton2 = new ScottPlot5_WinForms_Demo.SkiaButton();
            this.skiaButton3 = new ScottPlot5_WinForms_Demo.SkiaButton();
            this.skiaButton4 = new ScottPlot5_WinForms_Demo.SkiaButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(74, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(201, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Windows Forms Demo";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(71, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(244, 47);
            this.label1.TabIndex = 3;
            this.label1.Text = "ScottPlot 5.0.0";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(911, 558);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(905, 94);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoScroll = true;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.skiaButton1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.skiaButton2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.skiaButton3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.skiaButton4, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 103);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(905, 452);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // skiaButton1
            // 
            this.skiaButton1.Description = "Description";
            this.skiaButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skiaButton1.ForegroundColor = "#FFFFFF";
            this.skiaButton1.HighlightColor = "#9a4993";
            this.skiaButton1.IsHighlighted = false;
            this.skiaButton1.Location = new System.Drawing.Point(3, 3);
            this.skiaButton1.Name = "skiaButton1";
            this.skiaButton1.RegularBackgroundColor = "#71297f";
            this.skiaButton1.Size = new System.Drawing.Size(872, 150);
            this.skiaButton1.TabIndex = 0;
            this.skiaButton1.Title = "Title";
            // 
            // skiaButton2
            // 
            this.skiaButton2.Description = "Description";
            this.skiaButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skiaButton2.ForegroundColor = "#FFFFFF";
            this.skiaButton2.HighlightColor = "#9a4993";
            this.skiaButton2.IsHighlighted = false;
            this.skiaButton2.Location = new System.Drawing.Point(3, 159);
            this.skiaButton2.Name = "skiaButton2";
            this.skiaButton2.RegularBackgroundColor = "#71297f";
            this.skiaButton2.Size = new System.Drawing.Size(872, 150);
            this.skiaButton2.TabIndex = 1;
            this.skiaButton2.Title = "Title";
            // 
            // skiaButton3
            // 
            this.skiaButton3.Description = "Description";
            this.skiaButton3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skiaButton3.ForegroundColor = "#FFFFFF";
            this.skiaButton3.HighlightColor = "#9a4993";
            this.skiaButton3.IsHighlighted = false;
            this.skiaButton3.Location = new System.Drawing.Point(3, 315);
            this.skiaButton3.Name = "skiaButton3";
            this.skiaButton3.RegularBackgroundColor = "#71297f";
            this.skiaButton3.Size = new System.Drawing.Size(872, 150);
            this.skiaButton3.TabIndex = 2;
            this.skiaButton3.Title = "Title";
            // 
            // skiaButton4
            // 
            this.skiaButton4.Description = "Description";
            this.skiaButton4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skiaButton4.ForegroundColor = "#FFFFFF";
            this.skiaButton4.HighlightColor = "#9a4993";
            this.skiaButton4.IsHighlighted = false;
            this.skiaButton4.Location = new System.Drawing.Point(3, 471);
            this.skiaButton4.Name = "skiaButton4";
            this.skiaButton4.RegularBackgroundColor = "#71297f";
            this.skiaButton4.Size = new System.Drawing.Size(872, 150);
            this.skiaButton4.TabIndex = 3;
            this.skiaButton4.Title = "Title";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(33)))), ((int)(((byte)(122)))));
            this.ClientSize = new System.Drawing.Size(911, 558);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private PictureBox pictureBox1;
        private Label label2;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel2;
        private SkiaButton skiaButton1;
        private SkiaButton skiaButton2;
        private SkiaButton skiaButton3;
        private SkiaButton skiaButton4;
    }
}