namespace ScottPlotCookbook.Recipes.Axis;

internal class CustomizingTicks : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.Customization,
        PageName = "Customizing Ticks",
        PageDescription = "Advanced customization of tick marks and tick labels",
    };

    internal class CustomTickFormatter : RecipeTestBase
    {
        public override string Name => "Custom Tick Formatters";
        public override string Description => "Users can customize the logic used to create " +
            "tick labels from tick positions.";

        [Test]
        public override void Recipe()
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
            myPlot.XAxis.TickGenerator = myTickGenerator;
        }
    }

    internal class AltTickGen : RecipeTestBase
    {
        public override string Name => "Custom Tick Generators";
        public override string Description =>
            "Tick generators determine where ticks are to be placed and also " +
            "contain logic for generating tick labels from tick positions. " +
            "Alternative tick generators can be created and assigned to axes. " +
            "Some common tick generators are provided with ScottPlot, " +
            "and users also have the option create their own.";

        [Test]
        public override void Recipe()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            myPlot.XAxis.TickGenerator = new ScottPlot.TickGenerators.NumericFixedInterval(11);
        }
    }
}
