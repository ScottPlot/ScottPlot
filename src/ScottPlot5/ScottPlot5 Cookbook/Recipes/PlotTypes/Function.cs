namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Function : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Function";
    public string CategoryDescription => "Function plots are a type of line plot where Y positions " +
        "are defined by a function that depends on X rather than a collection of discrete data points.";

    public class FunctionQuickstart : RecipeBase
    {
        public override string Name => "Function Quickstart";
        public override string Description => "Create a function plot from a formula.";

        [Test]
        public override void Execute()
        {
            // Functions are defined as delegates with an input and output
            static double func1(double x) => (Math.Sin(x) * Math.Sin(x / 2));
            static double func2(double x) => (Math.Sin(x) * Math.Sin(x / 3));
            static double func3(double x) => (Math.Cos(x) * Math.Sin(x / 5));

            // Add functions to the plot
            myPlot.Add.Function(func1);
            myPlot.Add.Function(func2);
            myPlot.Add.Function(func3);

            // Manually set axis limits because functions do not have discrete data points
            myPlot.Axes.SetLimits(-10, 10, -1.5, 1.5);
        }
    }

    public class FunctionLimitX : RecipeBase
    {
        public override string Name => "Function Limit X";
        public override string Description => "A function can be limited to a range of X values.";

        [Test]
        public override void Execute()
        {
            var func = new Func<double, double>((x) => Math.Sin(x) * Math.Sin(x / 2));

            var f = myPlot.Add.Function(func);
            f.MinX = -3;
            f.MaxX = 3;

            myPlot.Axes.SetLimits(-5, 5, -.2, 1.0);
        }
    }
}
