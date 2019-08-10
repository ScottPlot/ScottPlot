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
            data = ScottPlot.DataGen.RandomWalk(rand, pointCount, 10, rand.NextDouble() * 10 - 5);
            signal = scottPlotUC1.plt.PlotSignalConst(data, data.Length * 0.1);
            //scottPlotUC1.plt.PlotSignal(data, data.Length * 0.1);
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
            RandomWalk(60_000_000);
        }

        // some global params for updates
        ScottPlot.PlottableSignal signal;
        double[] data;
        bool busy = false;
        private void BtnUpdateSignal_click(object sender, EventArgs e)
        {
            btnUpdateSignal.Enabled = false;
            int updateRangeSize = 1_000_000; // 0 means update by element
            System.Timers.Timer timer;
            // timer for auto redraw plot every 200 ms 
            timer = new System.Timers.Timer(200);
            timer.SynchronizingObject = this;
            timer.Elapsed += (o, arg) =>
            {
                if (busy) // simple mutex
                    return;
                busy = true;                
                scottPlotUC1.Render();
                busy = false;
            };
            timer.AutoReset = true;
            timer.Start();
            
            var pointstoUpdateCount = data.Length;
            // using simple implementation of RandomWalk,
            // because RandomWalk do usless min/max calculation, 
            var newSignal = new double[pointstoUpdateCount];
            newSignal[0] = rand.NextDouble() * 10 - 5;
            for (int i = 1; i < pointstoUpdateCount; i++)
            {
                newSignal[i] = newSignal[i - 1] + (rand.NextDouble() * 2 - 1) * 10;
            }              
            // run plot data updates at max speed in background thread                
            Task.Run(() =>
            {
                if (signal is ScottPlot.PlottableSignalConst)
                {
                    var signalConst = signal as ScottPlot.PlottableSignalConst;
                    signalConst.UpdateElement(0, rand.NextDouble() * 10 - 5);
                    if (updateRangeSize < 1)
                    {                        
                        for (int i = 1; i < pointstoUpdateCount; i++)
                            signalConst.UpdateElement(i, newSignal[i]);                
                    }
                    else
                    {
                        
                        for (int i = 0; i < pointstoUpdateCount - updateRangeSize; i+=updateRangeSize)
                        {
                            signalConst.UpdateRange(i, i + updateRangeSize, newSignal, i);
                        }
                        int lastPiece = (pointstoUpdateCount-updateRangeSize)/updateRangeSize * updateRangeSize;
                        signalConst.UpdateRange(lastPiece, pointstoUpdateCount, newSignal, lastPiece);

                    }
                }
                else
                {
                    data[0] = rand.NextDouble() * 10 - 5;
                    for (int i = 1; i < pointstoUpdateCount; i++)
                        data[i] = newSignal[i];
                }
                timer.Stop();
                timer.Dispose();
                // last update to make it look perfect and able to new run
                this.Invoke((MethodInvoker)(() =>
                {
                    while (busy)
                        Thread.Sleep(100);
                    scottPlotUC1.Render();
                    btnUpdateSignal.Enabled = true;
                }));
            });
        }
    }
}
