namespace ScottPlotCookbook.Recipes.PlotTypes;

public class TernaryAxis : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
    public string CategoryName => "Ternary Axis";
    public string CategoryDescription => "Create a ternary axis and add it to the plot " +
        "to display data on a triangular coordinate system.";

    public class TernaryAxisQuickStart : RecipeBase
    {
        public override string Name => "Ternary Axis Quickstart";
        public override string Description => "A ternary axis is a coordinate system " +
            "used to represent compositions of three components that sum to a constant, " +
            "typically shown as a triangular graph where each corner represents 100% of one component, " +
            "and any point within the triangle shows the relative proportions of all three.";

        [Test]
        public override void Execute()
        {
            // Add a ternary axis to the plot
            var ta = myPlot.Add.TernaryAxis();

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
