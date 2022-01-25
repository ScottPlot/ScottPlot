namespace ScottPlot.Demo.WinForms
{
    partial class DemoListControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label17 = new System.Windows.Forms.Label();
            this.StyleBrowserButton = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.MultiAxisLockButton = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.Location = new System.Drawing.Point(104, 700);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(301, 50);
            this.label17.TabIndex = 64;
            this.label17.Text = "View available predefined styles";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StyleBrowserButton
            // 
            this.StyleBrowserButton.Location = new System.Drawing.Point(4, 703);
            this.StyleBrowserButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.StyleBrowserButton.Name = "StyleBrowserButton";
            this.StyleBrowserButton.Size = new System.Drawing.Size(88, 44);
            this.StyleBrowserButton.TabIndex = 63;
            this.StyleBrowserButton.Text = "Style Browser";
            this.StyleBrowserButton.UseVisualStyleBackColor = true;
            this.StyleBrowserButton.Click += new System.EventHandler(this.StyleBrowserButton_Click);
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.Location = new System.Drawing.Point(104, 650);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(301, 20);
            this.label16.TabIndex = 62;
            this.label16.Text = "Selectively pan/zoom individual axes in multi-axis plots";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MultiAxisLockButton
            // 
            this.MultiAxisLockButton.Location = new System.Drawing.Point(4, 653);
            this.MultiAxisLockButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MultiAxisLockButton.Name = "MultiAxisLockButton";
            this.MultiAxisLockButton.Size = new System.Drawing.Size(88, 44);
            this.MultiAxisLockButton.TabIndex = 61;
            this.MultiAxisLockButton.Text = "Multi-Axis Lock";
            this.MultiAxisLockButton.UseVisualStyleBackColor = true;
            this.MultiAxisLockButton.Click += new System.EventHandler(this.MultiAxisLockButton_Click);
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.Location = new System.Drawing.Point(104, 600);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(301, 20);
            this.label15.TabIndex = 60;
            this.label15.Text = "Show available colormaps";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnColormapViewer
            // 
            this.btnColormapViewer.Location = new System.Drawing.Point(4, 603);
            this.btnColormapViewer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnColormapViewer.Name = "btnColormapViewer";
            this.btnColormapViewer.Size = new System.Drawing.Size(88, 44);
            this.btnColormapViewer.TabIndex = 59;
            this.btnColormapViewer.Text = "Colormap Viewer";
            this.btnColormapViewer.UseVisualStyleBackColor = true;
            this.btnColormapViewer.Click += new System.EventHandler(this.btnColormapViewer_Click);
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.Location = new System.Drawing.Point(104, 550);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(301, 20);
            this.label14.TabIndex = 58;
            this.label14.Text = "Demonstrate how axis boundaries can be used to constrain axis limits in interacti" +
    "ve plots";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AxisLimitsButton
            // 
            this.AxisLimitsButton.Location = new System.Drawing.Point(4, 553);
            this.AxisLimitsButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.AxisLimitsButton.Name = "AxisLimitsButton";
            this.AxisLimitsButton.Size = new System.Drawing.Size(88, 44);
            this.AxisLimitsButton.TabIndex = 57;
            this.AxisLimitsButton.Text = "Axis Limits";
            this.AxisLimitsButton.UseVisualStyleBackColor = true;
            this.AxisLimitsButton.Click += new System.EventHandler(this.AxisLimitsButton_Click);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.Location = new System.Drawing.Point(104, 500);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(301, 20);
            this.label13.TabIndex = 56;
            this.label13.Text = "Display multiple plots in a scrolling control";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ScrollViewerButton
            // 
            this.ScrollViewerButton.Location = new System.Drawing.Point(4, 503);
            this.ScrollViewerButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ScrollViewerButton.Name = "ScrollViewerButton";
            this.ScrollViewerButton.Size = new System.Drawing.Size(88, 44);
            this.ScrollViewerButton.TabIndex = 55;
            this.ScrollViewerButton.Text = "Plot in a Scroll Viewer";
            this.ScrollViewerButton.UseVisualStyleBackColor = true;
            this.ScrollViewerButton.Click += new System.EventHandler(this.ScrollViewerButton_Click);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.Location = new System.Drawing.Point(104, 450);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(301, 20);
            this.label12.TabIndex = 54;
            this.label12.Text = "Display a custom menu (or perform a different action) when the control is right-c" +
    "licked";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RightClickMenuButton
            // 
            this.RightClickMenuButton.Location = new System.Drawing.Point(4, 453);
            this.RightClickMenuButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.RightClickMenuButton.Name = "RightClickMenuButton";
            this.RightClickMenuButton.Size = new System.Drawing.Size(88, 44);
            this.RightClickMenuButton.TabIndex = 53;
            this.RightClickMenuButton.Text = "Custom Right-Click Menu";
            this.RightClickMenuButton.UseVisualStyleBackColor = true;
            this.RightClickMenuButton.Click += new System.EventHandler(this.RightClickMenuButton_Click);
            // 
            // PlotViewerButton
            // 
            this.PlotViewerButton.Location = new System.Drawing.Point(4, 3);
            this.PlotViewerButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PlotViewerButton.Name = "PlotViewerButton";
            this.PlotViewerButton.Size = new System.Drawing.Size(88, 44);
            this.PlotViewerButton.TabIndex = 51;
            this.PlotViewerButton.Text = "Plot Viewer";
            this.PlotViewerButton.UseVisualStyleBackColor = true;
            this.PlotViewerButton.Click += new System.EventHandler(this.PlotViewerButton_Click);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.Location = new System.Drawing.Point(104, 0);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(301, 50);
            this.label11.TabIndex = 52;
            this.label11.Text = "Create a ScottPlot programmatically then display it in an interactive window";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.Location = new System.Drawing.Point(104, 350);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(301, 20);
            this.label10.TabIndex = 50;
            this.label10.Text = "Display the value of the data point nearest the cursor";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnShowOnHover
            // 
            this.btnShowOnHover.Location = new System.Drawing.Point(4, 353);
            this.btnShowOnHover.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnShowOnHover.Name = "btnShowOnHover";
            this.btnShowOnHover.Size = new System.Drawing.Size(88, 44);
            this.btnShowOnHover.TabIndex = 49;
            this.btnShowOnHover.Text = "Show Value on Hover";
            this.btnShowOnHover.UseVisualStyleBackColor = true;
            this.btnShowOnHover.Click += new System.EventHandler(this.btnShowOnHover_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.Location = new System.Drawing.Point(104, 300);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(301, 20);
            this.label9.TabIndex = 48;
            this.label9.Text = "Display live data that grows with time";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.Location = new System.Drawing.Point(104, 250);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(301, 50);
            this.label8.TabIndex = 47;
            this.label8.Text = "Display live data from a fixed-length array that continuously changes";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GrowingData
            // 
            this.GrowingData.Location = new System.Drawing.Point(4, 303);
            this.GrowingData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.GrowingData.Name = "GrowingData";
            this.GrowingData.Size = new System.Drawing.Size(88, 44);
            this.GrowingData.TabIndex = 46;
            this.GrowingData.Text = "Growing Data";
            this.GrowingData.UseVisualStyleBackColor = true;
            this.GrowingData.Click += new System.EventHandler(this.GrowingData_Click);
            // 
            // LiveData
            // 
            this.LiveData.Location = new System.Drawing.Point(4, 253);
            this.LiveData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.LiveData.Name = "LiveData";
            this.LiveData.Size = new System.Drawing.Size(88, 44);
            this.LiveData.TabIndex = 45;
            this.LiveData.Text = "Live Data";
            this.LiveData.UseVisualStyleBackColor = true;
            this.LiveData.Click += new System.EventHandler(this.LiveData_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(104, 200);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(301, 50);
            this.label7.TabIndex = 44;
            this.label7.Text = "Link the axes of two plots together so adjusting one changes the other";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LinkedAxesButton
            // 
            this.LinkedAxesButton.Location = new System.Drawing.Point(4, 203);
            this.LinkedAxesButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.LinkedAxesButton.Name = "LinkedAxesButton";
            this.LinkedAxesButton.Size = new System.Drawing.Size(88, 44);
            this.LinkedAxesButton.TabIndex = 43;
            this.LinkedAxesButton.Text = "Linked Axes";
            this.LinkedAxesButton.UseVisualStyleBackColor = true;
            this.LinkedAxesButton.Click += new System.EventHandler(this.LinkedAxesButton_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(104, 150);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(301, 50);
            this.label6.TabIndex = 42;
            this.label6.Text = "Advanced styling and behavior customization";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ConfigButton
            // 
            this.ConfigButton.Location = new System.Drawing.Point(4, 153);
            this.ConfigButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ConfigButton.Name = "ConfigButton";
            this.ConfigButton.Size = new System.Drawing.Size(88, 44);
            this.ConfigButton.TabIndex = 41;
            this.ConfigButton.Text = "FormsPlot Config";
            this.ConfigButton.UseVisualStyleBackColor = true;
            this.ConfigButton.Click += new System.EventHandler(this.ConfigButton_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(104, 100);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(301, 50);
            this.label5.TabIndex = 40;
            this.label5.Text = "Checkboxes control visibility of individual plot objects";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ToggleVisibilityButton
            // 
            this.ToggleVisibilityButton.Location = new System.Drawing.Point(4, 103);
            this.ToggleVisibilityButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ToggleVisibilityButton.Name = "ToggleVisibilityButton";
            this.ToggleVisibilityButton.Size = new System.Drawing.Size(88, 44);
            this.ToggleVisibilityButton.TabIndex = 39;
            this.ToggleVisibilityButton.Text = "Toggle Visibility";
            this.ToggleVisibilityButton.UseVisualStyleBackColor = true;
            this.ToggleVisibilityButton.Click += new System.EventHandler(this.ToggleVisibilityButton_Click);
            // 
            // MouseTrackerButton
            // 
            this.MouseTrackerButton.Location = new System.Drawing.Point(4, 53);
            this.MouseTrackerButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MouseTrackerButton.Name = "MouseTrackerButton";
            this.MouseTrackerButton.Size = new System.Drawing.Size(88, 44);
            this.MouseTrackerButton.TabIndex = 35;
            this.MouseTrackerButton.Text = "Mouse Tracker";
            this.MouseTrackerButton.UseVisualStyleBackColor = true;
            this.MouseTrackerButton.Click += new System.EventHandler(this.MouseTrackerButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(104, 400);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(301, 20);
            this.label2.TabIndex = 38;
            this.label2.Text = "Demonstrate a control with a transparent background";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(104, 50);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(301, 50);
            this.label4.TabIndex = 36;
            this.label4.Text = "Display the position under the mouse cursor";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TransparentBackgroundButton
            // 
            this.TransparentBackgroundButton.Location = new System.Drawing.Point(4, 403);
            this.TransparentBackgroundButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TransparentBackgroundButton.Name = "TransparentBackgroundButton";
            this.TransparentBackgroundButton.Size = new System.Drawing.Size(88, 44);
            this.TransparentBackgroundButton.TabIndex = 37;
            this.TransparentBackgroundButton.Text = "Transparent Background";
            this.TransparentBackgroundButton.UseVisualStyleBackColor = true;
            this.TransparentBackgroundButton.Click += new System.EventHandler(this.TransparentBackgroundButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.PlotViewerButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label14, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.label17, 1, 14);
            this.tableLayoutPanel1.Controls.Add(this.label11, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.StyleBrowserButton, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.MouseTrackerButton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.MultiAxisLockButton, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.ToggleVisibilityButton, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnColormapViewer, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.ConfigButton, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label6, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.AxisLimitsButton, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.LinkedAxesButton, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label13, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.label7, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.ScrollViewerButton, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.LiveData, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label12, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.GrowingData, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.RightClickMenuButton, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.label8, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.label10, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.TransparentBackgroundButton, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label9, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnShowOnHover, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label16, 1, 13);
            this.tableLayoutPanel1.Controls.Add(this.label15, 1, 12);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 15;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(409, 750);
            this.tableLayoutPanel1.TabIndex = 65;
            // 
            // DemoListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DemoListControl";
            this.Size = new System.Drawing.Size(409, 750);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button StyleBrowserButton;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button MultiAxisLockButton;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnColormapViewer;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button AxisLimitsButton;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button ScrollViewerButton;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button RightClickMenuButton;
        private System.Windows.Forms.Button PlotViewerButton;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnShowOnHover;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button GrowingData;
        private System.Windows.Forms.Button LiveData;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button LinkedAxesButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button ConfigButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ToggleVisibilityButton;
        private System.Windows.Forms.Button MouseTrackerButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button TransparentBackgroundButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
