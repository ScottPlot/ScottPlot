namespace ScottPlotDemos
{
    partial class FormStyle
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
            this.btnDefault = new System.Windows.Forms.Button();
            this.btnDark = new System.Windows.Forms.Button();
            this.btnWindows = new System.Windows.Forms.Button();
            this.btnSmallAxes = new System.Windows.Forms.Button();
            this.btnBorderless = new System.Windows.Forms.Button();
            this.scottPlotUC1 = new ScottPlotDev2.ScottPlotUC();
            this.SuspendLayout();
            // 
            // btnDefault
            // 
            this.btnDefault.Location = new System.Drawing.Point(12, 12);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(75, 23);
            this.btnDefault.TabIndex = 1;
            this.btnDefault.Text = "Default";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnLight_Click);
            // 
            // btnDark
            // 
            this.btnDark.Location = new System.Drawing.Point(12, 41);
            this.btnDark.Name = "btnDark";
            this.btnDark.Size = new System.Drawing.Size(75, 23);
            this.btnDark.TabIndex = 2;
            this.btnDark.Text = "Dark";
            this.btnDark.UseVisualStyleBackColor = true;
            this.btnDark.Click += new System.EventHandler(this.btnDark_Click);
            // 
            // btnWindows
            // 
            this.btnWindows.Location = new System.Drawing.Point(12, 70);
            this.btnWindows.Name = "btnWindows";
            this.btnWindows.Size = new System.Drawing.Size(75, 23);
            this.btnWindows.TabIndex = 3;
            this.btnWindows.Text = "Windows";
            this.btnWindows.UseVisualStyleBackColor = true;
            this.btnWindows.Click += new System.EventHandler(this.btnWindows_Click);
            // 
            // btnSmallAxes
            // 
            this.btnSmallAxes.Location = new System.Drawing.Point(12, 99);
            this.btnSmallAxes.Name = "btnSmallAxes";
            this.btnSmallAxes.Size = new System.Drawing.Size(75, 23);
            this.btnSmallAxes.TabIndex = 4;
            this.btnSmallAxes.Text = "Small Axes";
            this.btnSmallAxes.UseVisualStyleBackColor = true;
            this.btnSmallAxes.Click += new System.EventHandler(this.btnSmallAxes_Click);
            // 
            // btnBorderless
            // 
            this.btnBorderless.Location = new System.Drawing.Point(12, 128);
            this.btnBorderless.Name = "btnBorderless";
            this.btnBorderless.Size = new System.Drawing.Size(75, 23);
            this.btnBorderless.TabIndex = 5;
            this.btnBorderless.Text = "Borderless";
            this.btnBorderless.UseVisualStyleBackColor = true;
            this.btnBorderless.Click += new System.EventHandler(this.btnBorderless_Click);
            // 
            // scottPlotUC1
            // 
            this.scottPlotUC1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scottPlotUC1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.scottPlotUC1.Location = new System.Drawing.Point(93, 12);
            this.scottPlotUC1.Name = "scottPlotUC1";
            this.scottPlotUC1.Size = new System.Drawing.Size(534, 333);
            this.scottPlotUC1.TabIndex = 0;
            // 
            // FormStyle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 359);
            this.Controls.Add(this.btnBorderless);
            this.Controls.Add(this.btnSmallAxes);
            this.Controls.Add(this.btnWindows);
            this.Controls.Add(this.btnDark);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.scottPlotUC1);
            this.Name = "FormStyle";
            this.Text = "FormStyle";
            this.ResumeLayout(false);

        }

        #endregion

        private ScottPlotDev2.ScottPlotUC scottPlotUC1;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.Button btnDark;
        private System.Windows.Forms.Button btnWindows;
        private System.Windows.Forms.Button btnSmallAxes;
        private System.Windows.Forms.Button btnBorderless;
    }
}