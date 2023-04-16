namespace ScottPlotCookbook.Recipes.PlotTypes;

internal class Bar : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.PlotTypes,
        PageName = "Bar Plot",
        PageDescription = "Bar plots represent values as horizontal or vertical rectangles",
    };

    internal class Quickstart : RecipeTestBase
    {
        public override string Name => "Bar Plot Quickstart";
        public override string Description => "Bar plots can be added from a series of values.";

        [Test]
        public override void Recipe()
        {
            double[] values = { 5, 10, 7, 13 };
            myPlot.Add.Bar(values);
            myPlot.AutoScale();
            myPlot.SetAxisLimits(bottom: 0);
        }
    }

    internal class BarPosition : RecipeTestBase
    {
        public override string Name => "Bar Positioning";
        public override string Description => "The exact position and size of each bar may be customized.";

        [Test]
        public override void Recipe()
        {
            List<ScottPlot.Plottables.Bar> bars = new()
            {
                new() { Position = 5, Value = 5, ValueBase = 3, },
                new() { Position = 10, Value = 7, ValueBase = 0, },
                new() { Position = 15, Value = 3, ValueBase = 2, },
            };

            myPlot.Add.Bar(bars);
        }
    }

    internal class BarSeries : RecipeTestBase
    {
        public override string Name => "Bar Series";
        public override string Description => "Bar plots can be grouped into bar series and plotted together.";

        [Test]
        public override void Recipe()
        {
            // TODO: the bars API needs to be greatly simplified
            List<ScottPlot.Plottables.Bar> bars1 = new() { new(1, 5), new(2, 7), new(3, 9) };
            List<ScottPlot.Plottables.Bar> bars2 = new() { new(1, 3), new(2, 8), new(3, 5) };
            List<ScottPlot.Plottables.Bar> bars3 = new() { new(1, 7), new(2, 10), new(3, 7) };

            ScottPlot.Plottables.BarSeries series1 = new()
            {
                Bars = bars1,
                Label = "Series 1",
                Color = Colors.Red
            };

            ScottPlot.Plottables.BarSeries series2 = new()
            {
                Bars = bars2,
                Label = "Series 2",
                Color = Colors.Green
            };

            ScottPlot.Plottables.BarSeries series3 = new()
            {
                Bars = bars3,
                Label = "Series 3",
                Color = Colors.Blue
            };

            List<ScottPlot.Plottables.BarSeries> seriesList = new() { series1, series2, series3 };

            myPlot.Add.Bar(seriesList);

            myPlot.AutoScale();
            myPlot.SetAxisLimits(bottom: 0);
        }
    }
}
