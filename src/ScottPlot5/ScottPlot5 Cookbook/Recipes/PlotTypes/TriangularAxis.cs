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

            // Get coordinates for points on the triangular axis
            Coordinates[] points = [
                ta.GetCoordinates(0.50, 0.80, 0.20),
                ta.GetCoordinates(0.50, 0.60, 0.40),
                ta.GetCoordinates(0.65, 0.70, 0.30),
                ta.GetCoordinates(0.80, 0.80, 0.20),
            ];

            // Add any plot type using those coordinates
            foreach (var point in points)
            {
                myPlot.Add.Marker(point, MarkerShape.FilledCircle, 10, Colors.Red);
            }
        }
    }
}
