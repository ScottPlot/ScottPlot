namespace ScottPlotCookbook.Recipes.PlotTypes;

public class ArrowCoordinated : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Arrow";
    public string CategoryDescription => "Arrows point to a location in coordinate space.";

    public class ArrowQuickstart : RecipeBase
    {
        public override string Name => "Arrow Quickstart";
        public override string Description => "Arrows can be placed on plots to point to a " +
            "location in coordinate space and extensively customized.";

        [Test]
        public override void Execute()
        {
            // create a line
            Coordinates arrowTip = new(0, 0);
            Coordinates arrowBase = new(1, 1);
            CoordinateLine arrowLine = new(arrowBase, arrowTip);

            // add a simple arrow
            myPlot.Add.Arrow(arrowLine);

            // arrow line and fill styles can be customized
            var arrow2 = myPlot.Add.Arrow(arrowLine.WithDelta(1, 0));
            arrow2.ArrowLineColor = Colors.Red;
            arrow2.ArrowMinimumLength = 100;
            arrow2.ArrowLineColor = Colors.Black;
            arrow2.ArrowFillColor = Colors.Transparent;

            // the shape of the arrowhead can be adjusted
            var skinny = myPlot.Add.Arrow(arrowLine.WithDelta(2, 0));
            skinny.ArrowFillColor = Colors.Green;
            skinny.ArrowLineWidth = 0;
            skinny.ArrowWidth = 3;
            skinny.ArrowheadLength = 20;
            skinny.ArrowheadAxisLength = 20;
            skinny.ArrowheadWidth = 7;

            var fat = myPlot.Add.Arrow(arrowLine.WithDelta(3, 0));
            fat.ArrowFillColor = Colors.Blue;
            fat.ArrowLineWidth = 0;
            fat.ArrowWidth = 18;
            fat.ArrowheadLength = 20;
            fat.ArrowheadAxisLength = 20;
            fat.ArrowheadWidth = 30;

            // offset backs the arrow away from the tip coordinate
            myPlot.Add.Marker(arrowLine.WithDelta(4, 0).End);
            var arrow4 = myPlot.Add.Arrow(arrowLine.WithDelta(4, 0));
            arrow4.ArrowOffset = 15;

            myPlot.Axes.SetLimits(-1, 6, -1, 2);
        }
    }
}
