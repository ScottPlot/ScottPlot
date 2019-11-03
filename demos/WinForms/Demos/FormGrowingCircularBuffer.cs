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
    public partial class FormGrowingCircular : Form
    {
        Random rand = new Random();

        public FormGrowingCircular()
        {
            InitializeComponent();
            InitArray();
            formsPlot1.plt.YLabel("Value");
            formsPlot1.plt.XLabel("Data Point");
            formsPlot1.plt.Title("Plotting a Circular Array");

            // since the array never changes once it's created, we can plot it immediately
            formsPlot1.plt.PlotSignal(largeBuffer);
        }

        int smallBufferSize = 10;
        int nextIndex = 0;

        double[] largeBuffer;
        private void InitArray(int smallBuffersInLargeBuffer = 100)
        {
            largeBuffer = new double[smallBufferSize * smallBuffersInLargeBuffer];
        }

        private void TimerNewData_Tick(object sender, EventArgs e)
        {
            // incoming data will fill a small buffer
            int lastIndex = (nextIndex==0) ? largeBuffer.Length - 1 : nextIndex - 1;
            double lastValue = largeBuffer[lastIndex];
            double[] smallBuffer = ScottPlot.DataGen.RandomWalk(rand, smallBufferSize, 1, lastValue);

            // copy the small buffer into the large buffer at the point of nextIndex
            Array.Copy(smallBuffer, 0, largeBuffer, nextIndex, smallBuffer.Length);
            
            // update the index for the next set of incoming data
            nextIndex += smallBufferSize;
            if (nextIndex + smallBufferSize > largeBuffer.Length)
                nextIndex = 0;

            // update the plot
            formsPlot1.plt.AxisAuto(0);
            formsPlot1.plt.Clear(signalPlots: false); // clear just the old vertical line
            formsPlot1.plt.PlotVLine(nextIndex, color: Color.Red, lineWidth: 2);
            formsPlot1.Render();
        }

        private void CbIncoming_CheckedChanged(object sender, EventArgs e)
        {
            timerNewData.Enabled = cbIncoming.Checked;
        }
    }
}
