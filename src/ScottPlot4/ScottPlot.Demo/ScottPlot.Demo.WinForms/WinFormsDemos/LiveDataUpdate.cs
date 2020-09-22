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

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class LiveDataUpdate : Form
    {
        Random rand = new Random();
        double[] liveData = new double[400];
        DataGen.Electrocardiogram ecg = new DataGen.Electrocardiogram();
        Stopwatch sw = Stopwatch.StartNew();
        PlottableVLine vline;

        public LiveDataUpdate()
        {
            InitializeComponent();
            formsPlot1.Configure(middleClickMarginX: 0);

            // plot the data array only once and we can updates its values later
            formsPlot1.plt.PlotSignal(liveData);
            formsPlot1.plt.AxisAutoX(margin: 0);
            formsPlot1.plt.Axis(y1: -1, y2: 2.5);

            // plot a red vertical line and save it so we can move it later
            vline = formsPlot1.plt.PlotVLine(0, Color.Red, lineWidth: 2);

            // customize styling
            formsPlot1.plt.Title("Electrocardiogram Strip Chart");
            formsPlot1.plt.YLabel("Potential (mV)");
            formsPlot1.plt.XLabel("Time (seconds)");
            formsPlot1.plt.Grid(false);
        }

        int nextValueIndex = -1;
        private void timerUpdateData_Tick(object sender, EventArgs e)
        {
            double nextValue = ecg.GetVoltage(sw.Elapsed.TotalSeconds);

            if (rollCheckbox.Checked)
            {
                // "roll" new values over old values like a traditional ECG machine
                nextValueIndex = (nextValueIndex < liveData.Length - 1) ? nextValueIndex + 1 : 0;
                liveData[nextValueIndex] = nextValue;
                vline.visible = true;
                vline.position = nextValueIndex;
            }
            else
            {
                // "scroll" the whole chart to the left
                Array.Copy(liveData, 1, liveData, 0, liveData.Length - 1);
                liveData[liveData.Length - 1] = nextValue;
                vline.visible = false;
            }
        }

        private void timerRender_Tick(object sender, EventArgs e)
        {
            formsPlot1.Render(skipIfCurrentlyRendering: true);
        }

        private void runCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (runCheckbox.Checked)
            {
                sw.Start();
                timerRender.Enabled = true;
                timerUpdateData.Enabled = true;
            }
            else
            {
                sw.Stop();
                timerRender.Enabled = false;
                timerUpdateData.Enabled = false;
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
