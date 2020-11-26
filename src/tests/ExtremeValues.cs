using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests
{
    class ExtremeValues
    {
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
