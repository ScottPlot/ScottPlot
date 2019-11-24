namespace ScottPlotDemos
{
    partial class FormFinancial
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
            this.formsPlot2 = new ScottPlot.FormsPlot();
            this.formsPlot1 = new ScottPlot.FormsPlot();
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
            // formsPlot2
            // 
            this.formsPlot2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot2.Location = new System.Drawing.Point(12, 329);
            this.formsPlot2.Name = "formsPlot2";
            this.formsPlot2.Size = new System.Drawing.Size(785, 116);
            this.formsPlot2.TabIndex = 2;
            this.formsPlot2.AxesChanged += new System.EventHandler(this.formsPlot2_AxesChanged);
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(12, 41);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(785, 282);
            this.formsPlot1.TabIndex = 0;
            this.formsPlot1.MouseMoved += new System.EventHandler(this.formsPlot1_MouseMoved);
            this.formsPlot1.AxesChanged += new System.EventHandler(this.formsPlot1_AxesChanged);
            // 
            // FormFinancial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 457);
            this.Controls.Add(this.rbOHLC);
            this.Controls.Add(this.rbCandle);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.formsPlot1);
            this.Controls.Add(this.formsPlot2);
            this.Name = "FormFinancial";
            this.Text = "ScottPlot Candlestick Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.Button button1;
        private ScottPlot.FormsPlot formsPlot2;
        private System.Windows.Forms.RadioButton rbCandle;
        private System.Windows.Forms.RadioButton rbOHLC;
    }
}

