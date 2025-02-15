namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Bracket : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
    public string CategoryName => "Bracket";
    public string CategoryDescription => "Brackets annotate a range along a line in coordinate space";

    public class BracketQuickstart : RecipeBase
    {
        public override string Name => "Bracket";
        public override string Description => "Brackets are useful for annotating linear ranges of data.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            myPlot.Add.Bracket(0, 1, 0, 0, "Bracket A");
            myPlot.Add.Bracket(25, -1, 38, -1, "Bracket B");
            myPlot.Add.Bracket(20, .55, 27, -.3, "Bracket C");

            myPlot.Axes.Margins(0.3, 0.4); // extra room for labels
        }
    }
}
