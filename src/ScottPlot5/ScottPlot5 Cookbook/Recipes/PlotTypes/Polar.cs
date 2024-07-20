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

    public class PolarAxisStyling : RecipeBase
    {
        public override string Name => "Polar Axis Styling";
        public override string Description => "The lines of polar axes may be extensively styled. " +
            "Polar axes have radial spokes (straight lines that extend from the origin to the maximum radius) " +
            "and circular axis lines (concentric circles centered at the origin).";

        [Test]
        public override void Execute()
        {
            var pol = myPlot.Add.Polar();

            // style radial axis lines
            var radialPalette = new ScottPlot.Palettes.Category10();
            for (int i = 0; i < pol.RadialAxis.Spokes.Length; i++)
            {
                var spoke = pol.RadialAxis.Spokes[i];
                spoke.LineColor = radialPalette.GetColor(i).WithAlpha(.5);
                spoke.LineWidth = 4;
                spoke.LinePattern = LinePattern.DenselyDashed;
            }

            // style the circular axis lines
            var circularColormap = new ScottPlot.Colormaps.Rain();
            for (int i = 0; i < pol.CircularAxis.AxisLines.Length; i++)
            {
                var circularLine = pol.CircularAxis.AxisLines[i];
                double fraction = (double)i / (pol.CircularAxis.AxisLines.Length - 1);
                circularLine.LineColor = circularColormap.GetColor(fraction).WithAlpha(.5);
                circularLine.LineWidth = 2;
                circularLine.LinePattern = LinePattern.Dashed;
            }

            myPlot.HideAxesAndGrid(); // hide the Cartesian defaults
        }
    }
}
