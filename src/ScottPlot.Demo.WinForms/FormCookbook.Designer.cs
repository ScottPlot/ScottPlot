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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DemoNameLabel = new System.Windows.Forms.Label();
            this.DemoFileLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DescriptionTextbox = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.PerformanceLabel = new System.Windows.Forms.Label();
            this.DemoFileUrl = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 16);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(244, 496);
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
            this.groupBox1.Size = new System.Drawing.Size(250, 515);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Demo Plots";
            // 
            // DemoNameLabel
            // 
            this.DemoNameLabel.AutoSize = true;
            this.DemoNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DemoNameLabel.Location = new System.Drawing.Point(268, 12);
            this.DemoNameLabel.Name = "DemoNameLabel";
            this.DemoNameLabel.Size = new System.Drawing.Size(226, 25);
            this.DemoNameLabel.TabIndex = 2;
            this.DemoNameLabel.Text = "Scatter Plot Quickstart";
            // 
            // DemoFileLabel
            // 
            this.DemoFileLabel.AutoSize = true;
            this.DemoFileLabel.ForeColor = System.Drawing.Color.Gray;
            this.DemoFileLabel.Location = new System.Drawing.Point(270, 37);
            this.DemoFileLabel.Name = "DemoFileLabel";
            this.DemoFileLabel.Size = new System.Drawing.Size(317, 13);
            this.DemoFileLabel.TabIndex = 3;
            this.DemoFileLabel.Text = "dotnet\\shared\\Microsoft.WindowsDesktop.App\\3.1.1 (Quickstart)";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.DescriptionTextbox);
            this.groupBox2.Location = new System.Drawing.Point(268, 67);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(567, 78);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Description";
            // 
            // DescriptionTextbox
            // 
            this.DescriptionTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptionTextbox.BackColor = System.Drawing.SystemColors.Control;
            this.DescriptionTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DescriptionTextbox.Location = new System.Drawing.Point(6, 19);
            this.DescriptionTextbox.Multiline = true;
            this.DescriptionTextbox.Name = "DescriptionTextbox";
            this.DescriptionTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DescriptionTextbox.Size = new System.Drawing.Size(555, 53);
            this.DescriptionTextbox.TabIndex = 0;
            this.DescriptionTextbox.Text = "description...";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.formsPlot1);
            this.groupBox3.Location = new System.Drawing.Point(268, 151);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(567, 334);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Interactive Plot";
            // 
            // formsPlot1
            // 
            this.formsPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot1.Location = new System.Drawing.Point(3, 16);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(561, 315);
            this.formsPlot1.TabIndex = 0;
            this.formsPlot1.Rendered += new System.EventHandler(this.formsPlot1_Rendered);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.PerformanceLabel);
            this.groupBox4.Location = new System.Drawing.Point(268, 491);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(567, 36);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Performance";
            // 
            // PerformanceLabel
            // 
            this.PerformanceLabel.AutoSize = true;
            this.PerformanceLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PerformanceLabel.ForeColor = System.Drawing.Color.Gray;
            this.PerformanceLabel.Location = new System.Drawing.Point(3, 16);
            this.PerformanceLabel.Name = "PerformanceLabel";
            this.PerformanceLabel.Size = new System.Drawing.Size(317, 13);
            this.PerformanceLabel.TabIndex = 4;
            this.PerformanceLabel.Text = "dotnet\\shared\\Microsoft.WindowsDesktop.App\\3.1.1 (Quickstart)";
            // 
            // DemoFileUrl
            // 
            this.DemoFileUrl.AutoSize = true;
            this.DemoFileUrl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DemoFileUrl.ForeColor = System.Drawing.Color.Blue;
            this.DemoFileUrl.Location = new System.Drawing.Point(271, 51);
            this.DemoFileUrl.Name = "DemoFileUrl";
            this.DemoFileUrl.Size = new System.Drawing.Size(317, 13);
            this.DemoFileUrl.TabIndex = 7;
            this.DemoFileUrl.Text = "dotnet\\shared\\Microsoft.WindowsDesktop.App\\3.1.1 (Quickstart)";
            this.DemoFileUrl.Click += new System.EventHandler(this.DemoFileUrl_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 539);
            this.Controls.Add(this.DemoFileUrl);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.DemoFileLabel);
            this.Controls.Add(this.DemoNameLabel);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "ScottPlot Demos (WinForms)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label DemoNameLabel;
        private System.Windows.Forms.Label DemoFileLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label PerformanceLabel;
        private FormsPlot formsPlot1;
        private System.Windows.Forms.TextBox DescriptionTextbox;
        private System.Windows.Forms.Label DemoFileUrl;
    }
}

