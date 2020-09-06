namespace ScottPlot.WinForms
{
    partial class Menu
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.Demos = new System.Windows.Forms.ListBox();
            this.DemoDescription = new System.Windows.Forms.RichTextBox();
            this.DemoTitle = new System.Windows.Forms.Label();
            this.LaunchDemo = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.Controls.Add(this.Demos);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(179, 372);
            this.panel1.TabIndex = 0;
            // 
            // Demos
            // 
            this.Demos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Demos.FormattingEnabled = true;
            this.Demos.ItemHeight = 15;
            this.Demos.Location = new System.Drawing.Point(0, 0);
            this.Demos.Name = "Demos";
            this.Demos.Size = new System.Drawing.Size(179, 372);
            this.Demos.TabIndex = 0;
            this.Demos.SelectedIndexChanged += new System.EventHandler(this.Demos_SelectedIndexChanged);
            // 
            // DemoDescription
            // 
            this.DemoDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DemoDescription.BackColor = System.Drawing.SystemColors.Control;
            this.DemoDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DemoDescription.Location = new System.Drawing.Point(197, 69);
            this.DemoDescription.Name = "DemoDescription";
            this.DemoDescription.ReadOnly = true;
            this.DemoDescription.Size = new System.Drawing.Size(443, 315);
            this.DemoDescription.TabIndex = 1;
            this.DemoDescription.Text = "description";
            // 
            // DemoTitle
            // 
            this.DemoTitle.AutoSize = true;
            this.DemoTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.DemoTitle.Location = new System.Drawing.Point(197, 12);
            this.DemoTitle.Name = "DemoTitle";
            this.DemoTitle.Size = new System.Drawing.Size(63, 25);
            this.DemoTitle.TabIndex = 2;
            this.DemoTitle.Text = "label1";
            // 
            // LaunchDemo
            // 
            this.LaunchDemo.Location = new System.Drawing.Point(197, 40);
            this.LaunchDemo.Name = "LaunchDemo";
            this.LaunchDemo.Size = new System.Drawing.Size(63, 23);
            this.LaunchDemo.TabIndex = 3;
            this.LaunchDemo.Text = "Launch";
            this.LaunchDemo.UseVisualStyleBackColor = true;
            this.LaunchDemo.Click += new System.EventHandler(this.LaunchDemo_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 396);
            this.Controls.Add(this.LaunchDemo);
            this.Controls.Add(this.DemoTitle);
            this.Controls.Add(this.DemoDescription);
            this.Controls.Add(this.panel1);
            this.Name = "Menu";
            this.Text = "ScottPlot Demos";
            this.Load += new System.EventHandler(this.Menu_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox Demos;
        private System.Windows.Forms.RichTextBox DemoDescription;
        private System.Windows.Forms.Label DemoTitle;
        private System.Windows.Forms.Button LaunchDemo;
    }
}