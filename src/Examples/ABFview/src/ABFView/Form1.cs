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
                scottPlotUC1.Clear();
                scottPlotUC1.PlotSignal(abf.data.ToArray(), 20_000);              
                scottPlotUC1.AxisSetToData();

            } else if (rb_sweep.Checked == true) { 
                // one sweep at a time
                scottPlotUC1.fig.title = string.Format("Single Sweep: {0:n0} Data Points", abf.sweepPointCount);
                
                // load subsweep data as a signal
                scottPlotUC1.Clear();
                scottPlotUC1.PlotSignal(abf.data.GetRange((int)nud_sweep.Value * abf.sweepPointCount, abf.sweepPointCount).ToArray(), 20_000);
                scottPlotUC1.AxisSetToData();

            } else if (rb_overlap.Checked == true)
            {
                // all sweeps overlapped
                scottPlotUC1.fig.title = string.Format("Overlapping Plot: {0:n0} Data Points", abf.data.Count);

                scottPlotUC1.Clear();
                for (int i=0; i < abf.sweepCount; i++)
                {
                    scottPlotUC1.PlotSignal(abf.data.GetRange(i * abf.sweepPointCount, abf.sweepPointCount).ToArray(), 
                                            sampleRate: 20_000);
                }
                scottPlotUC1.AxisSetToData();

            } else if (rb_stacked.Checked == true)
            {
                // all sweeps stacked
                scottPlotUC1.fig.title = string.Format("Stacked Sweeps: {0:n0} Data Points", abf.data.Count);

                scottPlotUC1.Clear();
                for (int i = 0; i < abf.sweepCount; i++)
                {
                    scottPlotUC1.PlotSignal(abf.data.GetRange(i * abf.sweepPointCount, abf.sweepPointCount).ToArray(),
                                            sampleRate: 20_000, offsetY: i * (int)nud_vsep.Value);
                }
                scottPlotUC1.AxisSetToData();
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
