namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Signal : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Signal Plot";
    public string CategoryDescription => "Signal plots display evenly-spaced data";

    public class Offset : RecipeBase
    {
        public override string Name => "Offset";
        public override string Description => "Signal plots can be offset by a given X and Y value.";

        [Test]
        public override void Execute()
        {
            double[] values = ScottPlot.Generate.Sin(51);

            var sig1 = myPlot.Add.Signal(values);
            sig1.Label = "Default";

            var sig2 = myPlot.Add.Signal(values);
            sig2.Data.XOffset = 10;
            sig2.Data.YOffset = .25;
            sig2.Label = "Offset";

            myPlot.Legend.IsVisible = true;
        }
    }
}
