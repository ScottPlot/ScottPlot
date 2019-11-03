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
    public partial class FormExtremeAxes : Form
    {
        double bigNumber = 987654321 * 10e50;
        double smallNumber = 123456789 * 10e-50;

        public FormExtremeAxes()
        {
            InitializeComponent();
            formsPlot1.plt.PlotSignal(ScottPlot.DataGen.Sin(1000), lineWidth: 3);
            formsPlot1.plt.XLabel("Horizontal Axis");
            formsPlot1.plt.YLabel("Vertical Axis");
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }

        private void BtnNormal_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }

        private void BtnBig_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Axis(-bigNumber, bigNumber, -bigNumber, bigNumber);
            formsPlot1.Render();
        }

        private void BtnSmall_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Axis(-smallNumber, smallNumber, -smallNumber, smallNumber);
            formsPlot1.Render();
        }

        private void ButtonBigOffset_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Axis(bigNumber, bigNumber + bigNumber / 10, bigNumber, bigNumber + bigNumber / 10);
            formsPlot1.Render();
        }

        private void BtnSmallOffset_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Axis(smallNumber, smallNumber + smallNumber / 10, smallNumber, smallNumber + smallNumber / 10);
            formsPlot1.Render();
        }

        private void BtnWav_Click(object sender, EventArgs e)
        {
            // simulate zooming in to a 10-millisecond window at the 5-minute mark of a wav file
            double time1 = 5 * 60;
            double time2 = time1 + .01;
            formsPlot1.plt.Axis(time1, time2, -2e16, 2e16);
            formsPlot1.Render();
        }
    }
}
