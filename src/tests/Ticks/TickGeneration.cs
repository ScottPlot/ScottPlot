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
    }
}
