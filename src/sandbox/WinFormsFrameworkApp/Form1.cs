using ScottPlot.Plottable;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsFrameworkApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            formsPlot1.Plot.AddSignal(ScottPlot.DataGen.Sin(51));
            formsPlot1.Plot.AddSignal(ScottPlot.DataGen.Cos(51));

            var vline1 = formsPlot1.Plot.AddVerticalLine(11, Color.Blue);
            vline1.DragEnabled = true;
            var vline2 = formsPlot1.Plot.AddVerticalLine(22, Color.Red);
            vline2.DragEnabled = true;

            formsPlot1.PlottableDragged += FormsPlot1_PlottableDragged;
            formsPlot1.PlottableDropped += FormsPlot1_PlottableDropped;
        }

        private void FormsPlot1_PlottableDragged(object sender, EventArgs e)
        {
            if (sender is ScottPlot.Plottable.VLine vline)
                label1.Text = $"dragged {vline.Color} to X={vline.X:N3}";
        }

        private void FormsPlot1_PlottableDropped(object sender, EventArgs e)
        {
            if (sender is ScottPlot.Plottable.VLine vline)
                label1.Text = $"dropped {vline.Color} at X={vline.X:N3}";
        }
    }
}
