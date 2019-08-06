namespace ScottPlotDemo
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
            this.btnClear = new System.Windows.Forms.Button();
            this.btnScatterRandom = new System.Windows.Forms.Button();
            this.btnScatterSin = new System.Windows.Forms.Button();
            this.btnMarker = new System.Windows.Forms.Button();
            this.btnSignal1k = new System.Windows.Forms.Button();
            this.btnSignal100k = new System.Windows.Forms.Button();
            this.btnSignal1m = new System.Windows.Forms.Button();
            this.btnSignal100m = new System.Windows.Forms.Button();
            this.btnVline = new System.Windows.Forms.Button();
            this.btnHline = new System.Windows.Forms.Button();
            this.cbBenchmark = new System.Windows.Forms.CheckBox();
            this.cbDark = new System.Windows.Forms.CheckBox();
            this.btnText = new System.Windows.Forms.Button();
            this.cbAntiAliasData = new System.Windows.Forms.CheckBox();
            this.cbAntiAliasFigure = new System.Windows.Forms.CheckBox();
            this.scottPlotUC1 = new ScottPlot.ScottPlotUC();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(771, 11);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // btnScatterRandom
            // 
            this.btnScatterRandom.Location = new System.Drawing.Point(305, 40);
            this.btnScatterRandom.Name = "btnScatterRandom";
            this.btnScatterRandom.Size = new System.Drawing.Size(126, 23);
            this.btnScatterRandom.TabIndex = 2;
            this.btnScatterRandom.Text = "XY Scatter Random";
            this.btnScatterRandom.UseVisualStyleBackColor = true;
            this.btnScatterRandom.Click += new System.EventHandler(this.BtnScatterRandom_Click);
            // 
            // btnScatterSin
            // 
            this.btnScatterSin.Location = new System.Drawing.Point(305, 11);
            this.btnScatterSin.Name = "btnScatterSin";
            this.btnScatterSin.Size = new System.Drawing.Size(126, 23);
            this.btnScatterSin.TabIndex = 3;
            this.btnScatterSin.Text = "XY Scatter Sin";
            this.btnScatterSin.UseVisualStyleBackColor = true;
            this.btnScatterSin.Click += new System.EventHandler(this.BtnScatterSin_Click);
            // 
            // btnMarker
            // 
            this.btnMarker.Location = new System.Drawing.Point(199, 12);
            this.btnMarker.Name = "btnMarker";
            this.btnMarker.Size = new System.Drawing.Size(100, 23);
            this.btnMarker.TabIndex = 4;
            this.btnMarker.Text = "Random Point";
            this.btnMarker.UseVisualStyleBackColor = true;
            this.btnMarker.Click += new System.EventHandler(this.BtnMarker_Click);
            // 
            // btnSignal1k
            // 
            this.btnSignal1k.Location = new System.Drawing.Point(507, 11);
            this.btnSignal1k.Name = "btnSignal1k";
            this.btnSignal1k.Size = new System.Drawing.Size(126, 23);
            this.btnSignal1k.TabIndex = 5;
            this.btnSignal1k.Text = "Signal (1k points)";
            this.btnSignal1k.UseVisualStyleBackColor = true;
            this.btnSignal1k.Click += new System.EventHandler(this.BtnSignal1k_Click);
            // 
            // btnSignal100k
            // 
            this.btnSignal100k.Location = new System.Drawing.Point(507, 40);
            this.btnSignal100k.Name = "btnSignal100k";
            this.btnSignal100k.Size = new System.Drawing.Size(126, 23);
            this.btnSignal100k.TabIndex = 6;
            this.btnSignal100k.Text = "Signal (100k points)";
            this.btnSignal100k.UseVisualStyleBackColor = true;
            this.btnSignal100k.Click += new System.EventHandler(this.BtnSignal100k_Click);
            // 
            // btnSignal1m
            // 
            this.btnSignal1m.Location = new System.Drawing.Point(639, 11);
            this.btnSignal1m.Name = "btnSignal1m";
            this.btnSignal1m.Size = new System.Drawing.Size(126, 23);
            this.btnSignal1m.TabIndex = 7;
            this.btnSignal1m.Text = "Signal (1M points)";
            this.btnSignal1m.UseVisualStyleBackColor = true;
            this.btnSignal1m.Click += new System.EventHandler(this.BtnSignal1m_Click);
            // 
            // btnSignal100m
            // 
            this.btnSignal100m.Location = new System.Drawing.Point(639, 40);
            this.btnSignal100m.Name = "btnSignal100m";
            this.btnSignal100m.Size = new System.Drawing.Size(126, 23);
            this.btnSignal100m.TabIndex = 8;
            this.btnSignal100m.Text = "Signal (10M points)";
            this.btnSignal100m.UseVisualStyleBackColor = true;
            this.btnSignal100m.Click += new System.EventHandler(this.BtnSignal100m_Click);
            // 
            // btnVline
            // 
            this.btnVline.Location = new System.Drawing.Point(437, 11);
            this.btnVline.Name = "btnVline";
            this.btnVline.Size = new System.Drawing.Size(64, 23);
            this.btnVline.TabIndex = 9;
            this.btnVline.Text = "VLine";
            this.btnVline.UseVisualStyleBackColor = true;
            this.btnVline.Click += new System.EventHandler(this.BtnVline_Click);
            // 
            // btnHline
            // 
            this.btnHline.Location = new System.Drawing.Point(437, 40);
            this.btnHline.Name = "btnHline";
            this.btnHline.Size = new System.Drawing.Size(64, 23);
            this.btnHline.TabIndex = 10;
            this.btnHline.Text = "HLine";
            this.btnHline.UseVisualStyleBackColor = true;
            this.btnHline.Click += new System.EventHandler(this.BtnHline_Click);
            // 
            // cbBenchmark
            // 
            this.cbBenchmark.AutoSize = true;
            this.cbBenchmark.Location = new System.Drawing.Point(12, 16);
            this.cbBenchmark.Name = "cbBenchmark";
            this.cbBenchmark.Size = new System.Drawing.Size(79, 17);
            this.cbBenchmark.TabIndex = 11;
            this.cbBenchmark.Text = "benchmark";
            this.cbBenchmark.UseVisualStyleBackColor = true;
            this.cbBenchmark.CheckedChanged += new System.EventHandler(this.CbBenchmark_CheckedChanged);
            // 
            // cbDark
            // 
            this.cbDark.AutoSize = true;
            this.cbDark.Location = new System.Drawing.Point(12, 45);
            this.cbDark.Name = "cbDark";
            this.cbDark.Size = new System.Drawing.Size(71, 17);
            this.cbDark.TabIndex = 12;
            this.cbDark.Text = "dark style";
            this.cbDark.UseVisualStyleBackColor = true;
            this.cbDark.CheckedChanged += new System.EventHandler(this.CbDark_CheckedChanged);
            // 
            // btnText
            // 
            this.btnText.Location = new System.Drawing.Point(199, 40);
            this.btnText.Name = "btnText";
            this.btnText.Size = new System.Drawing.Size(100, 23);
            this.btnText.TabIndex = 13;
            this.btnText.Text = "Random Text";
            this.btnText.UseVisualStyleBackColor = true;
            this.btnText.Click += new System.EventHandler(this.BtnText_Click);
            // 
            // cbAntiAliasData
            // 
            this.cbAntiAliasData.AutoSize = true;
            this.cbAntiAliasData.Checked = true;
            this.cbAntiAliasData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAntiAliasData.Location = new System.Drawing.Point(97, 45);
            this.cbAntiAliasData.Name = "cbAntiAliasData";
            this.cbAntiAliasData.Size = new System.Drawing.Size(91, 17);
            this.cbAntiAliasData.TabIndex = 14;
            this.cbAntiAliasData.Text = "anti-alias data";
            this.cbAntiAliasData.UseVisualStyleBackColor = true;
            this.cbAntiAliasData.CheckedChanged += new System.EventHandler(this.CbAntiAliasData_CheckedChanged);
            // 
            // cbAntiAliasFigure
            // 
            this.cbAntiAliasFigure.AutoSize = true;
            this.cbAntiAliasFigure.Checked = true;
            this.cbAntiAliasFigure.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAntiAliasFigure.Location = new System.Drawing.Point(97, 16);
            this.cbAntiAliasFigure.Name = "cbAntiAliasFigure";
            this.cbAntiAliasFigure.Size = new System.Drawing.Size(96, 17);
            this.cbAntiAliasFigure.TabIndex = 15;
            this.cbAntiAliasFigure.Text = "anti-alias figure";
            this.cbAntiAliasFigure.UseVisualStyleBackColor = true;
            this.cbAntiAliasFigure.CheckedChanged += new System.EventHandler(this.CbAntiAliasFigure_CheckedChanged);
            // 
            // scottPlotUC1
            // 
            this.scottPlotUC1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scottPlotUC1.Location = new System.Drawing.Point(12, 70);
            this.scottPlotUC1.Name = "scottPlotUC1";
            this.scottPlotUC1.Size = new System.Drawing.Size(836, 389);
            this.scottPlotUC1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 471);
            this.Controls.Add(this.cbAntiAliasFigure);
            this.Controls.Add(this.cbAntiAliasData);
            this.Controls.Add(this.btnText);
            this.Controls.Add(this.cbDark);
            this.Controls.Add(this.cbBenchmark);
            this.Controls.Add(this.btnHline);
            this.Controls.Add(this.btnVline);
            this.Controls.Add(this.btnSignal100m);
            this.Controls.Add(this.btnSignal1m);
            this.Controls.Add(this.btnSignal100k);
            this.Controls.Add(this.btnSignal1k);
            this.Controls.Add(this.btnMarker);
            this.Controls.Add(this.btnScatterSin);
            this.Controls.Add(this.btnScatterRandom);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.scottPlotUC1);
            this.Name = "Form1";
            this.Text = "ScottPlot Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScottPlot.ScottPlotUC scottPlotUC1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnScatterRandom;
        private System.Windows.Forms.Button btnScatterSin;
        private System.Windows.Forms.Button btnMarker;
        private System.Windows.Forms.Button btnSignal1k;
        private System.Windows.Forms.Button btnSignal100k;
        private System.Windows.Forms.Button btnSignal1m;
        private System.Windows.Forms.Button btnSignal100m;
        private System.Windows.Forms.Button btnVline;
        private System.Windows.Forms.Button btnHline;
        private System.Windows.Forms.CheckBox cbBenchmark;
        private System.Windows.Forms.CheckBox cbDark;
        private System.Windows.Forms.Button btnText;
        private System.Windows.Forms.CheckBox cbAntiAliasData;
        private System.Windows.Forms.CheckBox cbAntiAliasFigure;
    }
}

