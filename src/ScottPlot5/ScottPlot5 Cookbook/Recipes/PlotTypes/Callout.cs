namespace ScottPlotCookbook.Recipes.PlotTypes;

internal class Callout : ICategory
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

            Coordinates arrowLocation = new(7, 0);
            Coordinates textLocation = new(8, .3);

            myPlot.Add.Callout("hello", textLocation, arrowLocation);
        }
    }
}
