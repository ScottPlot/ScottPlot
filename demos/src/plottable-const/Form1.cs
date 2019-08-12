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
        ScottPlot.PlottableSignal signal;
        Random rand = new Random();
        double[] data;
        bool busy = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            data = ScottPlot.DataGen.RandomWalk(rand, 10_000_000);
            signal = scottPlotUC1.plt.PlotSignalConst(data);
            scottPlotUC1.lowQualityWhileDragging = true;
            scottPlotUC1.plt.Benchmark();
            scottPlotUC1.Render();
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
                    scottPlotUC1.Render();
                    btnUpdateData.Enabled = true;
                }));
            });
        }
    }
}
