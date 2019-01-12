using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemos
{
    public partial class FormScatter : Form
    {
        public FormScatter()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            scottPlotUC1.plt.settings.title = "ScottPlot Demo: XY Data";
            scottPlotUC1.plt.settings.axisLabelY = "Random Walk";
            scottPlotUC1.plt.settings.axisLabelX = "Sample Number";
            scottPlotUC1.plt.settings.benchmarkShow = true;
            scottPlotUC1.Render();
        }

        private void AddRandomWalk()
        {
            // create data to be plotted
            int pointCount = 100;
            double[] xs = new double[pointCount];
            double[] ys = new double[pointCount];

            // random walk to generate data
            Random rand = new Random();
            for (int i = 0; i < pointCount; i++)
            {
                xs[i] = i + 1;
                if (i == 0)
                    ys[i] = rand.NextDouble()*20-10;
                else
                    ys[i] = ys[i - 1] + rand.NextDouble() * 2 - 1;
            }

            // add the data and fit to what we have
            scottPlotUC1.plt.data.AddScatter(xs, ys);
            scottPlotUC1.plt.settings.AxisFit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddRandomWalk();
            scottPlotUC1.Render();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            scottPlotUC1.plt.data.Clear();
            scottPlotUC1.Render();
        }
    }
}
