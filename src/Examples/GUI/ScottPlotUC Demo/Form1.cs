using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotUC_Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            scottPlotUC1.showBenchmark = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false; // turn off live mode

            int pointCount = 1_000_000;
            scottPlotUC1.Xs = null;
            scottPlotUC1.Ys = scottPlotUC1.fig.gen.RandomWalk(pointCount);
            scottPlotUC1.fig.title = string.Format("Signal Mode: {0:n0} Points", scottPlotUC1.Ys.Length);
            scottPlotUC1.ResetAxis();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false; // turn off live mode

            int pointCount = 100;
            scottPlotUC1.Xs = scottPlotUC1.fig.gen.Sequence(pointCount);
            scottPlotUC1.Ys = scottPlotUC1.fig.gen.RandomWalk(pointCount);
            scottPlotUC1.fig.title = string.Format("X/Y Plotting Mode: {0:n0} Points", scottPlotUC1.Ys.Length);
            scottPlotUC1.ResetAxis();
        }

        System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
        private void UpdateLiveData(bool resetAxis = false)
        {
            int pointCount = 1_000;
            scottPlotUC1.Xs = null;
            scottPlotUC1.Ys = new double[pointCount];

            double offset = stopwatch.ElapsedMilliseconds/10;
            for (int i=0; i<pointCount; i++) scottPlotUC1.Ys[i] = Math.Sin(((double)i + offset) / 20);

            if (resetAxis)
            {
                scottPlotUC1.fig.title = string.Format("Live Signal: {0:n0} Points", scottPlotUC1.Ys.Length);
                scottPlotUC1.ResetAxis();
            } else
            {
                scottPlotUC1.fig.ClearGraph();
                scottPlotUC1.Redraw();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UpdateLiveData(true);
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateLiveData(false);
        }
    }
}
