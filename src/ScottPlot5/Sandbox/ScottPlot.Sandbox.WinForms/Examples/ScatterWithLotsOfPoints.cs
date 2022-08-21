using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.Examples
{
    public partial class ScatterWithLotsOfPoints : Form, IExampleForm
    {
        public string SandboxTitle => "Scatter Plot Point Test";

        public string SandboxDescription => "Create scatter plots using user-defined number of points";

        public ScatterWithLotsOfPoints()
        {
            InitializeComponent();
            PlotPoints(1_000);
        }

        private void button1_Click(object sender, EventArgs e) => PlotPoints(1_000);

        private void button3_Click_1(object sender, EventArgs e) => PlotPoints(10_000);

        private void button2_Click(object sender, EventArgs e) => PlotPoints(100_000);

        private void PlotPoints(int count)
        {
            formsPlot1.Plot.Plottables.Clear();
            Random rand = new();
            double[] xs = ScottPlot.Generate.Consecutive(count);
            double[] ys = ScottPlot.Generate.NoisySin(rand, count);
            formsPlot1.Plot.Plottables.AddScatter(xs, ys);
            formsPlot1.Plot.AutoScale();
            formsPlot1.Refresh();
        }
    }
}
