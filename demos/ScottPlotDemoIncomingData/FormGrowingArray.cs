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
    public partial class FormGrowingArray : Form
    {
        Random rand = new Random();
        double[] dataYs = new double[] { 0 }; // start with a point so it's not empty

        public FormGrowingArray()
        {
            InitializeComponent();
            scottPlotUC1.plt.YLabel("Value");
            scottPlotUC1.plt.XLabel("Data Point");
            scottPlotUC1.plt.Title("Plotting a Growing Array");
        }
        
        private void CbIncoming_CheckedChanged(object sender, EventArgs e)
        {
            timerNewData.Enabled = cbIncoming.Checked;
        }

        private void TimerNewData_Tick(object sender, EventArgs e)
        {
            // simulate an array containing new incoming data
            int newPoints = 10;
            double[] newData = ScottPlot.DataGen.RandomWalk(rand, newPoints, 1, dataYs.Last());

            // create a new full-size array to hold the old data and new data
            double[] newArray = new double[dataYs.Length + newData.Length];

            // copy the old data into the new full-size array
            Array.Copy(dataYs, 0, newArray, 0, dataYs.Length);

            // copy the new data into the new full-size array
            Array.Copy(newData, 0, newArray, dataYs.Length, newData.Length);

            // assign the new full-size array to a class-level variable so it is remembered
            dataYs = newArray;

            // let the plot timer know we need to replot
            dataHasBeenUpdated = true;
        }

        bool dataHasBeenUpdated = false;
        private void TimerUpdatePlot_Tick(object sender, EventArgs e)
        {
            if (dataHasBeenUpdated)
            {
                dataHasBeenUpdated = false;
                scottPlotUC1.plt.Clear();
                scottPlotUC1.plt.PlotSignal(dataYs);
                scottPlotUC1.plt.AxisAuto();
                scottPlotUC1.Render();
            }
        }
    }
}
