namespace ScottPlotCookbook.Recipes.Introduction;

public class AxisAndTicks : ICategory
{
    public string Chapter => "Introduction";
    public string CategoryName => "Axis and Ticks";
    public string CategoryDescription => "Examples of common customizations for axis labels and ticks";

    public class AxisLabels : RecipeBase
    {
        public override string Name => "Adding Axis Labels";
        public override string Description => "Axis labels are the text labels centered on each axis. " +
            "The text inside these labels can be changed, and the style of the text can be extensively customized.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            myPlot.Axes.Bottom.Label.Text = "Horizontal Axis";
            myPlot.Axes.Left.Label.Text = "Vertical Axis";
        }
    }

    public class SetAxisLimits : RecipeBase
    {
        public override string Name => "Manually Set Axis Limits";
        public override string Description => "Axis Limits can be set manually in different ways.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            // Interact with a specific axis
            myPlot.Axes.Bottom.Min = -100;
            myPlot.Axes.Bottom.Max = 150;
            myPlot.Axes.Left.Min = -5;
            myPlot.Axes.Left.Max = 5;

            // Call a helper function
            myPlot.Axes.SetLimits(-100, 150, -5, 5);
        }
    }

    public class GetAxisLimits : RecipeBase
    {
        public override string Name => "Read Axis Limits";
        public override string Description => "The current axis limits can be read in multiple ways.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            // Interact with a specific axis
            double top = myPlot.Axes.Left.Max;
            double bottom = myPlot.Axes.Left.Min;

            // Call a helper function
            AxisLimits limits = myPlot.Axes.GetLimits();
            double left = limits.Rect.Left;
            double center = limits.Rect.HorizontalCenter;
        }
    }

    public class AutoScale : RecipeBase
    {
        public override string Name => "Zoom to Fit Data";
        public override string Description => "The axis limits can be automatically adjusted to fit the data. " +
            "Optional arguments allow users to define the amount of whitespace around the edges of the data.";

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
            myPlot.FigureBackground = Colors.Magenta; // should not be seen
            myPlot.DataBackground = Colors.WhiteSmoke;

            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            myPlot.Layout.Frameless();
        }
    }
}
