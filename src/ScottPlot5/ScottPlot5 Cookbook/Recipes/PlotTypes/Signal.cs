namespace ScottPlotCookbook.Recipes.PlotTypes;

internal class Signal : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.PlotTypes,
        PageName = "Signal Plot",
        PageDescription = "Signal plots display evenly-spaced data",
    };

    internal class Offset : RecipeTestBase
    {
        public override string Name => "Offset";
        public override string Description => "Signal plots can be offset by a given X and Y value.";

        [Test]
        public override void Recipe()
        {
            double[] values = Generate.Sin(51);

            var sig1 = myPlot.Add.Signal(values);
            sig1.Label = "Default";

            var sig2 = myPlot.Add.Signal(values);
            sig2.Data.XOffset = 10;
            sig2.Data.YOffset = .25;
            sig2.Label = "Offset";
        }
    }
}
