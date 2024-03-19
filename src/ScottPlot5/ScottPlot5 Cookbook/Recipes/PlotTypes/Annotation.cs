namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Annotation : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Arrow";
    public string CategoryDescription => "Arrows point to a location in coordinate space.";

    public class AnnotationQuickstart : RecipeBase
    {
        public override string Name => "Annotation Quickstart";
        public override string Description => "Annotations are labels you can place on " +
            "the data area of a plot. Unlike Text added to the plot (which is placed in coordinate units on the axes), " +
            "Annotations are positioned relative to the data area (in pixel units) and do not move as the " +
            "plot is panned and zoomed.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            myPlot.Add.Annotation("This is an Annotation");
        }
    }
}
