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
            Random rand = new Random();
            double[] xs = ScottPlot.DataGen.Consecutive(100, 0.1, 0.0);
            double[] ys = new double[xs.Length];
            for (int i = 0; i < ys.Length; i++)
                ys[i] = xs[i] * xs[i];

            scottPlotUC1.plt.Title("Log Scale Test");
            scottPlotUC1.plt.PlotScatter(xs, ys);
            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.Render();
        }
    }
}
