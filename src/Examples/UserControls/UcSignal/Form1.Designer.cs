namespace UcSignal
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.ucSignal1 = new ScottPlot.ucSignal();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ucSignal1
            // 
            this.ucSignal1.AutoSize = true;
            this.ucSignal1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSignal1.Location = new System.Drawing.Point(0, 0);
            this.ucSignal1.Name = "ucSignal1";
            this.ucSignal1.Size = new System.Drawing.Size(1062, 631);
            this.ucSignal1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 631);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ucSignal1);
            this.Name = "Form1";
            this.Text = "ScottPlot ucSignal Demo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScottPlot.ucSignal ucSignal1;
        private System.Windows.Forms.Button button1;
    }
}

