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
            var polarAxis = myPlot.Add.PolarAxis(maximumRadius: 100);

            for (int i = 0; i < 10; i++)
            {
                double radius = Generate.RandomNumber(100);
                double degrees = Generate.RandomNumber(360);
                Coordinates pt = polarAxis.GetCoordinates(radius, degrees);
                myPlot.Add.Marker(pt);
            }

            myPlot.HideAxesAndGrid(); // hide the Cartesian defaults
        }
    }

    public class PolarAxisStyling : RecipeBase
    {
        public override string Name => "Polar Axis Styling";
        public override string Description => "The lines of polar axes may be extensively styled. " +
            "Polar axes have radial spokes (straight lines that extend from the origin to the maximum radius) " +
            "and circular axis lines (concentric circles centered at the origin).";

        [Test]
        public override void Execute()
        {
            var pol = myPlot.Add.PolarAxis();

            // style radial axis lines
            var radialPalette = new ScottPlot.Palettes.Category10();
            for (int i = 0; i < pol.Spokes.Count; i++)
            {
                pol.Spokes[i].LineColor = radialPalette.GetColor(i).WithAlpha(.5);
                pol.Spokes[i].LineWidth = 4;
                pol.Spokes[i].LinePattern = LinePattern.DenselyDashed;
            }

            // style the circular axis lines
            var circularColormap = new ScottPlot.Colormaps.Rain();
            for (int i = 0; i < pol.Circles.Count; i++)
            {
                double fraction = (double)i / (pol.Circles.Count - 1);
                pol.Circles[i].LineColor = circularColormap.GetColor(fraction).WithAlpha(.5);
                pol.Circles[i].LineWidth = 2;
                pol.Circles[i].LinePattern = LinePattern.Dashed;
            }
        }
    }
}
