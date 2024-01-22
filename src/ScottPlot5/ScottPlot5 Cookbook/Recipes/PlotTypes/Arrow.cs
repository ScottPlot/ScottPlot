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
            ScottPlot.Plot myPlot = new();

            // plot some sample data
            double[] sin = Generate.Sin(51);
            double[] cos = Generate.Cos(51);
            myPlot.Add.Signal(sin);
            myPlot.Add.Signal(cos);

            // add arrows using coordinates
            myPlot.Add.Arrow(27, .2, 25, 0);

            // you can define a minimum length so the line persists even when zooming out
            var arrow2 = myPlot.Add.Arrow(23, -.5, 27, -.25);
            arrow2.SetColor(ScottPlot.Colors.Red);
            arrow2.MinimumLengthPixels = 100;

            // the shape of the arrowhead can be adjusted
            var skinny = myPlot.Add.Arrow(12, .5, 12, 1);
            skinny.SetColor(ScottPlot.Colors.Green);
            skinny.ArrowheadLengthPixels = 24;
            skinny.ArrowheadWidthPixels = 8;

            var fat = myPlot.Add.Arrow(20, 1, 20, .6);
            fat.SetColor(ScottPlot.Colors.Blue);
            fat.ArrowheadLengthPixels = 24;
            fat.ArrowheadWidthPixels = 36;
            fat.LineStyle.Width = 8;

            // a marker can be drawn at the base of the arrow
            var arrow3 = myPlot.Add.Arrow(35, -.4, 30, -.58);
            arrow3.MarkerStyle.IsVisible = true;
            arrow3.MarkerStyle.Size = 15;

            // offset
            var arrow4 = myPlot.Add.Arrow(35, 0.6, 40, 0.3);
            arrow4.SetColor(ScottPlot.Colors.Fuchsia);
            arrow4.OffsetPixels = 10;
            arrow4.MinimumLengthPixels = 50;

            myPlot.SavePng("arrow_quickstart.png", 400, 300);
        }
    }
}
