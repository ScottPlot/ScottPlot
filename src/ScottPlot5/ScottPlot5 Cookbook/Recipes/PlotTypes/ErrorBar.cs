namespace ScottPlotCookbook.Recipes.PlotTypes;

internal class ErrorBar : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.PlotTypes,
        PageName = "Error Bars",
        PageDescription = "Error Bars communicate the range of possible values for a measurement",
    };

    internal class Quickstart : RecipeTestBase
    {
        public override string Name => "Error Bar Quickstart";
        public override string Description => "Error Bars go well with scatter plots.";

        [Test]
        public override void Recipe()
        {
            int points = 30;

            double[] xs = Generate.Consecutive(points);
            double[] ys = Generate.RandomWalk(points);
            double[] err = Generate.Random(points, 0.1, 1);

            var scatter = myPlot.Add.Scatter(xs, ys);
            var errorbars = myPlot.Add.ErrorBar(xs, ys, err);
            errorbars.Color = scatter.Color;
        }
    }

    internal class CustomErrors : RecipeTestBase
    {
        public override string Name => "ErrorBar Values";
        public override string Description => "Error size can be set for all dimensions.";

        [Test]
        public override void Recipe()
        {
            int points = 10;

            ScottPlot.RandomDataGenerator gen = new();

            double[] xs = Generate.Consecutive(points);
            double[] ys = Generate.RandomWalk(points);
            var scatter = myPlot.Add.Scatter(xs, ys);
            scatter.LineStyle.Width = 0;

            ScottPlot.Plottables.ErrorBar eb = new(
                xs: xs,
                ys: ys,
                xErrorsNegative: gen.RandomSample(points, .5),
                xErrorsPositive: gen.RandomSample(points, .5),
                yErrorsNegative: gen.RandomSample(points),
                yErrorsPositive: gen.RandomSample(points));

            eb.Color = scatter.Color;

            myPlot.Add.Plottable(eb);
        }
    }
}
