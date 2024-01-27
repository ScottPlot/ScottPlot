namespace ScottPlotCookbook.Recipes.PlotTypes;

public class AxisLines : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Axis Lines";
    public string CategoryDescription => "Axis lines indicate a position on an axis.";

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
            axLine1.Text = "Line 1";

            var axLine2 = myPlot.Add.HorizontalLine(0.75);
            axLine2.Text = "Line 2";

            // labels may be drawn on the side opposite of the axis label

            var axLine3 = myPlot.Add.VerticalLine(37);
            axLine3.Text = "Line 3";
            axLine3.LabelOppositeAxis = true;

            var axLine4 = myPlot.Add.HorizontalLine(-.75);
            axLine4.Text = "Line 4";
            axLine4.LabelOppositeAxis = true;
        }
    }

    public class AxisLineStyle : RecipeBase
    {
        public override string Name => "Axis Line Style";
        public override string Description => "Axis lines have extensive customization options.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            var vl1 = myPlot.Add.VerticalLine(24);
            vl1.LineWidth = 3;
            vl1.Color = Colors.Magenta;

            var hl1 = myPlot.Add.HorizontalLine(0.75);
            hl1.LineWidth = 2;
            hl1.Color = Colors.Green;
            hl1.LinePattern = LinePattern.Dashed;

            var hl2 = myPlot.Add.HorizontalLine(-.23);
            hl2.LineColor = Colors.Navy;
            hl2.LineWidth = 5;
            hl2.Text = "Hello";
            hl2.Label.FontSize = 24;
            hl2.Label.BackColor = Colors.Blue;
            hl2.Label.ForeColor = Colors.Yellow;
            hl2.LinePattern = LinePattern.DenselyDashed;
        }
    }
}
