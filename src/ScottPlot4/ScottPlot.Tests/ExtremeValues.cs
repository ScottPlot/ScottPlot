using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests
{
    class ExtremeValues
    {
        [Test]
        public void Test_DataContainsNan_Scatter()
        {
            double[] xs = new double[] { 1, double.NaN, 3, 4, 5, 6 };
            double[] ys = new double[] { 1, 4, 9, 16, double.NaN, 36 };

            var plt = new ScottPlot.Plot();
            plt.PlotScatter(xs, ys);

            plt.AxisAuto();
            var ax = plt.Axis();
            Console.WriteLine($"Automatic axis limits: x1={ax[0]}, x2={ax[0]}, y1={ax[0]}, y2={ax[0]}");

            plt.Axis(-2, 7, -10, 40);

            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string filePath = System.IO.Path.GetFullPath(name + ".png");
            plt.SaveFig(filePath);
            Console.WriteLine($"Saved {filePath}");
        }

        [Test]
        public void Test_DataContainsNan_Signal()
        {
            double[] ys = new double[] { 1, 4, double.NaN, 16, 25 };

            var plt = new ScottPlot.Plot();
            plt.PlotSignal(ys);

            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string filePath = System.IO.Path.GetFullPath(name + ".png");
            plt.SaveFig(filePath);
            Console.WriteLine($"Saved {filePath}");
        }
    }
}
