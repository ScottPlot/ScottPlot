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
            this.cookbookButton.BackgroundColor = Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(161)))));
            this.cookbookButton.Size = new Size(75, 59);
            this.cookbookButton.Text = "Launch ScottPlot Cookbook";
            // 
            // label1
            // 
            this.label1.Font = Fonts.Sans(16);
            this.label1.Size = new Size(160, 25);
            this.label1.Text = "ScottPlot Demo";
            // 
            // versionLabel
            // 
            this.versionLabel.Text = "version 9.9.99";
            // 
            // label3
            // 
            this.label3.Size = new Size(208, 59);
            this.label3.Text = "Simple examples that demonstrate the primary features of ScottPlot";
            this.label3.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label3.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // groupBox1
            //
            this.groupBox1.Text = "ScottPlot Cookbook";
            // 
            // groupBox2
            //
            this.groupBox2.Text = "Eto.Forms Control Demos";
            // 
            // label16
            // 
            this.label16.Size = new Size(210, 47);
            this.label16.Text = "Selectively pan/zoom individual axes in multi-axis plots";
            this.label16.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label16.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // MultiAxisLockButton
            // 
            this.MultiAxisLockButton.Size = new Size(75, 47);
            this.MultiAxisLockButton.Text = "Multi-Axis Lock";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackgroundColor = SystemColors.Control;
            this.richTextBox1.Size = new Size(291, 111);
            this.richTextBox1.Text = @"These examples demonstrate advanced functionality for the Eto.Forms ScottPlot control (PlotView).
Source code for these demos can be found on the ScottPlot Demo Website https://scottplot.net/demo/ along with additional information and advanced implementation recommendations.";
            // 
            // label15
            // 
            this.label15.Size = new Size(210, 47);
            this.label15.Text = "Show available colormaps";
            this.label15.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label15.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // btnColormapViewer
            // 
            this.btnColormapViewer.Size = new Size(75, 47);
            this.btnColormapViewer.Text = "Colormap Viewer";
            // 
            // label14
            // 
            this.label14.Size = new Size(210, 47);
            this.label14.Text = "Demonstrate how axis boundaries can be used to constrain axis limits in interactive plots";
            this.label14.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label14.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // AxisLimitsButton
            // 
            this.AxisLimitsButton.Size = new Size(75, 47);
            this.AxisLimitsButton.Text = "Axis Limits";
            // 
            // label13
            // 
            this.label13.Size = new Size(210, 47);
            this.label13.Text = "Display multiple plots in a scrolling control";
            this.label13.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label13.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // ScrollViewerButton
            // 
            this.ScrollViewerButton.Size = new Size(75, 47);
            this.ScrollViewerButton.Text = "Plot in a Scroll Viewer";
            // 
            // label12
            // 
            this.label12.Size = new Size(210, 47);
            this.label12.Text = "Display a custom menu (or perform a different action) when the control is right-clicked";
            this.label12.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label12.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // RightClickMenuButton
            // 
            this.RightClickMenuButton.Size = new Size(75, 47);
            this.RightClickMenuButton.Text = "Custom Right- Click Menu";
            // 
            // PlotViewerButton
            // 
            this.PlotViewerButton.Size = new Size(75, 47);
            this.PlotViewerButton.Text = "Plot Viewer";
            // 
            // label11
            // 
            this.label11.Size = new Size(210, 47);
            this.label11.Text = "Create a ScottPlot programmatically then display it in an interactive window";
            this.label11.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label11.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // label10
            // 
            this.label10.Size = new Size(210, 47);
            this.label10.Text = "Display the value of the data point nearest the cursor";
            this.label10.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label10.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // btnShowOnHover
            // 
            this.btnShowOnHover.Size = new Size(75, 47);
            this.btnShowOnHover.Text = "Show Value on Hover";
            // 
            // label9
            // 
            this.label9.Size = new Size(210, 47);
            this.label9.Text = "Display live data that grows with time";
            this.label9.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label9.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // label8
            // 
            this.label8.Size = new Size(210, 47);
            this.label8.Text = "Display live data from a fixed-length array that continuously changes";
            this.label8.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label8.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // GrowingData
            // 
            this.GrowingData.Size = new Size(75, 47);
            this.GrowingData.Text = "Growing Data";
            // 
            // LiveData
            // 
            this.LiveData.Size = new Size(75, 47);
            this.LiveData.Text = "Live Data";
            // 
            // label7
            // 
            this.label7.Size = new Size(210, 47);
            this.label7.Text = "Link the axes of two plots together so adjusting one changes the other";
            this.label7.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label7.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // LinkedAxesButton
            // 
            this.LinkedAxesButton.Size = new Size(75, 47);
            this.LinkedAxesButton.Text = "Linked Axes";
            // 
            // label6
            // 
            this.label6.Size = new Size(210, 47);
            this.label6.Text = "Advanced styling and behavior customization";
            this.label6.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label6.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // ConfigButton
            // 
            this.ConfigButton.Size = new Size(75, 47);
            this.ConfigButton.Text = "FormsPlot Config";
            // 
            // label5
            // 
            this.label5.Size = new Size(210, 47);
            this.label5.Text = "Checkboxes control visibility of individual plot objects";
            this.label5.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label5.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // ToggleVisibilityButton
            // 
            this.ToggleVisibilityButton.Size = new Size(75, 47);
            this.ToggleVisibilityButton.Text = "Toggle Visibility";
            // 
            // MouseTrackerButton
            // 
            this.MouseTrackerButton.Size = new Size(75, 47);
            this.MouseTrackerButton.Text = "Mouse Tracker";
            // 
            // label2
            // 
            this.label2.Size = new Size(210, 47);
            this.label2.Text = "Demonstrate a control with a transparent background";
            this.label2.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label2.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // label4
            // 
            this.label4.Size = new Size(210, 47);
            this.label4.Text = "Display the position under the mouse cursor";
            this.label4.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label4.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // TransparentBackgroundButton
            // 
            this.TransparentBackgroundButton.Size = new Size(75, 47);
            this.TransparentBackgroundButton.Text = "Transparent Background";
            // 
            // StyleBrowserButton
            // 
            this.StyleBrowserButton.Size = new Size(75, 47);
            this.StyleBrowserButton.Text = "Style Browser";
            // 
            // label17
            // 
            this.label17.Size = new Size(210, 47);
            this.label17.Text = "View available predefined styles";
            this.label17.VerticalAlignment = global::Eto.Forms.VerticalAlignment.Center;
            this.label17.TextAlignment = global::Eto.Forms.TextAlignment.Left;
            // 
            // FormMain
            // 
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
