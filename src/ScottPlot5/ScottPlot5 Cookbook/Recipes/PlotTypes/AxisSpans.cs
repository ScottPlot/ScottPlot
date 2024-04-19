namespace ScottPlotCookbook.Recipes.PlotTypes;

public class AxisSpans : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Axis Spans";
    public string CategoryDescription => "Axis spans indicate a range of an axis.";

    public class AxisSpanQuickstart : RecipeBase
    {
        public override string Name => "Axis Span Quickstart";
        public override string Description => "Axis spans label a range of an axis. " +
            "Vertical spans shade the full width of a vertical range, " +
            "and horizontal spans shade the full height of a horizontal range.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            var hSpan = myPlot.Add.HorizontalSpan(10, 20);
            var vSpan = myPlot.Add.VerticalSpan(0.25, 0.75);

            hSpan.LegendText = "Horizontal Span";
            vSpan.LegendText = "Vertical Span";
            myPlot.ShowLegend();
        }
    }

    public class AxisSpanStyling : RecipeBase
    {
        public override string Name => "Axis Span Styling";
        public override string Description => "Axis spans can be extensively customized.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            var hs = myPlot.Add.HorizontalSpan(10, 20);
            hs.LegendText = "Hello";
            hs.LineStyle.Width = 2;
            hs.LineStyle.Color = Colors.Magenta;
            hs.LineStyle.Pattern = LinePattern.Dashed;
            hs.FillStyle.Color = Colors.Magenta.WithAlpha(.2);
        }
    }
}
