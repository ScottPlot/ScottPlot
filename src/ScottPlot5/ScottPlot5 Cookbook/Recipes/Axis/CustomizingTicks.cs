namespace ScottPlotCookbook.Recipes.Axis;

internal class CustomizingTicks : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.Customization,
        PageName = "Customizing Ticks",
        PageDescription = "Advanced customization of tick marks and tick labels",
    };

    internal class AltTickGen : RecipeTestBase
    {
        public override string Name => "Custom Tick Generators";
        public override string Description => "Alternative tick generators can be created and assigned to axes. " +
            "Some common tick generators are provided with ScottPlot, and users also have the option create their own.";

        [Test]
        public override void Recipe()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            ITickGenerator customTickGenerator = new ScottPlot.TickGenerators.NumericFixedInterval(11);

            myPlot.XAxis.TickGenerator = customTickGenerator;
        }
    }
}
