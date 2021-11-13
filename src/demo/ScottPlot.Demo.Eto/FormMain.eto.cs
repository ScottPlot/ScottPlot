using Eto.Drawing;
using Eto.Forms;
using ScottPlot.Eto;

namespace ScottPlot.Demo.Eto
{
    partial class FormMain : Form
    {
        private void InitializeComponent()
        {
            this.cookbookButton = new Button();
            this.label1 = new Label();
            this.versionLabel = new Label();
            this.label3 = new Label();
            this.groupBox1 = new GroupBox();
            this.groupBox2 = new GroupBox();
            this.label16 = new Label();
            this.MultiAxisLockButton = new Button();
            this.richTextBox1 = new Label();
            this.label15 = new Label();
            this.btnColormapViewer = new Button();
            this.label14 = new Label();
            this.AxisLimitsButton = new Button();
            this.label13 = new Label();
            this.ScrollViewerButton = new Button();
            this.label12 = new Label();
            this.RightClickMenuButton = new Button();
            this.PlotViewerButton = new Button();
            this.label11 = new Label();
            this.label10 = new Label();
            this.btnShowOnHover = new Button();
            this.label9 = new Label();
            this.label8 = new Label();
            this.GrowingData = new Button();
            this.LiveData = new Button();
            this.label7 = new Label();
            this.LinkedAxesButton = new Button();
            this.label6 = new Label();
            this.ConfigButton = new Button();
            this.label5 = new Label();
            this.ToggleVisibilityButton = new Button();
            this.MouseTrackerButton = new Button();
            this.label2 = new Label();
            this.label4 = new Label();
            this.TransparentBackgroundButton = new Button();
            this.StyleBrowserButton = new Button();
            this.label17 = new Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();

            var layout = new DynamicLayout { Rows = { label1, versionLabel, groupBox1, groupBox2 }, Spacing = Size.Empty + 4 };

            this.Content = new Scrollable { Content = layout, Border = BorderType.None };

            this.groupBox1.Content = new DynamicLayout() { Rows = { new DynamicRow(cookbookButton, label3) }, Spacing = Size.Empty + 4 };

            var layout2 = new DynamicLayout();
            layout2.BeginVertical(padding: 4);
            layout2.AddRow(richTextBox1);
            layout2.EndVertical();
            layout2.BeginVertical(spacing: Size.Empty + 4);
            layout2.AddRow(PlotViewerButton, label11);
            layout2.AddRow(MouseTrackerButton, label4);
            layout2.AddRow(ToggleVisibilityButton, label5);
            layout2.AddRow(ConfigButton, label6);
            layout2.AddRow(LinkedAxesButton, label7);
            layout2.AddRow(LiveData, label8);
            layout2.AddRow(GrowingData, label9);
            layout2.AddRow(btnShowOnHover, label10);
            layout2.AddRow(TransparentBackgroundButton, label2);
            layout2.AddRow(RightClickMenuButton, label12);
            layout2.AddRow(ScrollViewerButton, label13);
            layout2.AddRow(AxisLimitsButton, label14);
            layout2.AddRow(btnColormapViewer, label15);
            layout2.AddRow(MultiAxisLockButton, label16);
            layout2.AddRow(StyleBrowserButton, label17);
            layout2.EndVertical();
            this.groupBox2.Content = layout2;

            Helpers.SetPaddingRecursive(this, 5);

            // 
            // cookbookButton
            //
            //this.cookbookButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left)));
            this.cookbookButton.BackgroundColor = Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(161)))));
            //this.cookbookButton.Location = new System.Drawing.Point(6, 19);
            //this.cookbookButton.Name = "cookbookButton";
            this.cookbookButton.Size = new Size(75, 59);
            this.cookbookButton.TabIndex = 0;
            this.cookbookButton.Text = "Launch ScottPlot Cookbook";
            //this.cookbookButton.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            //this.label1.AutoSize = true;
            this.label1.Font = Fonts.Sans(16); // new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            //this.label1.Location = new System.Drawing.Point(12, 9);
            //this.label1.Name = "label1";
            this.label1.Size = new Size(160, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "ScottPlot Demo";
            // 
            // versionLabel
            // 
            //this.versionLabel.Location = new System.Drawing.Point(14, 34);
            //this.versionLabel.Name = "versionLabel";
            //this.versionLabel.Size = new Size(96, 25);
            this.versionLabel.TabIndex = 3;
            this.versionLabel.Text = "version 9.9.99";
            //this.versionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //this.versionLabel.VerticalAlignment = VerticalAlignment.Top;
            //this.versionLabel.TextAlignment = TextAlignment.Right;
            // 
            // label3
            // 
            //this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.label3.Location = new System.Drawing.Point(88, 19);
            //this.label3.Name = "label3";
            this.label3.Size = new Size(208, 59);
            this.label3.TabIndex = 5;
            this.label3.Text = "Simple examples that demonstrate the primary features of ScottPlot";
            //this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label3.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // groupBox1
            //
            //this.groupBox1.Controls.Add(this.cookbookButton);
            //this.groupBox1.Controls.Add(this.label3);
            //this.groupBox1.Location = new System.Drawing.Point(12, 62);
            //this.groupBox1.Name = "groupBox1";
            //this.groupBox1.Size = new Size(303, 85);
            //this.groupBox1.TabIndex = 6;
            //this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ScottPlot Cookbook";
            // 
            // groupBox2
            //
            //this.groupBox2.Location = new System.Drawing.Point(12, 153);
            //this.groupBox2.Name = "groupBox2";
            //this.groupBox2.Size = new Size(303, 940);
            //this.groupBox2.TabIndex = 7;
            //this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Eto.Forms Control Demos";
            // 
            // label16
            // 
            //this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.label16.Location = new System.Drawing.Point(88, 824);
            //this.label16.Name = "label16";
            this.label16.Size = new Size(210, 47);
            this.label16.TabIndex = 32;
            this.label16.Text = "Selectively pan/zoom individual axes in multi-axis plots";
            //this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label16.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label16.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // MultiAxisLockButton
            // 
            //this.MultiAxisLockButton.Location = new System.Drawing.Point(6, 824);
            //this.MultiAxisLockButton.Name = "MultiAxisLockButton";
            this.MultiAxisLockButton.Size = new Size(75, 47);
            this.MultiAxisLockButton.TabIndex = 31;
            this.MultiAxisLockButton.Text = "Multi-Axis Lock";
            //this.MultiAxisLockButton.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            //this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackgroundColor = SystemColors.Control;
            //this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            //this.richTextBox1.Location = new System.Drawing.Point(6, 19);
            //this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new Size(291, 111);
            //this.richTextBox1.TabIndex = 30;
            //this.richTextBox1.Wrap = WrapMode.Word;
            this.richTextBox1.Text = @"These examples demonstrate advanced functionality for the Eto.Forms ScottPlot control (PlotView).
Source code for these demos can be found on the ScottPlot Demo Website https://scottplot.net/demo/ along with additional information and advanced implementation recommendations.";
            // 
            // label15
            // 
            //this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.label15.Location = new System.Drawing.Point(87, 771);
            //this.label15.Name = "label15";
            this.label15.Size = new Size(210, 47);
            this.label15.TabIndex = 29;
            this.label15.Text = "Show available colormaps";
            //this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label15.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label15.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // btnColormapViewer
            // 
            //this.btnColormapViewer.Location = new System.Drawing.Point(6, 771);
            //this.btnColormapViewer.Name = "btnColormapViewer";
            this.btnColormapViewer.Size = new Size(75, 47);
            this.btnColormapViewer.TabIndex = 28;
            this.btnColormapViewer.Text = "Colormap Viewer";
            //this.btnColormapViewer.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            //this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.label14.Location = new System.Drawing.Point(87, 718);
            //this.label14.Name = "label14";
            this.label14.Size = new Size(210, 47);
            this.label14.TabIndex = 27;
            this.label14.Text = "Demonstrate how axis boundaries can be used to constrain axis limits in interacti" +
    "ve plots";
            //this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label14.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label14.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // AxisLimitsButton
            // 
            //this.AxisLimitsButton.Location = new System.Drawing.Point(6, 718);
            //this.AxisLimitsButton.Name = "AxisLimitsButton";
            this.AxisLimitsButton.Size = new Size(75, 47);
            this.AxisLimitsButton.TabIndex = 26;
            this.AxisLimitsButton.Text = "Axis Limits";
            //this.AxisLimitsButton.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            //this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.label13.Location = new System.Drawing.Point(87, 665);
            //this.label13.Name = "label13";
            this.label13.Size = new Size(210, 47);
            this.label13.TabIndex = 25;
            this.label13.Text = "Display multiple plots in a scrolling control";
            //this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label13.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label13.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // ScrollViewerButton
            // 
            //this.ScrollViewerButton.Location = new System.Drawing.Point(6, 665);
            //this.ScrollViewerButton.Name = "ScrollViewerButton";
            this.ScrollViewerButton.Size = new Size(75, 47);
            this.ScrollViewerButton.TabIndex = 24;
            this.ScrollViewerButton.Text = "Plot in a Scroll Viewer";
            //this.ScrollViewerButton.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            //this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.label12.Location = new System.Drawing.Point(87, 612);
            //this.label12.Name = "label12";
            this.label12.Size = new Size(210, 47);
            this.label12.TabIndex = 23;
            this.label12.Text = "Display a custom menu (or perform a different action) when the control is right-c" +
    "licked";
            //this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label12.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label12.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // RightClickMenuButton
            // 
            //this.RightClickMenuButton.Location = new System.Drawing.Point(6, 612);
            //this.RightClickMenuButton.Name = "RightClickMenuButton";
            this.RightClickMenuButton.Size = new Size(75, 47);
            this.RightClickMenuButton.TabIndex = 22;
            this.RightClickMenuButton.Text = "Custom Right- Click Menu";
            //this.RightClickMenuButton.UseVisualStyleBackColor = true;
            // 
            // PlotViewerButton
            // 
            //this.PlotViewerButton.Location = new System.Drawing.Point(6, 135);
            //this.PlotViewerButton.Name = "PlotViewerButton";
            this.PlotViewerButton.Size = new Size(75, 47);
            this.PlotViewerButton.TabIndex = 20;
            this.PlotViewerButton.Text = "Plot Viewer";
            //this.PlotViewerButton.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            //this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.label11.Location = new System.Drawing.Point(87, 135);
            //this.label11.Name = "label11";
            this.label11.Size = new Size(210, 47);
            this.label11.TabIndex = 21;
            this.label11.Text = "Create a ScottPlot programmatically then display it in an interactive window";
            //this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label11.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label11.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // label10
            // 
            //this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.label10.Location = new System.Drawing.Point(87, 506);
            //this.label10.Name = "label10";
            this.label10.Size = new Size(210, 47);
            this.label10.TabIndex = 19;
            this.label10.Text = "Display the value of the data point nearest the cursor";
            //this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label10.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label10.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // btnShowOnHover
            // 
            //this.btnShowOnHover.Location = new System.Drawing.Point(6, 506);
            //this.btnShowOnHover.Name = "btnShowOnHover";
            this.btnShowOnHover.Size = new Size(75, 47);
            this.btnShowOnHover.TabIndex = 18;
            this.btnShowOnHover.Text = "Show Value on Hover";
            //this.btnShowOnHover.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            //this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.label9.Location = new System.Drawing.Point(87, 453);
            //this.label9.Name = "label9";
            this.label9.Size = new Size(210, 47);
            this.label9.TabIndex = 17;
            this.label9.Text = "Display live data that grows with time";
            //this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label9.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label9.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // label8
            // 
            //this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.label8.Location = new System.Drawing.Point(87, 400);
            //this.label8.Name = "label8";
            this.label8.Size = new Size(210, 47);
            this.label8.TabIndex = 16;
            this.label8.Text = "Display live data from a fixed-length array that continuously changes";
            //this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label8.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label8.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // GrowingData
            // 
            //this.GrowingData.Location = new System.Drawing.Point(6, 453);
            //this.GrowingData.Name = "GrowingData";
            this.GrowingData.Size = new Size(75, 47);
            this.GrowingData.TabIndex = 15;
            this.GrowingData.Text = "Growing Data";
            //this.GrowingData.UseVisualStyleBackColor = true;
            // 
            // LiveData
            // 
            //this.LiveData.Location = new System.Drawing.Point(5, 400);
            //this.LiveData.Name = "LiveData";
            this.LiveData.Size = new Size(75, 47);
            this.LiveData.TabIndex = 14;
            this.LiveData.Text = "Live Data";
            //this.LiveData.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            //this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.label7.Location = new System.Drawing.Point(87, 348);
            //this.label7.Name = "label7";
            this.label7.Size = new Size(210, 47);
            this.label7.TabIndex = 13;
            this.label7.Text = "Link the axes of two plots together so adjusting one changes the other";
            //this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label7.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label7.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // LinkedAxesButton
            // 
            //this.LinkedAxesButton.Location = new System.Drawing.Point(6, 348);
            //this.LinkedAxesButton.Name = "LinkedAxesButton";
            this.LinkedAxesButton.Size = new Size(75, 47);
            this.LinkedAxesButton.TabIndex = 12;
            this.LinkedAxesButton.Text = "Linked Axes";
            //this.LinkedAxesButton.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            //this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.label6.Location = new System.Drawing.Point(87, 294);
            //this.label6.Name = "label6";
            this.label6.Size = new Size(210, 47);
            this.label6.TabIndex = 11;
            this.label6.Text = "Advanced styling and behavior customization";
            //this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label6.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // ConfigButton
            // 
            //this.ConfigButton.Location = new System.Drawing.Point(6, 294);
            //this.ConfigButton.Name = "ConfigButton";
            this.ConfigButton.Size = new Size(75, 47);
            this.ConfigButton.TabIndex = 10;
            this.ConfigButton.Text = "FormsPlot Config";
            //this.ConfigButton.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            //this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.label5.Location = new System.Drawing.Point(87, 241);
            //this.label5.Name = "label5";
            this.label5.Size = new Size(210, 47);
            this.label5.TabIndex = 9;
            this.label5.Text = "Checkboxes control visibility of individual plot objects";
            //this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label5.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // ToggleVisibilityButton
            // 
            //this.ToggleVisibilityButton.Location = new System.Drawing.Point(6, 241);
            //this.ToggleVisibilityButton.Name = "ToggleVisibilityButton";
            this.ToggleVisibilityButton.Size = new Size(75, 47);
            this.ToggleVisibilityButton.TabIndex = 8;
            this.ToggleVisibilityButton.Text = "Toggle Visibility";
            //this.ToggleVisibilityButton.UseVisualStyleBackColor = true;
            // 
            // MouseTrackerButton
            // 
            //this.MouseTrackerButton.Location = new System.Drawing.Point(6, 188);
            //this.MouseTrackerButton.Name = "MouseTrackerButton";
            this.MouseTrackerButton.Size = new Size(75, 47);
            this.MouseTrackerButton.TabIndex = 4;
            this.MouseTrackerButton.Text = "Mouse Tracker";
            //this.MouseTrackerButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            //this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.label2.Location = new System.Drawing.Point(87, 559);
            //this.label2.Name = "label2";
            this.label2.Size = new Size(210, 47);
            this.label2.TabIndex = 7;
            this.label2.Text = "Demonstrate a control with a transparent background";
            //this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label2.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // label4
            // 
            //this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.label4.Location = new System.Drawing.Point(87, 188);
            //this.label4.Name = "label4";
            this.label4.Size = new Size(210, 47);
            this.label4.TabIndex = 5;
            this.label4.Text = "Display the position under the mouse cursor";
            //this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label4.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // TransparentBackgroundButton
            // 
            //this.TransparentBackgroundButton.Location = new System.Drawing.Point(6, 559);
            //this.TransparentBackgroundButton.Name = "TransparentBackgroundButton";
            this.TransparentBackgroundButton.Size = new Size(75, 47);
            this.TransparentBackgroundButton.TabIndex = 6;
            this.TransparentBackgroundButton.Text = "Transparent Background";
            //this.TransparentBackgroundButton.UseVisualStyleBackColor = true;
            // 
            // StyleBrowserButton
            // 
            //this.StyleBrowserButton.Location = new System.Drawing.Point(6, 878);
            //this.StyleBrowserButton.Name = "StyleBrowserButton";
            this.StyleBrowserButton.Size = new Size(75, 47);
            this.StyleBrowserButton.TabIndex = 33;
            this.StyleBrowserButton.Text = "Style Browser";
            //this.StyleBrowserButton.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            //this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.label17.Location = new System.Drawing.Point(88, 877);
            //this.label17.Name = "label17";
            this.label17.Size = new Size(210, 47);
            this.label17.TabIndex = 34;
            this.label17.Text = "View available predefined styles";
            //this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label17.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label17.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // FormMain
            // 
            //this.AutoScaleDimensions = new SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.AutoScroll = true;
            //this.ClientSize = new Size(342, 547);
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            //this.Name = "FormMain";
            this.Shown += (o, e) =>
            {
                this.Location = (Point)Screen.PrimaryScreen.WorkingArea.Center - this.Size / 2;
            };
            this.Width = 400;
            this.Height = (int)Screen.PrimaryScreen.WorkingArea.Height * 3 / 4;
            this.Title = "ScottPlot Demo (Eto.Forms)";
            this.groupBox1.ResumeLayout();
            this.groupBox2.ResumeLayout();
            this.ResumeLayout();
        }

        private Button cookbookButton;
        private Label label1;
        private Label versionLabel;
        private Label label3;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button MouseTrackerButton;
        private Label label2;
        private Label label4;
        private Button TransparentBackgroundButton;
        private Label label6;
        private Button ConfigButton;
        private Label label5;
        private Button ToggleVisibilityButton;
        private Label label7;
        private Button LinkedAxesButton;
        private Label label9;
        private Label label8;
        private Button GrowingData;
        private Button LiveData;
        private Label label10;
        private Button btnShowOnHover;
        private Button PlotViewerButton;
        private Label label11;
        private Label label12;
        private Button RightClickMenuButton;
        private Label label13;
        private Button ScrollViewerButton;
        private Label label14;
        private Button AxisLimitsButton;
        private Label label15;
        private Button btnColormapViewer;
        private Label richTextBox1;
        private Label label16;
        private Button MultiAxisLockButton;
        private Label label17;
        private Button StyleBrowserButton;
    }
}
