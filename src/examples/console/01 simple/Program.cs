using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_example_1
{
    class Program
    {
        static void Main(string[] args)
        {
            // create data with our own routines
            int nPoints = 250;
            double[] Xs = ScottPlot.Generate.Sequence(nPoints);
            double[] Ys1 = ScottPlot.Generate.Sine(nPoints, 4);
            double[] Ys2 = ScottPlot.Generate.Sine(nPoints, 4.5, .8);

            // create a ScottPlot figure
            ScottPlot.Figure SP = new ScottPlot.Figure();

            // manually set axis before drawing anything (very important)
            SP.AxisAuto(Xs, Ys1, 0, 0.1);

            // prepare by first drawing a grid
            SP.Grid();

            // add the line to the bitmap
            SP.PlotLine(Xs, Ys1, "g", 5, true);
            SP.PlotLine(Xs, Ys2, "b", 2);

            // render the bitmap and save it as a file
            SP.SaveFig("demo.jpg");
        }
    }
}
