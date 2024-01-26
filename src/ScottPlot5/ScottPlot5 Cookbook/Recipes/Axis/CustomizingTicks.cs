using System.ComponentModel;

namespace ScottPlotCookbook.Recipes.Axis;

public class CustomizingTicks : ICategory
{
    public string Chapter => "Axis";
    public string CategoryName => "Customizing Ticks";
    public string CategoryDescription => "Advanced customization of tick marks and tick labels";

    public class CustomTickFormatter : RecipeBase
    {
        public override string Name => "Custom Tick Formatters";
        public override string Description => "Users can customize the logic used to create " +
            "tick labels from tick positions. " +
"Old versions of ScottPlot achieved this using a ManualTickPositions method.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(100, 1, -50);
            myPlot.Add.Scatter(xs, Generate.Sin(100));
            myPlot.Add.Scatter(xs, Generate.Cos(100));

            // create a static function containing the string formatting logic
            static string CustomFormatter(double position)
            {
                if (position == 0)
                    return "0";
                else if (position > 0)
                    return $"+{position}";
                else
                    return $"({-position})";
            }

            // create a custom tick generator using your custom label formatter
            ScottPlot.TickGenerators.NumericAutomatic myTickGenerator = new()
            {
                LabelFormatter = CustomFormatter
            };

            // tell an axis to use the custom tick generator
            myPlot.Axes.Bottom.TickGenerator = myTickGenerator;
        }
    }

    public class AltTickGen : RecipeBase
    {
        public override string Name => "Custom Tick Generators";
        public override string Description =>
            "Tick generators determine where ticks are to be placed and also " +
            "contain logic for generating tick labels from tick positions. " +
            "Alternative tick generators can be created and assigned to axes. " +
            "Some common tick generators are provided with ScottPlot, " +
            "and users also have the option create their own.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            myPlot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericFixedInterval(11);
        }
    }

    public class CustomTicks : RecipeBase
    {
        public override string Name => "Custom Tick Positions";
        public override string Description => "Users can define ticks to be placed at specific locations.";

        [Test]
        public override void Execute()
        {
            // display sample data
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            // create a manual tick generator and add ticks
            ScottPlot.TickGenerators.NumericManual ticks = new();

            // add major ticks with their labels
            ticks.AddMajor(0, "zero");
            ticks.AddMajor(20, "twenty");
            ticks.AddMajor(50, "fifty");

            // add minor ticks
            ticks.AddMinor(22);
            ticks.AddMinor(25);
            ticks.AddMinor(32);
            ticks.AddMinor(35);
            ticks.AddMinor(42);
            ticks.AddMinor(45);

            // tell the horizontal axis to use the custom tick genrator
            myPlot.Axes.Bottom.TickGenerator = ticks;
        }
    }

    public class RotatedTicks : RecipeBase
    {
        public override string Name => "Rotated Tick Labels";
        public override string Description => "Users can customize tick label rotation.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            myPlot.Axes.Bottom.TickLabelStyle.Rotation = -45;
            myPlot.Axes.Bottom.TickLabelStyle.OffsetY = -8;
            myPlot.Axes.Bottom.TickLabelStyle.Alignment = Alignment.MiddleRight;
        }
    }

    public class DisableGridLines : RecipeBase
    {
        public override string Name => "Disable Grid Lines";
        public override string Description => "Users can disable grid lines for specific axes.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            ScottPlot.Grids.DefaultGrid grid = myPlot.GetDefaultGrid();
            grid.MajorLineStyle.Width = 1; // TODO: demonstrate how to disable just vertical or horizontal grid lines
        }
    }

    public class StandardMinorTickDistribution : RecipeBase
    {
        public override string Name => "Minor Tick Density";
        public override string Description =>
            "Minor tick marks are automatically generated at intervals between major tick marks. " +
            "By default they are evenly spaced, but their density may be customized.";

        [Test]
        public override void Execute()
        {
            // plot sample data
            double[] xs = Generate.Consecutive(100);
            double[] ys = Generate.NoisyExponential(100);
            var sp = myPlot.Add.Scatter(xs, ys);
            sp.LineWidth = 0;

            // create a minor tick generator with 10 minor ticks per major tick
            ScottPlot.TickGenerators.EvenlySpacedMinorTickGenerator minorTickGen = new(10);

            // create a numeric tick generator that uses our custom minor tick generator
            ScottPlot.TickGenerators.NumericAutomatic tickGen = new();
            tickGen.MinorTickGenerator = minorTickGen;

            // tell the left axis to use our custom tick generator
            myPlot.Axes.Left.TickGenerator = tickGen;
        }
    }

    public class LogScaleTicks : RecipeBase
    {
        public override string Name => "Log Scale Tick Marks";
        public override string Description =>
            "The apperance of logarithmic scaling can be achieved by log-scaling " +
            "the data to be displayed then customizing the minor ticks and grid.";

        [Test]
        public override void Execute()
        {
            // start with original data
            double[] xs = Generate.Consecutive(100);
            double[] ys = Generate.NoisyExponential(100);

            // log-scale the data and account for negative values
            double[] logYs = ys.Select(Math.Log10).ToArray();

            // add log-scaled data to th eplot
            var sp = myPlot.Add.Scatter(xs, logYs);
            sp.LineWidth = 0;

            // create a minor tick generator that places log-distributed minor ticks
            ScottPlot.TickGenerators.LogMinorTickGenerator minorTickGen = new();

            // create a numeric tick generator that uses our custom minor tick generator
            ScottPlot.TickGenerators.NumericAutomatic tickGen = new();
            tickGen.MinorTickGenerator = minorTickGen;

            // create a custom tick formatter to set the label text for each tick
            static string LogTickLabelFormatter(double y) => $"{Math.Pow(10, y):N0}";

            // tell our major tick generator to only show major ticks that are whole integers
            tickGen.IntegerTicksOnly = true;

            // tell our custom tick generator to use our new label formatter
            tickGen.LabelFormatter = LogTickLabelFormatter;

            // tell the left axis to use our custom tick generator
            myPlot.Axes.Left.TickGenerator = tickGen;

            // show grid lines for minor ticks
            var grid = myPlot.GetDefaultGrid();
            grid.MajorLineStyle.Color = Colors.Black.WithOpacity(.15);
            grid.MinorLineStyle.Color = Colors.Black.WithOpacity(.05);
            grid.MinorLineStyle.Width = 1;
        }
    }
}
