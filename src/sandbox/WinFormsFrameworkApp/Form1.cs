using ScottPlot.Plottable;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsFrameworkApp
{
    public partial class Form1 : Form
    {
        private readonly Random Rand = new Random();

        public Form1()
        {
            InitializeComponent();
            buttonSignal1M_Click(null, null);
        }

        private void btnScatter10_Click(object sender, EventArgs e)
        {
            int pointCount = 10;
            double[] xs = ScottPlot.DataGen.Random(Rand, pointCount);
            double[] ys = ScottPlot.DataGen.Random(Rand, pointCount);
            formsPlot1.Plot.Clear();
            formsPlot1.Plot.AddScatter(xs, ys);
            formsPlot1.Plot.Title("Scatter Plot with 10 Points");
            formsPlot1.Render();
        }

        private void buttonScatter1k_Click(object sender, EventArgs e)
        {
            int pointCount = 2_000;
            double[] xs = ScottPlot.DataGen.Random(Rand, pointCount);
            double[] ys = ScottPlot.DataGen.Random(Rand, pointCount);
            formsPlot1.Plot.Clear();
            formsPlot1.Plot.AddScatter(xs, ys);
            formsPlot1.Plot.Title("Scatter Plot with 2,000 Points");
            formsPlot1.Render();
        }

        private void buttonSignal1M_Click(object sender, EventArgs e)
        {
            int pointCount = 1_000_000;
            int sampleRate = 48_000;
            double[] data = ScottPlot.DataGen.RandomWalk(Rand, pointCount);
            formsPlot1.Plot.Clear();
            formsPlot1.Plot.AddSignal(data, sampleRate);
            formsPlot1.Plot.Title("Signal Plot with 1,000,000 Points");
            formsPlot1.Render();
        }
    }
}
