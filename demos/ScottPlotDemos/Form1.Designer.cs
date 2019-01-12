namespace ScottPlotDemos
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnWebsite = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAddSin10M = new System.Windows.Forms.Button();
            this.btnAddSin1M = new System.Windows.Forms.Button();
            this.btnAddSin1k = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonAddVline = new System.Windows.Forms.Button();
            this.btnAddHline = new System.Windows.Forms.Button();
            this.btnAddXY = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.scottPlotUC1 = new ScottPlotDev2.ScottPlotUC();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(622, 342);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnWebsite);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(154, 336);
            this.panel1.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(9, 294);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(117, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save Image";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnWebsite
            // 
            this.btnWebsite.Location = new System.Drawing.Point(9, 9);
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
            this.groupBox2.Location = new System.Drawing.Point(9, 153);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(117, 106);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Add Signal (points)";
            // 
            // btnAddSin10M
            // 
            this.btnAddSin10M.Location = new System.Drawing.Point(6, 77);
            this.btnAddSin10M.Name = "btnAddSin10M";
            this.btnAddSin10M.Size = new System.Drawing.Size(75, 23);
            this.btnAddSin10M.TabIndex = 4;
            this.btnAddSin10M.Text = "Sin (10M)";
            this.btnAddSin10M.UseVisualStyleBackColor = true;
            this.btnAddSin10M.Click += new System.EventHandler(this.btnAddSin10M_Click);
            // 
            // btnAddSin1M
            // 
            this.btnAddSin1M.Location = new System.Drawing.Point(6, 48);
            this.btnAddSin1M.Name = "btnAddSin1M";
            this.btnAddSin1M.Size = new System.Drawing.Size(75, 23);
            this.btnAddSin1M.TabIndex = 3;
            this.btnAddSin1M.Text = "Sin (1M)";
            this.btnAddSin1M.UseVisualStyleBackColor = true;
            this.btnAddSin1M.Click += new System.EventHandler(this.btnAddSin1M_Click);
            // 
            // btnAddSin1k
            // 
            this.btnAddSin1k.Location = new System.Drawing.Point(6, 19);
            this.btnAddSin1k.Name = "btnAddSin1k";
            this.btnAddSin1k.Size = new System.Drawing.Size(75, 23);
            this.btnAddSin1k.TabIndex = 2;
            this.btnAddSin1k.Text = "Sin (1k)";
            this.btnAddSin1k.UseVisualStyleBackColor = true;
            this.btnAddSin1k.Click += new System.EventHandler(this.btnAddSin1k_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(9, 265);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(117, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonAddVline);
            this.groupBox1.Controls.Add(this.btnAddHline);
            this.groupBox1.Controls.Add(this.btnAddXY);
            this.groupBox1.Location = new System.Drawing.Point(9, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(117, 106);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add Random";
            // 
            // buttonAddVline
            // 
            this.buttonAddVline.Location = new System.Drawing.Point(6, 77);
            this.buttonAddVline.Name = "buttonAddVline";
            this.buttonAddVline.Size = new System.Drawing.Size(75, 23);
            this.buttonAddVline.TabIndex = 4;
            this.buttonAddVline.Text = "VertLine";
            this.buttonAddVline.UseVisualStyleBackColor = true;
            this.buttonAddVline.Click += new System.EventHandler(this.buttonAddVline_Click);
            // 
            // btnAddHline
            // 
            this.btnAddHline.Location = new System.Drawing.Point(6, 48);
            this.btnAddHline.Name = "btnAddHline";
            this.btnAddHline.Size = new System.Drawing.Size(75, 23);
            this.btnAddHline.TabIndex = 3;
            this.btnAddHline.Text = "HorizLine";
            this.btnAddHline.UseVisualStyleBackColor = true;
            this.btnAddHline.Click += new System.EventHandler(this.btnAddHline_Click);
            // 
            // btnAddXY
            // 
            this.btnAddXY.Location = new System.Drawing.Point(6, 19);
            this.btnAddXY.Name = "btnAddXY";
            this.btnAddXY.Size = new System.Drawing.Size(75, 23);
            this.btnAddXY.TabIndex = 2;
            this.btnAddXY.Text = "XY Scatter";
            this.btnAddXY.UseVisualStyleBackColor = true;
            this.btnAddXY.Click += new System.EventHandler(this.btnAddXY_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.scottPlotUC1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(170, 10);
            this.panel2.Margin = new System.Windows.Forms.Padding(10);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(442, 322);
            this.panel2.TabIndex = 1;
            // 
            // scottPlotUC1
            // 
            this.scottPlotUC1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.scottPlotUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scottPlotUC1.Location = new System.Drawing.Point(0, 0);
            this.scottPlotUC1.Name = "scottPlotUC1";
            this.scottPlotUC1.Size = new System.Drawing.Size(440, 320);
            this.scottPlotUC1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 342);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "ScottPlot Demo: left-click-drag pan, right-click-drag zoom, middle-click auto-axi" +
    "s";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnWebsite;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAddSin10M;
        private System.Windows.Forms.Button btnAddSin1M;
        private System.Windows.Forms.Button btnAddSin1k;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonAddVline;
        private System.Windows.Forms.Button btnAddHline;
        private System.Windows.Forms.Button btnAddXY;
        private System.Windows.Forms.Panel panel2;
        private ScottPlotDev2.ScottPlotUC scottPlotUC1;
        private System.Windows.Forms.Button btnSave;
    }
}

