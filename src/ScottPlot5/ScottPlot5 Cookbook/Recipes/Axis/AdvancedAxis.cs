namespace ScottPlotCookbook.Recipes.Axis;

public class AdvancedAxis : ICategory
{
    public string Chapter => "Axis";
    public string CategoryName => "Advanced Axis Features";
    public string CategoryDescription => "How to further customize axes";

    public class InvertedAxis : RecipeBase
    {
        public override string Name => "Inverted Axis";
        public override string Description => "Users can display data on an inverted axis " +
            "by setting axis limits setting the lower edge to a value more positive than the upper edge.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            myPlot.Axes.SetLimitsY(bottom: 1.5, top: -1.5);
        }
    }

    public class InvertedAutoAxis : RecipeBase
    {
        public override string Name => "Inverted Auto-Axis";
        public override string Description => "Customize the logic for the " +
            "automatic axis scaler to ensure that axis limits " +
            "for a particular axis are always inverted when autoscaled.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            myPlot.Axes.AutoScaler.InvertedY = true;
        }
    }

    public class SquareAxisUnits : RecipeBase
    {
        public override string Name => "SquareAxisUnits";
        public override string Description => "Axis rules can be put in place which " +
            "force the vertical scale (units per pixel) to match the horizontal scale " +
            "so circles always appear as circles and not stretched ellipses.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Circle(0, 0, 10);

            // force pixels to have a 1:1 scale ratio
            myPlot.Axes.SquareUnits();

            // even if you try to "stretch" the axis, it will adjust the axis limits automatically
            myPlot.Axes.SetLimits(-10, 10, -20, 20);
        }
    }
}
