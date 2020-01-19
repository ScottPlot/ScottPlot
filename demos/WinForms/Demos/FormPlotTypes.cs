using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemos
{
    public partial class FormPlotTypes : Form
    {
        Random rand = new Random();

        public FormPlotTypes()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetLabels();
            StyleLight();

            BtnScatterSin_Click(null, null);
            BtnScatterSin_Click(null, null);
            BtnScatterSin_Click(null, null);
        }

        private void Clear()
        {
            formsPlot1.plt.Clear();
            formsPlot1.plt.Axis(0, 10, -10, 10);
            formsPlot1.Render();
        }

        private void SetLabels()
        {
            formsPlot1.plt.YLabel("signal (mV)");
            formsPlot1.plt.XLabel("experiment duration (hours)");
            formsPlot1.plt.Title("ScottPlot Interactive Demo");
        }

        private void StyleLight()
        {
            BackColor = SystemColors.Control;
            cbDark.ForeColor = Color.Black;
            cbBenchmark.ForeColor = Color.Black;
            cbAntiAliasData.ForeColor = Color.Black;
            cbAntiAliasFigure.ForeColor = Color.Black;
            formsPlot1.plt.Style(ScottPlot.Style.Control);
            formsPlot1.Render();
        }

        private void StyleDark()
        {
            BackColor = ColorTranslator.FromHtml("#07263b");
            cbDark.ForeColor = Color.White;
            cbBenchmark.ForeColor = Color.White;
            cbAntiAliasData.ForeColor = Color.White;
            cbAntiAliasFigure.ForeColor = Color.White;
            formsPlot1.plt.Style(ScottPlot.Style.Blue1);
            formsPlot1.Render();
        }

        private void CbBenchmark_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.plt.Benchmark(cbBenchmark.Checked);
            formsPlot1.Render();
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
            formsPlot1.plt.AntiAlias(cbAntiAliasFigure.Checked, cbAntiAliasData.Checked);
            formsPlot1.Render();
        }

        private void CbAntiAliasData_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.plt.AntiAlias(cbAntiAliasFigure.Checked, cbAntiAliasData.Checked);
            formsPlot1.Render();
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

            formsPlot1.plt.PlotScatter(xs, ys);
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }

        private void BtnScatterRandom_Click(object sender, EventArgs e)
        {
            double[] xs = ScottPlot.DataGen.Random(rand, 100, 10);
            double[] ys = ScottPlot.DataGen.Random(rand, 100, 20, -10);

            formsPlot1.plt.PlotScatter(xs, ys);
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }

        private void BtnMarker_Click(object sender, EventArgs e)
        {
            double x = rand.NextDouble() * 10;
            double y = rand.NextDouble() * 20 - 10;
            float markerSize = (float)(rand.NextDouble() * 10 + 1);
            Color pointColor = ScottPlot.DataGen.RandomColor(rand);
            formsPlot1.plt.PlotPoint(x, y, markerSize: markerSize, color: pointColor);
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }
        private void BtnText_Click(object sender, EventArgs e)
        {
            double x = rand.NextDouble() * 10;
            double y = rand.NextDouble() * 20 - 10;
            float fontSize = (float)(rand.NextDouble() * 20 + 8);
            Color fontColor = ScottPlot.DataGen.RandomColor(rand);
            formsPlot1.plt.PlotText("demo", x, y, fontSize: fontSize, color: fontColor);
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }

        private void BtnVline_Click(object sender, EventArgs e)
        {
            double position = rand.NextDouble() * 20 - 10;
            double width = (float)(rand.NextDouble() * 5 + 1);
            formsPlot1.plt.PlotVLine(position, lineWidth: (float)width, draggable: true);
            formsPlot1.Render();
        }

        private void BtnHline_Click(object sender, EventArgs e)
        {
            double position = rand.NextDouble() * 20 - 10;
            double width = (float)(rand.NextDouble() * 5 + 1);
            formsPlot1.plt.PlotHLine(position, lineWidth: (float)width, draggable: true);
            formsPlot1.Render();
        }

        private void PlotSignalRandomWalk(int pointCount, bool useSignalConst = true)
        {
            double[] data = ScottPlot.DataGen.RandomWalk(rand, pointCount, 10, rand.NextDouble() * 10 - 5);
            if (useSignalConst)
                formsPlot1.plt.PlotSignalConst(data);
            else
                formsPlot1.plt.PlotSignal(data);
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }

        private void BtnSignal1k_Click(object sender, EventArgs e)
        {
            PlotSignalRandomWalk(1_000, useSignalConst: false);
        }

        private void BtnSignal100k_Click(object sender, EventArgs e)
        {
            PlotSignalRandomWalk(100_000);
        }

        private void BtnSignal1m_Click(object sender, EventArgs e)
        {
            PlotSignalRandomWalk(1_000_000);
        }

        private void BtnSignal10m_Click(object sender, EventArgs e)
        {
            PlotSignalRandomWalk(10_000_000);
        }
    }
}
