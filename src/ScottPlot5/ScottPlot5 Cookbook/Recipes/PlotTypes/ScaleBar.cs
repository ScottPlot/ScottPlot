namespace ScottPlotCookbook.Recipes.PlotTypes;

public class ScaleBar : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
    public string CategoryName => "Scale Bar";
    public string CategoryDescription => "Scalebars display a horizontal and/or vertical range using a line segment and may be used " +
        "to convey axis scale as a minimal alternative to using axis frames, ticks, and tick labels.";

    public class ScaleBarQuickstart : RecipeBase
    {
        public override string Name => "ScaleBar Quickstart";
        public override string Description => "A ScaleBar can be added to a plot to convey scale information " +
            "allowing axis frames, ticks, and labels to be hidden.";

        [Test]
        public override void Execute()
        {
            // plot sample data
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            // add a scale bar
            myPlot.Add.ScaleBar(5, 0.25);

            // disable the grid and axis labels
            myPlot.HideGrid();
            myPlot.Axes.Frameless();
        }
    }

    public class ScaleBarLabels : RecipeBase
    {
        public override string Name => "ScaleBar Labels";
        public override string Description => "Text may be added to each dimension of an L shaped scalebar";

        [Test]
        public override void Execute()
        {
            // plot sample data
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            // add a scale bar
            var scalebar = myPlot.Add.ScaleBar(5, 0.25);
            scalebar.XLabel = "5 mV";
            scalebar.YLabel = "1 µF";

            // disable the grid and axis labels
            myPlot.HideGrid();
            myPlot.Axes.Frameless();
        }
    }

    public class ScaleBarStyling : RecipeBase
    {
        public override string Name => "ScaleBar Styling";
        public override string Description => "The ScaleBar has many properties which may be customized ";

        [Test]
        public override void Execute()
        {
            // plot sample data
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            // add a scale bar
            var scalebar = myPlot.Add.ScaleBar(5, 0.25);
            scalebar.LineWidth = 5;
            scalebar.LineColor = Colors.Magenta;

            // disable the grid and axis labels
            myPlot.HideGrid();
            myPlot.Axes.Frameless();
        }
    }

    public class ScaleBarSingleDimension : RecipeBase
    {
        public override string Name => "ScaleBar Single Dimension";
        public override string Description => "Set Width or Height to 0 to cause the ScaleBar to use a single axis only";

        [Test]
        public override void Execute()
        {
            // plot sample data
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            // add a scale bar
            var scalebar = myPlot.Add.ScaleBar(7, 0);
            scalebar.LineWidth = 2;
            scalebar.XLabel = "70 ms";
            scalebar.XLabelStyle.Bold = true;
            scalebar.LabelPadding = new(0);

            // disable the grid and axis labels
            myPlot.HideGrid();
            myPlot.Axes.Frameless();
        }
    }
}

