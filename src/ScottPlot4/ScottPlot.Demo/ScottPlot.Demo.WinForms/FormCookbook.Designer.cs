namespace ScottPlot.Demo.WinForms
{
    partial class FormCookbook
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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DemoNameLabel = new System.Windows.Forms.Label();
            this.DescriptionTextbox = new System.Windows.Forms.TextBox();
            this.sourceCodeGroupbox = new System.Windows.Forms.GroupBox();
            this.sourceCodeTextbox = new System.Windows.Forms.TextBox();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.gbPlot = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbBenchmark = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.sourceCodeGroupbox.SuspendLayout();
            this.gbPlot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.SystemColors.Control;
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 16);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(294, 668);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.treeView1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 687);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Demo Plots";
            // 
            // DemoNameLabel
            // 
            this.DemoNameLabel.AutoSize = true;
            this.DemoNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DemoNameLabel.Location = new System.Drawing.Point(313, 12);
            this.DemoNameLabel.Name = "DemoNameLabel";
            this.DemoNameLabel.Size = new System.Drawing.Size(226, 25);
            this.DemoNameLabel.TabIndex = 2;
            this.DemoNameLabel.Text = "Scatter Plot Quickstart";
            // 
            // DescriptionTextbox
            // 
            this.DescriptionTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptionTextbox.BackColor = System.Drawing.SystemColors.Control;
            this.DescriptionTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DescriptionTextbox.Location = new System.Drawing.Point(318, 40);
            this.DescriptionTextbox.Multiline = true;
            this.DescriptionTextbox.Name = "DescriptionTextbox";
            this.DescriptionTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DescriptionTextbox.Size = new System.Drawing.Size(604, 53);
            this.DescriptionTextbox.TabIndex = 0;
            this.DescriptionTextbox.Text = "description...";
            // 
            // sourceCodeGroupbox
            // 
            this.sourceCodeGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceCodeGroupbox.Controls.Add(this.sourceCodeTextbox);
            this.sourceCodeGroupbox.Location = new System.Drawing.Point(318, 478);
            this.sourceCodeGroupbox.Name = "sourceCodeGroupbox";
            this.sourceCodeGroupbox.Size = new System.Drawing.Size(604, 221);
            this.sourceCodeGroupbox.TabIndex = 6;
            this.sourceCodeGroupbox.TabStop = false;
            this.sourceCodeGroupbox.Text = "Source Code";
            // 
            // sourceCodeTextbox
            // 
            this.sourceCodeTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceCodeTextbox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceCodeTextbox.Location = new System.Drawing.Point(3, 16);
            this.sourceCodeTextbox.Multiline = true;
            this.sourceCodeTextbox.Name = "sourceCodeTextbox";
            this.sourceCodeTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.sourceCodeTextbox.Size = new System.Drawing.Size(598, 202);
            this.sourceCodeTextbox.TabIndex = 0;
            this.sourceCodeTextbox.WordWrap = false;
            // 
            // formsPlot1
            // 
            this.formsPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot1.Location = new System.Drawing.Point(3, 16);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(595, 328);
            this.formsPlot1.TabIndex = 0;
            this.formsPlot1.Rendered += new System.EventHandler(this.formsPlot1_Rendered);
            // 
            // gbPlot
            // 
            this.gbPlot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbPlot.Controls.Add(this.pictureBox1);
            this.gbPlot.Controls.Add(this.formsPlot1);
            this.gbPlot.Location = new System.Drawing.Point(321, 99);
            this.gbPlot.Name = "gbPlot";
            this.gbPlot.Size = new System.Drawing.Size(601, 347);
            this.gbPlot.TabIndex = 7;
            this.gbPlot.TabStop = false;
            this.gbPlot.Text = "Interactive Plot";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(168, 101);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(344, 182);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // tbBenchmark
            // 
            this.tbBenchmark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBenchmark.BackColor = System.Drawing.SystemColors.Control;
            this.tbBenchmark.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbBenchmark.Enabled = false;
            this.tbBenchmark.Location = new System.Drawing.Point(318, 452);
            this.tbBenchmark.Name = "tbBenchmark";
            this.tbBenchmark.Size = new System.Drawing.Size(604, 13);
            this.tbBenchmark.TabIndex = 8;
            this.tbBenchmark.Text = "Rendered in...";
            // 
            // FormCookbook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 711);
            this.Controls.Add(this.tbBenchmark);
            this.Controls.Add(this.gbPlot);
            this.Controls.Add(this.DescriptionTextbox);
            this.Controls.Add(this.sourceCodeGroupbox);
            this.Controls.Add(this.DemoNameLabel);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormCookbook";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ScottPlot Demos (WinForms)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.sourceCodeGroupbox.ResumeLayout(false);
            this.sourceCodeGroupbox.PerformLayout();
            this.gbPlot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label DemoNameLabel;
        private System.Windows.Forms.GroupBox sourceCodeGroupbox;
        private FormsPlot formsPlot1;
        private System.Windows.Forms.TextBox DescriptionTextbox;
        private System.Windows.Forms.TextBox sourceCodeTextbox;
        private System.Windows.Forms.GroupBox gbPlot;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox tbBenchmark;
    }
}

