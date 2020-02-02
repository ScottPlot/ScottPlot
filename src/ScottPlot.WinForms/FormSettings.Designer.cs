namespace ScottPlot.UserControls
{
    partial class FormSettings
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
            this.tbY2 = new System.Windows.Forms.TextBox();
            this.tbY1 = new System.Windows.Forms.TextBox();
            this.tbX2 = new System.Windows.Forms.TextBox();
            this.tbX1 = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbQualityLowWhileDragging = new System.Windows.Forms.CheckBox();
            this.rbQualityHigh = new System.Windows.Forms.RadioButton();
            this.rbQualityLow = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tbLabel = new System.Windows.Forms.TextBox();
            this.lbPlotObjects = new System.Windows.Forms.ListBox();
            this.btnExportCSV = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbYdateTime = new System.Windows.Forms.CheckBox();
            this.cbYminor = new System.Windows.Forms.CheckBox();
            this.btnFitDataY = new System.Windows.Forms.Button();
            this.tbYlabel = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cbXdateTime = new System.Windows.Forms.CheckBox();
            this.cbXminor = new System.Windows.Forms.CheckBox();
            this.btnFitDataX = new System.Windows.Forms.Button();
            this.tbXlabel = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbGrid = new System.Windows.Forms.CheckBox();
            this.cbTicksMult = new System.Windows.Forms.CheckBox();
            this.cbTicksOffset = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbLegend = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cbStyle = new System.Windows.Forms.ComboBox();
            this.btnTighten = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.btnCopyCSV = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(618, 334);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(59, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Apply";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // tbY2
            // 
            this.tbY2.Location = new System.Drawing.Point(64, 44);
            this.tbY2.Name = "tbY2";
            this.tbY2.Size = new System.Drawing.Size(100, 20);
            this.tbY2.TabIndex = 4;
            // 
            // tbY1
            // 
            this.tbY1.Location = new System.Drawing.Point(64, 70);
            this.tbY1.Name = "tbY1";
            this.tbY1.Size = new System.Drawing.Size(100, 20);
            this.tbY1.TabIndex = 5;
            // 
            // tbX2
            // 
            this.tbX2.Location = new System.Drawing.Point(64, 44);
            this.tbX2.Name = "tbX2";
            this.tbX2.Size = new System.Drawing.Size(100, 20);
            this.tbX2.TabIndex = 3;
            // 
            // tbX1
            // 
            this.tbX1.Location = new System.Drawing.Point(64, 70);
            this.tbX1.Name = "tbX1";
            this.tbX1.Size = new System.Drawing.Size(100, 20);
            this.tbX1.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbQualityLowWhileDragging);
            this.groupBox3.Controls.Add(this.rbQualityHigh);
            this.groupBox3.Controls.Add(this.rbQualityLow);
            this.groupBox3.Location = new System.Drawing.Point(263, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(245, 47);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Image Quality";
            // 
            // cbQualityLowWhileDragging
            // 
            this.cbQualityLowWhileDragging.AutoSize = true;
            this.cbQualityLowWhileDragging.Location = new System.Drawing.Point(104, 19);
            this.cbQualityLowWhileDragging.Name = "cbQualityLowWhileDragging";
            this.cbQualityLowWhileDragging.Size = new System.Drawing.Size(113, 17);
            this.cbQualityLowWhileDragging.TabIndex = 2;
            this.cbQualityLowWhileDragging.Text = "low while dragging";
            this.cbQualityLowWhileDragging.UseVisualStyleBackColor = true;
            // 
            // rbQualityHigh
            // 
            this.rbQualityHigh.AutoSize = true;
            this.rbQualityHigh.Location = new System.Drawing.Point(53, 19);
            this.rbQualityHigh.Name = "rbQualityHigh";
            this.rbQualityHigh.Size = new System.Drawing.Size(45, 17);
            this.rbQualityHigh.TabIndex = 1;
            this.rbQualityHigh.TabStop = true;
            this.rbQualityHigh.Text = "high";
            this.rbQualityHigh.UseVisualStyleBackColor = true;
            // 
            // rbQualityLow
            // 
            this.rbQualityLow.AutoSize = true;
            this.rbQualityLow.Location = new System.Drawing.Point(6, 19);
            this.rbQualityLow.Name = "rbQualityLow";
            this.rbQualityLow.Size = new System.Drawing.Size(41, 17);
            this.rbQualityLow.TabIndex = 0;
            this.rbQualityLow.TabStop = true;
            this.rbQualityLow.Text = "low";
            this.rbQualityLow.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.groupBox9);
            this.groupBox4.Controls.Add(this.groupBox8);
            this.groupBox4.Controls.Add(this.lbPlotObjects);
            this.groupBox4.Location = new System.Drawing.Point(525, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(217, 316);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Plottable Objects";
            // 
            // tbLabel
            // 
            this.tbLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLabel.Enabled = false;
            this.tbLabel.Location = new System.Drawing.Point(6, 19);
            this.tbLabel.Name = "tbLabel";
            this.tbLabel.Size = new System.Drawing.Size(193, 20);
            this.tbLabel.TabIndex = 6;
            this.tbLabel.TextChanged += new System.EventHandler(this.TbLabel_TextChanged);
            // 
            // lbPlotObjects
            // 
            this.lbPlotObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPlotObjects.FormattingEnabled = true;
            this.lbPlotObjects.Location = new System.Drawing.Point(6, 19);
            this.lbPlotObjects.Name = "lbPlotObjects";
            this.lbPlotObjects.ScrollAlwaysVisible = true;
            this.lbPlotObjects.Size = new System.Drawing.Size(205, 173);
            this.lbPlotObjects.TabIndex = 4;
            this.lbPlotObjects.SelectedIndexChanged += new System.EventHandler(this.LbPlotObjects_SelectedIndexChanged);
            // 
            // btnExportCSV
            // 
            this.btnExportCSV.Enabled = false;
            this.btnExportCSV.Location = new System.Drawing.Point(88, 19);
            this.btnExportCSV.Name = "btnExportCSV";
            this.btnExportCSV.Size = new System.Drawing.Size(76, 23);
            this.btnExportCSV.TabIndex = 3;
            this.btnExportCSV.Text = "Save CSV";
            this.btnExportCSV.UseVisualStyleBackColor = true;
            this.btnExportCSV.Click += new System.EventHandler(this.BtnExportCSV_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cbYdateTime);
            this.groupBox5.Controls.Add(this.cbYminor);
            this.groupBox5.Controls.Add(this.btnFitDataY);
            this.groupBox5.Controls.Add(this.tbYlabel);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.tbY1);
            this.groupBox5.Controls.Add(this.tbY2);
            this.groupBox5.Location = new System.Drawing.Point(12, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(245, 170);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Vertical Axis";
            // 
            // cbYdateTime
            // 
            this.cbYdateTime.AutoSize = true;
            this.cbYdateTime.Location = new System.Drawing.Point(64, 148);
            this.cbYdateTime.Name = "cbYdateTime";
            this.cbYdateTime.Size = new System.Drawing.Size(122, 17);
            this.cbYdateTime.TabIndex = 15;
            this.cbYdateTime.Text = "date and time format";
            this.cbYdateTime.UseVisualStyleBackColor = true;
            // 
            // cbYminor
            // 
            this.cbYminor.AutoSize = true;
            this.cbYminor.Location = new System.Drawing.Point(64, 125);
            this.cbYminor.Name = "cbYminor";
            this.cbYminor.Size = new System.Drawing.Size(104, 17);
            this.cbYminor.TabIndex = 14;
            this.cbYminor.Text = "show minor ticks";
            this.cbYminor.UseVisualStyleBackColor = true;
            // 
            // btnFitDataY
            // 
            this.btnFitDataY.Location = new System.Drawing.Point(64, 96);
            this.btnFitDataY.Name = "btnFitDataY";
            this.btnFitDataY.Size = new System.Drawing.Size(100, 23);
            this.btnFitDataY.TabIndex = 13;
            this.btnFitDataY.Text = "Fit data";
            this.btnFitDataY.UseVisualStyleBackColor = true;
            this.btnFitDataY.Click += new System.EventHandler(this.BtnFitDataY_Click);
            // 
            // tbYlabel
            // 
            this.tbYlabel.Location = new System.Drawing.Point(64, 18);
            this.tbYlabel.Name = "tbYlabel";
            this.tbYlabel.Size = new System.Drawing.Size(170, 20);
            this.tbYlabel.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "label";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "upper limit";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "lower limit";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cbXdateTime);
            this.groupBox6.Controls.Add(this.cbXminor);
            this.groupBox6.Controls.Add(this.btnFitDataX);
            this.groupBox6.Controls.Add(this.tbXlabel);
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.tbX2);
            this.groupBox6.Controls.Add(this.tbX1);
            this.groupBox6.Location = new System.Drawing.Point(12, 188);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(245, 170);
            this.groupBox6.TabIndex = 13;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Horizontal Axis";
            // 
            // cbXdateTime
            // 
            this.cbXdateTime.AutoSize = true;
            this.cbXdateTime.Location = new System.Drawing.Point(64, 148);
            this.cbXdateTime.Name = "cbXdateTime";
            this.cbXdateTime.Size = new System.Drawing.Size(122, 17);
            this.cbXdateTime.TabIndex = 16;
            this.cbXdateTime.Text = "date and time format";
            this.cbXdateTime.UseVisualStyleBackColor = true;
            // 
            // cbXminor
            // 
            this.cbXminor.AutoSize = true;
            this.cbXminor.Location = new System.Drawing.Point(64, 125);
            this.cbXminor.Name = "cbXminor";
            this.cbXminor.Size = new System.Drawing.Size(104, 17);
            this.cbXminor.TabIndex = 15;
            this.cbXminor.Text = "show minor ticks";
            this.cbXminor.UseVisualStyleBackColor = true;
            // 
            // btnFitDataX
            // 
            this.btnFitDataX.Location = new System.Drawing.Point(64, 96);
            this.btnFitDataX.Name = "btnFitDataX";
            this.btnFitDataX.Size = new System.Drawing.Size(100, 23);
            this.btnFitDataX.TabIndex = 14;
            this.btnFitDataX.Text = "Fit data";
            this.btnFitDataX.UseVisualStyleBackColor = true;
            this.btnFitDataX.Click += new System.EventHandler(this.BtnFitDataX_Click);
            // 
            // tbXlabel
            // 
            this.tbXlabel.Location = new System.Drawing.Point(64, 18);
            this.tbXlabel.Name = "tbXlabel";
            this.tbXlabel.Size = new System.Drawing.Size(170, 20);
            this.tbXlabel.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "label";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 47);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "upper limit";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 73);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "lower limit";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(683, 334);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(59, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbGrid);
            this.groupBox1.Controls.Add(this.cbTicksMult);
            this.groupBox1.Controls.Add(this.cbTicksOffset);
            this.groupBox1.Location = new System.Drawing.Point(263, 171);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 47);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tick Display";
            // 
            // cbGrid
            // 
            this.cbGrid.AutoSize = true;
            this.cbGrid.Location = new System.Drawing.Point(136, 19);
            this.cbGrid.Name = "cbGrid";
            this.cbGrid.Size = new System.Drawing.Size(43, 17);
            this.cbGrid.TabIndex = 3;
            this.cbGrid.Text = "grid";
            this.cbGrid.UseVisualStyleBackColor = true;
            // 
            // cbTicksMult
            // 
            this.cbTicksMult.AutoSize = true;
            this.cbTicksMult.Location = new System.Drawing.Point(64, 19);
            this.cbTicksMult.Name = "cbTicksMult";
            this.cbTicksMult.Size = new System.Drawing.Size(66, 17);
            this.cbTicksMult.TabIndex = 2;
            this.cbTicksMult.Text = "multiplier";
            this.cbTicksMult.UseVisualStyleBackColor = true;
            // 
            // cbTicksOffset
            // 
            this.cbTicksOffset.AutoSize = true;
            this.cbTicksOffset.Location = new System.Drawing.Point(6, 19);
            this.cbTicksOffset.Name = "cbTicksOffset";
            this.cbTicksOffset.Size = new System.Drawing.Size(52, 17);
            this.cbTicksOffset.TabIndex = 2;
            this.cbTicksOffset.Text = "offset";
            this.cbTicksOffset.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbLegend);
            this.groupBox2.Location = new System.Drawing.Point(263, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(245, 47);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Legend";
            // 
            // cbLegend
            // 
            this.cbLegend.AutoSize = true;
            this.cbLegend.Location = new System.Drawing.Point(6, 19);
            this.cbLegend.Name = "cbLegend";
            this.cbLegend.Size = new System.Drawing.Size(103, 17);
            this.cbLegend.TabIndex = 2;
            this.cbLegend.Text = "display on graph";
            this.cbLegend.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.cbStyle);
            this.groupBox7.Location = new System.Drawing.Point(263, 65);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(245, 47);
            this.groupBox7.TabIndex = 17;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Color Style";
            // 
            // cbStyle
            // 
            this.cbStyle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStyle.FormattingEnabled = true;
            this.cbStyle.Location = new System.Drawing.Point(6, 19);
            this.cbStyle.Name = "cbStyle";
            this.cbStyle.Size = new System.Drawing.Size(233, 21);
            this.cbStyle.TabIndex = 0;
            // 
            // btnTighten
            // 
            this.btnTighten.Location = new System.Drawing.Point(263, 224);
            this.btnTighten.Name = "btnTighten";
            this.btnTighten.Size = new System.Drawing.Size(130, 23);
            this.btnTighten.TabIndex = 18;
            this.btnTighten.Text = "Tighten Layout";
            this.btnTighten.UseVisualStyleBackColor = true;
            this.btnTighten.Click += new System.EventHandler(this.BtnTighten_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.Controls.Add(this.tbLabel);
            this.groupBox8.Location = new System.Drawing.Point(6, 204);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(205, 50);
            this.groupBox8.TabIndex = 5;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Label";
            // 
            // groupBox9
            // 
            this.groupBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox9.Controls.Add(this.btnCopyCSV);
            this.groupBox9.Controls.Add(this.btnExportCSV);
            this.groupBox9.Location = new System.Drawing.Point(6, 260);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(205, 50);
            this.groupBox9.TabIndex = 6;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Data Export";
            // 
            // btnCopyCSV
            // 
            this.btnCopyCSV.Enabled = false;
            this.btnCopyCSV.Location = new System.Drawing.Point(6, 19);
            this.btnCopyCSV.Name = "btnCopyCSV";
            this.btnCopyCSV.Size = new System.Drawing.Size(76, 23);
            this.btnCopyCSV.TabIndex = 4;
            this.btnCopyCSV.Text = "Copy CSV";
            this.btnCopyCSV.UseVisualStyleBackColor = true;
            this.btnCopyCSV.Click += new System.EventHandler(this.btnCopyCSV_Click);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 369);
            this.Controls.Add(this.btnTighten);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Name = "FormSettings";
            this.Text = "Plot Settings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox tbY1;
        private System.Windows.Forms.TextBox tbY2;
        private System.Windows.Forms.TextBox tbX2;
        private System.Windows.Forms.TextBox tbX1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbQualityHigh;
        private System.Windows.Forms.RadioButton rbQualityLow;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnExportCSV;
        private System.Windows.Forms.ListBox lbPlotObjects;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox cbQualityLowWhileDragging;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnFitDataY;
        private System.Windows.Forms.TextBox tbYlabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnFitDataX;
        private System.Windows.Forms.TextBox tbXlabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbTicksMult;
        private System.Windows.Forms.CheckBox cbTicksOffset;
        private System.Windows.Forms.CheckBox cbYminor;
        private System.Windows.Forms.CheckBox cbXminor;
        private System.Windows.Forms.CheckBox cbYdateTime;
        private System.Windows.Forms.CheckBox cbXdateTime;
        private System.Windows.Forms.CheckBox cbGrid;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbLegend;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ComboBox cbStyle;
        private System.Windows.Forms.Button btnTighten;
        private System.Windows.Forms.TextBox tbLabel;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button btnCopyCSV;
        private System.Windows.Forms.GroupBox groupBox8;
    }
}