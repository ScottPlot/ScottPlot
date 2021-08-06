using ScottPlot;
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

            double[,] data = DataGen.Random2D(new Random(), 10, 10);
            var hm = formsPlot1.Plot.AddHeatmap(data, lockScales: false);
            formsPlot1.Plot.AddColorbar(hm.Colormap);
            formsPlot1.Plot.SetAxisLimits(2.5, 2.6, 2.5, 2.6);

            // hide major tick marks (but not labels)
            formsPlot1.Plot.YAxis.Ticks(major: false, minor: true, majorLabels: true);

            // make ticks 0 to allow labels to be placed close to the edge of the plot
            (_, var ticks, _) = formsPlot1.Plot.YAxis.GetSettings(showWarning: false);
            ticks.MajorTickLength = 0;
        }
    }
}
