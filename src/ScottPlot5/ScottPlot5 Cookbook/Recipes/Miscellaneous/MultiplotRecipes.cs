namespace ScottPlotCookbook.Recipes.Miscellaneous;

public class MultiplotRecipes : ICategory
{
    public string Chapter => "Miscellaneous";
    public string CategoryName => "Multiplot";
    public string CategoryDescription => "Use Multiplot to create figures with multiple subplots";

    public class MultiplotQuickstart : MultiplotRecipeBase
    {
        public override string Name => "Multiplot Quickstart";
        public override string Description => "Use the Multiplot class to create figures with multiple subplots.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Plot plot1 = new();
            plot1.Add.Signal(Generate.Sin());

            ScottPlot.Plot plot2 = new();
            plot2.Add.Signal(Generate.Cos());

            multiplot.AddPlot(plot1);
            multiplot.AddPlot(plot2);
            multiplot.LayoutRows();
        }
    }
}
