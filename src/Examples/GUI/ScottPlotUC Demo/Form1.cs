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
        


        private void btn_xy_mode(object sender, EventArgs e)
        {
            scottPlotUC1.fig.title = string.Format("X/Y Plotting Mode: 100 Points");
            timer1.Enabled = false; // turn off live mode
            scottPlotUC1.signals = null; // remove all signals
            scottPlotUC1.Xs = scottPlotUC1.fig.gen.Sequence(100);
            scottPlotUC1.Ys = scottPlotUC1.fig.gen.RandomWalk(100);
            scottPlotUC1.ResetAxis();
        }

        private void btn_animated_sine(object sender, EventArgs e)
        {
            scottPlotUC1.fig.title = string.Format("Live Signal: 1000 Points");
            scottPlotUC1.Xs = null; // clear the old signals
            scottPlotUC1.Ys = null; // clear the old signals
            
            scottPlotUC1.signals = new ScottPlot.ScottPlotUC.signal[1]; // prepare for a single signal
            scottPlotUC1.fig.Axis(0, .05, -1.1, 1.1); // we know what the limits should be
            timer1.Enabled = true; // start automatic updates
        }

        System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
        public double[] AnimatedSine(int pointCount = 1000)
        {
            double offset = stopwatch.ElapsedMilliseconds / 10;
            double[] vals = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
                vals[i] = Math.Sin(((double)i + offset) / 20);
            return vals;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            scottPlotUC1.signals[0] = new ScottPlot.ScottPlotUC.signal(AnimatedSine(), 20000);
            scottPlotUC1.fig.ClearGraph();
            scottPlotUC1.Redraw();
        }

        private void btn_oneMillionPoints(object sender, EventArgs e)
        {
            // 1 million point random signal
            scottPlotUC1.fig.title = string.Format("Signal Mode: 1,000,000 Points");
            scottPlotUC1.Xs = null; // clear the old signals
            scottPlotUC1.Ys = null; // clear the old signals
            timer1.Enabled = false; // turn off live mode

            scottPlotUC1.signals = new ScottPlot.ScottPlotUC.signal[]
            {
                new ScottPlot.ScottPlotUC.signal(scottPlotUC1.fig.gen.RandomWalk(1_000_000), 20_000)
            };
            scottPlotUC1.ResetAxis();
            scottPlotUC1.Redraw();
        }

        private void btn_sweeps(object sender, EventArgs e)
        {
            // several sweeps of 100,000 data points
            scottPlotUC1.fig.title = string.Format("Sweep Mode: 6 x 100,000 Points");
            scottPlotUC1.Xs = null; // clear the old signals
            scottPlotUC1.Ys = null; // clear the old signals
            timer1.Enabled = false; // turn off live mode

            scottPlotUC1.signals = new ScottPlot.ScottPlotUC.signal[]
            {
                new ScottPlot.ScottPlotUC.signal(scottPlotUC1.fig.gen.RandomWalk(100_000), 20_000, 0, 000, color: Color.Blue),
                new ScottPlot.ScottPlotUC.signal(scottPlotUC1.fig.gen.RandomWalk(100_000), 20_000, 0, 100, color: Color.Green),
                new ScottPlot.ScottPlotUC.signal(scottPlotUC1.fig.gen.RandomWalk(100_000), 20_000, 0, 200,color: Color.Red),
                new ScottPlot.ScottPlotUC.signal(scottPlotUC1.fig.gen.RandomWalk(100_000), 20_000, 0, 300, color: Color.Orange),
                new ScottPlot.ScottPlotUC.signal(scottPlotUC1.fig.gen.RandomWalk(100_000), 20_000, 0, 400,color: Color.Black),
            };
            scottPlotUC1.ResetAxis();
            scottPlotUC1.Redraw();
        }
    }
}
