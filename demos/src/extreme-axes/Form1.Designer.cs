namespace extreme_axes
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
            this.btnNormal = new System.Windows.Forms.Button();
            this.btnBig = new System.Windows.Forms.Button();
            this.btnSmall = new System.Windows.Forms.Button();
            this.buttonBigOffset = new System.Windows.Forms.Button();
            this.btnSmallOffset = new System.Windows.Forms.Button();
            this.btnWav = new System.Windows.Forms.Button();
            this.scottPlotUC1 = new ScottPlot.FormsPlot();
            this.SuspendLayout();
            // 
            // btnNormal
            // 
            this.btnNormal.Location = new System.Drawing.Point(12, 12);
            this.btnNormal.Name = "btnNormal";
            this.btnNormal.Size = new System.Drawing.Size(54, 23);
            this.btnNormal.TabIndex = 1;
            this.btnNormal.Text = "normal";
            this.btnNormal.UseVisualStyleBackColor = true;
            this.btnNormal.Click += new System.EventHandler(this.BtnNormal_Click);
            // 
            // btnBig
            // 
            this.btnBig.Location = new System.Drawing.Point(72, 12);
            this.btnBig.Name = "btnBig";
            this.btnBig.Size = new System.Drawing.Size(48, 23);
            this.btnBig.TabIndex = 2;
            this.btnBig.Text = "big";
            this.btnBig.UseVisualStyleBackColor = true;
            this.btnBig.Click += new System.EventHandler(this.BtnBig_Click);
            // 
            // btnSmall
            // 
            this.btnSmall.Location = new System.Drawing.Point(126, 12);
            this.btnSmall.Name = "btnSmall";
            this.btnSmall.Size = new System.Drawing.Size(55, 23);
            this.btnSmall.TabIndex = 3;
            this.btnSmall.Text = "small";
            this.btnSmall.UseVisualStyleBackColor = true;
            this.btnSmall.Click += new System.EventHandler(this.BtnSmall_Click);
            // 
            // buttonBigOffset
            // 
            this.buttonBigOffset.Location = new System.Drawing.Point(187, 12);
            this.buttonBigOffset.Name = "buttonBigOffset";
            this.buttonBigOffset.Size = new System.Drawing.Size(75, 23);
            this.buttonBigOffset.TabIndex = 4;
            this.buttonBigOffset.Text = "big + offset";
            this.buttonBigOffset.UseVisualStyleBackColor = true;
            this.buttonBigOffset.Click += new System.EventHandler(this.ButtonBigOffset_Click);
            // 
            // btnSmallOffset
            // 
            this.btnSmallOffset.Location = new System.Drawing.Point(268, 12);
            this.btnSmallOffset.Name = "btnSmallOffset";
            this.btnSmallOffset.Size = new System.Drawing.Size(103, 23);
            this.btnSmallOffset.TabIndex = 5;
            this.btnSmallOffset.Text = "small + offset";
            this.btnSmallOffset.UseVisualStyleBackColor = true;
            this.btnSmallOffset.Click += new System.EventHandler(this.BtnSmallOffset_Click);
            // 
            // btnWav
            // 
            this.btnWav.Location = new System.Drawing.Point(377, 12);
            this.btnWav.Name = "btnWav";
            this.btnWav.Size = new System.Drawing.Size(48, 23);
            this.btnWav.TabIndex = 6;
            this.btnWav.Text = "wav";
            this.btnWav.UseVisualStyleBackColor = true;
            this.btnWav.Click += new System.EventHandler(this.BtnWav_Click);
            // 
            // scottPlotUC1
            // 
            this.scottPlotUC1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scottPlotUC1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scottPlotUC1.Location = new System.Drawing.Point(12, 41);
            this.scottPlotUC1.Name = "scottPlotUC1";
            this.scottPlotUC1.Size = new System.Drawing.Size(750, 386);
            this.scottPlotUC1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 439);
            this.Controls.Add(this.btnWav);
            this.Controls.Add(this.btnSmallOffset);
            this.Controls.Add(this.buttonBigOffset);
            this.Controls.Add(this.btnSmall);
            this.Controls.Add(this.btnBig);
            this.Controls.Add(this.btnNormal);
            this.Controls.Add(this.scottPlotUC1);
            this.Name = "Form1";
            this.Text = "ScottPlot Demo - extreme axis tester";
            this.ResumeLayout(false);

        }

        #endregion

        private ScottPlot.FormsPlot scottPlotUC1;
        private System.Windows.Forms.Button btnNormal;
        private System.Windows.Forms.Button btnBig;
        private System.Windows.Forms.Button btnSmall;
        private System.Windows.Forms.Button buttonBigOffset;
        private System.Windows.Forms.Button btnSmallOffset;
        private System.Windows.Forms.Button btnWav;
    }
}

