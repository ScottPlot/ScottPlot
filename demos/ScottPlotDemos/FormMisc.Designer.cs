namespace ScottPlotDemos
{
    partial class FormMisc
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnWebsite = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAddSin10M = new System.Windows.Forms.Button();
            this.btnAddSin1M = new System.Windows.Forms.Button();
            this.btnAddSin1k = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAddVline = new System.Windows.Forms.Button();
            this.btnAddHline = new System.Windows.Forms.Button();
            this.btnAddXY = new System.Windows.Forms.Button();
            this.scottPlotUC1 = new ScottPlotDev2.ScottPlotUC();
            this.btnAddText = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(12, 415);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(111, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save Image";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnWebsite
            // 
            this.btnWebsite.Location = new System.Drawing.Point(12, 12);
            this.btnWebsite.Name = "btnWebsite";
            this.btnWebsite.Size = new System.Drawing.Size(111, 26);
            this.btnWebsite.TabIndex = 3;
            this.btnWebsite.Text = "ScottPlot on GitHub";
            this.btnWebsite.UseVisualStyleBackColor = true;
            this.btnWebsite.Click += new System.EventHandler(this.btnWebsite_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAddSin10M);
            this.groupBox2.Controls.Add(this.btnAddSin1M);
            this.groupBox2.Controls.Add(this.btnAddSin1k);
            this.groupBox2.Location = new System.Drawing.Point(12, 44);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(111, 106);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Add Signal (points)";
            // 
            // btnAddSin10M
            // 
            this.btnAddSin10M.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSin10M.Location = new System.Drawing.Point(6, 77);
            this.btnAddSin10M.Name = "btnAddSin10M";
            this.btnAddSin10M.Size = new System.Drawing.Size(99, 23);
            this.btnAddSin10M.TabIndex = 4;
            this.btnAddSin10M.Text = "Sin (10M)";
            this.btnAddSin10M.UseVisualStyleBackColor = true;
            this.btnAddSin10M.Click += new System.EventHandler(this.btnAddSin10M_Click);
            // 
            // btnAddSin1M
            // 
            this.btnAddSin1M.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSin1M.Location = new System.Drawing.Point(6, 48);
            this.btnAddSin1M.Name = "btnAddSin1M";
            this.btnAddSin1M.Size = new System.Drawing.Size(99, 23);
            this.btnAddSin1M.TabIndex = 3;
            this.btnAddSin1M.Text = "Sin (1M)";
            this.btnAddSin1M.UseVisualStyleBackColor = true;
            this.btnAddSin1M.Click += new System.EventHandler(this.btnAddSin1M_Click);
            // 
            // btnAddSin1k
            // 
            this.btnAddSin1k.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSin1k.Location = new System.Drawing.Point(6, 19);
            this.btnAddSin1k.Name = "btnAddSin1k";
            this.btnAddSin1k.Size = new System.Drawing.Size(99, 23);
            this.btnAddSin1k.TabIndex = 2;
            this.btnAddSin1k.Text = "Sin (1k)";
            this.btnAddSin1k.UseVisualStyleBackColor = true;
            this.btnAddSin1k.Click += new System.EventHandler(this.btnAddSin1k_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Location = new System.Drawing.Point(12, 386);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(111, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAddText);
            this.groupBox1.Controls.Add(this.btnAddVline);
            this.groupBox1.Controls.Add(this.btnAddHline);
            this.groupBox1.Controls.Add(this.btnAddXY);
            this.groupBox1.Location = new System.Drawing.Point(12, 156);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(111, 136);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add Random";
            // 
            // btnAddVline
            // 
            this.btnAddVline.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddVline.Location = new System.Drawing.Point(6, 77);
            this.btnAddVline.Name = "btnAddVline";
            this.btnAddVline.Size = new System.Drawing.Size(99, 23);
            this.btnAddVline.TabIndex = 4;
            this.btnAddVline.Text = "VertLine";
            this.btnAddVline.UseVisualStyleBackColor = true;
            this.btnAddVline.Click += new System.EventHandler(this.buttonAddVline_Click);
            // 
            // btnAddHline
            // 
            this.btnAddHline.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddHline.Location = new System.Drawing.Point(6, 48);
            this.btnAddHline.Name = "btnAddHline";
            this.btnAddHline.Size = new System.Drawing.Size(99, 23);
            this.btnAddHline.TabIndex = 3;
            this.btnAddHline.Text = "HorizLine";
            this.btnAddHline.UseVisualStyleBackColor = true;
            this.btnAddHline.Click += new System.EventHandler(this.btnAddHline_Click);
            // 
            // btnAddXY
            // 
            this.btnAddXY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddXY.Location = new System.Drawing.Point(6, 19);
            this.btnAddXY.Name = "btnAddXY";
            this.btnAddXY.Size = new System.Drawing.Size(99, 23);
            this.btnAddXY.TabIndex = 2;
            this.btnAddXY.Text = "XY Scatter";
            this.btnAddXY.UseVisualStyleBackColor = true;
            this.btnAddXY.Click += new System.EventHandler(this.btnAddXY_Click);
            // 
            // scottPlotUC1
            // 
            this.scottPlotUC1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scottPlotUC1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.scottPlotUC1.Location = new System.Drawing.Point(129, 12);
            this.scottPlotUC1.Name = "scottPlotUC1";
            this.scottPlotUC1.Size = new System.Drawing.Size(629, 426);
            this.scottPlotUC1.TabIndex = 0;
            // 
            // btnAddText
            // 
            this.btnAddText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddText.Location = new System.Drawing.Point(6, 106);
            this.btnAddText.Name = "btnAddText";
            this.btnAddText.Size = new System.Drawing.Size(99, 23);
            this.btnAddText.TabIndex = 5;
            this.btnAddText.Text = "Text";
            this.btnAddText.UseVisualStyleBackColor = true;
            this.btnAddText.Click += new System.EventHandler(this.BtnAddText_Click);
            // 
            // FormMisc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 450);
            this.Controls.Add(this.btnWebsite);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.scottPlotUC1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormMisc";
            this.Text = "ScottPlot Demo: left-click-drag pan, right-click-drag zoom, middle-click auto-axi" +
    "s";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnWebsite;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAddSin10M;
        private System.Windows.Forms.Button btnAddSin1M;
        private System.Windows.Forms.Button btnAddSin1k;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAddVline;
        private System.Windows.Forms.Button btnAddHline;
        private System.Windows.Forms.Button btnAddXY;
        private ScottPlotDev2.ScottPlotUC scottPlotUC1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAddText;
    }
}

