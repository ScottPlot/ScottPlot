namespace ScottPlotCookbook.Recipes.PlotTypes;

internal class Regression : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.Statistics,
        PageName = "Regression",
        PageDescription = "Statistical operations to fit lines to data",
    };

    internal class Linear : RecipeTestBase
    {
        public override string Name => "LinearRegression";
        public override string Description => "Fit a line to a collection of X/Y data points.";

        [Test]
        public override void Recipe()
        {
            double[] xs = new double[] { 1, 2, 3, 4, 5, 6, 7 };
            double[] ys = new double[] { 2, 2, 3, 3, 3.8, 4.2, 4 };

            // plot original data as a scatter plot
            var sp = myPlot.Add.Scatter(xs, ys);
            sp.LineStyle = LineStyle.None;
            sp.MarkerStyle.Size = 10;

            // calculate the regression line
            ScottPlot.Statistics.LinearRegression reg = new(xs, ys);

            // plot the regression line
            Coordinates pt1 = new(xs.First(), reg.GetValue(xs.First()));
            Coordinates pt2 = new(xs.Last(), reg.GetValue(xs.Last()));
            var line = myPlot.Add.Line(pt1, pt2);
            line.MarkerStyle = MarkerStyle.None;
            line.LineStyle.Pattern = LinePattern.Dash;
            line.LineStyle.Width = 2;

            // note the formula at the top of the plot
            myPlot.Title($"y = {reg.Slope:0.###}x + {reg.Offset:0.###} (r²={reg.Rsquared:0.###})");
        }
    }
}
