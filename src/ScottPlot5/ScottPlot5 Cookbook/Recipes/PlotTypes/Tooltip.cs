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

            Coordinates tip = new(25, ys[25]);
            Coordinates label = tip.WithDelta(4, .7);
            myPlot.Add.Tooltip(tip, "Special Point", label);
        }
    }

    public class TooltipFont : RecipeBase
    {
        public override string Name => "Tooltip Label Styling";
        public override string Description => "Tooltips fonts can be customized.";

        [Test]
        public override void Execute()
        {
            double[] ys = Generate.Sin(50);
            var plt = myPlot.Add.Signal(ys);
            plt.MaximumMarkerSize = 20;

            Coordinates tip = new(25, ys[25]);
            Coordinates label = tip.WithDelta(4, .7);
            var tooltip = myPlot.Add.Tooltip(tip, "Hello", label);
            tooltip.LabelFontName = "Comic Sans MS";
            tooltip.LabelFontSize = 36;
            tooltip.LabelBold = true;
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
            var plt = myPlot.Add.Signal(ys);
            plt.MaximumMarkerSize = 20;

            Coordinates tip = new(25, ys[25]);
            Coordinates label = tip.WithDelta(4, .7);
            var tooltip = myPlot.Add.Tooltip(tip, "Special Point", label);
            tooltip.FillColor = Colors.Blue;
            tooltip.LineColor = Colors.Navy;
            tooltip.LineWidth = 3;
            tooltip.LabelFontColor = Colors.Yellow;
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
            double[] widthFraction = [0.3, 0.5, 0.7, 1.0];

            for (int i = 0; i < widthFraction.Length; i++)
            {

                Coordinates tip = new(0, i * 2);
                Coordinates label = tip.WithDelta(2, 1);
                var tooltip = myPlot.Add.Tooltip(tip, $"Width={widthFraction[i]}", label);
                tooltip.Alignment = Alignment.MiddleCenter;
                tooltip.TailWidthPercentage = widthFraction[i];
            }

            myPlot.Axes.SetLimits(-3, 7, -1, 9);
        }
    }
}
