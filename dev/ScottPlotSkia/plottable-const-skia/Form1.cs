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

namespace plottable_const
{
    public partial class Form1 : Form
    {
        ScottPlot.Plottable signal;
        Random rand = new Random();
        double[] data;
        bool busy = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = formsPlot1.plt;
            plt.Title("Impressive Graph", fontName: "courier new", fontSize: 24, color: Color.Purple, bold: true);
            plt.YLabel("vertical units", fontName: "impact", fontSize: 24, color: Color.Red, bold: true);
            plt.XLabel("horizontal units", fontName: "georgia", fontSize: 24, color: Color.Blue, bold: true);
            plt.PlotScatter(dataXs, dataSin, label: "sin");
            plt.PlotScatter(dataXs, dataCos, label: "cos");
            plt.PlotText("very graph", 25, .8, fontName: "comic sans ms", fontSize: 24, color: Color.Blue, bold: true);
            plt.PlotText("so data", 0, 0, fontName: "comic sans ms", fontSize: 42, color: Color.Magenta, bold: true);
            plt.PlotText("many documentation", 3, -.6, fontName: "comic sans ms", fontSize: 18, color: Color.DarkCyan, bold: true);
            plt.PlotText("wow.", 10, .6, fontName: "comic sans ms", fontSize: 36, color: Color.Green, bold: true);
            plt.PlotText("NuGet", 32, 0, fontName: "comic sans ms", fontSize: 24, color: Color.Gold, bold: true);
            plt.Legend(fontName: "comic sans ms", fontSize: 16, bold: true, fontColor: Color.DarkBlue);

            formsPlotSkia1.plt.Title("Impressive Graph", fontName: "courier new", fontSize: 24, color: Color.Purple, bold: true);
            formsPlotSkia1.plt.YLabel("vertical units", fontName: "impact", fontSize: 24, color: Color.Red, bold: true);
            formsPlotSkia1.plt.XLabel("horizontal units", fontName: "georgia", fontSize: 24, color: Color.Blue, bold: true);
            formsPlotSkia1.plt.PlotScatter(dataXs, dataSin, label: "sin");
            formsPlotSkia1.plt.PlotScatter(dataXs, dataCos, label: "cos");
            formsPlotSkia1.plt.PlotText("very graph", 25, .8, fontName: "comic sans ms", fontSize: 24, color: Color.Blue, bold: true);
            formsPlotSkia1.plt.PlotText("so data", 0, 0, fontName: "comic sans ms", fontSize: 42, color: Color.Magenta, bold: true);
            formsPlotSkia1.plt.PlotText("many documentation", 3, -.6, fontName: "comic sans ms", fontSize: 18, color: Color.DarkCyan, bold: true);
            formsPlotSkia1.plt.PlotText("wow.", 10, .6, fontName: "comic sans ms", fontSize: 36, color: Color.Green, bold: true);
            formsPlotSkia1.plt.PlotText("NuGet", 32, 0, fontName: "comic sans ms", fontSize: 24, color: Color.Gold, bold: true);
            formsPlotSkia1.plt.Legend(fontName: "comic sans ms", fontSize: 16, bold: true, fontColor: Color.DarkBlue);
        }

        private void BtnUpdateData_Click(object sender, EventArgs e)
        {
            btnUpdateData.Enabled = false;
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
                formsPlot1.Render();
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
                if (signal is ScottPlot.PlottableSignalConst<double>)
                {
                    var signalConst = signal as ScottPlot.PlottableSignalConst<double>;
                    signalConst.updateData(0, rand.NextDouble() * 10 - 5);
                    if (updateRangeSize < 1)
                    {
                        for (int i = 1; i < pointstoUpdateCount; i++)
                            signalConst.updateData(i, newSignal[i]);
                    }
                    else
                    {

                        for (int i = 0; i < pointstoUpdateCount - updateRangeSize; i += updateRangeSize)
                        {
                            signalConst.updateData(i, i + updateRangeSize, newSignal, i);
                        }
                        int lastPiece = (pointstoUpdateCount - updateRangeSize) / updateRangeSize * updateRangeSize;
                        signalConst.updateData(lastPiece, pointstoUpdateCount, newSignal, lastPiece);

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
                    formsPlot1.Render();
                    btnUpdateData.Enabled = true;
                }));
            });
        }
    }
}
