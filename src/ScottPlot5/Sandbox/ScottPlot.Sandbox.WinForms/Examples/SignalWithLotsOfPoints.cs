using ScottPlot.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.Examples
{
    public partial class SignalWithLotsOfPoints : Form, IExampleForm
    {
        public string SandboxTitle => "Signal Plot Point Test";

        public string SandboxDescription => "Create signal plots using user-defined number of points";

        public SignalWithLotsOfPoints()
        {
            InitializeComponent();
            PlotPoints(1_000);
        }

        private void button1_Click(object sender, EventArgs e) => PlotPoints(1_000);
        private void button2_Click(object sender, EventArgs e) => PlotPoints(100_000);
        private void button3_Click(object sender, EventArgs e) => PlotPoints(10_000_000);

        private void PlotPoints(int count)
        {
            formsPlot1.Plot.Plottables.Clear();
            Random rand = new();
            double[] ys = ScottPlot.Generate.NoisySin(rand, count);
            formsPlot1.Plot.Plottables.AddSignal(ys);
            formsPlot1.Plot.AutoScale();
            formsPlot1.Refresh();
        }
    }
}
