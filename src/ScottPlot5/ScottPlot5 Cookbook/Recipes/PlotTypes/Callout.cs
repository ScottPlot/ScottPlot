namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Callout : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Callout";

    public string CategoryDescription => "Callouts display a label and are " +
        "connected with an arrow that marks a point on the plot.";

    public class CalloutQuickstart : RecipeBase
    {
        public override string Name => "Callout Quickstart";
        public override string Description => "Callouts display a label and are " +
        "connected with an arrow that marks a point on the plot.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(15);
            double[] ys = Generate.Sin(15);
            myPlot.Add.Scatter(xs, ys);

            myPlot.Add.Callout("Hello",
                textLocation: new(7.5, .8),
                tipLocation: new(xs[6], ys[6]));

            myPlot.Add.Callout("World",
                textLocation: new(10, 0),
                tipLocation: new(xs[13], ys[13]));
        }
    }
}
