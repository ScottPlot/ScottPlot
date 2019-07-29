namespace ScottPlotDemoIncomingData
{
    partial class FormMain
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnGrowingArray = new System.Windows.Forms.Button();
            this.btnCircularBuffer = new System.Windows.Forms.Button();
            this.btnRollingBuffer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 20;
            // 
            // btnGrowingArray
            // 
            this.btnGrowingArray.Location = new System.Drawing.Point(12, 12);
            this.btnGrowingArray.Name = "btnGrowingArray";
            this.btnGrowingArray.Size = new System.Drawing.Size(100, 34);
            this.btnGrowingArray.TabIndex = 0;
            this.btnGrowingArray.Text = "growing array";
            this.btnGrowingArray.UseVisualStyleBackColor = true;
            this.btnGrowingArray.Click += new System.EventHandler(this.BtnGrowingArray_Click);
            // 
            // btnCircularBuffer
            // 
            this.btnCircularBuffer.Location = new System.Drawing.Point(118, 12);
            this.btnCircularBuffer.Name = "btnCircularBuffer";
            this.btnCircularBuffer.Size = new System.Drawing.Size(100, 34);
            this.btnCircularBuffer.TabIndex = 1;
            this.btnCircularBuffer.Text = "circular buffer";
            this.btnCircularBuffer.UseVisualStyleBackColor = true;
            this.btnCircularBuffer.Click += new System.EventHandler(this.BtnCircularBuffer_Click);
            // 
            // btnRollingBuffer
            // 
            this.btnRollingBuffer.Location = new System.Drawing.Point(224, 12);
            this.btnRollingBuffer.Name = "btnRollingBuffer";
            this.btnRollingBuffer.Size = new System.Drawing.Size(100, 34);
            this.btnRollingBuffer.TabIndex = 2;
            this.btnRollingBuffer.Text = "rolling buffer";
            this.btnRollingBuffer.UseVisualStyleBackColor = true;
            this.btnRollingBuffer.Click += new System.EventHandler(this.BtnRollingBuffer_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 62);
            this.Controls.Add(this.btnRollingBuffer);
            this.Controls.Add(this.btnCircularBuffer);
            this.Controls.Add(this.btnGrowingArray);
            this.Name = "Form1";
            this.Text = "ScottPlot Demo - Incoming Data";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnGrowingArray;
        private System.Windows.Forms.Button btnCircularBuffer;
        private System.Windows.Forms.Button btnRollingBuffer;
    }
}

