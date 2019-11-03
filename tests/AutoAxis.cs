using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests
{
    public class AutoAxis
    {
        [Test]
        public void Test_AutoAxis_ScatterDiagonalLine()
        {
            var plt = new ScottPlot.Plot();
            plt.PlotScatter(
                xs: new double[] { 1, 2 },
                ys: new double[] { 1, 2 }
                );
            plt.AxisAuto();
            Console.WriteLine(plt.GetSettings().axes);

            Assert.IsTrue(plt.GetSettings().axes.x.span > 0);
            Assert.IsTrue(plt.GetSettings().axes.y.span > 0);

            //string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            //plt.SaveFig(System.IO.Path.GetFullPath(name + ".png"));
        }

        [Test]
        public void Test_AutoAxis_ScatterSinglePoint()
        {
            var plt = new ScottPlot.Plot();
            plt.PlotScatter(
                xs: new double[] { 1, 2 },
                ys: new double[] { 1, 2 }
                );
            plt.AxisAuto();
            Console.WriteLine(plt.GetSettings().axes);

            Assert.IsTrue(plt.GetSettings().axes.x.span > 0);
            Assert.IsTrue(plt.GetSettings().axes.y.span > 0);
        }

        [Test]
        public void Test_AutoAxis_ScatterHorizontalLine()
        {
            var plt = new ScottPlot.Plot();
            plt.PlotScatter(
                xs: new double[] { 1, 2 },
                ys: new double[] { 1, 1 }
                );
            plt.AxisAuto();

            Assert.IsTrue(plt.GetSettings().axes.x.span > 0);
            Assert.IsTrue(plt.GetSettings().axes.y.span > 0);
        }

        [Test]
        public void Test_AutoAxis_ScatterVerticalLine()
        {
            var plt = new ScottPlot.Plot();
            plt.PlotScatter(
                xs: new double[] { 1, 1 },
                ys: new double[] { 1, 2 }
                );
            plt.AxisAuto();
            Console.WriteLine(plt.GetSettings().axes);

            Assert.IsTrue(plt.GetSettings().axes.x.span > 0);
            Assert.IsTrue(plt.GetSettings().axes.y.span > 0);
        }
    }
}
