using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABFView
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            scottPlotUC1.showBenchmark = true;
            button1_Click(null, null);
        }

        private ABF abf;

        private void button1_Click(object sender, EventArgs e)
        {
            string abfFileName = @"../../../../data/17o05028_ic_steps.abf";
            abf = new ABF(abfFileName);
            scottPlotUC1.fig.title = abf.abfID;
            nud_sweep.Maximum = abf.sweepCount - 1;
        }

        private void UpdatePlot()
        {
            if (rb_continuous.Checked == true)
            {
                // continuous plot
                scottPlotUC1.fig.title = string.Format("Continuous Plot: {0:n0} Data Points",abf.data.Count);
                scottPlotUC1.signals = new ScottPlot.ScottPlotUC.signal[]
                {
                    new ScottPlot.ScottPlotUC.signal(abf.data.ToArray(), 20_000),
                };
                scottPlotUC1.ResetAxis();

            } else if (rb_sweep.Checked == true) { 
                // one sweep at a time
                scottPlotUC1.fig.title = string.Format("Single Sweep: {0:n0} Data Points", abf.sweepPointCount);

                // get sweep data
                double[] sweepData = abf.data.GetRange((int)nud_sweep.Value * abf.sweepPointCount, abf.sweepPointCount).ToArray();

                // load it as a signal
                scottPlotUC1.signals = new ScottPlot.ScottPlotUC.signal[1];
                scottPlotUC1.signals[0] = new ScottPlot.ScottPlotUC.signal(sweepData, 20_000);
                scottPlotUC1.ResetAxis();

            } else if (rb_overlap.Checked == true)
            {
                // all sweeps overlapped
                scottPlotUC1.fig.title = string.Format("Overlapping Plot: {0:n0} Data Points", abf.data.Count);
                scottPlotUC1.signals = new ScottPlot.ScottPlotUC.signal[abf.sweepCount];

                for (int i=0; i < abf.sweepCount; i++)
                {
                    double[] sweepData = abf.data.GetRange(i*abf.sweepPointCount,abf.sweepPointCount).ToArray();
                    scottPlotUC1.signals[i] = new ScottPlot.ScottPlotUC.signal(sweepData, 20_000);
                }
                scottPlotUC1.ResetAxis();

            } else if (rb_stacked.Checked == true)
            {
                // all sweeps overlapped
                scottPlotUC1.fig.title = string.Format("Stacked Sweeps: {0:n0} Data Points", abf.data.Count);
                scottPlotUC1.signals = new ScottPlot.ScottPlotUC.signal[abf.sweepCount];

                for (int i = 0; i < abf.sweepCount; i++)
                {
                    double[] sweepData = abf.data.GetRange(i * abf.sweepPointCount, abf.sweepPointCount).ToArray();
                    scottPlotUC1.signals[i] = new ScottPlot.ScottPlotUC.signal(sweepData, 20_000, 0, i*(int)nud_vsep.Value);
                }
                scottPlotUC1.ResetAxis();
            }
        }

        private void rb_continuous_CheckedChanged(object sender, EventArgs e)
        {
            nud_sweep.Enabled = false;
            nud_vsep.Enabled = false;
            UpdatePlot();
        }

        private void rb_overlap_CheckedChanged(object sender, EventArgs e)
        {
            nud_sweep.Enabled = false;
            nud_vsep.Enabled = false;
            UpdatePlot();
        }

        private void rb_stacked_CheckedChanged(object sender, EventArgs e)
        {
            nud_sweep.Enabled = false;
            nud_vsep.Enabled = true;
            UpdatePlot();
        }

        private void rb_sweep_CheckedChanged(object sender, EventArgs e)
        {
            nud_sweep.Enabled = true;
            nud_vsep.Enabled = false;
            UpdatePlot();
        }

        private void nud_sweep_ValueChanged(object sender, EventArgs e)
        {
            UpdatePlot();
        }

        private void nud_vsep_ValueChanged(object sender, EventArgs e)
        {
            UpdatePlot();
        }
    }
}
