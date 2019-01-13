using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDev2
{
    public partial class ScottPlotUC : UserControl
    {
        public ScottPlot.Plot plt;

        public ScottPlotUC()
        {
            InitializeComponent();
            plt = new ScottPlot.Plot();
            //CreateDemoPlot();
        }

        public void CreateDemoPlot()
        {

            // create some data to plot between -1 and 1 (vertical) from 0 to 50 (horizontal)
            int pointCount = 20_000 * 60;
            var demoXs = new double[pointCount];
            var demoYs = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                demoXs[i] = i;
                demoYs[i] = -Math.Sin(50 * i / (double)pointCount);
            }

            // create the plot and add the data
            plt = new ScottPlot.Plot();
            //plt.data.AddScatter(demoXs, demoYs);
            //plt.data.AddPoint(25, .65, 10, Color.Blue);
            //plt.data.AddVertLine(42, 3, Color.Green);
            plt.data.AddSignal(demoYs, 20_000);
            plt.settings.AxisFit();

        }

        public void Render()
        {
            pb.Image = plt.figure.GetBitmap();
            Application.DoEvents();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        // EVENTS

        private void pb_Layout(object sender, LayoutEventArgs e)
        {
            plt.settings.Resize(pb.Width, pb.Height);
            Render();
        }

        private void ScottPlotUC_Load(object sender, EventArgs e)
        {
            pb_Layout(null, null);
        }

        private void pb_Click(object sender, EventArgs e)
        {

        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        // MOUSE TRACKING FOR PAN AND ZOOM

        private void pb_MouseDown(object sender, MouseEventArgs e)
        {
            plt.settings.mouse.Down();
        }

        private void pb_MouseUp(object sender, MouseEventArgs e)
        {
            plt.settings.mouse.Up();
            Render();
        }

        private bool currentlyProcessingMoveEvent = false;
        private void pb_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentlyProcessingMoveEvent)
            {
                return;
            }
            else
            {
                currentlyProcessingMoveEvent = true;
                if (plt.settings.mouse.MoveRequiresRender())
                    Render();
                currentlyProcessingMoveEvent = false;
            }
        }

        private void pb_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                plt.settings.AxisFit();
                Render();
            }
        }
    }
}
