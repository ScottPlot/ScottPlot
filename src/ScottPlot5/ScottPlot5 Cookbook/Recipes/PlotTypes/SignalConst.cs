namespace ScottPlotCookbook.Recipes.PlotTypes;

public class SignalConst : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "SignalConst";
    public string CategoryDescription => "SignalConst is a type of signal plot " +
        "which contains immutable data points and occupies more memory but offers " +
        "greater performance for extremely large datasets. It is rarely needed, but " +
        "best use for plotting data containing millions of points.";

    public class SignalConstQuickstart : RecipeBase
    {
        public override string Name => "SignalConst Quickstart";
        public override string Description => "SignalConst can display " +
            "data with millions of points at high framerates, ideal for " +
            "interactive manipulation of large datasets.";

        [Test]
        public override void Execute()
        {
            double[] data = Generate.RandomWalk(1_000_000);
            myPlot.Add.SignalConst(data);
        }
    }
}
