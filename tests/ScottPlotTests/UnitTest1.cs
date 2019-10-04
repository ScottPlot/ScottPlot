using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScottPlotTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var plt = new ScottPlot.Plot();
            plt.PlotScatter(
                xs: new double[] { 2, 4, 6 },
                ys: new double[] { 4, 16, 36 }
                );
            System.Drawing.Bitmap bmp = plt.GetBitmap(); // force a render
            Console.WriteLine(plt.GetHashCode()); // force a render too
        }
    }
}
