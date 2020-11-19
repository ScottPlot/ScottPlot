
namespace FormsPlotSandbox
{
    partial class FormExperimentalControl
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
            this.FastButton = new System.Windows.Forms.Button();
            this.SlowButton = new System.Windows.Forms.Button();
            this.formsPlotExperimental1 = new ScottPlot.WinForms.FormsPlotExperimental();
            this.ClearButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FastButton
            // 
            this.FastButton.Location = new System.Drawing.Point(12, 12);
            this.FastButton.Name = "FastButton";
            this.FastButton.Size = new System.Drawing.Size(75, 23);
            this.FastButton.TabIndex = 1;
            this.FastButton.Text = "Fast Plot";
            this.FastButton.UseVisualStyleBackColor = true;
            this.FastButton.Click += new System.EventHandler(this.FastButton_Click);
            // 
            // SlowButton
            // 
            this.SlowButton.Location = new System.Drawing.Point(93, 12);
            this.SlowButton.Name = "SlowButton";
            this.SlowButton.Size = new System.Drawing.Size(75, 23);
            this.SlowButton.TabIndex = 2;
            this.SlowButton.Text = "Slow Plot";
            this.SlowButton.UseVisualStyleBackColor = true;
            this.SlowButton.Click += new System.EventHandler(this.SlowButton_Click);
            // 
            // formsPlotExperimental1
            // 
            this.formsPlotExperimental1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlotExperimental1.Location = new System.Drawing.Point(12, 41);
            this.formsPlotExperimental1.Name = "formsPlotExperimental1";
            this.formsPlotExperimental1.Size = new System.Drawing.Size(560, 308);
            this.formsPlotExperimental1.TabIndex = 0;
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(174, 12);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(75, 23);
            this.ClearButton.TabIndex = 3;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // FormExperimentalControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.SlowButton);
            this.Controls.Add(this.FastButton);
            this.Controls.Add(this.formsPlotExperimental1);
            this.Name = "FormExperimentalControl";
            this.Text = "ScottPlot Experimental Control";
            this.Load += new System.EventHandler(this.FormExperimentalControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ScottPlot.WinForms.FormsPlotExperimental formsPlotExperimental1;
        private System.Windows.Forms.Button FastButton;
        private System.Windows.Forms.Button SlowButton;
        private System.Windows.Forms.Button ClearButton;
    }
}