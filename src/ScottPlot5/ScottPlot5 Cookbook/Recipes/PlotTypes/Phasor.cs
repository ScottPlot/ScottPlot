namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Phasor : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Phasor Plot";
    public string CategoryDescription => "Phasor plots display vectors on a radial axis centered at the origin";

    public class PhasorQuickstart : RecipeBase
    {
        public override string Name => "Phasor Line Plot";
        public override string Description => "A phasor line plot contains a collection of polar coordinates " +
            "which are rendered as arrows.";

        [Test]
        public override void Execute()
        {
            // these are the points we will display
            PolarCoordinates[] points = [
                new (10, Angle.FromDegrees(15)),
                new (20, Angle.FromDegrees(120)),
                new (30, Angle.FromDegrees(240)),
            ];

            // start by adding a polar axis to the plot
            var polarAxis = myPlot.Add.PolarAxis(30);
            polarAxis.LinePattern = LinePattern.Dotted;

            // then add phasor lines on top of the polar axis
            myPlot.Add.PhasorLines(points);
        }
    }
}
