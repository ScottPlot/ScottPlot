using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;
using ScottPlot.Plottable;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    public partial class LiveDataUpdate : Form
    {
        Random rand = new Random();
        double[] liveData = new double[400];
        DataGen.Electrocardiogram ecg = new DataGen.Electrocardiogram();
        Stopwatch sw = Stopwatch.StartNew();
        VLine vline;

        public LiveDataUpdate()
        {
            InitializeComponent();

            // plot the data array only once and we can updates its values later
            formsPlot1.Plot.AddSignal(liveData);
            formsPlot1.Plot.AxisAutoX(margin: 0);
            formsPlot1.Plot.SetAxisLimits(yMin: -1, yMax: 2.5);

            // plot a red vertical line and save it so we can move it later
            vline = formsPlot1.Plot.AddVerticalLine(0, Color.Red, 2);

            // customize styling
            formsPlot1.Plot.Title("Electrocardiogram Strip Chart");
            formsPlot1.Plot.YLabel("Potential (mV)");
            formsPlot1.Plot.XLabel("Time (seconds)");
            formsPlot1.Plot.Grid(false);

            Closed += (sender, args) =>
            {
                timerUpdateData?.Stop();
                timerRender?.Stop();
            };

            this.timerUpdateData.Interval = 0.005;
            this.timerUpdateData.Elapsed += this.timerUpdateData_Tick;
            this.timerUpdateData.Start();
            this.timerRender.Interval = 0.02;
            this.timerRender.Elapsed += this.timerRender_Tick;
            this.timerRender.Start();
            this.runCheckbox.CheckedChanged += this.runCheckbox_CheckedChanged;
            this.rollCheckbox.CheckedChanged += this.rollCheckbox_CheckedChanged;
        }

        int nextValueIndex = -1;
        private void timerUpdateData_Tick(object sender, EventArgs e)
        {
            double nextValue = ecg.GetVoltage(sw.Elapsed.TotalSeconds);

            if (rollCheckbox.Checked ?? false)
            {
                // "roll" new values over old values like a traditional ECG machine
                nextValueIndex = (nextValueIndex < liveData.Length - 1) ? nextValueIndex + 1 : 0;
                liveData[nextValueIndex] = nextValue;
                vline.IsVisible = true;
                vline.X = nextValueIndex;
            }
            else
            {
                // "scroll" the whole chart to the left
                Array.Copy(liveData, 1, liveData, 0, liveData.Length - 1);
                liveData[liveData.Length - 1] = nextValue;
                vline.IsVisible = false;
            }
        }

        private void timerRender_Tick(object sender, EventArgs e)
        {
            formsPlot1.Refresh();
        }

        private void runCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (runCheckbox.Checked ?? false)
            {
                sw.Start();
                timerRender.Start();
                timerUpdateData.Start();
            }
            else
            {
                sw.Stop();
                timerRender.Stop();
                timerUpdateData.Stop();
            }
        }

        private void rollCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            // clear old data values
            for (int i = 0; i < liveData.Length; i++)
                liveData[i] = 0;
        }
    }
}
