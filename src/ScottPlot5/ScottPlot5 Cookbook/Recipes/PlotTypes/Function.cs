namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Function : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
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
            static double func1(double x) => (Math.Sin(x) * Math.Sin(x / 2));

            var f = myPlot.Add.Function(func1);
            f.MinX = -3;
            f.MaxX = 3;

            myPlot.Axes.SetLimits(-5, 5, -.2, 1.0);
        }
    }

    public class FunctionDynamic : RecipeBase
    {
        public override string Name => "Dynamically Generated Functions";
        public override string Description => "When a function cannot be represented " +
            "as a static method (e.g., one that requires custom parameters) it can be " +
            "represented as variable of type Func<double, double> and plotted accordingly.";

        [Test]
        public override void Execute()
        {
            static double LogNormalDist(double x, double a, double b)
            {
                double expNum = Math.Log(x / a);
                double exp = Math.Exp(-(expNum * expNum) / (2 * b * b));
                double y = Math.Sqrt(2 * Math.PI) * b * x * exp;
                return double.IsNaN(y) ? 0 : y;
            }

            double[] testValues = Generate.Range(0.8, 1.2, 0.05);
            Color[] colors = new ScottPlot.Colormaps.MellowRainbow().GetColors(testValues.Length);
            for (int i = 0; i < testValues.Length; i++)
            {
                double testValue = testValues[i];
                var myFunc = new Func<double, double>((x) => LogNormalDist(x, testValue, 0.5));
                var funcPlot = myPlot.Add.Function(myFunc);
                funcPlot.LegendText = $"{testValue:0.00}";
                funcPlot.LineWidth = 2;
                funcPlot.LineColor = colors[i];
            }

            myPlot.ShowLegend();
            myPlot.Legend.Orientation = Orientation.Horizontal;

            myPlot.Axes.SetLimitsX(-0.5, 4);
            myPlot.Axes.SetLimitsY(-0.8, 2);
        }
    }
}
