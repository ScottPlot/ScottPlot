namespace ScottPlotCookbook.Recipes.Axis;

internal class DateTimeAxes : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.Customization,
        PageName = "DateTime Axes",
        PageDescription = "Plot data values on a DataTime axes",
    };

    internal class Quickstart : RecipeTestBase
    {
        public override string Name => "DateTime Axis Quickstart";
        public override string Description => ".";

        [Test]
        public override void Recipe()
        {
            // begin with an array of DateTime values
            DateTime[] dates = Generate.DateTime.Days(100);

            // convert DateTime to OLE Automation (OADate) format
            double[] xs = dates.Select(x => x.ToOADate()).ToArray();
            double[] ys = Generate.RandomWalk(xs.Length);
            myPlot.Add.Scatter(xs, ys);

            // tell the plot to display dates on the bottom axis
            myPlot.Axes.DateTimeTicks(Edge.Bottom);
        }
    }
}
