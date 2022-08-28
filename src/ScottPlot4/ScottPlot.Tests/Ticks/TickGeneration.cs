using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlotTests.Ticks
{
    class TickGeneration
    {
        [Test]
        public void Test_DefinedSpacing_NumericAxis()
        {
            int pointCount = 20;

            // create a series of day numbers
            double[] days = ScottPlot.DataGen.Consecutive(pointCount);

            // simulate data for each date
            double[] values = new double[pointCount];
            Random rand = new Random(0);
            for (int i = 1; i < pointCount; i++)
                values[i] = values[i - 1] + rand.NextDouble();


            var pltDefault = new ScottPlot.Plot();
            pltDefault.Title("Default xSpacing");
            pltDefault.AddScatter(days, values);

            var pltTest = new ScottPlot.Plot();
            pltTest.Title("xSpacing = 1 unit");
            pltTest.AddScatter(days, values);

            // force inter-tick distance on a numerical axis
            pltTest.XAxis.ManualTickSpacing(1);

            //TestTools.SaveFig(pltDefault);
            //TestTools.SaveFig(pltTest);
        }

        [Test]
        public void Test_DefinedSpacing_DateTimeAxis()
        {
            int pointCount = 20;

            // create a series of dates
            double[] dates = new double[pointCount];
            var firstDay = new DateTime(2020, 1, 22);
            for (int i = 0; i < pointCount; i++)
                dates[i] = firstDay.AddDays(i).ToOADate();

            // simulate data for each date
            double[] values = new double[pointCount];
            Random rand = new Random(0);
            for (int i = 1; i < pointCount; i++)
                values[i] = values[i - 1] + rand.NextDouble();

            var pltDefault = new ScottPlot.Plot();
            pltDefault.Title("Default xSpacing");
            pltDefault.AddScatter(dates, values);
            pltDefault.XAxis.DateTimeFormat(true);

            var pltTest = new ScottPlot.Plot();
            pltTest.Title("xSpacing = 1 day");
            pltTest.AddScatter(dates, values);
            pltTest.XAxis.DateTimeFormat(true);
            pltTest.XAxis.TickLabelStyle(rotation: 45);
            pltTest.Layout(bottom: 60); // need extra height to accomodate rotated labels

            // force 1 tick per day on a DateTime axis
            pltTest.XAxis.ManualTickSpacing(1, ScottPlot.Ticks.DateTimeUnit.Day);

            //TestTools.SaveFig(pltDefault);
            //TestTools.SaveFig(pltTest);
        }

        [Test]
        public void Test_LargePlot_DateTimeAxis()
        {
            Random rand = new Random(0);
            double[] data = DataGen.RandomWalk(rand, 100_000);
            DateTime firstDay = new DateTime(2020, 1, 1);

            var plt = new ScottPlot.Plot(4000, 400);
            var sig = plt.AddSignal(data, sampleRate: 60 * 24);
            sig.OffsetX = firstDay.ToOADate();

            plt.XAxis.DateTimeFormat(true);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_ManualTicks_CanBeEnabledAndDisabled()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddSignal(ScottPlot.DataGen.Sin(51));

            // tick positions are automatic by default
            string originalTicks = TestTools.GetXTickString(plt);

            // set manual positions
            double[] manualXs = { -100, 15, 25, 35, 1234 };
            string[] manyalLabels = { "x", "a", "b", "c", "y" };
            plt.XAxis.ManualTickPositions(manualXs, manyalLabels);
            Assert.AreNotEqual(originalTicks, TestTools.GetXTickString(plt));

            // reset to automatic ticks
            plt.XAxis.AutomaticTickPositions();
            Assert.AreEqual(originalTicks, TestTools.GetXTickString(plt));
        }

        [Test]
        public void Test_AutomaticTicks_AdditionalTicksAppear()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddSignal(ScottPlot.DataGen.Sin(51));

            // tick positions are automatic by default
            string originalTicks = TestTools.GetXTickString(plt);

            // set additional positions
            double[] positions = { -100, 15, 25, 35, 1234 };
            string[] labels = { "x", "a", "b", "c", "y" };
            plt.XAxis.AutomaticTickPositions(positions, labels);
            Assert.AreNotEqual(originalTicks, TestTools.GetXTickString(plt));

            // reset to automatic ticks
            plt.XAxis.AutomaticTickPositions();
            Assert.AreEqual(originalTicks, TestTools.GetXTickString(plt));
        }

        [Test]
        public void Test_ManualTicks_DifferentLengths()
        {
            ScottPlot.Plot plt = new();

            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            double[] positions = { 10, 20, 30 };
            string[] badLabels = { "ten", "twenty" };
            string[] goodLabels = { "ten", "twenty", "thirty" };

            Assert.Throws<ArgumentException>(() => plt.XAxis.ManualTickPositions(positions, badLabels));

            Assert.DoesNotThrow(() => plt.XAxis.ManualTickPositions(positions, goodLabels));

        }
    }
}
