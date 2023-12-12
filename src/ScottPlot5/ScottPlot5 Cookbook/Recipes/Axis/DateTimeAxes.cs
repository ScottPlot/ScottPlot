namespace ScottPlotCookbook.Recipes.Axis;

internal class DateTimeAxes : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.Customization,
        PageName = "DateTime Axes",
        PageDescription = "Plot data values on a DataTime axes",
    };

    internal class Quickstart : RecipeBase
    {
        public override string Name => "DateTime Axis Quickstart";
        public override string Description => ".";

        [Test]
        public override void Execute()
        {
            // begin with an array of DateTime values
            DateTime[] dates = ScottPlot.Generate.DateTime.Days(100);

            // convert DateTime to OLE Automation (OADate) format
            double[] xs = dates.Select(x => x.ToOADate()).ToArray();
            double[] ys = ScottPlot.Generate.RandomWalk(xs.Length);
            myPlot.Add.Scatter(xs, ys);

            // tell the plot to display dates on the bottom axis
            myPlot.AxisStyler.DateTimeTicks(Edge.Bottom);
        }
    }
}
