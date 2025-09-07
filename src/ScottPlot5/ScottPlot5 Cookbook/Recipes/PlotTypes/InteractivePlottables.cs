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
            for (int i = 0; i < 5; i++)
            {
                CoordinateLine line = Generate.RandomLine();
                myPlot.Add.InteractiveLineSegment(line);
            }
        }
    }

    public class InteractiveVerticalLine : RecipeBase
    {
        public override string Name => "Interactive Vertical Line";
        public override string Description => "An interactive vertical line has an X position " +
            "and extends to infinity along the Y axis.";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 5; i++)
            {
                double x = Generate.RandomNumber();
                myPlot.Add.InteractiveVerticalLine(x);
            }
        }
    }

    public class InteractiveHorizontalLine : RecipeBase
    {
        public override string Name => "Interactive Horizontal Line";
        public override string Description => "An interactive horizontal line has a Y position " +
            "and extends to infinity along the X axis.";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 5; i++)
            {
                double x = Generate.RandomNumber();
                myPlot.Add.InteractiveHorizontalLine(x);
            }
        }
    }

    public class InteractiveSpans : RecipeBase
    {
        public override string Name => "Interactive Spans";
        public override string Description => "Interactive vertical and horizontal spans let the user " +
            "select ranges along the vertical and horizontal axes, respectively.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.InteractiveVerticalSpan(3, 5);
            myPlot.Add.InteractiveHorizontalSpan(3, 5);
            myPlot.Axes.SetLimits(0, 10, 0, 10);
        }
    }
}
