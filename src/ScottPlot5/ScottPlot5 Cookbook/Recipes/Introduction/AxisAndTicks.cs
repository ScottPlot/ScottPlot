namespace ScottPlotCookbook.Recipes.Introduction;

public class AxisAndTicks : ICategory
{
    public string Chapter => "Introduction";
    public string CategoryName => "Axis and Ticks";
    public string CategoryDescription => "Examples of common customizations for axis labels and ticks";

    public class SetAxisLimits : RecipeBase
    {
        public override string Name => "Set Axis Limits";
        public override string Description => "Axis Limits can be set by the user.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            myPlot.Axes.SetLimits(-100, 150, -5, 5);
        }
    }

    public class GetAxisLimits : RecipeBase
    {
        public override string Name => "Read Axis Limits";
        public override string Description => "Use GetLimits() to obtain the current axis limits.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            AxisLimits limits = myPlot.Axes.GetLimits();
            double xMin = limits.Left;
            double xMax = limits.Right;
            double yMin = limits.Bottom;
            double yMax = limits.Top;
        }
    }

    public class AutoScale : RecipeBase
    {
        public override string Name => "AutoScale Axis Limits to Fit Data";
        public override string Description => "The axis limits can be automatically adjusted to fit the data. " +
            "Optional arguments allow users to define the amount of whitespace around the edges of the data." +
            "In older versions of ScottPlot this functionality was achieved by a method named AxisAuto().";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            // set limits that do not fit the data
            myPlot.Axes.SetLimits(-100, 150, -5, 5);

            // reset limits to fit the data
            myPlot.Axes.AutoScale();
        }
    }

    public class Frameless : RecipeBase
    {
        public override string Name => "Frameless Plot";
        public override string Description => "How to create a plot containig only the data area and no axes.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            // make the data area cover the full figure
            myPlot.Layout.Frameless();

            // set the data area background so we can observe its size
            myPlot.DataBackground.Color = Colors.WhiteSmoke;
        }
    }
}
