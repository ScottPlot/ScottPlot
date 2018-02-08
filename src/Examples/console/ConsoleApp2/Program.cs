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

            double[] Xs = fig.gen.Sequence(1_000_000, 1.0 / 20e3); // 20 kHz
            double[] Ys = fig.gen.RandomWalk(1_000_000);
            fig.AxisAuto(Xs, Ys, null, .9);

            fig.BenchmarkThis();

            fig.PlotSignal(Ys, 1.0 / 20000);

            fig.Save("test.png");

            Console.Write("\nDONE");
        }
    }
}
