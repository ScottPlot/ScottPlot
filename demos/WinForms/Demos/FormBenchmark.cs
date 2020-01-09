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
    public partial class FormBenchmark : Form
    {
        private double[] xs100;
        private double[] ys100;
        private double[] tenMillionPoints;

        public FormBenchmark()
        {
            InitializeComponent();
            CreateData();

            formsPlot1.plt.Benchmark();
            formsPlot1.plt.YLabel("vertical units");
            formsPlot1.plt.XLabel("horizontal units");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbPlotType.Items.Add("scatter plot (100)");
            cbPlotType.Items.Add("signal (10M)");
            cbPlotType.SelectedItem = cbPlotType.Items[0];
            //RunBenchmark();
        }

        private void CreateData()
        {
            var rand = new Random(0);
            xs100 = ScottPlot.DataGen.Consecutive(100);
            ys100 = ScottPlot.DataGen.NoisySin(rand, 100);
            tenMillionPoints = ScottPlot.DataGen.RandomWalk(rand, 10_000_000);
        }

        private void PlotScatter100()
        {
            formsPlot1.plt.Title("Scatter Plot (100 points)");
            formsPlot1.plt.Clear();
            formsPlot1.plt.PlotScatter(xs100, ys100);
            formsPlot1.Render();
        }

        private void PlotSignal10M()
        {
            formsPlot1.plt.Title("Signal Plot (10 million points)");
            formsPlot1.plt.Clear();
            formsPlot1.plt.PlotSignal(tenMillionPoints);
            formsPlot1.Render();
        }

        private void PlotResults(double[] timesMsec)
        {
            double mean = timesMsec.Sum() / timesMsec.Length;
            Array.Sort(timesMsec);
            double median = timesMsec[(int)(timesMsec.Length / 2)];

            var hist = new ScottPlot.Statistics.Histogram(timesMsec);
            formsPlot2.plt.Clear();
            formsPlot2.plt.PlotBar(hist.bins, hist.counts);
            formsPlot2.plt.PlotVLine(mean, lineWidth: 3, label: "mean");
            formsPlot2.plt.PlotVLine(median, lineWidth: 3, label: "median", lineStyle: ScottPlot.LineStyle.Dot);
            formsPlot2.plt.Legend();
            formsPlot2.plt.AxisAuto();
            formsPlot2.plt.Axis(y1: 0);
            formsPlot2.plt.YLabel("Count");
            formsPlot2.plt.XLabel("Render Time (ms)");
            formsPlot2.plt.Title($"Median: {median} ms");
            formsPlot2.Render();
        }

        private void RunBenchmark()
        {
            btnRun.Enabled = false;
            nudIterations.Enabled = false;
            cbPlotType.Enabled = false;

            double[] timesMsec = new double[(int)nudIterations.Value];
            var settings = formsPlot1.plt.GetSettings();
            progressBar1.Maximum = (int)nudIterations.Value;

            for (int i = 0; i < nudIterations.Value; i++)
            {
                progressBar1.Value = i + 1;
                switch (cbPlotType.SelectedItem.ToString())
                {
                    case "scatter plot (100)":
                        PlotScatter100();
                        break;
                    case "signal (10M)":
                        PlotSignal10M();
                        break;
                    default:
                        throw new NotImplementedException();
                }
                if (i == 0)
                    formsPlot1.plt.AxisAuto();
                Application.DoEvents();
                timesMsec[i] = settings.benchmark.msec;
            }

            progressBar1.Value = 0;

            btnRun.Enabled = true;
            nudIterations.Enabled = true;
            cbPlotType.Enabled = true;

            PlotResults(timesMsec.ToArray());
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            RunBenchmark();
        }
    }
}
