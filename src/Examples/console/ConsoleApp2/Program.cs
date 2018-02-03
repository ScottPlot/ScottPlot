using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing; // added reference
using ScottPlot; // added reference

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {

            var fig = new ScottPlot.Figure(640, 460);

            fig.Zoom(.9, .9);
            fig.PanPixels(5, 5);

            // draw a blue X
            fig.PlotLine(-10, 10, -10, 10, 5, Color.Blue);
            fig.PlotLine(-10, 10, 10, -10, 5, Color.Blue);

            // draw a red rectangle
            double[] Xs = { -10, 10, 10, -10, -10 };
            double[] Ys = { -10, -10, 10, 10, -10 };
            fig.PlotLines(Xs, Ys, 5, Color.Red);

            fig.Save("test.png");

            Console.Write("\nDONE");
        }
    }
}
