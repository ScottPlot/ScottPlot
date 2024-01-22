namespace ScottPlotCookbook.Recipes.PlotTypes;

public class ErrorBar : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Error Bars";
    public string CategoryDescription => "Error Bars communicate the range of possible values for a measurement";

    public class ErrorBarQuickstart : RecipeBase
    {
        public override string Name => "Error Bar Quickstart";
        public override string Description => "Error Bars go well with scatter plots.";

        [Test]
        public override void Execute()
        {
            int points = 30;

            double[] xs = Generate.Consecutive(points);
            double[] ys = Generate.RandomWalk(points);
            double[] err = Generate.RandomSample(points, 0.1, 1);

            var scatter = myPlot.Add.Scatter(xs, ys);
            var errorbars = myPlot.Add.ErrorBar(xs, ys, err);
            errorbars.Color = scatter.Color;
        }
    }

    public class CustomErrors : RecipeBase
    {
        public override string Name => "ErrorBar Values";
        public override string Description => "Error size can be set for all dimensions.";

        [Test]
        public override void Execute()
        {
            int points = 10;

            double[] xs = Generate.Consecutive(points);
            double[] ys = Generate.RandomWalk(points);
            var scatter = myPlot.Add.Scatter(xs, ys);
            scatter.LineStyle.Width = 0;

            ScottPlot.Plottables.ErrorBar eb = new(
                xs: xs,
                ys: ys,
                xErrorsNegative: Generate.RandomSample(points, .5),
                xErrorsPositive: Generate.RandomSample(points, .5),
                yErrorsNegative: Generate.RandomSample(points),
                yErrorsPositive: Generate.RandomSample(points));

            eb.Color = scatter.Color;

            myPlot.Add.Plottable(eb);
        }
    }
}
