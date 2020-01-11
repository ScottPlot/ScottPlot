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
    public partial class FormIncomingData : Form
    {
        private double[] dataValues;
        private ScottPlot.PlottableSignal signal;

        public FormIncomingData()
        {
            InitializeComponent();

            int dataPoints = 100 * 60 * 60; // 1hr of 100Hz data
            dataValues = new double[dataPoints];
            signal = formsPlot1.plt.PlotSignal(dataValues, 100);
            formsPlot1.plt.Axis(-2, 35, -100, 300);
        }

        private void FormIncomingData_Load(object sender, EventArgs e)
        {

        }

        private void RunCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            NewDataTimer.Enabled = RunCheckbox.Checked;
            RenderTimer.Enabled = RunCheckbox.Checked;
        }

        private void AdjustAxisButton_Click(object sender, EventArgs e)
        {
            // there's room to make this function smarter (or perhaps modify AxisAuto() to future-pad data?)
            formsPlot1.plt.AxisAuto();
            double maxX = nextIndex / 100.0;
            maxX += maxX / 3;
            double minX = -maxX / 10;
            formsPlot1.plt.Axis(minX, maxX);
        }

        int nextIndex = 0;
        double lastValue = 123.45;
        Random rand = new Random(0);
        private void NewDataTimer_Tick(object sender, EventArgs e)
        {
            lastValue += (rand.NextDouble() - .5) * 10;
            dataValues[nextIndex] = lastValue;
            signal.maxRenderIndex = nextIndex;
            nextIndex += 1;

            LatestValueLabel.Text = string.Format("{0:0.000}", lastValue);
        }

        bool busyRendering;
        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            if (!busyRendering)
            {
                busyRendering = true;
                formsPlot1.Render();
                busyRendering = false;
            }
        }
    }
}
