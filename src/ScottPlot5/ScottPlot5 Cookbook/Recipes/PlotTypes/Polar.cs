namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Polar : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Polar Plot";
    public string CategoryDescription => "A polar plot represents data points in a circular layout, " +
        "where each point's distance from the origin and its angle relative to a reference direction " +
        "correspond to its magnitude and phase angle, respectively";

    public class PolarQuickStart : RecipeBase
    {
        public override string Name => "Polar Plot Quickstart";
        public override string Description => "Points defined by a radius and angle may be displayed " +
            "on a polar axis using the polar plot type";

        [Test]
        public override void Execute()
        {
            PolarCoordinates[] polarPoints =
            [
                new(radius: 10, angle: 45),
                new(radius: 50, angle: 90),
                new(radius: 30, angle: 127),
                new(radius: 15, angle: 270),
                new(radius: 125, angle: 180),
            ];

            myPlot.Add.Polar(polarPoints);

            myPlot.HideAxesAndGrid(); // hide the Cartesian defaults
        }
    }
}
