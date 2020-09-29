using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
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

            var mplt = new ScottPlot.MultiPlot(1000, 400, 1, 2);

            var pltDefault = mplt.GetSubplot(0, 0);
            pltDefault.Title("Default xSpacing");
            pltDefault.PlotScatter(days, values);

            var pltTest = mplt.GetSubplot(0, 1);
            pltTest.Title("xSpacing = 1 unit");
            pltTest.PlotScatter(days, values);

            // force inter-tick distance on a numerical axis
            pltTest.Grid(xSpacing: 1);

            TestTools.SaveFig(mplt);
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

            var mplt = new ScottPlot.MultiPlot(1000, 400, 1, 2);

            var pltDefault = mplt.GetSubplot(0, 0);
            pltDefault.Title("Default xSpacing");
            pltDefault.PlotScatter(dates, values);
            pltDefault.Ticks(dateTimeX: true);

            var pltTest = mplt.GetSubplot(0, 1);
            pltTest.Title("xSpacing = 1 day");
            pltTest.PlotScatter(dates, values);
            pltTest.Ticks(dateTimeX: true, xTickRotation: 45);
            pltTest.Layout(xScaleHeight: 60); // need extra height to accomodate rotated labels

            // force 1 tick per day on a DateTime axis
            pltTest.Grid(xSpacing: 1, xSpacingDateTimeUnit: ScottPlot.Config.DateTimeUnit.Day);

            TestTools.SaveFig(mplt);
        }

        [Test]
        public void Test_LargePlot_DateTimeAxis()
        {
            Random rand = new Random(0);
            double[] data = DataGen.RandomWalk(rand, 100_000);
            DateTime firstDay = new DateTime(2020, 1, 1);

            var plt = new ScottPlot.Plot(4000, 400);
            plt.PlotSignal(data, sampleRate: 60 * 24, xOffset: firstDay.ToOADate());
            plt.Ticks(dateTimeX: true);

            TestTools.SaveFig(plt);
        }
    }
}
