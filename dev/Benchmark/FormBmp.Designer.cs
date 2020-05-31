namespace Benchmark
{
    partial class FormBmp
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
            this.TestEmpty = new System.Windows.Forms.Button();
            this.TestScatter = new System.Windows.Forms.Button();
            this.TestSignal = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.TestSignalConst = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TestEmpty
            // 
            this.TestEmpty.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TestEmpty.Location = new System.Drawing.Point(12, 22);
            this.TestEmpty.Name = "TestEmpty";
            this.TestEmpty.Size = new System.Drawing.Size(105, 37);
            this.TestEmpty.TabIndex = 0;
            this.TestEmpty.Text = "Empty";
            this.TestEmpty.UseVisualStyleBackColor = true;
            this.TestEmpty.Click += new System.EventHandler(this.TestEmpty_Click);
            // 
            // TestScatter
            // 
            this.TestScatter.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TestScatter.Location = new System.Drawing.Point(12, 65);
            this.TestScatter.Name = "TestScatter";
            this.TestScatter.Size = new System.Drawing.Size(105, 37);
            this.TestScatter.TabIndex = 3;
            this.TestScatter.Text = "Scatter";
            this.TestScatter.UseVisualStyleBackColor = true;
            this.TestScatter.Click += new System.EventHandler(this.TestScatter_Click);
            // 
            // TestSignal
            // 
            this.TestSignal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TestSignal.Location = new System.Drawing.Point(12, 108);
            this.TestSignal.Name = "TestSignal";
            this.TestSignal.Size = new System.Drawing.Size(105, 37);
            this.TestSignal.TabIndex = 4;
            this.TestSignal.Text = "Signal";
            this.TestSignal.UseVisualStyleBackColor = true;
            this.TestSignal.Click += new System.EventHandler(this.TestSignal_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.progressBar1);
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(123, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(387, 445);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Processing area";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 416);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(375, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox1.Location = new System.Drawing.Point(6, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(375, 382);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.formsPlot1);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(516, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(493, 445);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Results";
            // 
            // formsPlot1
            // 
            this.formsPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot1.Location = new System.Drawing.Point(3, 25);
            this.formsPlot1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(487, 417);
            this.formsPlot1.TabIndex = 0;
            // 
            // TestSignalConst
            // 
            this.TestSignalConst.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TestSignalConst.Location = new System.Drawing.Point(12, 151);
            this.TestSignalConst.Name = "TestSignalConst";
            this.TestSignalConst.Size = new System.Drawing.Size(105, 37);
            this.TestSignalConst.TabIndex = 5;
            this.TestSignalConst.Text = "SignalConst";
            this.TestSignalConst.UseVisualStyleBackColor = true;
            this.TestSignalConst.Click += new System.EventHandler(this.TestSignalConst_Click);
            // 
            // FormBmp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 469);
            this.Controls.Add(this.TestEmpty);
            this.Controls.Add(this.TestScatter);
            this.Controls.Add(this.TestSignal);
            this.Controls.Add(this.TestSignalConst);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "FormBmp";
            this.Text = "ScottPlot Benchmark";
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button TestEmpty;
        private System.Windows.Forms.Button TestScatter;
        private System.Windows.Forms.Button TestSignal;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button TestSignalConst;
    }
}

