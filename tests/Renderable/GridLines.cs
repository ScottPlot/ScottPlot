using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlotTests.Renderable
{
    class GridLines
    {
        [Test]
        public void Test_GridLines_Color()
        {
            var plt = new ScottPlot.Plot();

            plt.Grid(color: Color.White);
            var mean1 = TestTools.MeanPixel(plt.GetBitmap());
            plt.Grid(color: Color.Blue);
            var mean2 = TestTools.MeanPixel(plt.GetBitmap());

            Assert.AreEqual(mean2.A, mean1.A);
            Assert.Less(mean2.R, mean1.R);
            Assert.Less(mean2.G, mean1.G);
            Assert.AreEqual(mean2.B, mean1.B);
        }

        [Test]
        public void Test_GridLines_Disable()
        {
            var plt = new ScottPlot.Plot();

            plt.Grid(color: Color.White);
            var mean1 = TestTools.MeanPixel(plt.GetBitmap());

            // blue grid lines that aren't enabled should look the same as white grid lines
            plt.Grid(color: Color.Blue);
            plt.Grid(enable: false);
            var mean2 = TestTools.MeanPixel(plt.GetBitmap());

            Assert.AreEqual(mean2.A, mean1.A);
            Assert.AreEqual(mean2.R, mean1.R);
            Assert.AreEqual(mean2.G, mean1.G);
            Assert.AreEqual(mean2.B, mean1.B);
        }

        [Test]
        public void Test_GridLines_IndividualVisibility()
        {
            var plt = new ScottPlot.Plot();

            var meanBoth = TestTools.MeanPixel(plt.GetBitmap());

            plt.Grid(enableHorizontal: true, enableVertical: false);
            var meanJustHorizontal = TestTools.MeanPixel(plt.GetBitmap());

            plt.Grid(enableHorizontal: false, enableVertical: true);
            var meanJustVertical = TestTools.MeanPixel(plt.GetBitmap());

            plt.Grid(enableHorizontal: false, enableVertical: false);
            var meanNeither = TestTools.MeanPixel(plt.GetBitmap());

            // grid lines decrease mean pixel intensity
            Assert.Less(meanBoth.R, meanJustHorizontal.R);
            Assert.Less(meanBoth.R, meanJustVertical.R);

            // horizontal lines are longer than vertical lines so it should be darker
            Assert.Less(meanJustHorizontal.R, meanJustVertical.R);

            // no grid lines should be the brightest
            Assert.Greater(meanNeither.R, meanJustVertical.R);
            Assert.Greater(meanNeither.R, meanJustHorizontal.R);
            Assert.Greater(meanNeither.R, meanBoth.R);
        }

        [Test]
        public void Test_GridLines_LineWidth()
        {
            var plt = new ScottPlot.Plot();

            var mean1 = TestTools.MeanPixel(plt.GetBitmap());

            plt.Grid(lineWidth: 2);
            var mean2 = TestTools.MeanPixel(plt.GetBitmap());

            // thicker dark lines reduce mean pixel intensity
            Assert.AreEqual(mean2.A, mean1.A);
            Assert.Less(mean2.R, mean1.R);
            Assert.Less(mean2.G, mean1.G);
            Assert.Less(mean2.B, mean1.B);
        }

        [Test]
        public void Test_GridLines_LineStyle()
        {
            var plt = new ScottPlot.Plot();

            var meanSolid = TestTools.MeanPixel(plt.GetBitmap());

            plt.Grid(lineStyle: ScottPlot.LineStyle.Dash);
            var meanDash = TestTools.MeanPixel(plt.GetBitmap());

            plt.Grid(lineStyle: ScottPlot.LineStyle.Dot);
            var meanDot = TestTools.MeanPixel(plt.GetBitmap());

            // more continuous dark lines reduce mean pixel intensity
            Assert.Greater(meanDash.R, meanSolid.R);
            Assert.Greater(meanDot.R, meanDash.R);
        }
    }
}
