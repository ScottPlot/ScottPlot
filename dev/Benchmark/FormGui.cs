using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Benchmark
{
    public partial class FormGui : Form
    {
        ScottPlot.PlottableScatter scat;
        ScottPlot.PlottableSignal sig;
        ScottPlot.PlottableSignalConst<double> sigConst;

        public FormGui()
        {
            InitializeComponent();

            Random rand = new Random(0);

            sig = formsPlot1.plt.PlotSignal(
                ys: ScottPlot.DataGen.RandomWalk(rand, 10_000_000),
                label: "Signal (10M)");

            sigConst = formsPlot1.plt.PlotSignalConst(
                ys: ScottPlot.DataGen.RandomWalk(rand, 10_000_000),
                label: "SignalConst (10M)");

            scat = formsPlot1.plt.PlotScatter(
                xs: ScottPlot.DataGen.Consecutive(1_000, spacing: 10_000),
                ys: ScottPlot.DataGen.RandomWalk(rand, 1_000, 20),
                label: "Scatter (1k)"); ;

            formsPlot1.plt.Legend();
            formsPlot1.plt.Axis();
            formsPlot1.Render();
        }

        private double[] AxisMidWay(double frac)
        {
            double x1A = 185735;
            double x1B = -500_000;
            double x1 = x1A + (x1B - x1A) * frac;

            double x2A = 185748;
            double x2B = 5_500_000;
            double x2 = x2A + (x2B - x2A) * frac;

            double y1A = -11;
            double y1B = -2000;
            double y1 = y1A + (y1B - y1A) * frac;

            double y2A = -6;
            double y2B = 2000;
            double y2 = y2A + (y2B - y2A) * frac;

            return new double[] { x1, x2, y1, y2 };
        }

        int renders = 0;
        Stopwatch sw;
        private void timer1_Tick(object sender, EventArgs e)
        {
            int slowness = 50;
            int rendersPerCycle = (int)(slowness * Math.PI * 2);
            double pos = (double)renders++ / slowness;
            double frac = (Math.Sin(pos) + 1);
            progressBar1.Maximum = rendersPerCycle;
            progressBar1.Value = renders % rendersPerCycle;

            frac = frac * frac;
            formsPlot1.plt.Axis(AxisMidWay(frac));
            formsPlot1.Render();

            if (renders % rendersPerCycle == 1)
            {
                if (sw is null)
                {
                    sw = Stopwatch.StartNew();
                }
                else
                {
                    double elapsedSec = (double)sw.ElapsedTicks / Stopwatch.Frequency;
                    double fps = rendersPerCycle / elapsedSec;
                    TimeLabel.Text = $"{fps:0.00} FPS";
                    sw.Restart();
                }
            }
        }

        private void RunCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = RunCheckbox.Checked;
        }

        private void AntiAliasCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.plt.AntiAlias(
                figure: AntiAliasCheckbox.Checked,
                data: AntiAliasCheckbox.Checked,
                legend: AntiAliasCheckbox.Checked);
        }
    }
}
