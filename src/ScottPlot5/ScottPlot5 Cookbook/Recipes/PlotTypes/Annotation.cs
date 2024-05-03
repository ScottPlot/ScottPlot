namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Annotation : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Annotation";
    public string CategoryDescription => "Annotations are always-visible text labels positioned over the data area.";

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

    public class AnnotationCustomize : RecipeBase
    {
        public override string Name => "Annotation Customization";
        public override string Description => "Annotations can be extensively customized.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            var anno = myPlot.Add.Annotation("Customized\nAnnotation");
            anno.LabelFontSize = 32;
            anno.LabelFontName = Fonts.Serif;
            anno.LabelBackgroundColor = Colors.RebeccaPurple.WithAlpha(.3);
            anno.LabelFontColor = Colors.RebeccaPurple;
            anno.LabelBorderColor = Colors.Green;
            anno.LabelBorderWidth = 3;
            anno.LabelShadowColor = Colors.Transparent;
            anno.OffsetY = 40;
            anno.OffsetX = 20;
        }
    }

    public class AnnotationPositions : RecipeBase
    {
        public override string Name => "Annotation Positions";
        public override string Description => "Annotations are aligned with the data area.";

        [Test]
        public override void Execute()
        {
            foreach (Alignment alignment in Enum.GetValues(typeof(Alignment)))
            {
                myPlot.Add.Annotation(alignment.ToString(), alignment);
            }
        }
    }
}
