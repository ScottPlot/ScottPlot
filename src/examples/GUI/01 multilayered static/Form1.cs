using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_example_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSine_Click(object sender, EventArgs e)
        {
            // create some data worth plotting
            int nPoints = 1000;
            double[] Xs = ScottPlot.Generate.Sequence(nPoints);
            double[] Ys1 = ScottPlot.Generate.Sine(nPoints, 4);
            double[] Ys2 = ScottPlot.Generate.Sine(nPoints, 5, .75);
            double[] Ys3 = ScottPlot.Generate.Sine(nPoints, 20, .5);

            // make the graph size match the size of the user control
            scottPlotUC11.GraphSizeDetect();

            // manually set axis before drawing anything (very important)
            scottPlotUC11.SP.AxisAuto(Xs, Ys1, 0, 0.1);

            // prepare by first drawing a grid
            scottPlotUC11.SP.Grid();

            // add the line to the bitmap
            scottPlotUC11.SP.PlotLine(Xs, Ys1, "g", 5, true);
            scottPlotUC11.SP.PlotLine(Xs, Ys2, "b", 2);
            scottPlotUC11.SP.PlotLine(Xs, Ys3, "r", 1, true);

            // render the bitmap and save it as a file
            scottPlotUC11.GraphUpdate();
        }

        private void btnRand_Click(object sender, EventArgs e)
        {
            // create some data worth plotting
            int nPoints = 50;
            double[] Xs = ScottPlot.Generate.Sequence(nPoints);
            double[] Ys1 = ScottPlot.Generate.Random(nPoints);
            double[] Ys2 = ScottPlot.Generate.Random(nPoints,.7,.5);
            double[] Ys3 = ScottPlot.Generate.Random(nPoints,.5,1.5);

            // make the graph size match the size of the user control
            scottPlotUC11.GraphSizeDetect();

            // manually set axis before drawing anything (very important)
            scottPlotUC11.SP.AxisSet(-5, 55, -.5, 2.5);

            // prepare by first drawing a grid
            scottPlotUC11.SP.Grid();

            // add the line to the bitmap
            scottPlotUC11.SP.PlotLine(Xs, Ys1, "g", 5, true);
            scottPlotUC11.SP.PlotLine(Xs, Ys2, "b", 5);
            scottPlotUC11.SP.PlotLine(Xs, Ys3, "r", 1, true);

            // render the bitmap and save it as a file
            scottPlotUC11.GraphUpdate();
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            scottPlotUC11.SP.Clear();
            scottPlotUC11.SP.Grid();
            scottPlotUC11.GraphUpdate();
        }

    }
}
