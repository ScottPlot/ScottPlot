using NUnit.Framework;

namespace ScottPlotTests.PlotTypes
{
    public class Bar
    {
        [Test]
        public void Test_Bar_Series()
        {
            double[] values = { 10, 15, 12, 6, 8, 4, 12 };

            var plt = new ScottPlot.Plot(400, 300);
            plt.AddBar(values);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_PositiveAndNegative()
        {
            var plt = new ScottPlot.Plot(600, 400);

            string[] labels = { "one", "two", "three", "four", "five" };
            double[] positions = { 0, 1, 2, 3, 4 };
            double[] values = { 3, -6, 2, -8, 4 };
            double[] errors = { 1, 2, 3, .5, 1.5 };

            plt.AddBar(values, errors, positions);
            plt.XTicks(positions, labels);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_PositiveOnly()
        {
            var plt = new ScottPlot.Plot(600, 400);

            string[] labels = { "one", "two", "three", "four", "five" };
            double[] positions = { 0, 1, 2, 3, 4 };
            double[] values = { 3, 6, 2, 8, 4 };
            double[] errors = { 1, 2, 3, .5, 1.5 };

            plt.AddBar(values, errors, positions);
            plt.XTicks(positions, labels);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_NegativeOnly()
        {
            var plt = new ScottPlot.Plot(600, 400);

            string[] labels = { "one", "two", "three", "four", "five" };
            double[] positions = { 0, 1, 2, 3, 4 };
            double[] values = { -3, -6, -2, -8, -4 };
            double[] errors = { 1, 2, 3, .5, 1.5 };

            plt.AddBar(values, errors, positions);
            plt.XTicks(positions, labels);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_SeriesWithError()
        {
            double[] positions = { 0, 1, 2, 3, 4, 5, 6 };
            double[] values = { 10, 15, 12, 6, 8, 4, 12 };
            double[] errors = { 4, 1, 7, 3, 6, 2, 3 };

            var plt = new ScottPlot.Plot(400, 300);
            plt.AddBar(values, errors, positions);
            plt.Grid(lineStyle: ScottPlot.LineStyle.Dot);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_MutiSeries()
        {
            double[] ys1 = { 10, 15, 12, 6, 8, };
            double[] ys2 = { 6, 8, 8, 9, 5 };

            var plt = new ScottPlot.Plot(400, 300);

            var bar1 = plt.AddBar(ys1);
            bar1.PositionOffset = -.20;
            bar1.BarWidth = .3;
            bar1.ShowValuesAboveBars = true;
            bar1.Orientation = ScottPlot.Orientation.Horizontal;
            bar1.Label = "Series A";

            var bar2 = plt.AddBar(ys2);
            bar2.PositionOffset = +.20;
            bar2.BarWidth = 0.3;
            bar2.ShowValuesAboveBars = true;
            bar2.Label = "Series B";
            bar2.Orientation = ScottPlot.Orientation.Horizontal;

            plt.Grid(lineStyle: ScottPlot.LineStyle.Dot);
            plt.XAxis.Grid(false);
            plt.Legend(location: ScottPlot.Alignment.UpperRight);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_SeriesWithErrorHorizontal()
        {
            double[] positions = { 0, 1, 2, 3, 4, 5, 6 };
            double[] values = { 10, 15, 12, 6, 8, 4, 12 };
            double[] errors = { 4, 1, 7, 3, 6, 2, 3 };

            var plt = new ScottPlot.Plot(400, 300);
            var bar = plt.AddBar(values, errors, positions);
            bar.Orientation = ScottPlot.Orientation.Horizontal;
            plt.Grid(lineStyle: ScottPlot.LineStyle.Dot);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_Stacked()
        {
            double[] valuesA = { 1, 2, 3, 2, 1, };
            double[] valuesB = { 3, 3, 2, 1, 3 };

            // to simulate stacking, shift B up by A
            double[] valuesB2 = new double[valuesB.Length];
            for (int i = 0; i < valuesB.Length; i++)
                valuesB2[i] = valuesA[i] + valuesB[i];

            var plt = new ScottPlot.Plot(400, 300);

            // plot the uppermost bar first
            var bar1 = plt.AddBar(valuesB2);
            bar1.Label = "Series B";

            // plot lower bars last (in front)
            var bar2 = plt.AddBar(valuesA);
            bar2.Label = "Series A";

            plt.Legend(location: ScottPlot.Alignment.UpperRight);
            plt.SetAxisLimits(yMax: 7);
            plt.Title("Stacked Bar Charts");
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_ShowValues()
        {
            double[] ys1 = { 10, 15, 12, 6, 8, };
            double[] ys2 = { 6, 8, 8, 9, 5 };

            var plt = new ScottPlot.Plot(400, 300);

            var bar1 = plt.AddBar(ys1);
            bar1.PositionOffset = -.20;
            bar1.BarWidth = .3;
            bar1.ShowValuesAboveBars = true;
            bar1.Label = "Series A";

            var bar2 = plt.AddBar(ys2);
            bar2.PositionOffset = +.20;
            bar2.BarWidth = 0.3;
            bar2.ShowValuesAboveBars = true;
            bar2.Label = "Series B";

            plt.Grid(lineStyle: ScottPlot.LineStyle.Dot);
            plt.XAxis.Grid(false);
            plt.Legend(location: ScottPlot.Alignment.UpperRight);
            plt.SetAxisLimits(yMin: 0);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_ShowValuesHorizontal()
        {
            double[] ys1 = { 10, 15, 12, 6, 8, };
            double[] ys2 = { 6, 8, 8, 9, 5 };

            var plt = new ScottPlot.Plot(400, 300);

            var bar1 = plt.AddBar(ys1);
            bar1.PositionOffset = -.20;
            bar1.BarWidth = .3;
            bar1.ShowValuesAboveBars = true;
            bar1.Orientation = ScottPlot.Orientation.Horizontal;
            bar1.Label = "Series A";

            var bar2 = plt.AddBar(ys2);
            bar2.PositionOffset = +.20;
            bar2.BarWidth = 0.3;
            bar2.ShowValuesAboveBars = true;
            bar2.Label = "Series B";
            bar2.Orientation = ScottPlot.Orientation.Horizontal;

            plt.Grid(lineStyle: ScottPlot.LineStyle.Dot);
            plt.XAxis.Grid(false);
            plt.Legend(location: ScottPlot.Alignment.UpperRight);
            plt.SetAxisLimits(xMin: 0);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_NegativeBars_AutoAxisProperly()
        {
            double[] heights = { 1, 1, 1 };
            double[] bases = { 0, 1, -1 };

            var plt = new ScottPlot.Plot();
            var bar = plt.AddBar(heights);
            bar.ValueOffsets = bases;

            plt.AxisAuto();
            var limits = plt.GetAxisLimits();
            Assert.GreaterOrEqual(limits.YMax, 2);
            Assert.LessOrEqual(limits.YMin, -1);
        }

        [Test]
        public void Test_NegativeBars_AutoAxisProperlySin()
        {
            // recreates https://github.com/ScottPlot/ScottPlot/issues/1750
            double[] heights = ScottPlot.DataGen.Ones(21);
            double[] bases = ScottPlot.DataGen.Sin(21);

            ScottPlot.Plot plt = new(400, 300);
            var bar = plt.AddBar(heights);
            bar.ValueOffsets = bases;

            //TestTools.SaveFig(plt);

            plt.AxisAuto();
            var limits = plt.GetAxisLimits();
            Assert.GreaterOrEqual(limits.YMax, 2);
            Assert.LessOrEqual(limits.YMin, -1);
        }

        [Test]
        public void Test_Bar_ZoomFarIn()
        {
            var plt = new ScottPlot.Plot(400, 300);

            double[] values = { 1e6, 2e6, 3e6 };
            var bar = plt.AddBar(values);

            plt.SetAxisLimits(-1, 3, 1000, 1000 + .001);
            MeanPixel bmpVisible = new(plt);
            //TestTools.SaveFig(plt, "visible");

            bar.IsVisible = false;
            MeanPixel bmpHidden = new(plt);
            //TestTools.SaveFig(plt, "hidden");

            Assert.That(bmpVisible.IsDifferentThan(bmpHidden));
        }
    }
}
