namespace ScottPlotCookbook.Recipes.PlotTypes;

public class ArrowCoordinated : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Arrow";
    public string CategoryDescription => "Points to a location in coordinate space.";

    public class Quickstart : RecipeBase
    {
        public override string Name => "Arrow Quickstart";
        public override string Description => "Points to a location in coordinate space.";

        [Test]
        public override void Execute()
        {
            // plot some sample data
            double[] sin = Generate.Sin(51);
            double[] cos = Generate.Cos(51);
            myPlot.Add.Signal(sin);
            myPlot.Add.Signal(cos);

            // add arrows using coordinates
            myPlot.Add.Arrow(27, .2, 25, 0);

            // you can define a minimum length so the line persists even when zooming out
            var arrow2 = myPlot.Add.Arrow(23, -.5, 27, -.25);
            arrow2.Color = Colors.Red;
            arrow2.MinimumLength = 100;

            // the shape of the arrowhead can be adjusted
            var skinny = myPlot.Add.Arrow(12, .5, 12, 1);
            skinny.Color = Colors.Green;
            skinny.ArrowheadLength = 24;
            skinny.ArrowheadWidth = 8;

            var fat = myPlot.Add.Arrow(20, 1, 20, .6);
            fat.Color = Colors.Blue;
            fat.ArrowheadLength = 24;
            fat.ArrowheadWidth = 36;
            fat.LineWidth = 8;

            // offset backs the arrow away from the tip coordinate
            myPlot.Add.Marker(40, 0.3);
            var arrow4 = myPlot.Add.Arrow(35, 0.6, 40, 0.3);
            arrow4.Color = Colors.Fuchsia;
            arrow4.Offset = 10;
            arrow4.MinimumLength = 50;
        }
    }
}
