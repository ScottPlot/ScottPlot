using ScottPlot.Grids;
using ScottPlot.Interfaces;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.UnitTests
{
    [TestFixture]
    internal class StylingTests
    {
        public void TestHandDrawnStyleAppliesToAxes()
        {
            Plot myPlot = new();

            myPlot.Add.Signal(Generate.Sin());

            myPlot.Style.SetLineStylePatterns(LinePattern.HandDrawn);

            int styleableCount = 0;
            foreach (var axis in myPlot.Axes.GetAxes())
            {
                Assert.That(axis.FrameLineStyle.Pattern, Is.EqualTo(LinePattern.HandDrawn));
                styleableCount++;
            }
            Assert.That(styleableCount, Is.GreaterThan(0));
        }

        public void TestHandDrawnStyleAppliesToGrid()
        {
            Plot myPlot = new();

            myPlot.Add.Signal(Generate.Sin());

            myPlot.Style.SetLineStylePatterns(LinePattern.HandDrawn);

            int styleableCount = 0;
            foreach (var grid in myPlot.Axes.Grids.OfType<DefaultGrid>())
            {
                Assert.That(grid.MajorLineStyle.Pattern, Is.EqualTo(LinePattern.HandDrawn));
            }
            Assert.That(styleableCount, Is.GreaterThan(0));
        }

        [Test]
        public void TestHandDrawnStyleAppliesToOHLC()
        {
            Plot myPlot = new();

            var prices = Generate.RandomOHLCs(30);
            myPlot.Add.OHLC(prices);

            AssertPlottableHasHandDrawnStyle(myPlot);
        }

        [Test]
        public void TestHandDrawnStyleAppliesToBar()
        {
            Plot myPlot = new();

            // add bars
            double[] values = { 5, 10, 7, 13 };
            BarPlot bar = myPlot.Add.Bars(values);

            // TODO: create unit test that validates bar plot has hand drawn style -
            // bar and box do not implement IHoldLineStyle
        }

        [Test]
        public void TestHandDrawnStyleAppliesToPie()
        {
            Plot myPlot = new();

            double[] values = { 5, 2, 8, 4, 8 };
            var pie = myPlot.Add.Pie(values);

            AssertPlottableHasHandDrawnStyle(myPlot);
        }

        [Test]
        public void TestHandDrawnStyleAppliesToBox()
        {
            Plot myPlot = new();

            List<Box> boxes1 = new()
            {
                Generate.RandomBox(1),
                Generate.RandomBox(2),
                Generate.RandomBox(3),
            };

            var bp1 = myPlot.Add.Boxes(boxes1);
            bp1.Label = "Group 1";

            AssertPlottableHasHandDrawnStyle(myPlot);
        }

        [Test]
        public void TestHandDrawnStyleAppliesToLine()
        {
            Plot myPlot = new();

            myPlot.Add.Signal(Generate.Sin());

            AssertPlottableHasHandDrawnStyle(myPlot);
        }

        [Test]
        public void TestHandDrawnStyleAppliesToScatter()
        {
            Plot myPlot = new();

            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };
            var scatterPlot = myPlot.Add.Scatter(dataX, dataY);

            AssertPlottableHasHandDrawnStyle(myPlot);
        }

        void AssertPlottableHasHandDrawnStyle(Plot myPlot)
        {
            myPlot.Style.SetLineStylePatterns(LinePattern.HandDrawn);
            int styleableCount = 0;

            foreach (var plottable in myPlot.GetPlottables())
            {
                if (plottable is IHoldLineStyle)
                {
                    Assert.That(((IHoldLineStyle)plottable).LineStyle.Pattern, Is.EqualTo(LinePattern.HandDrawn));
                    styleableCount++;
                }
            }

            Assert.That(styleableCount, Is.GreaterThan(0));
        }
    }
}
