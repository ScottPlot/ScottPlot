using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] xs = new double[] { 1, 2, 3, 4, 5 };
            double[] ys = new double[] { 1, 4, 9, 16, 25 };
            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotScatter(xs, ys);
            plt.AxisAuto();
            plt.SaveFig("demo.png");
        }
    }
}
