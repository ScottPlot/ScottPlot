namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    partial class PlotsInScrollViewer
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
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.formsPlot2 = new ScottPlot.FormsPlot();
            this.formsPlot3 = new ScottPlot.FormsPlot();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rbScroll = new System.Windows.Forms.RadioButton();
            this.rbZoom = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formsPlot1
            // 
            this.formsPlot1.BackColor = System.Drawing.Color.Transparent;
            this.formsPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot1.Location = new System.Drawing.Point(3, 3);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(452, 327);
            this.formsPlot1.TabIndex = 0;
            // 
            // formsPlot2
            // 
            this.formsPlot2.BackColor = System.Drawing.Color.Transparent;
            this.formsPlot2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot2.Location = new System.Drawing.Point(3, 336);
            this.formsPlot2.Name = "formsPlot2";
            this.formsPlot2.Size = new System.Drawing.Size(452, 327);
            this.formsPlot2.TabIndex = 1;
            // 
            // formsPlot3
            // 
            this.formsPlot3.BackColor = System.Drawing.Color.Transparent;
            this.formsPlot3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot3.Location = new System.Drawing.Point(3, 669);
            this.formsPlot3.Name = "formsPlot3";
            this.formsPlot3.Size = new System.Drawing.Size(452, 328);
            this.formsPlot3.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Location = new System.Drawing.Point(12, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(485, 433);
            this.panel1.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.formsPlot3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.formsPlot1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.formsPlot2, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(458, 1000);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // rbScroll
            // 
            this.rbScroll.AutoSize = true;
            this.rbScroll.Location = new System.Drawing.Point(6, 19);
            this.rbScroll.Name = "rbScroll";
            this.rbScroll.Size = new System.Drawing.Size(97, 17);
            this.rbScroll.TabIndex = 4;
            this.rbScroll.Text = "Scroll up/down";
            this.rbScroll.UseVisualStyleBackColor = true;
            this.rbScroll.CheckedChanged += new System.EventHandler(this.rbScroll_CheckedChanged);
            // 
            // rbZoom
            // 
            this.rbZoom.AutoSize = true;
            this.rbZoom.Checked = true;
            this.rbZoom.Location = new System.Drawing.Point(109, 19);
            this.rbZoom.Name = "rbZoom";
            this.rbZoom.Size = new System.Drawing.Size(83, 17);
            this.rbZoom.TabIndex = 5;
            this.rbZoom.TabStop = true;
            this.rbZoom.Text = "Zoom in/out";
            this.rbZoom.UseVisualStyleBackColor = true;
            this.rbZoom.CheckedChanged += new System.EventHandler(this.rbZoom_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbScroll);
            this.groupBox1.Controls.Add(this.rbZoom);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(197, 44);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mouse Wheel Action";
            // 
            // PlotsInScrollViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(509, 507);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "PlotsInScrollViewer";
            this.Text = "Plots in a Scroll Viewer";
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FormsPlot formsPlot1;
        private FormsPlot formsPlot2;
        private FormsPlot formsPlot3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton rbScroll;
        private System.Windows.Forms.RadioButton rbZoom;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}