namespace ScottPlotDemos
{
    partial class FormPadding
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
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.btnBoth = new System.Windows.Forms.Button();
            this.btnHoriz = new System.Windows.Forms.Button();
            this.btnVert = new System.Windows.Forms.Button();
            this.btnNone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(12, 41);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(776, 397);
            this.formsPlot1.TabIndex = 0;
            // 
            // btnBoth
            // 
            this.btnBoth.Location = new System.Drawing.Point(12, 12);
            this.btnBoth.Name = "btnBoth";
            this.btnBoth.Size = new System.Drawing.Size(75, 23);
            this.btnBoth.TabIndex = 1;
            this.btnBoth.Text = "typical axes";
            this.btnBoth.UseVisualStyleBackColor = true;
            this.btnBoth.Click += new System.EventHandler(this.btnBoth_Click);
            // 
            // btnHoriz
            // 
            this.btnHoriz.Location = new System.Drawing.Point(93, 12);
            this.btnHoriz.Name = "btnHoriz";
            this.btnHoriz.Size = new System.Drawing.Size(111, 23);
            this.btnHoriz.TabIndex = 2;
            this.btnHoriz.Text = "horizontal axes only";
            this.btnHoriz.UseVisualStyleBackColor = true;
            this.btnHoriz.Click += new System.EventHandler(this.btnHoriz_Click);
            // 
            // btnVert
            // 
            this.btnVert.Location = new System.Drawing.Point(210, 12);
            this.btnVert.Name = "btnVert";
            this.btnVert.Size = new System.Drawing.Size(111, 23);
            this.btnVert.TabIndex = 3;
            this.btnVert.Text = "vertical axes only";
            this.btnVert.UseVisualStyleBackColor = true;
            this.btnVert.Click += new System.EventHandler(this.btnVert_Click);
            // 
            // btnNone
            // 
            this.btnNone.Location = new System.Drawing.Point(327, 12);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(70, 23);
            this.btnNone.TabIndex = 4;
            this.btnNone.Text = "no axes";
            this.btnNone.UseVisualStyleBackColor = true;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // FormPadding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnNone);
            this.Controls.Add(this.btnVert);
            this.Controls.Add(this.btnHoriz);
            this.Controls.Add(this.btnBoth);
            this.Controls.Add(this.formsPlot1);
            this.Name = "FormPadding";
            this.Text = "FormPadding";
            this.Load += new System.EventHandler(this.FormPadding_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.Button btnBoth;
        private System.Windows.Forms.Button btnHoriz;
        private System.Windows.Forms.Button btnVert;
        private System.Windows.Forms.Button btnNone;
    }
}