namespace ScottPlotCookbook.Recipes.PlotTypes;

public class AxisLines : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Primitive Shapes";
    public string CategoryDescription => "Axis lines span an entire axis.";

    public class AxisLineQuickstart : RecipeBase
    {
        public override string Name => "Axis Lines";
        public override string Description => "Axis lines are vertical or horizontal lines that span an entire axis.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            myPlot.Add.VerticalLine(24);
            myPlot.Add.HorizontalLine(0.73);
        }
    }

    public class AxisLineLabel : RecipeBase
    {
        public override string Name => "Axis Line Label";
        public override string Description => "Axis lines have labels that can be used " +
            "to display arbitrary on the axes they are attached to.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            // by default labels are drawn on the same side as the axis label

            var axLine1 = myPlot.Add.VerticalLine(24);
            axLine1.Label.Text = "Line 1";

            var axLine2 = myPlot.Add.HorizontalLine(0.75);
            axLine2.Label.Text = "Line 2";

            // labels may be drawn on the side opposite of the axis label

            var axLine3 = myPlot.Add.VerticalLine(37);
            axLine3.Label.Text = "Line 3";
            axLine3.LabelOppositeAxis = true;

            var axLine4 = myPlot.Add.HorizontalLine(-.75);
            axLine4.Label.Text = "Line 4";
            axLine4.LabelOppositeAxis = true;
        }
    }

    public class AxisLineStyle : RecipeBase
    {
        public override string Name => "Axis Line Style";
        public override string Description => "Axis lines have extensive " +
            "line style customization options.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            var vl = myPlot.Add.VerticalLine(24);
            vl.LineWidth = 3;
            vl.Color = Colors.Magenta;

            var hl = myPlot.Add.HorizontalLine(0.75);
            hl.LineWidth = 2;
            hl.Color = Colors.Green;
            hl.LinePattern = LinePattern.Dashed;
        }
    }
}
