using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlotTypes
{
    class AxisLine
    {
        [Test]
        public void Test_LineLabels_LookGood()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));

            var hline = plt.AddHorizontalLine(.5);
            var vline = plt.AddVerticalLine(25);
            var vline2 = plt.AddVerticalLine(30);

            hline.PositionLabel = true;
            hline.LineWidth = 2;
            hline.PositionLabelBackground = hline.Color;

            vline.PositionLabel = true;
            vline.LineWidth = 2;
            vline.PositionLabelBackground = vline.Color;

            vline2.PositionLabel = true;
            vline2.PositionLabelOppositeAxis = true;

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_AxisLine_GetDataLimits()
        {
            ScottPlot.Plot plt = new(400, 300);

            plt.AddHorizontalLine(5);
            plt.AxisAuto();

            var limits = plt.GetDataLimits();
            Assert.That(limits.XMin, Is.EqualTo(double.NegativeInfinity));
            Assert.That(limits.XMax, Is.EqualTo(double.PositiveInfinity));
            Assert.That(limits.YMin, Is.EqualTo(5));
            Assert.That(limits.YMax, Is.EqualTo(5));
        }

        [Test]
        public void Test_AxisLineVector_GetDataLimits()
        {
            var plt = new ScottPlot.Plot(400, 300);

            var horizontalLine = new ScottPlot.Plottable.HLineVector()
            {
                Ys = new double[] { 5 },
                Min = 1,
                Max = 10,
            };

            plt.Add(horizontalLine);
            TestTools.SaveFig(plt);

            var limits = plt.GetDataLimits();
            Assert.That(limits.XMin, Is.EqualTo(1));
            Assert.That(limits.XMax, Is.EqualTo(10));
            Assert.That(limits.YMin, Is.EqualTo(5));
            Assert.That(limits.YMax, Is.EqualTo(5));
        }
    }
}
