namespace ScottPlotCookbook.Recipes.PlotTypes;

public class TriangularAxis : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
    public string CategoryName => "Triangular Axis";
    public string CategoryDescription => "Create a triangular axis and add it to the plot " +
        "to display data on a triangular coordinate system.";

    public class TriangularAxisQuickStart : RecipeBase
    {
        public override string Name => "Triangular Axis Quickstart";
        public override string Description => "Create a triangular axis and add it to the plot " +
            "to display data on a triangular grid, and interact with it to convert triangular " +
            "units into Cartesian coordinates that can be used for placing any plot type on top.";

        [Test]
        public override void Execute()
        {
            // Add a triangular axis to the plot
            var ta = myPlot.Add.TriangularAxis();

            // Get points from various locations in triangular space
            Coordinates[] points = [
                ta.GetCoordinates(0.50, 0.40),
                ta.GetCoordinates(0.60, 0.40),
                ta.GetCoordinates(0.65, 0.50),
            ];

            // Any plot type may be added on top of the triangular axis
            myPlot.Add.Markers(points, MarkerShape.FilledCircle, 10, Colors.Red);
        }
    }

    public class TriangularAxisReversed : RecipeBase
    {
        public override string Name => "Triangular Axis Reverse";
        public override string Description => "Triangular axes typically ascend in a clockwise " +
            "direction for general applications, but triangular plots with counterclockwise labeling " +
            "are sometimes used for geological applications.";

        [Test]
        public override void Execute()
        {
            // Add a COUNTER-CLOCKWISE triangular axis to the plot
            var ta = myPlot.Add.TriangularAxis(clockwise: false);

            // Get points from various locations in triangular space
            Coordinates[] points = [
                ta.GetCoordinates(0.50, 0.40),
                ta.GetCoordinates(0.60, 0.40),
                ta.GetCoordinates(0.65, 0.50),
            ];

            // Any plot type may be added on top of the triangular axis
            myPlot.Add.Markers(points, MarkerShape.FilledCircle, 10, Colors.Red);
        }
    }

    public class TriangularAxisStyling : RecipeBase
    {
        public override string Name => "Triangular Axis Styling";
        public override string Description => "Triangular axis background " +
            "and grid lines may be customized.";

        [Test]
        public override void Execute()
        {
            var ta = myPlot.Add.TriangularAxis();

            // Customize the background
            ta.FillStyle.Color = Colors.Blue.WithAlpha(.1);

            // Customize the grid lines
            ta.GridLineStyle.Color = Colors.Black.WithAlpha(.5);
            ta.GridLineStyle.Pattern = LinePattern.Dotted;

            // Add sample data
            Coordinates[] points = [
                ta.GetCoordinates(0.50, 0.40),
                ta.GetCoordinates(0.60, 0.40),
                ta.GetCoordinates(0.65, 0.50),
            ];
            myPlot.Add.Markers(points, MarkerShape.FilledCircle, 10, Colors.Red);
        }
    }

    public class TriangularAxisEdgeStyling : RecipeBase
    {
        public override string Name => "Triangular Axis Edge Styling";
        public override string Description => "Styling options for edge lines, " +
            "tick marks, tick labels, and title text may be customized individually for each axis.";

        [Test]
        public override void Execute()
        {
            var ta = myPlot.Add.TriangularAxis();

            // the edge line extends from one corner to the other
            ta.Left.EdgeLineStyle.Width = 3;
            ta.Left.EdgeLineStyle.Color = Colors.Blue;

            // tick labels and marks may be styled individually
            ta.Left.TickLabelStyle.ForeColor = Colors.Blue;
            ta.Left.TickMarkStyle.Color = Colors.Blue;
            ta.Left.TickMarkStyle.Width = 3;
            ta.Left.TickOffset = new(-10, 0);
            ta.Left.TickLabelStyle.Bold = true;
            ta.Left.TickLabelStyle.OffsetX = -4;

            // an axis title may be added and styled
            ta.Left.LabelText = "Hello, World";
            ta.Left.LabelStyle.ForeColor = Colors.Blue;
            ta.Left.LabelStyle.FontSize = 26;
            ta.Left.LabelStyle.Bold = false;
            ta.Left.LabelStyle.OffsetX = -20;

            // Add sample data
            Coordinates[] points = [
                ta.GetCoordinates(0.50, 0.40),
                ta.GetCoordinates(0.60, 0.40),
                ta.GetCoordinates(0.65, 0.50),
            ];
            myPlot.Add.Markers(points, MarkerShape.FilledCircle, 10, Colors.Red);
        }
    }

    public class TriangularEdgeTitle : RecipeBase
    {
        public override string Name => "Triangular Axis Titles";
        public override string Description => "Triangular axis edges have a helper method " +
            "to easily add a title and color all the edge components similarly.";

        [Test]
        public override void Execute()
        {
            var ta = myPlot.Add.TriangularAxis();

            // Give each axis a title and color
            ta.Bottom.Title("Methane", Colors.Red);
            ta.Left.Title("Nitrogen", Colors.Green);
            ta.Right.Title("Oxygen", Colors.Blue);

            // Add sample data
            Coordinates[] points = [
                ta.GetCoordinates(0.50, 0.40),
                ta.GetCoordinates(0.60, 0.40),
                ta.GetCoordinates(0.65, 0.50),
            ];
            myPlot.Add.Markers(points, MarkerShape.FilledCircle, 10, Colors.Red);
        }
    }
}
