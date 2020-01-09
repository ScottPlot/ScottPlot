namespace ScottPlotDemos
{
    partial class FormRegression
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
            this.randomizeButton = new System.Windows.Forms.Button();
            this.realLineLabel = new System.Windows.Forms.Label();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.fittedLineLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // randomizeButton
            // 
            this.randomizeButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.randomizeButton.Location = new System.Drawing.Point(12, 12);
            this.randomizeButton.Name = "randomizeButton";
            this.randomizeButton.Size = new System.Drawing.Size(112, 41);
            this.randomizeButton.TabIndex = 0;
            this.randomizeButton.Text = "Randomize";
            this.randomizeButton.UseVisualStyleBackColor = true;
            this.randomizeButton.Click += new System.EventHandler(this.randomizeButton_Click);
            // 
            // realLineLabel
            // 
            this.realLineLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.realLineLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.realLineLabel.Location = new System.Drawing.Point(3, 16);
            this.realLineLabel.Name = "realLineLabel";
            this.realLineLabel.Size = new System.Drawing.Size(280, 22);
            this.realLineLabel.TabIndex = 3;
            this.realLineLabel.Text = "asdf";
            this.realLineLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(12, 59);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(776, 379);
            this.formsPlot1.TabIndex = 4;
            // 
            // fittedLineLabel
            // 
            this.fittedLineLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fittedLineLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fittedLineLabel.Location = new System.Drawing.Point(3, 16);
            this.fittedLineLabel.Name = "fittedLineLabel";
            this.fittedLineLabel.Size = new System.Drawing.Size(360, 22);
            this.fittedLineLabel.TabIndex = 5;
            this.fittedLineLabel.Text = "asdf";
            this.fittedLineLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.realLineLabel);
            this.groupBox1.Location = new System.Drawing.Point(130, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(286, 41);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Original Data";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.fittedLineLabel);
            this.groupBox2.Location = new System.Drawing.Point(422, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(366, 41);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Fitted Line";
            // 
            // FormRegression
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.formsPlot1);
            this.Controls.Add(this.randomizeButton);
            this.Name = "FormRegression";
            this.Text = "Regression Demo";
            this.Load += new System.EventHandler(this.FormRegression_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button randomizeButton;
        private System.Windows.Forms.Label realLineLabel;
        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.Label fittedLineLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}