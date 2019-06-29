using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemoLogScale
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            double[] xs = ScottPlot.DataGen.Consecutive(100, 0.1, 0.0);
            double[] ys = new double[xs.Length];
            for (int i = 0; i < ys.Length; i++)
                ys[i] = xs[i] * xs[i];

            Random rand = new Random();
            double[] xs2 = ScottPlot.DataGen.Random(rand, 100, 10);
            double[] ys2 = ScottPlot.DataGen.Random(rand, 100, 100);

            scottPlotUC1.plt.Title("Log Scale Test");
            scottPlotUC1.plt.PlotScatter(xs, ys);
            scottPlotUC1.plt.PlotScatter(xs2, ys2, lineWidth: 0);
            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.Render();
        }

        private void CbLogScale_CheckedChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
