namespace ScottPlot.Demo.WinForms
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.cookbookButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.StyleBrowserButton = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.MultiAxisLockButton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btnColormapViewer = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.AxisLimitsButton = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.ScrollViewerButton = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.RightClickMenuButton = new System.Windows.Forms.Button();
            this.PlotViewerButton = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnShowOnHover = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.GrowingData = new System.Windows.Forms.Button();
            this.LiveData = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.LinkedAxesButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.ConfigButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.ToggleVisibilityButton = new System.Windows.Forms.Button();
            this.MouseTrackerButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TransparentBackgroundButton = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.DetachedLegendButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cookbookButton
            // 
            this.cookbookButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.cookbookButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(161)))));
            this.cookbookButton.Location = new System.Drawing.Point(8, 29);
            this.cookbookButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cookbookButton.Name = "cookbookButton";
            this.cookbookButton.Size = new System.Drawing.Size(100, 91);
            this.cookbookButton.TabIndex = 0;
            this.cookbookButton.Text = "Launch ScottPlot Cookbook";
            this.cookbookButton.UseVisualStyleBackColor = false;
            this.cookbookButton.Click += new System.EventHandler(this.CookbookButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(16, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "ScottPlot Demo";
            // 
            // versionLabel
            // 
            this.versionLabel.Location = new System.Drawing.Point(19, 52);
            this.versionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(128, 38);
            this.versionLabel.TabIndex = 3;
            this.versionLabel.Text = "version 9.9.99";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(117, 29);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(277, 91);
            this.label3.TabIndex = 5;
            this.label3.Text = "Simple examples that demonstrate the primary features of ScottPlot";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cookbookButton);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(16, 95);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(404, 131);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ScottPlot Cookbook";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.DetachedLegendButton);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.StyleBrowserButton);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.MultiAxisLockButton);
            this.groupBox2.Controls.Add(this.richTextBox1);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.btnColormapViewer);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.AxisLimitsButton);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.ScrollViewerButton);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.RightClickMenuButton);
            this.groupBox2.Controls.Add(this.PlotViewerButton);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.btnShowOnHover);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.GrowingData);
            this.groupBox2.Controls.Add(this.LiveData);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.LinkedAxesButton);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.ConfigButton);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.ToggleVisibilityButton);
            this.groupBox2.Controls.Add(this.MouseTrackerButton);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.TransparentBackgroundButton);
            this.groupBox2.Location = new System.Drawing.Point(16, 235);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(404, 1528);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "WinForms Control Demos";
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.Location = new System.Drawing.Point(117, 1349);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(280, 72);
            this.label17.TabIndex = 34;
            this.label17.Text = "View available predefined styles";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StyleBrowserButton
            // 
            this.StyleBrowserButton.Location = new System.Drawing.Point(8, 1351);
            this.StyleBrowserButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.StyleBrowserButton.Name = "StyleBrowserButton";
            this.StyleBrowserButton.Size = new System.Drawing.Size(100, 72);
            this.StyleBrowserButton.TabIndex = 33;
            this.StyleBrowserButton.Text = "Style Browser";
            this.StyleBrowserButton.UseVisualStyleBackColor = true;
            this.StyleBrowserButton.Click += new System.EventHandler(this.StyleBrowserButton_Click);
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.Location = new System.Drawing.Point(117, 1268);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(280, 72);
            this.label16.TabIndex = 32;
            this.label16.Text = "Selectively pan/zoom individual axes in multi-axis plots";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MultiAxisLockButton
            // 
            this.MultiAxisLockButton.Location = new System.Drawing.Point(8, 1268);
            this.MultiAxisLockButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MultiAxisLockButton.Name = "MultiAxisLockButton";
            this.MultiAxisLockButton.Size = new System.Drawing.Size(100, 72);
            this.MultiAxisLockButton.TabIndex = 31;
            this.MultiAxisLockButton.Text = "Multi-Axis Lock";
            this.MultiAxisLockButton.UseVisualStyleBackColor = true;
            this.MultiAxisLockButton.Click += new System.EventHandler(this.MultiAxisLockButton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(8, 29);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(388, 171);
            this.richTextBox1.TabIndex = 30;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            this.richTextBox1.Click += new System.EventHandler(this.WebsiteLink_Click);
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.Location = new System.Drawing.Point(116, 1186);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(280, 72);
            this.label15.TabIndex = 29;
            this.label15.Text = "Show available colormaps";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnColormapViewer
            // 
            this.btnColormapViewer.Location = new System.Drawing.Point(8, 1186);
            this.btnColormapViewer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnColormapViewer.Name = "btnColormapViewer";
            this.btnColormapViewer.Size = new System.Drawing.Size(100, 72);
            this.btnColormapViewer.TabIndex = 28;
            this.btnColormapViewer.Text = "Colormap Viewer";
            this.btnColormapViewer.UseVisualStyleBackColor = true;
            this.btnColormapViewer.Click += new System.EventHandler(this.ColormapViewerButton_Click);
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.Location = new System.Drawing.Point(116, 1105);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(280, 72);
            this.label14.TabIndex = 27;
            this.label14.Text = "Demonstrate how axis boundaries can be used to constrain axis limits in interacti" +
    "ve plots";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AxisLimitsButton
            // 
            this.AxisLimitsButton.Location = new System.Drawing.Point(8, 1105);
            this.AxisLimitsButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AxisLimitsButton.Name = "AxisLimitsButton";
            this.AxisLimitsButton.Size = new System.Drawing.Size(100, 72);
            this.AxisLimitsButton.TabIndex = 26;
            this.AxisLimitsButton.Text = "Axis Limits";
            this.AxisLimitsButton.UseVisualStyleBackColor = true;
            this.AxisLimitsButton.Click += new System.EventHandler(this.AxisLimitsButton_Click);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.Location = new System.Drawing.Point(116, 1023);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(280, 72);
            this.label13.TabIndex = 25;
            this.label13.Text = "Display multiple plots in a scrolling control";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ScrollViewerButton
            // 
            this.ScrollViewerButton.Location = new System.Drawing.Point(8, 1023);
            this.ScrollViewerButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ScrollViewerButton.Name = "ScrollViewerButton";
            this.ScrollViewerButton.Size = new System.Drawing.Size(100, 72);
            this.ScrollViewerButton.TabIndex = 24;
            this.ScrollViewerButton.Text = "Plot in a Scroll Viewer";
            this.ScrollViewerButton.UseVisualStyleBackColor = true;
            this.ScrollViewerButton.Click += new System.EventHandler(this.ScrollViewerButton_Click);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.Location = new System.Drawing.Point(116, 942);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(280, 72);
            this.label12.TabIndex = 23;
            this.label12.Text = "Display a custom menu (or perform a different action) when the control is right-c" +
    "licked";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RightClickMenuButton
            // 
            this.RightClickMenuButton.Location = new System.Drawing.Point(8, 942);
            this.RightClickMenuButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RightClickMenuButton.Name = "RightClickMenuButton";
            this.RightClickMenuButton.Size = new System.Drawing.Size(100, 72);
            this.RightClickMenuButton.TabIndex = 22;
            this.RightClickMenuButton.Text = "Custom Right-Click Menu";
            this.RightClickMenuButton.UseVisualStyleBackColor = true;
            this.RightClickMenuButton.Click += new System.EventHandler(this.RightClickMenuButton_Click);
            // 
            // PlotViewerButton
            // 
            this.PlotViewerButton.Location = new System.Drawing.Point(8, 208);
            this.PlotViewerButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PlotViewerButton.Name = "PlotViewerButton";
            this.PlotViewerButton.Size = new System.Drawing.Size(100, 72);
            this.PlotViewerButton.TabIndex = 20;
            this.PlotViewerButton.Text = "Plot Viewer";
            this.PlotViewerButton.UseVisualStyleBackColor = true;
            this.PlotViewerButton.Click += new System.EventHandler(this.PlotViewerButton_Click);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.Location = new System.Drawing.Point(116, 208);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(280, 72);
            this.label11.TabIndex = 21;
            this.label11.Text = "Create a ScottPlot programmatically then display it in an interactive window";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.Location = new System.Drawing.Point(116, 778);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(280, 72);
            this.label10.TabIndex = 19;
            this.label10.Text = "Display the value of the data point nearest the cursor";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnShowOnHover
            // 
            this.btnShowOnHover.Location = new System.Drawing.Point(8, 778);
            this.btnShowOnHover.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnShowOnHover.Name = "btnShowOnHover";
            this.btnShowOnHover.Size = new System.Drawing.Size(100, 72);
            this.btnShowOnHover.TabIndex = 18;
            this.btnShowOnHover.Text = "Show Value on Hover";
            this.btnShowOnHover.UseVisualStyleBackColor = true;
            this.btnShowOnHover.Click += new System.EventHandler(this.ShowOnHover_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.Location = new System.Drawing.Point(116, 697);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(280, 72);
            this.label9.TabIndex = 17;
            this.label9.Text = "Display live data that grows with time";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.Location = new System.Drawing.Point(116, 615);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(280, 72);
            this.label8.TabIndex = 16;
            this.label8.Text = "Display live data from a fixed-length array that continuously changes";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GrowingData
            // 
            this.GrowingData.Location = new System.Drawing.Point(8, 697);
            this.GrowingData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GrowingData.Name = "GrowingData";
            this.GrowingData.Size = new System.Drawing.Size(100, 72);
            this.GrowingData.TabIndex = 15;
            this.GrowingData.Text = "Growing Data";
            this.GrowingData.UseVisualStyleBackColor = true;
            this.GrowingData.Click += new System.EventHandler(this.GrowingData_Click);
            // 
            // LiveData
            // 
            this.LiveData.Location = new System.Drawing.Point(7, 615);
            this.LiveData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LiveData.Name = "LiveData";
            this.LiveData.Size = new System.Drawing.Size(100, 72);
            this.LiveData.TabIndex = 14;
            this.LiveData.Text = "Live Data";
            this.LiveData.UseVisualStyleBackColor = true;
            this.LiveData.Click += new System.EventHandler(this.LiveData_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(116, 535);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(280, 72);
            this.label7.TabIndex = 13;
            this.label7.Text = "Link the axes of two plots together so adjusting one changes the other";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LinkedAxesButton
            // 
            this.LinkedAxesButton.Location = new System.Drawing.Point(8, 535);
            this.LinkedAxesButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LinkedAxesButton.Name = "LinkedAxesButton";
            this.LinkedAxesButton.Size = new System.Drawing.Size(100, 72);
            this.LinkedAxesButton.TabIndex = 12;
            this.LinkedAxesButton.Text = "Linked Axes";
            this.LinkedAxesButton.UseVisualStyleBackColor = true;
            this.LinkedAxesButton.Click += new System.EventHandler(this.LinkedAxesButton_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(116, 452);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(280, 72);
            this.label6.TabIndex = 11;
            this.label6.Text = "Advanced styling and behavior customization";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ConfigButton
            // 
            this.ConfigButton.Location = new System.Drawing.Point(8, 452);
            this.ConfigButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ConfigButton.Name = "ConfigButton";
            this.ConfigButton.Size = new System.Drawing.Size(100, 72);
            this.ConfigButton.TabIndex = 10;
            this.ConfigButton.Text = "FormsPlot Config";
            this.ConfigButton.UseVisualStyleBackColor = true;
            this.ConfigButton.Click += new System.EventHandler(this.ConfigButton_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(116, 371);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(280, 72);
            this.label5.TabIndex = 9;
            this.label5.Text = "Checkboxes control visibility of individual plot objects";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ToggleVisibilityButton
            // 
            this.ToggleVisibilityButton.Location = new System.Drawing.Point(8, 371);
            this.ToggleVisibilityButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ToggleVisibilityButton.Name = "ToggleVisibilityButton";
            this.ToggleVisibilityButton.Size = new System.Drawing.Size(100, 72);
            this.ToggleVisibilityButton.TabIndex = 8;
            this.ToggleVisibilityButton.Text = "Toggle Visibility";
            this.ToggleVisibilityButton.UseVisualStyleBackColor = true;
            this.ToggleVisibilityButton.Click += new System.EventHandler(this.ToggleVisibilityButton_Click);
            // 
            // MouseTrackerButton
            // 
            this.MouseTrackerButton.Location = new System.Drawing.Point(8, 289);
            this.MouseTrackerButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MouseTrackerButton.Name = "MouseTrackerButton";
            this.MouseTrackerButton.Size = new System.Drawing.Size(100, 72);
            this.MouseTrackerButton.TabIndex = 4;
            this.MouseTrackerButton.Text = "Mouse Tracker";
            this.MouseTrackerButton.UseVisualStyleBackColor = true;
            this.MouseTrackerButton.Click += new System.EventHandler(this.MouseTrackerButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(116, 860);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(280, 72);
            this.label2.TabIndex = 7;
            this.label2.Text = "Demonstrate a control with a transparent background";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(116, 289);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(280, 72);
            this.label4.TabIndex = 5;
            this.label4.Text = "Display the position under the mouse cursor";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TransparentBackgroundButton
            // 
            this.TransparentBackgroundButton.Location = new System.Drawing.Point(8, 860);
            this.TransparentBackgroundButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TransparentBackgroundButton.Name = "TransparentBackgroundButton";
            this.TransparentBackgroundButton.Size = new System.Drawing.Size(100, 72);
            this.TransparentBackgroundButton.TabIndex = 6;
            this.TransparentBackgroundButton.Text = "Transparent Background";
            this.TransparentBackgroundButton.UseVisualStyleBackColor = true;
            this.TransparentBackgroundButton.Click += new System.EventHandler(this.TransparentBackgroundButton_Click);
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.Location = new System.Drawing.Point(117, 1432);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(280, 72);
            this.label18.TabIndex = 36;
            this.label18.Text = "Show how to detach a legend from a plot";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DetachedLegendButton
            // 
            this.DetachedLegendButton.Location = new System.Drawing.Point(8, 1434);
            this.DetachedLegendButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DetachedLegendButton.Name = "DetachedLegendButton";
            this.DetachedLegendButton.Size = new System.Drawing.Size(100, 72);
            this.DetachedLegendButton.TabIndex = 35;
            this.DetachedLegendButton.Text = "Detached Legend";
            this.DetachedLegendButton.UseVisualStyleBackColor = true;
            this.DetachedLegendButton.Click += new System.EventHandler(this.DetachLegendButton_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(477, 842);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ScottPlot Demo (WinForms)";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cookbookButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button MouseTrackerButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button TransparentBackgroundButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button ConfigButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ToggleVisibilityButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button LinkedAxesButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button GrowingData;
        private System.Windows.Forms.Button LiveData;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnShowOnHover;
        private System.Windows.Forms.Button PlotViewerButton;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button RightClickMenuButton;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button ScrollViewerButton;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button AxisLimitsButton;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnColormapViewer;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button MultiAxisLockButton;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button StyleBrowserButton;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button DetachedLegendButton;
    }
}