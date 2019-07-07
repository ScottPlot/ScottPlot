namespace ScottPlotDemoCandlestick
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
            this.rbCandle = new System.Windows.Forms.RadioButton();
            this.rbOHLC = new System.Windows.Forms.RadioButton();
            this.scottPlotUC2 = new ScottPlot.ScottPlotUC();
            this.scottPlotUC1 = new ScottPlot.ScottPlotUC();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Randomize";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // rbCandle
            // 
            this.rbCandle.AutoSize = true;
            this.rbCandle.Checked = true;
            this.rbCandle.Location = new System.Drawing.Point(93, 12);
            this.rbCandle.Name = "rbCandle";
            this.rbCandle.Size = new System.Drawing.Size(57, 17);
            this.rbCandle.TabIndex = 3;
            this.rbCandle.TabStop = true;
            this.rbCandle.Text = "candle";
            this.rbCandle.UseVisualStyleBackColor = true;
            this.rbCandle.CheckedChanged += new System.EventHandler(this.RbCandle_CheckedChanged);
            // 
            // rbOHLC
            // 
            this.rbOHLC.AutoSize = true;
            this.rbOHLC.Location = new System.Drawing.Point(156, 12);
            this.rbOHLC.Name = "rbOHLC";
            this.rbOHLC.Size = new System.Drawing.Size(54, 17);
            this.rbOHLC.TabIndex = 4;
            this.rbOHLC.Text = "OHLC";
            this.rbOHLC.UseVisualStyleBackColor = true;
            this.rbOHLC.CheckedChanged += new System.EventHandler(this.RbOHLC_CheckedChanged);
            // 
            // scottPlotUC2
            // 
            this.scottPlotUC2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scottPlotUC2.Location = new System.Drawing.Point(12, 329);
            this.scottPlotUC2.Name = "scottPlotUC2";
            this.scottPlotUC2.Size = new System.Drawing.Size(785, 116);
            this.scottPlotUC2.TabIndex = 2;
            this.scottPlotUC2.MouseDragged += new System.EventHandler(this.ScottPlotUC2_MouseDragged);
            // 
            // scottPlotUC1
            // 
            this.scottPlotUC1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scottPlotUC1.Location = new System.Drawing.Point(12, 12);
            this.scottPlotUC1.Name = "scottPlotUC1";
            this.scottPlotUC1.Size = new System.Drawing.Size(785, 311);
            this.scottPlotUC1.TabIndex = 0;
            this.scottPlotUC1.MouseDragged += new System.EventHandler(this.ScottPlotUC1_MouseDragged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 457);
            this.Controls.Add(this.rbOHLC);
            this.Controls.Add(this.rbCandle);
            this.Controls.Add(this.scottPlotUC2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.scottPlotUC1);
            this.Name = "Form1";
            this.Text = "ScottPlot Candlestick Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScottPlot.ScottPlotUC scottPlotUC1;
        private System.Windows.Forms.Button button1;
        private ScottPlot.ScottPlotUC scottPlotUC2;
        private System.Windows.Forms.RadioButton rbCandle;
        private System.Windows.Forms.RadioButton rbOHLC;
    }
}

