namespace ScottPlotCookbook.Recipes.PlotTypes;

internal class Text : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.PlotTypes,
        PageName = "Text",
        PageDescription = "Text lables placed on the plot in coordinate space",
    };

    internal class Quickstart : RecipeTestBase
    {
        public override string Name => "Text Quickstart";
        public override string Description => "Text can be placed anywhere in coordinate space.";

        [Test]
        public override void Recipe()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());
            myPlot.Add.Text("Hello, World", 25, 0.5);
        }
    }

    internal class Formatting : RecipeTestBase
    {
        public override string Name => "Text Formatting";
        public override string Description => "Text formatting can be extensively customized.";

        [Test]
        public override void Recipe()
        {
            var text = myPlot.Add.Text("Hello, World", 42, 69);
            text.Label.FontSize = 26;
            text.Label.Bold = true;
            text.Label.Rotation = -45;
            text.Label.ForeColor = Colors.Yellow;
            text.Label.BackColor = Colors.Navy.WithAlpha(.5);
            text.Label.BorderColor = Colors.Magenta;
            text.Label.BorderWidth = 3;
            text.Label.Padding = 10;
            text.Label.Alignment = Alignment.MiddleCenter;
        }
    }
}
