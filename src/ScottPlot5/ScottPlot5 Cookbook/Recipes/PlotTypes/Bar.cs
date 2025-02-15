namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Bar : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
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

    public class BarFillStyle : RecipeBase
    {
        public override string Name => "Bar Fill Style";
        public override string Description => "Each bar may be individually styled.";

        [Test]
        public override void Execute()
        {
            // add bars with sample data
            double[] values = { 3, 7, 9 };
            var barPlot = myPlot.Add.Bars(values);

            // bars may be styled after they have been added
            barPlot.Bars[0].FillColor = Colors.Orange;
            barPlot.Bars[1].FillColor = Colors.Green;
            barPlot.Bars[2].FillColor = Colors.Navy;

            barPlot.Bars[0].FillHatch = new ScottPlot.Hatches.Striped();
            barPlot.Bars[1].FillHatch = new ScottPlot.Hatches.Dots();
            barPlot.Bars[2].FillHatch = new ScottPlot.Hatches.Checker();

            foreach (var bar in barPlot.Bars)
            {
                bar.LineWidth = 2;
                bar.LineColor = bar.FillColor.Darken(0.5);
                bar.FillHatchColor = bar.FillColor.Lighten(0.1);
            }

            // tell the plot to autoscale with no padding beneath the bars
            myPlot.Axes.Margins(bottom: 0);
        }
    }

    public class BarCustomSingle : RecipeBase
    {
        public override string Name => "Customized Bar Plot";
        public override string Description => "Ultimate control of bar plot styling and positioning " +
            "may be achieved by creating each bar individually, styling it as desired, and setting " +
            "its exact size and location. This level of customization may be used to create extremely " +
            "advanced stacked or grouped bar plots.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Bar bar1 = new()
            {
                Position = 2,
                Value = 5,
            };

            myPlot.Add.Bar(bar1);

            ScottPlot.Bar bar2 = new()
            {
                Position = 5,
                Value = 7,
                ValueBase = 2,
                Error = 1,
                FillColor = Colors.Magenta,
                LineWidth = 3,
                LineColor = Colors.Navy,
                FillHatch = new ScottPlot.Hatches.Striped(),
                FillHatchColor = Colors.Magenta.Lighten(.2),
            };

            myPlot.Add.Bar(bar2);

            myPlot.HideGrid();
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

    public class BarWithCustomText : RecipeBase
    {
        public override string Name => "Bar with Custom Text";
        public override string Description => "Full control over bar labels may be achieved by " +
            "placing fully customizable text objects on top of bars according to their dimensions.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Palettes.Category10 palette = new();

            // create 5 groups of stacked bars
            for (int i = 0; i < 5; i++)
            {
                // create 3 individual bars with stacking
                List<ScottPlot.Bar> bars = [];
                double valueBase = 0;
                for (int j = 0; j < 3; j++)
                {
                    double barSize = Generate.RandomInteger(10, 20);
                    ScottPlot.Bar bar1 = new()
                    {
                        FillColor = palette.GetColor(j),
                        Position = i,
                        ValueBase = valueBase,
                        Value = valueBase + barSize,
                        Label = $"{barSize}",
                        CenterLabel = true,
                    };

                    bars.Add(bar1);
                    valueBase += barSize;
                }

                // plot the stacked bars
                var barPlot = myPlot.Add.Bars(bars);
                barPlot.Horizontal = true;
            }

            // style the plot so the bars start on the left edge
            myPlot.Axes.Margins(left: 0);

            // add custom group labels
            double[] tickPositions = Generate.Consecutive(5);
            string[] tickLabels = Enumerable.Range(1, 5).Select(x => $"Worker #{x}").ToArray();
            myPlot.Axes.Left.SetTicks(tickPositions, tickLabels);
        }
    }

    public class RangeChart : RecipeBase
    {
        public override string Name => "Range Chart";
        public override string Description => "A range chart displays a discrete set of named value ranges";

        [Test]
        public override void Execute()
        {
            List<(string name, CoordinateRange range)> ranges =
            [
                ("Africa", new(-35, 37)),
                ("Antarctica", new(-90, -60)),
                ("Asia", new(-11, 81)),
                ("Europe", new(-36, 71)),
                ("North America", new(-7, 83)),
                ("South America", new(-56, 13)),
                ("Australia", new(-47, -28)),
            ];
            myPlot.Add.Ranges(ranges);

            // style the axes
            myPlot.Title("Latitude Range of the Continents");
            myPlot.Axes.Bottom.TickLabelStyle.Rotation = -45;
            myPlot.Axes.Bottom.TickLabelStyle.Alignment = Alignment.MiddleRight;
            myPlot.Axes.Bottom.MinimumSize = 100;

            // use tick labels with a degree symbol
            ScottPlot.TickGenerators.NumericAutomatic tickGen = new();
            myPlot.Axes.Left.TickGenerator = tickGen;
            tickGen.LabelFormatter = (x) => $"{x}º";

            // add a horizontal line at zero and push it beneath the range plot
            var hl = myPlot.Add.HorizontalLine(0, 1, Colors.Black, LinePattern.DenselyDashed);
            myPlot.MoveToBack(hl);
        }
    }

    public class RangeChartHozontal : RecipeBase
    {
        public override string Name => "Horizontal Range Chart";
        public override string Description => "Range charts may be created using horizontally oriented bars";

        [Test]
        public override void Execute()
        {
            List<(string name, CoordinateRange range)> ranges =
            [
                ("Ontario", new(-9, 51)),
                ("England", new(0, 63)),
                ("Kentucky", new(-4, 72)),
            ];

            myPlot.Add.Ranges(ranges, horizontal: true);

            myPlot.XLabel("Temperature (ºF)");
        }
    }

    public class StackedRangeChart : RecipeBase
    {
        public override string Name => "Stacked Range Chart";
        public override string Description => "Stacked range charts depict multiple ranges for a discrete set of items";

        [Test]
        public override void Execute()
        {
            // prepare a custom color palette
            string[] colorCodes = ["#3369cc", "#95bce3", "#f4a861", "#fd8d00"];
            ScottPlot.Palettes.Custom palette = new(colorCodes);

            // create a stacked bar chart with a collection of named ranges
            string[] rangeNames = ["Yearly Low", "Mean Daily Low", "Mean Daily High", "Yearly High"];
            List<(string name, double[] edges)> ranges =
            [
                ("Ontario", [-9, 3, 7, 13, 27]),
                ("England", [4, 7, 12, 16, 24]),
                ("Kentucky", [-4, 7, 13, 20, 30]),
            ];
            myPlot.Add.StackedRanges(ranges, palette);

            // use tick labels with a degree symbol
            ScottPlot.TickGenerators.NumericAutomatic tickGen = new();
            myPlot.Axes.Left.TickGenerator = tickGen;
            tickGen.LabelFormatter = (x) => $"{x}º";

            // display the legend outside the data area
            myPlot.ShowLegend(Edge.Right);

            // add items to the legend manually
            for (int i = 0; i < rangeNames.Length; i++)
            {
                LegendItem item = new()
                {
                    LabelText = rangeNames[i],
                    FillColor = palette.GetColor(i),
                };
                myPlot.Legend.ManualItems.Add(item);
            }
            myPlot.Legend.ManualItems.Reverse();

            // improve styling and alignment
            myPlot.Legend.OutlineStyle.IsVisible = false;
            myPlot.Legend.ShadowColor = Colors.Transparent;
            myPlot.Legend.Padding = new(0);
            myPlot.Axes.Right.MaximumSize = 0;
        }
    }

    public class StackedRangeHorizontal : RecipeBase
    {
        public override string Name => "Stacked Horizontal Range Chart";
        public override string Description => "Horizontally oriented stacked range charts may be created";

        [Test]
        public override void Execute()
        {
            List<(string name, double[] edges)> ranges =
            [
                ("Ontario", [-9, 3, 7, 13, 27]),
                ("England", [4, 7, 12, 16, 24]),
                ("Kentucky", [-4, 7, 13, 20, 30]),
            ];

            myPlot.Add.StackedRanges(ranges, horizontal: true);
        }
    }
}
