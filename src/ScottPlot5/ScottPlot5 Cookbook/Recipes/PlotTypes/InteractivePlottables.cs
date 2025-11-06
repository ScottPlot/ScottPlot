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

    public class InteractiveHorizontalLineSegment : RecipeBase
    {
        public override string Name => "Interactive Horizontal Line Segment";
        public override string Description => "Horizontal line segments can be expanded horizontally " +
            "by dragging the left and right edges, or slid vertically by dragging the center line.";

        [Test]
        public override void Execute()
        {
            for (int i = 1; i <= 5; i++)
            {
                double y = i;
                double x1 = i;
                double x2 = i * 2;
                myPlot.Add.InteractiveHorizontalLineSegment(x1, x2, y);
            }
        }
    }

    public class InteractiveVerticalLineSegment : RecipeBase
    {
        public override string Name => "Interactive Vertical Line Segment";
        public override string Description => "Vertical line segments can be expanded vertically " +
            "by dragging the top and bottom edges, or slid horizontally by dragging the center line.";

        [Test]
        public override void Execute()
        {
            for (int i = 1; i <= 5; i++)
            {
                double x = i;
                double y1 = i;
                double y2 = i * 2;
                myPlot.Add.InteractiveVerticalLineSegment(x, y1, y2);
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

    public class InteractiveMarker : RecipeBase
    {
        public override string Name => "Interactive Marker";
        public override string Description => "Interactive markers respond to hover events and can be dragged.";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 10; i++)
            {
                Coordinates point = Generate.RandomCoordinates();
                var marker = myPlot.Add.InteractiveMarker(point);
                marker.MarkerStyle.Shape = Generate.RandomMarkerShape();
            }
        }
    }

    public class InteractiveRectangle : RecipeBase
    {
        public override string Name => "Interactive Rectangles";
        public override string Description => "Interactive rectangles can be resized by dragging their edges " +
            "or repositioned by dragging their bodies";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 5; i++)
            {
                CoordinateRect rect = Generate.RandomCoordinateRect();
                myPlot.Add.InteractiveRectangle(rect);
            }
        }
    }

    public class InteractivePolygon : RecipeBase
    {
        public override string Name => "Interactive Polygons";
        public override string Description => "Interactive polygons can be resized by dragging their vertices " +
            "or repositioned by dragging their bodies";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 5; i++)
            {
                var count = Generate.RandomInteger(3, 7);
                var vertices = new Coordinates[count];
                for (int j = 0; j < count; j++)
                {
                    vertices[j] = Generate.RandomCoordinates();
                }

                myPlot.Add.InteractivePolygon(vertices);
            }
        }
    }
}
