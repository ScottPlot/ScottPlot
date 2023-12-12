namespace ScottPlotCookbook.Recipes.Introduction;

internal class AxisAndTicks : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.Customization,
        PageName = "Axis and Ticks",
        PageDescription = "Examples of common customizations for axis labels and ticks",
    };

    internal class AxisLabels : RecipeBase
    {
        public override string Name => "Adding Axis Labels";
        public override string Description => "Axis labels are the text labels centered on each axis. " +
            "The text inside these labels can be changed, and the style of the text can be extensively customized.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(ScottPlot.Generate.Sin(51));
            myPlot.Add.Signal(ScottPlot.Generate.Cos(51));

            myPlot.XAxis.Label.Text = "Horizontal Axis";
            myPlot.YAxis.Label.Text = "Vertical Axis";
        }
    }

    internal class SetAxisLimits : RecipeBase
    {
        public override string Name => "Manually Set Axis Limits";
        public override string Description => "Axis Limits can be set manually in different ways.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(ScottPlot.Generate.Sin(51));
            myPlot.Add.Signal(ScottPlot.Generate.Cos(51));

            // Interact with a specific axis
            myPlot.XAxis.Min = -100;
            myPlot.XAxis.Max = 150;
            myPlot.YAxis.Min = -5;
            myPlot.YAxis.Max = 5;

            // Call a helper function
            myPlot.SetAxisLimits(-100, 150, -5, 5);
        }
    }

    internal class GetAxisLimits : RecipeBase
    {
        public override string Name => "Read Axis Limits";
        public override string Description => "The current axis limits can be read in multiple ways.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(ScottPlot.Generate.Sin(51));
            myPlot.Add.Signal(ScottPlot.Generate.Cos(51));

            // Interact with a specific axis
            double top = myPlot.YAxis.Max;
            double bottom = myPlot.YAxis.Min;

            // Call a helper function
            AxisLimits limits = myPlot.GetAxisLimits();
            double left = limits.Rect.Left;
            double center = limits.Rect.HorizontalCenter;
        }
    }

    internal class AutoScale : RecipeBase
    {
        public override string Name => "Zoom to Fit Data";
        public override string Description => "The axis limits can be automatically adjusted to fit the data. " +
            "Optional arguments allow users to define the amount of whitespace around the edges of the data.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(ScottPlot.Generate.Sin(51));
            myPlot.Add.Signal(ScottPlot.Generate.Cos(51));

            // set limits that do not fit the data
            myPlot.SetAxisLimits(-100, 150, -5, 5);

            // reset limits to fit the data
            myPlot.AutoScale();
        }
    }

    internal class Frameless : RecipeBase
    {
        public override string Name => "Frameless Plot";
        public override string Description => "How to create a plot containig only the data area and no axes.";

        [Test]
        public override void Execute()
        {
            myPlot.FigureBackground = Colors.Magenta; // should not be seen
            myPlot.DataBackground = Colors.WhiteSmoke;

            myPlot.Add.Signal(ScottPlot.Generate.Sin(51));
            myPlot.Add.Signal(ScottPlot.Generate.Cos(51));

            myPlot.XAxes.ForEach(x => x.IsVisible = false);
            myPlot.YAxes.ForEach(x => x.IsVisible = false);
            myPlot.TitlePanel.IsVisible = false;
        }
    }
}
