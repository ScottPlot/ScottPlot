using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InteractiveSimple
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void scottPlotUC11_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            // create some data worth plotting
            int nPoints = 100000;
            double[] Xs = ScottPlot.Generate.Sequence(nPoints);
            double[] Ys = ScottPlot.Generate.Sine(nPoints, 4);

            // make the graph size match the size of the user control
            scottPlotUC11.GraphSizeDetect();

            // manually set axis before drawing anything (very important)
            scottPlotUC11.SP.AxisAuto(Xs, Ys, 0, 0.1);

            // prepare by first drawing a grid
            scottPlotUC11.SP.Grid();

            // add the line to the bitmap
            scottPlotUC11.SP.PlotLine(Xs, Ys);

            // render the bitmap and save it as a file
            scottPlotUC11.GraphUpdate();
            */
        }
    }
}
