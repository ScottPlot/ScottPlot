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

            // create a plot using the primary X and Y axes
            var sp1 = formsPlot1.Plot.AddSignal(ScottPlot.DataGen.Sin(51));
            sp1.XAxisIndex = 0;
            sp1.YAxisIndex = 0;

            // create a plot using the secondary X and Y axes
            var sp2 = formsPlot1.Plot.AddSignal(ScottPlot.DataGen.Cos(51));
            sp2.XAxisIndex = 1;
            sp2.YAxisIndex = 1;

            // enable tick marks for secondary axes (hidden by default)
            formsPlot1.Plot.XAxis2.Ticks(true);
            formsPlot1.Plot.YAxis2.Ticks(true);

            // set view limits for the primary axis (default axis index is 0)
            formsPlot1.Plot.SetInnerViewLimits(20, 40, -.5, .5);
            formsPlot1.Plot.SetOuterViewLimits(0, 51, -1, 1);

            // set view limits for the secondary axis
            formsPlot1.Plot.SetInnerViewLimits(20, 40, -.5, .5, xAxisIndex: 1, yAxisIndex: 1);
            formsPlot1.Plot.SetOuterViewLimits(0, 51, -1, 1, xAxisIndex: 1, yAxisIndex: 1);

            formsPlot1.Refresh();
        }
    }
}
