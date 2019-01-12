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
    public partial class FormSignal : Form
    {
        public FormSignal()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            scottPlotUC1.plt.settings.title = "ScottPlot Demo: Signal Data";
            scottPlotUC1.plt.settings.axisLabelY = "Random Walk";
            scottPlotUC1.plt.settings.axisLabelX = "Time (seconds)";
            scottPlotUC1.plt.settings.benchmarkShow = true;
            scottPlotUC1.Render();
        }

        private void AddRandomWalk(int pointCount)
        {
            // create data to be plotted
            
            double[] ys = new double[pointCount];

            // random walk to generate data
            Random rand = new Random();
            for (int i = 0; i < pointCount; i++)
            {
                if (i == 0)
                    ys[i] = (rand.NextDouble() * 2 - 1) * (double)pointCount/1_000;
                else
                    ys[i] = ys[i - 1] + rand.NextDouble() * 2 - 1;
            }

            // add the data and fit to what we have
            scottPlotUC1.plt.data.AddSignal(ys, 20_000);
            scottPlotUC1.plt.settings.AxisFit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddRandomWalk(100_000);
            scottPlotUC1.Render();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddRandomWalk(1_000_000);
            scottPlotUC1.Render();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            scottPlotUC1.plt.data.Clear();
            scottPlotUC1.Render();
        }
    }
}
