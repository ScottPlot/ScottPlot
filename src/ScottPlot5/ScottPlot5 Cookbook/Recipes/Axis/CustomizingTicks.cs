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
            "tick labels from tick positions.";

        [Test]
        public override void Execute()
        {
            double[] xs = ScottPlot.Generate.Consecutive(100, 1, -50);
            myPlot.Add.Scatter(xs, ScottPlot.Generate.Sin(100));
            myPlot.Add.Scatter(xs, ScottPlot.Generate.Cos(100));

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
            myPlot.Add.Signal(ScottPlot.Generate.Sin(51));
            myPlot.Add.Signal(ScottPlot.Generate.Cos(51));

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
}
