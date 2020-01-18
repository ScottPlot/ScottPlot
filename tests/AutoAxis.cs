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
        public void Test_AutoAxis_CandlestickSinglePoint()
        {
            var plt = new ScottPlot.Plot();
            plt.PlotCandlestick(ScottPlot.DataGen.RandomStockPrices(rand: null, pointCount: 1));
            plt.GetBitmap(); // force a render

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

        [Test]
        public void Test_AutoAxis_WorksAfterClear()
        {
            var plt = new ScottPlot.Plot();

            plt.PlotPoint(0.1, 0.1);
            plt.PlotPoint(-0.1, -0.1);
            plt.AxisAuto();
            plt.GetBitmap(); // force a render
            Assert.Greater(plt.Axis()[0], -5);

            plt.PlotPoint(999, 999);
            plt.PlotPoint(-999, -999);
            plt.AxisAuto();
            plt.GetBitmap(); // force a render
            Assert.Less(plt.Axis()[0], -800);

            plt.Clear();
            plt.PlotPoint(0.1, 0.1);
            plt.PlotPoint(-0.1, -0.1);
            plt.GetBitmap(); // force a render
            Assert.Greater(plt.Axis()[0], -5);
        }
    }
}
