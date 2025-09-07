namespace ScottPlotCookbook.Recipes.PlotTypes;

public class InteractivePlottables : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
    public string CategoryName => "Interactive";
    public string CategoryDescription => "Interactive plottables interact with the mouse " +
        "without requiring the user to manually wire mouse tracking.";

    public class InteractiveLineSegment : RecipeBase
    {
        public override string Name => "Interactive Line Segment";
        public override string Description => "Two draggable points with a straight line between them.";

        [Test]
        public override void Execute()
        {
            CoordinateLine line = Generate.RandomLine();
            myPlot.Add.InteractiveLineSegment(line); ;
        }
    }
}
