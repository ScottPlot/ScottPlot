using Eto.Drawing;
using Eto.Forms;
using System;

namespace EtoApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            var pv = new ScottPlot.Eto.PlotView();

            int pointCount = 10;
            var rand = new Random(0);
            double[] xs = ScottPlot.DataGen.Random(rand, pointCount);
            double[] ys = ScottPlot.DataGen.Random(rand, pointCount);
            pv.Plot.AddScatter(xs, ys);
            pv.Refresh();

            Content = pv;
        }
    }
}
