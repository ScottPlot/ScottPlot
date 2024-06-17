using ScottPlot.Plottables;

namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Population : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Population Plot";
    public string CategoryDescription => "Population plots display collections of individual values.";

    public class PopulationQuickstart : RecipeBase
    {
        public override string Name => "Population Quickstart";
        public override string Description => "A Population can be created from a collection " +
            "of values, styled as desired, and placed anywhere on the plot.";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 5; i++)
            {
                double[] values = Generate.RandomNormal(10, mean: 3 + i);
                myPlot.Add.Population(values, x: i);
            }

            // make the bottom of the plot snap to zero by default
            myPlot.Axes.Margins(bottom: 0);

            // replace the default numeric ticks with custom ones
            double[] tickPositions = Generate.Consecutive(5);
            string[] tickLabels = Enumerable.Range(1, 5).Select(x => $"Group {x}").ToArray();
            myPlot.Axes.Bottom.SetTicks(tickPositions, tickLabels);

            // refine appearance of the plot
            myPlot.Axes.Bottom.MajorTickStyle.Length = 0;
            myPlot.Axes.Margins(bottom: 0);
            myPlot.HideGrid();
        }
    }

    public class PopulationBox : RecipeBase
    {
        public override string Name => "Population Box Plot";
        public override string Description => "Population statistics can be displayed using box plots.";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 5; i++)
            {
                double[] values = Generate.RandomNormal(10, mean: 3 + i);
                var pop = myPlot.Add.Population(values, x: i);

                // disable visibility of the bar symbol
                pop.Bar.IsVisible = false;

                // enable visibility of the box symbol
                pop.Box.IsVisible = true;
            }

            // refine appearance of the plot
            myPlot.HideGrid();
        }
    }

    public class PopulationBoxValues : RecipeBase
    {
        public override string Name => "Population Box Values";
        public override string Description => "The values displayed by the box midline, body, and whisker can be " +
            "configured by assigning a static function to the box value configuration property.";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 5; i++)
            {
                double[] values = Generate.RandomNormal(10, mean: 3 + i);
                var pop = myPlot.Add.Population(values, x: i);
                pop.Bar.IsVisible = false;
                pop.Box.IsVisible = true;

                pop.BoxValueConfig = PopulationSymbol.BoxValueConfigurator_MeanStdErrStDev;
            }

            // refine appearance of the plot
            myPlot.HideGrid();
        }
    }

    public class PopulationBarStyle : RecipeBase
    {
        public override string Name => "Population Bar Styling";
        public override string Description => "The bar symbol in population plots can be extensively styled.";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 5; i++)
            {
                double[] values = Generate.RandomNormal(10, mean: 3 + i);
                var pop = myPlot.Add.Population(values, x: i);

                pop.Bar.FillColor = pop.Marker.MarkerLineColor.WithAlpha(.5);
                pop.Bar.BorderLineWidth = 2;
                pop.Bar.ErrorLineWidth = 2;
                pop.Bar.ErrorNegative = false;
            }

            // refine appearance of the plot
            myPlot.Axes.Margins(bottom: 0);
            myPlot.HideGrid();
        }
    }

    public class PopulationBoxStyle : RecipeBase
    {
        public override string Name => "Population Box Styling";
        public override string Description => "The box symbol in population plots can be extensively styled.";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 5; i++)
            {
                double[] values = Generate.RandomNormal(10, mean: 3 + i);
                var pop = myPlot.Add.Population(values, x: i);
                pop.Bar.IsVisible = false;
                pop.Box.IsVisible = true;
                pop.Box.LineWidth = 2;
                pop.Box.FillColor = pop.Marker.MarkerLineColor.WithAlpha(.5);
            }

            // refine appearance of the plot
            myPlot.HideGrid();
        }
    }

    public class PopulationMarkerStyle : RecipeBase
    {
        public override string Name => "Population Marker Styling";
        public override string Description => "The data markers in population plots can be extensively styled.";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 5; i++)
            {
                double[] values = Generate.RandomNormal(10, mean: 3 + i);
                var pop = myPlot.Add.Population(values, x: i);

                pop.Marker.LineWidth = 2;
                pop.Marker.Color = Colors.Black.WithAlpha(.5);
                pop.Marker.Shape = MarkerShape.OpenTriangleUp;
            }

            // refine appearance of the plot
            myPlot.Axes.Margins(bottom: 0);
            myPlot.HideGrid();
        }
    }

    public class PopulationArrangement : RecipeBase
    {
        public override string Name => "Population Arrangement";
        public override string Description => "The user may customize where data is drawn relative to the bar or box. " +
            "Centering everything can be used to achieve an effect where data points are drawn over the bar or box.";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 5; i++)
            {
                double[] values = Generate.RandomNormal(10, mean: 3 + i);
                var pop = myPlot.Add.Population(values, x: i);

                pop.MarkerAlignment = HorizontalAlignment.Center;
                pop.BarAlignment = HorizontalAlignment.Center;
                pop.Marker.Shape = MarkerShape.OpenDiamond;
                pop.Marker.Color = Colors.Black.WithAlpha(.5);
                pop.Bar.FillColor = Colors.Gray;
                pop.Bar.BorderLineWidth = 2;
                pop.Bar.ErrorLineWidth = 2;
                pop.Width = 0.5;
            }

            // refine appearance of the plot
            myPlot.Axes.Margins(bottom: 0);
            myPlot.HideGrid();
        }
    }

    public class PopulationGroups : RecipeBase
    {
        public override string Name => "Population Groups";
        public override string Description => "Groups of populations can be achieved by " +
            "customizing position, color, axis labels, and legend items.";

        [Test]
        public override void Execute()
        {
            // define the groups
            string[] groupNames = { "Gen X", "Gen Y", "Gen Z" };
            string[] categoryNames = { "Python", "C#", "Rust" };
            Color[] categoryColors = { Colors.C0, Colors.C1, Colors.C2 };

            // add random data to the plot
            for (int groupIndex = 0; groupIndex < groupNames.Length; groupIndex++)
            {
                for (int categoryIndex = 0; categoryIndex < categoryNames.Length; categoryIndex++)
                {
                    double[] values = Generate.RandomNormal(10, mean: 2 + groupIndex * 2);
                    double x = groupIndex * (categoryNames.Length + 1) + categoryIndex;
                    var pop = myPlot.Add.Population(values, x);
                    pop.Marker.MarkerLineColor = categoryColors[categoryIndex].WithAlpha(.75);
                    pop.Marker.Size = 7;
                    pop.Marker.LineWidth = 1.5f;
                    pop.Bar.FillColor = categoryColors[categoryIndex];
                }
            }

            // apply group names to horizontal tick labels
            double tickDelta = categoryNames.Length + 1;
            double[] tickPositions = Enumerable.Range(0, groupNames.Length)
                .Select(x => x * tickDelta + tickDelta / 2 - 1)
                .ToArray();
            myPlot.Axes.Bottom.SetTicks(tickPositions, groupNames);
            myPlot.Axes.Bottom.MajorTickStyle.Length = 0;

            // show category colors in the legend
            for (int i = 0; i < categoryNames.Length; i++)
            {
                LegendItem item = new()
                {
                    FillColor = categoryColors[i],
                    LabelText = categoryNames[i]
                };
                myPlot.Legend.ManualItems.Add(item);
            }
            myPlot.Legend.Orientation = Orientation.Horizontal;
            myPlot.Legend.Alignment = Alignment.UpperLeft;

            // refine appearance of the plot
            myPlot.Axes.Margins(bottom: 0, top: 0.3);
            myPlot.YLabel("Bugs per Hour");
            myPlot.HideGrid();
        }
    }
}
