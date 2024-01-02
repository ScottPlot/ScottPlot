namespace ScottPlotCookbook.Recipes.Axis;

public class DateTimeAxes : ICategory
{
    public string Chapter => "Axis";
    public string CategoryName => "DateTime Axes";
    public string CategoryDescription => "Plot data values on a DataTime axes";

    public class DateTimeAxisQuickstart : RecipeBase
    {
        public override string Name => "DateTime Axis Quickstart";
        public override string Description => "Axis tick labels can be displayed using a time format.";

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
            myPlot.Axes.DateTimeTicks(Edge.Bottom);
        }
    }
}
