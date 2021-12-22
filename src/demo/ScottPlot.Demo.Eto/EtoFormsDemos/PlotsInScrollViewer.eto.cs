using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class PlotsInScrollViewer : Form
    {
        private void InitializeComponent()
        {
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.formsPlot2 = new ScottPlot.Eto.PlotView();
            this.formsPlot3 = new ScottPlot.Eto.PlotView();
            this.panel1 = new Scrollable();
            this.rbScroll = new RadioButton();
            this.rbZoom = new RadioButton(rbScroll);
            this.groupBox1 = new GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();

            this.groupBox1.Content = new StackLayout(rbScroll, rbZoom) { Padding = 5, Orientation = global::Eto.Forms.Orientation.Horizontal };

            panel1.Content = new DynamicLayout(formsPlot1, formsPlot2, formsPlot3) { Padding = 2 };
            panel1.Size = new Size(480, 430);

            this.Content = new StackLayout(groupBox1, panel1) { Padding = 5 };

            // 
            // formsPlot1
            // 
            this.formsPlot1.BackgroundColor = Colors.Transparent;
            this.formsPlot1.Size = new Size(452, 327);
            // 
            // formsPlot2
            // 
            this.formsPlot2.BackgroundColor = Colors.Transparent;
            this.formsPlot2.Size = new Size(452, 327);
            // 
            // formsPlot3
            // 
            this.formsPlot3.BackgroundColor = Colors.Transparent;
            this.formsPlot3.Size = new Size(452, 328);
            // 
            // rbScroll
            // 
            this.rbScroll.Size = new Size(97, 17);
            this.rbScroll.Text = "Scroll up/down";
            // 
            // rbZoom
            // 
            this.rbZoom.Checked = true;
            this.rbZoom.Size = new Size(83, 17);
            this.rbZoom.Text = "Zoom in/out";
            // 
            // groupBox1
            // 
            this.groupBox1.Text = "Mouse Wheel Action";
            // 
            // PlotsInScrollViewer
            // 
            this.Title = "Plots in a Scroll Viewer";
            this.groupBox1.ResumeLayout();
            this.ResumeLayout();
        }

        private ScottPlot.Eto.PlotView formsPlot1;
        private ScottPlot.Eto.PlotView formsPlot2;
        private ScottPlot.Eto.PlotView formsPlot3;
        private Scrollable panel1;
        private RadioButton rbScroll;
        private RadioButton rbZoom;
        private GroupBox groupBox1;
    }
}
