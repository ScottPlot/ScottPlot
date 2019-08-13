﻿namespace Legend
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
            this.cbLocations = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbShadowDirection = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbMarker = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.nudMarkerSize = new System.Windows.Forms.NumericUpDown();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbPenStyle = new System.Windows.Forms.ComboBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.nupFontSize = new System.Windows.Forms.NumericUpDown();
            this.scottPlotUC1 = new ScottPlot.FormsPlot();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMarkerSize)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupFontSize)).BeginInit();
            this.SuspendLayout();
            // 
            // cbLocations
            // 
            this.cbLocations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLocations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocations.FormattingEnabled = true;
            this.cbLocations.Location = new System.Drawing.Point(6, 19);
            this.cbLocations.Name = "cbLocations";
            this.cbLocations.Size = new System.Drawing.Size(168, 21);
            this.cbLocations.TabIndex = 0;
            this.cbLocations.SelectedIndexChanged += new System.EventHandler(this.Cboxes_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbLocations);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 47);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Legend Location";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbShadowDirection);
            this.groupBox2.Location = new System.Drawing.Point(198, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(112, 47);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Legend Shadow";
            // 
            // cbShadowDirection
            // 
            this.cbShadowDirection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbShadowDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbShadowDirection.FormattingEnabled = true;
            this.cbShadowDirection.Location = new System.Drawing.Point(0, 18);
            this.cbShadowDirection.Name = "cbShadowDirection";
            this.cbShadowDirection.Size = new System.Drawing.Size(100, 21);
            this.cbShadowDirection.TabIndex = 0;
            this.cbShadowDirection.SelectedIndexChanged += new System.EventHandler(this.Cboxes_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbMarker);
            this.groupBox3.Location = new System.Drawing.Point(316, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(99, 47);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Marker Shape";
            // 
            // cbMarker
            // 
            this.cbMarker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMarker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMarker.FormattingEnabled = true;
            this.cbMarker.Location = new System.Drawing.Point(6, 19);
            this.cbMarker.Name = "cbMarker";
            this.cbMarker.Size = new System.Drawing.Size(87, 21);
            this.cbMarker.TabIndex = 0;
            this.cbMarker.SelectedIndexChanged += new System.EventHandler(this.Cboxes_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.nudMarkerSize);
            this.groupBox4.Location = new System.Drawing.Point(421, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(92, 47);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Marker Size";
            // 
            // nudMarkerSize
            // 
            this.nudMarkerSize.DecimalPlaces = 1;
            this.nudMarkerSize.Location = new System.Drawing.Point(7, 19);
            this.nudMarkerSize.Name = "nudMarkerSize";
            this.nudMarkerSize.Size = new System.Drawing.Size(79, 20);
            this.nudMarkerSize.TabIndex = 0;
            this.nudMarkerSize.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudMarkerSize.ValueChanged += new System.EventHandler(this.Cboxes_SelectedIndexChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cbPenStyle);
            this.groupBox5.Location = new System.Drawing.Point(519, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(103, 47);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Line Style";
            // 
            // cbPenStyle
            // 
            this.cbPenStyle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPenStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPenStyle.FormattingEnabled = true;
            this.cbPenStyle.Location = new System.Drawing.Point(6, 19);
            this.cbPenStyle.Name = "cbPenStyle";
            this.cbPenStyle.Size = new System.Drawing.Size(91, 21);
            this.cbPenStyle.TabIndex = 0;
            this.cbPenStyle.SelectedIndexChanged += new System.EventHandler(this.Cboxes_SelectedIndexChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.nupFontSize);
            this.groupBox6.Location = new System.Drawing.Point(628, 12);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(115, 47);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Legend Font Size";
            // 
            // nupFontSize
            // 
            this.nupFontSize.DecimalPlaces = 1;
            this.nupFontSize.Location = new System.Drawing.Point(6, 19);
            this.nupFontSize.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nupFontSize.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nupFontSize.Name = "nupFontSize";
            this.nupFontSize.Size = new System.Drawing.Size(103, 20);
            this.nupFontSize.TabIndex = 1;
            this.nupFontSize.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.nupFontSize.ValueChanged += new System.EventHandler(this.Cboxes_SelectedIndexChanged);
            // 
            // scottPlotUC1
            // 
            this.scottPlotUC1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scottPlotUC1.Location = new System.Drawing.Point(12, 65);
            this.scottPlotUC1.Name = "scottPlotUC1";
            this.scottPlotUC1.Size = new System.Drawing.Size(737, 354);
            this.scottPlotUC1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 431);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.scottPlotUC1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "ScottPlot Legend Test";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudMarkerSize)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nupFontSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbLocations;
        private System.Windows.Forms.GroupBox groupBox1;
        private ScottPlot.FormsPlot scottPlotUC1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbShadowDirection;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbMarker;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown nudMarkerSize;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cbPenStyle;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.NumericUpDown nupFontSize;
    }
}

