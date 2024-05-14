namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Crosshair : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Crosshair";

    public string CategoryDescription => "A Crosshair combines a horizontal " +
        "axis line and vertical axis line to mark a location in coordinate space.";

    public class CrosshairQuickstart : RecipeBase
    {
        public override string Name => "Crosshair Quickstart";
        public override string Description => "A Crosshair combines a horizontal " +
        "axis line and vertical axis line to mark a location in coordinate space.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            myPlot.Add.Crosshair(13, .25);
        }
    }

    public class CrosshairCustomization : RecipeBase
    {
        public override string Name => "Crosshair Customization";
        public override string Description => "Crosshairs can be extensively customized.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            var cross = myPlot.Add.Crosshair(13, .25);

            // properties set style for both lines
            cross.LineWidth = 2;
            cross.LineColor = Colors.Magenta;

            // each line's styles can be individually accessed as well
            cross.HorizontalLine.LinePattern = LinePattern.Dotted;
        }
    }
}
