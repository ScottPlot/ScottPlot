using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class LiveDataIncoming : Form
    {
        public double[] data = new double[100_000];
        int nextDataIndex = 1;
        PlottableSignal signalPlot;
        Random rand = new Random(0);

        public LiveDataIncoming()
        {
            InitializeComponent();
            signalPlot = formsPlot1.plt.PlotSignal(data);
            formsPlot1.plt.YLabel("Value");
            formsPlot1.plt.XLabel("Sample Number");
        }

        private void LiveDataIncoming_Load(object sender, EventArgs e)
        {

        }

        private void dataTimer_Tick(object sender, EventArgs e)
        {
            if (nextDataIndex >= data.Length)
            {
                throw new OverflowException("data array isn't long enough to accomodate new data");
                // in this situation the solution would be:
                //   1. clear the plot
                //   2. create a new larger array
                //   3. copy the old data into the start of the larger array
                //   4. plot the new (larger) array
                //   5. continue to update the new array
            }

            double randomValue = Math.Round(rand.NextDouble() - .5, 3);
            double latestValue = data[nextDataIndex - 1] + randomValue;
            data[nextDataIndex] = latestValue;
            tbLastValue.Text = (latestValue > 0) ? "+" + latestValue.ToString() : latestValue.ToString();
            tbLatestValue.Text = nextDataIndex.ToString();

            signalPlot.maxRenderIndex = nextDataIndex;

            nextDataIndex += 1;
        }

        private void renderTimer_Tick(object sender, EventArgs e)
        {
            if (cbAutoAxis.Checked)
                formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }

        private void cbAutoAxis_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutoAxis.Checked == false)
            {
                double[] autoAxisLimits = formsPlot1.plt.AxisAuto(verticalMargin: .5);
                double oldX2 = autoAxisLimits[1];
                formsPlot1.plt.Axis(x2: oldX2 + 1000);
            }
        }
    }
}
