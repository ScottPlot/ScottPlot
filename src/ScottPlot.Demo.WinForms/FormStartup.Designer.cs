namespace ScottPlot.Demo.WinForms
{
    partial class FormStartup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStartup));
            this.cookbookButton = new System.Windows.Forms.Button();
            this.winFormsDemosButtom = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cookbookButton
            // 
            this.cookbookButton.Location = new System.Drawing.Point(12, 62);
            this.cookbookButton.Name = "cookbookButton";
            this.cookbookButton.Size = new System.Drawing.Size(80, 72);
            this.cookbookButton.TabIndex = 0;
            this.cookbookButton.Text = "ScottPlot Cookbook";
            this.cookbookButton.UseVisualStyleBackColor = true;
            this.cookbookButton.Click += new System.EventHandler(this.cookbookButton_Click);
            // 
            // winFormsDemosButtom
            // 
            this.winFormsDemosButtom.Location = new System.Drawing.Point(12, 140);
            this.winFormsDemosButtom.Name = "winFormsDemosButtom";
            this.winFormsDemosButtom.Size = new System.Drawing.Size(80, 72);
            this.winFormsDemosButtom.TabIndex = 1;
            this.winFormsDemosButtom.Text = "WinForms Demos";
            this.winFormsDemosButtom.UseVisualStyleBackColor = true;
            this.winFormsDemosButtom.Click += new System.EventHandler(this.winFormsDemosButtom_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "ScottPlot Demos";
            // 
            // versionLabel
            // 
            this.versionLabel.Location = new System.Drawing.Point(14, 34);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(96, 25);
            this.versionLabel.TabIndex = 3;
            this.versionLabel.Text = "version 9.9.99";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(98, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(363, 72);
            this.label3.TabIndex = 5;
            this.label3.Text = resources.GetString("label3.Text");
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(98, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(363, 72);
            this.label4.TabIndex = 6;
            this.label4.Text = "A collection of example applications demonstrating how to accomplish WinForms-spe" +
    "cific tasks (like event binding and mouse tracking).";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormStartup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 222);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.winFormsDemosButtom);
            this.Controls.Add(this.cookbookButton);
            this.Name = "FormStartup";
            this.Text = "ScottPlot Demos (WinForms)";
            this.Load += new System.EventHandler(this.FormStartup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cookbookButton;
        private System.Windows.Forms.Button winFormsDemosButtom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}