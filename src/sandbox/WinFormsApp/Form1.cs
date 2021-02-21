using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        readonly ScottPlot.FormsPlot formsPlot1;
        readonly Random Rand = new Random(0);
        readonly ScottPlot.Plottable.SignalPlotList SignalPlotList;
        readonly Stopwatch Stopwatch = Stopwatch.StartNew();

        public Form1()
        {
            InitializeComponent();

            // The plot control is added manually because the .NET core winforms designer is still buggy
            // and does not always show it in the toolbox
            formsPlot1 = new ScottPlot.FormsPlot() { Dock = DockStyle.Fill };
            Controls.Add(formsPlot1);

            SignalPlotList = formsPlot1.Plot.AddSignalList();
            formsPlot1.Plot.SetAxisLimitsY(-2, 2);

            timer1.Enabled = true;
            timer2.Enabled = true;
        }

        // high frequency tick (1 ms) adds data
        private void timer1_Tick(object sender, EventArgs e)
        {
            double newValue = Math.Sin(Stopwatch.Elapsed.TotalSeconds) + Rand.NextDouble() - .5;
            SignalPlotList.Add(newValue);
        }

        // low frequency tick (20 ms) renders the plot
        private void timer2_Tick(object sender, EventArgs e)
        {
            var axisLimits = formsPlot1.Plot.GetAxisLimits();

            if (axisLimits.XMax < SignalPlotList.LastX)
                formsPlot1.Plot.SetAxisLimits(xMax: axisLimits.XMax + 250);

            formsPlot1.Render();
        }
    }
}
