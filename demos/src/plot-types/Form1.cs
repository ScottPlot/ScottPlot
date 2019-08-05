using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemo
{
    public partial class Form1 : Form
    {
        Random rand = new Random();

        public Form1()
        {
            InitializeComponent();
            SetLabels();
            StyleLight();
            Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BtnScatterSin_Click(null, null);
            BtnScatterSin_Click(null, null);
            BtnScatterSin_Click(null, null);
        }

        private void Clear()
        {
            scottPlotUC1.plt.Clear();
            scottPlotUC1.plt.Axis(0, 10, -10, 10);
            scottPlotUC1.Render();
        }

        private void SetLabels()
        {
            scottPlotUC1.plt.YLabel("signal (mV)");
            scottPlotUC1.plt.XLabel("experiment duration (hours)");
            scottPlotUC1.plt.Title("ScottPlot Interactive Demo");
        }

        private void StyleLight()
        {
            BackColor = SystemColors.Control;
            cbDark.ForeColor = Color.Black;
            cbBenchmark.ForeColor = Color.Black;
            cbAntiAliasData.ForeColor = Color.Black;
            cbAntiAliasFigure.ForeColor = Color.Black;
            scottPlotUC1.plt.Style(ScottPlot.Style.Control);
            scottPlotUC1.Render();
        }

        private void StyleDark()
        {
            BackColor = ColorTranslator.FromHtml("#07263b");
            cbDark.ForeColor = Color.White;
            cbBenchmark.ForeColor = Color.White;
            cbAntiAliasData.ForeColor = Color.White;
            cbAntiAliasFigure.ForeColor = Color.White;
            scottPlotUC1.plt.Style(ScottPlot.Style.Blue1);
            scottPlotUC1.Render();
        }

        private void CbBenchmark_CheckedChanged(object sender, EventArgs e)
        {
            scottPlotUC1.plt.Benchmark(cbBenchmark.Checked);
            scottPlotUC1.Render();
        }

        private void CbDark_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDark.Checked)
                StyleDark();
            else
                StyleLight();
        }
        private void CbAntiAliasFigure_CheckedChanged(object sender, EventArgs e)
        {
            scottPlotUC1.plt.AntiAlias(cbAntiAliasFigure.Checked, cbAntiAliasData.Checked);
            scottPlotUC1.Render();
        }

        private void CbAntiAliasData_CheckedChanged(object sender, EventArgs e)
        {
            scottPlotUC1.plt.AntiAlias(cbAntiAliasFigure.Checked, cbAntiAliasData.Checked);
            scottPlotUC1.Render();
        }


        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void BtnScatterSin_Click(object sender, EventArgs e)
        {
            double[] xs = ScottPlot.DataGen.Consecutive(100, 10 / 100.0);
            double[] ys = ScottPlot.DataGen.Sin(pointCount: 100,
                oscillations: 2,
                phase: rand.NextDouble(),
                mult: rand.NextDouble() * 10 + 1,
                offset: rand.NextDouble() * 10 - 5);

            scottPlotUC1.plt.PlotScatter(xs, ys);
            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.Render();
        }

        private void BtnScatterRandom_Click(object sender, EventArgs e)
        {
            double[] xs = ScottPlot.DataGen.Random(rand, 100, 10);
            double[] ys = ScottPlot.DataGen.Random(rand, 100, 20, -10);

            scottPlotUC1.plt.PlotScatter(xs, ys);
            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.Render();
        }

        private void BtnMarker_Click(object sender, EventArgs e)
        {
            double x = rand.NextDouble() * 10;
            double y = rand.NextDouble() * 20 - 10;
            float markerSize = (float)(rand.NextDouble() * 10 + 1);
            Color pointColor = ScottPlot.DataGen.RandomColor(rand);
            scottPlotUC1.plt.PlotPoint(x, y, markerSize: markerSize, color: pointColor);
            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.Render();
        }
        private void BtnText_Click(object sender, EventArgs e)
        {
            double x = rand.NextDouble() * 10;
            double y = rand.NextDouble() * 20 - 10;
            float fontSize = (float)(rand.NextDouble() * 20 + 8);
            Color fontColor = ScottPlot.DataGen.RandomColor(rand);
            scottPlotUC1.plt.PlotText("demo", x, y, fontSize: fontSize, color: fontColor);
            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.Render();
        }

        private void BtnVline_Click(object sender, EventArgs e)
        {
            double position = rand.NextDouble() * 20 - 10;
            double width = (float)(rand.NextDouble() * 5 + 1);
            scottPlotUC1.plt.PlotVLine(position, lineWidth: (float)width, draggable: true);
            scottPlotUC1.Render();
        }

        private void BtnHline_Click(object sender, EventArgs e)
        {
            double position = rand.NextDouble() * 20 - 10;
            double width = (float)(rand.NextDouble() * 5 + 1);
            scottPlotUC1.plt.PlotHLine(position, lineWidth: (float)width, draggable: true);
            scottPlotUC1.Render();
        }

        private void RandomWalk(int pointCount)
        {
            double[] data = ScottPlot.DataGen.RandomWalk(rand, pointCount, 10, rand.NextDouble() * 10 - 5);
            scottPlotUC1.plt.PlotSignal(data, data.Length * 0.1);
            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.Render();
        }

        private void BtnSignal1k_Click(object sender, EventArgs e)
        {
            RandomWalk(1_000);
        }

        private void BtnSignal100k_Click(object sender, EventArgs e)
        {
            RandomWalk(100_000);
        }

        private void BtnSignal1m_Click(object sender, EventArgs e)
        {
            RandomWalk(1_000_000);
        }

        private void BtnSignal100m_Click(object sender, EventArgs e)
        {
            RandomWalk(10_000_000);
        }


    }
}
