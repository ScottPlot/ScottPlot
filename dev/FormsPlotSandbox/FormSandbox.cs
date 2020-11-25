using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsPlotSandbox
{
    public partial class FormSandbox : Form
    {
        private System.Threading.Thread dataUpdateThread;
        private PlottableScatter scatterPlot;

        public FormSandbox()
        {
            InitializeComponent();

            // create a scatter plot and continuously update its X/Y arrays from another thread
            scatterPlot = formsPlot1.plt.PlotScatter(new double[] { 1, 2, 3 }, new double[] { 1, 4, 9 });
            formsPlot1.plt.Axis(0, 100, -10, 10);
            dataUpdateThread = new System.Threading.Thread(UpdateDataForever);
            dataUpdateThread.Start();

            // create a timer to continuously re-render the plot
            Timer MyTimer = new Timer { Interval = 1 };
            MyTimer.Tick += new EventHandler(MyTimer_Tick);
            MyTimer.Start();
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            formsPlot1.Render();
        }

        private void UpdateDataForever()
        {
            Random rand = new Random();
            while (true)
            {
                /* This is an intentionaly sily thing to do, but it was chosen because 
                 * replacing the X/Y arrays with new ones mid-render frequently causes 
                 * crashes mid-way in the render system if it is not locked before editing.
                 */

                formsPlot1.plt.RenderLock();
                int arrayLength = rand.Next(10, 100);
                scatterPlot.xs = DataGen.Consecutive(arrayLength);
                scatterPlot.ys = DataGen.RandomWalk(rand, arrayLength);
                formsPlot1.plt.RenderUnlock();
            }
        }
    }
}
