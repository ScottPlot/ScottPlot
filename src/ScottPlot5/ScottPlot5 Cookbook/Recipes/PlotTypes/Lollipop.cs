namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Lollipop : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
    public string CategoryName => "Lollipop Plot";
    public string CategoryDescription => "A lollipop chart is a variation of a bar chart that uses a line (stem) " +
        "extending from a baseline to a marker (head) to represent data points. " +
        "Lollipop highlight individual data points with less visual clutter than to traditional bar charts.";

    public class LollipopQuickStart : RecipeBase
    {
        public override string Name => "Lollipop Plot Quickstart";
        public override string Description => "Lollipop plots can be created from a sequence of values";

        [Test]
        public override void Execute()
        {
            double[] values = Generate.Sin(25);
            myPlot.Add.Lollipop(values);
        }
    }

    public class LollipopPositions : RecipeBase
    {
        public override string Name => "Lollipop Positions";
        public override string Description => "The position of each lollipop may be defined.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Range(0, 6.28, 0.314);
            double[] ys = xs.Select(Math.Sin).ToArray();
            var lollipop = myPlot.Add.Lollipop(ys, xs);
        }
    }

    public class BarLollipopCustom : RecipeBase
    {
        public override string Name => "Lollipop Plot Customizations";
        public override string Description => "The stem line and head marker can be extensively customized.";

        [Test]
        public override void Execute()
        {
            double[] values = Generate.Sin(21);
            var lollipop = myPlot.Add.Lollipop(values);

            lollipop.MarkerColor = Colors.Red;
            lollipop.MarkerSize = 15;
            lollipop.MarkerShape = MarkerShape.FilledDiamond;

            lollipop.LineColor = Colors.Green;
            lollipop.LineWidth = 3;
            lollipop.LinePattern = LinePattern.Dotted;
        }
    }

    public class LollipopHorizontal : RecipeBase
    {
        public override string Name => "Horizontal Lollipop Plot";
        public override string Description => "Change the lollipop plot's Orientation to Horizontal " +
            "to cause stems to be drawn horizontally instead of vertically.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Sin(21);
            double[] ys = Generate.Consecutive(21);
            Coordinates[] coordinates = Coordinates.Zip(xs, ys);

            var lollipop = myPlot.Add.Lollipop(coordinates);
            lollipop.Orientation = Orientation.Horizontal;
        }
    }
}
