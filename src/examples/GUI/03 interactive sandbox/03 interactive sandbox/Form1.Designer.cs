namespace _03_interactive_sandbox
{
    partial class form_interactive
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
            this.btnNewData = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ucInteractive1 = new ScottPlot.ucInteractive();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.richTextBox1);
            this.panel1.Controls.Add(this.btnNewData);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(826, 74);
            this.panel1.TabIndex = 5;
            // 
            // btnNewData
            // 
            this.btnNewData.Location = new System.Drawing.Point(337, 12);
            this.btnNewData.Name = "btnNewData";
            this.btnNewData.Size = new System.Drawing.Size(94, 62);
            this.btnNewData.TabIndex = 7;
            this.btnNewData.Text = "new data";
            this.btnNewData.UseVisualStyleBackColor = true;
            this.btnNewData.Click += new System.EventHandler(this.btnNewData_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(245, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "Interactive Data Visualization";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 46);
            this.label1.TabIndex = 3;
            this.label1.Text = "ScottPlot";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ucInteractive1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 74);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(826, 441);
            this.panel2.TabIndex = 6;
            // 
            // ucInteractive1
            // 
            this.ucInteractive1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucInteractive1.Location = new System.Drawing.Point(10, 10);
            this.ucInteractive1.Name = "ucInteractive1";
            this.ucInteractive1.Size = new System.Drawing.Size(806, 421);
            this.ucInteractive1.TabIndex = 0;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.richTextBox1.Enabled = false;
            this.richTextBox1.Location = new System.Drawing.Point(437, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.Size = new System.Drawing.Size(259, 62);
            this.richTextBox1.TabIndex = 8;
            this.richTextBox1.Text = "points: 10,000\nrender time: 23.123 ms\nframe rate: 42.34 Hz";
            // 
            // form_interactive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 515);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "form_interactive";
            this.Text = "ScottPlot";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private ScottPlot.ucInteractive ucInteractive1;
        private System.Windows.Forms.Button btnNewData;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

