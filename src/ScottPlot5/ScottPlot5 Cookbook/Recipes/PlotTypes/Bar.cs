namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Bar : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Bar Plot";
    public string CategoryDescription => "Bar plots represent values as horizontal or vertical rectangles";

    public class Quickstart : RecipeBase
    {
        public override string Name => "Bar Plot Quickstart";
        public override string Description => "Bar plots can be added from a series of values.";

        [Test]
        public override void Execute()
        {
            // add bars
            double[] values = { 5, 10, 7, 13 };
            myPlot.Add.Bars(values);

            // tell the plot to autoscale with no padding beneath the bars
            myPlot.Axes.Margins(bottom: 0);
        }
    }

    public class BarLegend : RecipeBase
    {
        public override string Name => "Bar Plot Legend";
        public override string Description => "A collection of bars can appear in the legend as a single item.";

        [Test]
        public override void Execute()
        {
            double[] xs1 = { 1, 2, 3, 4 };
            double[] ys1 = { 5, 10, 7, 13 };
            var bars1 = myPlot.Add.Bars(xs1, ys1);
            bars1.LegendText = "Alpha";

            double[] xs2 = { 6, 7, 8, 9 };
            double[] ys2 = { 7, 12, 9, 15 };
            var bars2 = myPlot.Add.Bars(xs2, ys2);
            bars2.LegendText = "Beta";

            myPlot.ShowLegend(Alignment.UpperLeft);
            myPlot.Axes.Margins(bottom: 0);
        }
    }

    public class BarValues : RecipeBase
    {
        public override string Name => "Bar with Value Labels";
        public override string Description => "Set the `Label` property of bars " +
            "to have text displayed above each bar.";

        [Test]
        public override void Execute()
        {
            double[] values = { 5, 10, 7, 13 };
            var barPlot = myPlot.Add.Bars(values);

            // define the content of labels
            foreach (var bar in barPlot.Bars)
            {
                bar.Label = bar.Value.ToString();
            }

            // customize label style
            barPlot.ValueLabelStyle.Bold = true;
            barPlot.ValueLabelStyle.FontSize = 18;

            myPlot.Axes.Margins(bottom: 0, top: .2);
        }
    }


    public class BarValuesHorizontal : RecipeBase
    {
        public override string Name => "Bar with Value Labels (horizontal)";
        public override string Description => "Set the `Label` property of bars " +
            "to have text displayed beside (left or right) of each bar.";

        [Test]
        public override void Execute()
        {
            double[] values = { -20, 10, 7, 13 };

            // set the label for each bar
            var barPlot = myPlot.Add.Bars(values);
            foreach (var bar in barPlot.Bars)
            {
                bar.Label = "Label " + bar.Value.ToString();
            }

            // customize label style
            barPlot.ValueLabelStyle.Bold = true;
            barPlot.ValueLabelStyle.FontSize = 18;
            barPlot.Horizontal = true;

            // add extra margin to account for label
            myPlot.Axes.SetLimitsX(-45, 35);
            myPlot.Add.VerticalLine(0, 1, Colors.Black);
        }
    }


    public class BarPosition : RecipeBase
    {
        public override string Name => "Bar Positioning";
        public override string Description => "The exact position and size of each bar may be customized.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Bar[] bars =
            {
                new() { Position = 1, Value = 5, ValueBase = 3, FillColor = Colors.Red },
                new() { Position = 2, Value = 7, ValueBase = 0, FillColor = Colors.Blue },
                new() { Position = 4, Value = 3, ValueBase = 2, FillColor = Colors.Green },
            };

            myPlot.Add.Bars(bars);
        }
    }

    public class BarWithError : RecipeBase
    {
        public override string Name => "Bars with Error";
        public override string Description => "Bars can have errorbars.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Bar[] bars =
            {
                new() { Position = 1, Value = 5, Error = 1, FillColor = Colors.Red },
                new() { Position = 2, Value = 7, Error = 2, FillColor = Colors.Orange },
                new() { Position = 3, Value = 6, Error = 1, FillColor = Colors.Green },
                new() { Position = 4, Value = 8, Error = 2, FillColor = Colors.Blue },
            };

            myPlot.Add.Bars(bars);

            // tell the plot to autoscale with no padding beneath the bars
            myPlot.Axes.Margins(bottom: 0);
        }
    }

    public class BarTickLabels : RecipeBase
    {
        public override string Name => "Bars with Labeled Ticks";
        public override string Description => "Bars can be labeled by manually specifying axis tick mark positions and labels.";

        [Test]
        public override void Execute()
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

            myPlot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            myPlot.Axes.Bottom.MajorTickStyle.Length = 0;
            myPlot.HideGrid();

            // tell the plot to autoscale with no padding beneath the bars
            myPlot.Axes.Margins(bottom: 0);
        }
    }

    public class BarStackVertically : RecipeBase
    {
        public override string Name => "Stacked Bar Plot";
        public override string Description => "Bars can be positioned on top of each other.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Palettes.Category10 palette = new();

            ScottPlot.Bar[] bars =
            {
                // first set of stacked bars
                new() { Position = 1, ValueBase = 0, Value = 2, FillColor = palette.GetColor(0) },
                new() { Position = 1, ValueBase = 2, Value = 5, FillColor = palette.GetColor(1) },
                new() { Position = 1, ValueBase = 5, Value = 10, FillColor = palette.GetColor(2) },

                // second set of stacked bars
                new() { Position = 2, ValueBase = 0, Value = 4, FillColor = palette.GetColor(0) },
                new() { Position = 2, ValueBase = 4, Value = 7, FillColor = palette.GetColor(1) },
                new() { Position = 2, ValueBase = 7, Value = 10, FillColor = palette.GetColor(2) },
            };

            myPlot.Add.Bars(bars);

            Tick[] ticks =
            {
                new(1, "Spring"),
                new(2, "Summer"),
            };

            myPlot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            myPlot.Axes.Bottom.MajorTickStyle.Length = 0;
            myPlot.HideGrid();

            // tell the plot to autoscale with no padding beneath the bars
            myPlot.Axes.Margins(bottom: 0);
        }
    }

    public class GroupedBarPlot : RecipeBase
    {
        public override string Name => "Grouped Bar Plot";
        public override string Description => "Bars can be grouped by position and color.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Palettes.Category10 palette = new();

            ScottPlot.Bar[] bars =
            {
                // first group
                new() { Position = 1, Value = 2, FillColor = palette.GetColor(0), Error = 1 },
                new() { Position = 2, Value = 5, FillColor = palette.GetColor(1), Error = 2 },
                new() { Position = 3, Value = 7, FillColor = palette.GetColor(2), Error = 1 },

                // second group
                new() { Position = 5, Value = 4, FillColor = palette.GetColor(0), Error = 2 },
                new() { Position = 6, Value = 7, FillColor = palette.GetColor(1), Error = 1 },
                new() { Position = 7, Value = 13, FillColor = palette.GetColor(2), Error = 3 },
                
                // third group
                new() { Position = 9, Value = 5, FillColor = palette.GetColor(0), Error = 1 },
                new() { Position = 10, Value = 6, FillColor = palette.GetColor(1), Error = 3 },
                new() { Position = 11, Value = 11, FillColor = palette.GetColor(2), Error = 2 },
            };

            myPlot.Add.Bars(bars);

            // build the legend manually
            myPlot.Legend.IsVisible = true;
            myPlot.Legend.Alignment = Alignment.UpperLeft;
            myPlot.Legend.ManualItems.Add(new() { LabelText = "Monday", FillColor = palette.GetColor(0) });
            myPlot.Legend.ManualItems.Add(new() { LabelText = "Tuesday", FillColor = palette.GetColor(1) });
            myPlot.Legend.ManualItems.Add(new() { LabelText = "Wednesday", FillColor = palette.GetColor(2) });

            // show group labels on the bottom axis
            Tick[] ticks =
            {
                new(2, "Group 1"),
                new(6, "Group 2"),
                new(10, "Group 3"),
            };
            myPlot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            myPlot.Axes.Bottom.MajorTickStyle.Length = 0;
            myPlot.HideGrid();

            // tell the plot to autoscale with no padding beneath the bars
            myPlot.Axes.Margins(bottom: 0);
        }
    }

    public class HorizontalBar : RecipeBase
    {
        public override string Name => "Horizontal Bar Plot";
        public override string Description => "Bar plots can be displayed horizontally.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Bar[] bars =
            {
                new() { Position = 1, Value = 5, Error = 1, },
                new() { Position = 2, Value = 7, Error = 2, },
                new() { Position = 3, Value = 6, Error = 1, },
                new() { Position = 4, Value = 8, Error = 2, },
            };

            var barPlot = myPlot.Add.Bars(bars);
            barPlot.Horizontal = true;

            myPlot.Axes.Margins(left: 0);
        }
    }

    public class StackedBars : RecipeBase
    {
        public override string Name => "Stacked Bar Chart";
        public override string Description => "Bars can be stacked to present data in groups.";

        [Test]
        public override void Execute()
        {
            string[] categoryNames = { "Phones", "Computers", "Tablets" };
            Color[] categoryColors = { Colors.C0, Colors.C1, Colors.C2 };

            for (int x = 0; x < 4; x++)
            {
                double[] values = Generate.RandomSample(categoryNames.Length, 1000, 5000);

                double nextBarBase = 0;

                for (int i = 0; i < values.Length; i++)
                {
                    ScottPlot.Bar bar = new()
                    {
                        Value = nextBarBase + values[i],
                        FillColor = categoryColors[i],
                        ValueBase = nextBarBase,
                        Position = x,
                    };

                    myPlot.Add.Bar(bar);

                    nextBarBase += values[i];
                }
            }

            // use custom tick labels on the bottom
            ScottPlot.TickGenerators.NumericManual tickGen = new();
            for (int x = 0; x < 4; x++)
            {
                tickGen.AddMajor(x, $"Q{x + 1}");
            }
            myPlot.Axes.Bottom.TickGenerator = tickGen;

            // display groups in the legend
            for (int i = 0; i < 3; i++)
            {
                LegendItem item = new()
                {
                    LabelText = categoryNames[i],
                    FillColor = categoryColors[i]
                };
                myPlot.Legend.ManualItems.Add(item);
            }
            myPlot.Legend.Orientation = Orientation.Horizontal;
            myPlot.ShowLegend(Alignment.UpperRight);

            // tell the plot to autoscale with no padding beneath the bars
            myPlot.Axes.Margins(bottom: 0, top: .3);
        }
    }
}
