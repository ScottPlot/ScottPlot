using NUnit.Framework;
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
            pltDefault.PlotScatter(days, values);
            pltDefault.Title("Default xSpacing");

            var pltTest = mplt.GetSubplot(0, 1);
            pltTest.PlotScatter(days, values);
            pltTest.Grid(xSpacing: 1); // force inter-tick distance
            pltTest.Title("xSpacing = 1 unit");

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
            pltDefault.PlotScatter(dates, values);
            pltDefault.Ticks(dateTimeX: true);
            pltDefault.Title("Default xSpacing");

            var pltTest = mplt.GetSubplot(0, 1);
            pltTest.PlotScatter(dates, values);
            pltTest.Ticks(dateTimeX: true);
            pltTest.Grid(xSpacing: 1); // force 1 tick per day
            pltTest.Title("xSpacing = 1 day");

            TestTools.SaveFig(mplt);
        }
    }
}
