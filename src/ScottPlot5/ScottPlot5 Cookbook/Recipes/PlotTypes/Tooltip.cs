namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Tooltip : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
    public string CategoryName => "Tooltip";
    public string CategoryDescription => "Tooltips are annotations that point to an X/Y coordinate on the plot.";

    public class TooltipQuickstart : RecipeBase
    {
        public override string Name => "Tooltip Quickstart";
        public override string Description => "Tooltips are annotations that point to an X/Y coordinate on the plot.";

        [Test]
        public override void Execute()
        {
            double[] ys = Generate.Sin(50);
            var plt = myPlot.Add.Signal(ys);
            plt.MaximumMarkerSize = 20;

            myPlot.Add.Tooltip("Special Point", 19, ys[17], 17, ys[17]);
        }
    }

    public class TooltipFont : RecipeBase
    {
        public override string Name => "Tooltip Font";
        public override string Description => "Tooltips fonts can be customized.";

        [Test]
        public override void Execute()
        {
            double[] ys = Generate.Sin(50);
            var signal = myPlot.Add.Signal(ys);
            signal.MaximumMarkerSize = 20;

            var tt1 = myPlot.Add.Tooltip("Top", 14, ys[12], 12, ys[12]);
            tt1.LabelFontColor = Colors.Magenta;
            tt1.LabelFontSize = 24;

            var tt2 = myPlot.Add.Tooltip("Negative Slope", 27, ys[25], 25, ys[25]);
            tt2.LabelFontName = "Comic Sans MS";
            tt2.LabelStyle.Bold = true;
        }
    }

    public class TooltipColors : RecipeBase
    {
        public override string Name => "Tooltip Colors";
        public override string Description => "Tooltips border and fill styles can be customized.";

        [Test]
        public override void Execute()
        {
            double[] ys = Generate.Sin(50);
            var signal = myPlot.Add.Signal(ys);
            signal.MaximumMarkerSize = 20;

            var tooltip = myPlot.Add.Tooltip("This point has\na negative slope", 22, 0.8, 25, ys[25]);
            tooltip.LabelFontSize = 24;
            tooltip.LabelFontColor = Colors.White;
            tooltip.FillColor = Colors.Blue;
            tooltip.LineWidth = 5;
            tooltip.LineColor = Colors.Navy;
        }
    }

    public class TooltipTailWidth : RecipeBase
    {
        public override string Name => "Tooltip Tail Width";
        public override string Description => "Customizable tooltip tail width percentage. " +
            "The actual width of the tail is the lesser of the length or width of the tooltip body.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Rectangle(-10, 10, -10, 10);

            var top = myPlot.Add.Tooltip("Top(0.3)", 0, 3, 0, 10);
            top.Alignment = Alignment.MiddleCenter;
            top.TailWidthPercentage = 0.3F;

            var bottom = myPlot.Add.Tooltip("Bottom(0.8)", 0, -3, 0, -10);
            bottom.Alignment = Alignment.MiddleCenter;
            bottom.TailWidthPercentage = 0.8F;

            var left = myPlot.Add.Tooltip("Left(0.3)", -3, 0, -10, 0);
            left.Alignment = Alignment.MiddleCenter;
            left.TailWidthPercentage = 0.3F;

            var right = myPlot.Add.Tooltip("Right(0.8)", 3, 0, 10, 0);
            right.Alignment = Alignment.MiddleCenter;
            right.TailWidthPercentage = 0.8F;
        }
    }
}
