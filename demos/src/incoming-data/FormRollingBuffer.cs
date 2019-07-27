using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemoIncomingData
{
    public partial class FormRollingBuffer : Form
    {
        Random rand = new Random();

        public FormRollingBuffer()
        {
            InitializeComponent();
            InitArray();
            scottPlotUC1.plt.YLabel("Value");
            scottPlotUC1.plt.XLabel("Data Point");
            scottPlotUC1.plt.Title("Plotting a Rolling Array");

            // since the array never changes once it's created, we can plot it immediately
            scottPlotUC1.plt.PlotSignal(largeBuffer); 
        }

        int smallBufferSize = 10;

        double[] largeBuffer;
        private void InitArray(int smallBuffersInLargeBuffer = 100)
        {
            largeBuffer = new double[smallBufferSize * smallBuffersInLargeBuffer];
        }

        private void CbIncoming_CheckedChanged(object sender, EventArgs e)
        {
            timerNewData.Enabled = cbIncoming.Checked;
        }

        private void TimerNewData_Tick(object sender, EventArgs e)
        {
            // incoming data will fill a small buffer
            double[] smallBuffer = ScottPlot.DataGen.RandomWalk(rand, smallBufferSize, 1, largeBuffer.Last());

            // roll the old data to the left (by the size of one small buffer)
            Array.Copy(largeBuffer, smallBuffer.Length, largeBuffer, 0, largeBuffer.Length - smallBuffer.Length);

            // copy the small buffer into the end of the large buffer
            Array.Copy(smallBuffer, 0, largeBuffer, largeBuffer.Length - smallBuffer.Length, smallBuffer.Length);

            // update the plot
            scottPlotUC1.plt.AxisAuto(0);
            scottPlotUC1.Render();

            // note: If new data comes in at extremely high frequency, you don't want to update the plot that often.
            //  In this case create a separate timer just for updating the plot. 
            //  This is demonstrated in the growing array demo in this same project.
        }
    }
}
