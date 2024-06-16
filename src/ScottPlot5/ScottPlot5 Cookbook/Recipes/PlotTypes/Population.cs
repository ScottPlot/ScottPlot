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

            myPlot.Axes.Margins(bottom: 0);
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

            myPlot.Axes.Margins(bottom: 0);
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

            myPlot.Axes.Margins(bottom: 0);
        }
    }
}
