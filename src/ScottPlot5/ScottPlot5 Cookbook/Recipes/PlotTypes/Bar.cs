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
            myPlot.Add.Bars(values);
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
            List<ScottPlot.Bar> bars = new()
            {
                new() { Position = 1, Value = 5, ValueBase = 3, },
                new() { Position = 2, Value = 7, ValueBase = 0, },
                new() { Position = 4, Value = 3, ValueBase = 2, },
            };

            myPlot.Add.Bars(bars);
        }
    }

    internal class BarWithError : RecipeTestBase
    {
        public override string Name => "Bars with Error";
        public override string Description => "Bars can have errorbars.";

        [Test]
        public override void Recipe()
        {
            List<ScottPlot.Bar> bars = new()
            {
                new() { Position = 1, Value = 5, Error = 1, },
                new() { Position = 2, Value = 7, Error = 2, },
                new() { Position = 3, Value = 6, Error = 1, },
                new() { Position = 4, Value = 8, Error = 2, },
            };

            myPlot.Add.Bars(bars);
        }
    }

    internal class BarTickLabels : RecipeTestBase
    {
        public override string Name => "Bars with Labeled Ticks";
        public override string Description => "Bars can be labeled by manually specifying axis tick mark positions and labels.";

        [Test]
        public override void Recipe()
        {
            myPlot.Add.Bar(position: 1, value: 5, error: 1);
            myPlot.Add.Bar(position: 2, value: 7, error: 2);
            myPlot.Add.Bar(position: 3, value: 6, error: 1);
            myPlot.Add.Bar(position: 4, value: 8, error: 2);

            Tick[] ticks =
            {
                new(1, "Apple"),
                new(2, "Orange"),
                new(3, "Pear"),
                new(4, "Banana"),
            };

            myPlot.BottomAxis.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
        }
    }
}
