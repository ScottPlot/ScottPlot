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
    public partial class FormBmp : Form
    {
        private readonly ScottPlot.Plot plt = new ScottPlot.Plot();

        public FormBmp()
        {
            InitializeComponent();
            formsPlot1.Reset();
        }

        private void RunBenchmark(string label = null, int rendersPerSize = 10)
        {
            double[] widths = ScottPlot.DataGen.Consecutive(20, 100, 200);
            double[] mean = new double[widths.Length];
            double[] err = new double[widths.Length];

            progressBar1.Maximum = rendersPerSize * widths.Length;

            for (int i = 0; i < widths.Length; i++)
            {
                int width = (int)widths[i];
                int height = width * 3 / 4;
                plt.Resize(width, height);
                plt.GetBitmap();

                double[] times = new double[rendersPerSize];
                for (int j = 0; j < rendersPerSize; j++)
                {
                    progressBar1.Value = rendersPerSize * i + j + 1;
                    plt.GetBitmap(renderFirst: true);
                    times[j] = plt.GetSettings(false).Benchmark.msec;
                }

                mean[i] = ScottPlot.Statistics.Common.Mean(times);
                err[i] = ScottPlot.Statistics.Common.StDev(times);

                pictureBox1.Image?.Dispose();
                pictureBox1.Image = (Bitmap)plt.GetBitmap().Clone();
                Application.DoEvents();
            }

            formsPlot1.plt.PlotScatter(widths, mean, errorY: err, label: label);
            formsPlot1.plt.AxisAuto();
            formsPlot1.plt.Title("Benchmark (4:3 aspect)");
            formsPlot1.plt.YLabel("Render Time (ms)");
            formsPlot1.plt.XLabel("Plot Width (px)");
            formsPlot1.plt.Legend(location: ScottPlot.legendLocation.upperLeft);
            formsPlot1.Render();
        }

        private void TestEmpty_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Clear();

            plt.Clear();
            RunBenchmark("Baseline");
        }

        private void TestScatter_Click(object sender, EventArgs e)
        {
            Random rand = new Random(0);

            formsPlot1.plt.Clear();
            RunBenchmark("Baseline");

            plt.Clear();
            plt.PlotScatter(
                ScottPlot.DataGen.RandomWalk(rand, 100),
                ScottPlot.DataGen.RandomWalk(rand, 100));
            plt.AxisAuto();
            RunBenchmark("Scatter (100 pts)");

            plt.Clear();
            plt.PlotScatter(
                ScottPlot.DataGen.RandomWalk(rand, 1000),
                ScottPlot.DataGen.RandomWalk(rand, 1000));
            plt.AxisAuto();
            RunBenchmark("Scatter (1k pts)");
        }

        private void TestSignal_Click(object sender, EventArgs e)
        {
            Random rand = new Random(0);

            formsPlot1.plt.Clear();

            plt.Clear();
            RunBenchmark("Baseline");

            plt.Clear();
            plt.PlotSignal(ScottPlot.DataGen.RandomWalk(rand, 10_000));
            plt.AxisAuto();
            RunBenchmark("Signal (10k pts)");

            plt.Clear();
            plt.PlotSignal(ScottPlot.DataGen.RandomWalk(rand, 1_000_000));
            plt.AxisAuto();
            RunBenchmark("Signal (1M pts)");
        }

        private void TestSignalConst_Click(object sender, EventArgs e)
        {
            Random rand = new Random(0);

            formsPlot1.plt.Clear();

            plt.Clear();
            RunBenchmark("Baseline");

            plt.Clear();
            plt.PlotSignal(ScottPlot.DataGen.RandomWalk(rand, 1_000_000));
            plt.AxisAuto();
            RunBenchmark("Signal (1M pts)");

            plt.Clear();
            plt.PlotSignalConst(ScottPlot.DataGen.RandomWalk(rand, 1_000_000));
            plt.AxisAuto();
            RunBenchmark("SignalConst (1M pts)");
        }
    }
}
