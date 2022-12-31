namespace ScottPlotCookbook.Recipes.Introduction;

internal class Legend : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.Customization,
        PageName = "Configuring Legends",
        PageDescription = "A legend is a key typically displayed in the corner of a plot",
    };

    internal class LegendStyle : RecipeTestBase
    {
        public override string Name => "Legend Customization";
        public override string Description => "The default legend can be easily accessed and customized. " +
            "It is possible to add multiple legends, including custom ones implementing ILegend.";

        [Test]
        public override void Recipe()
        {
            var sig1 = myPlot.Add.Signal(Generate.Sin(51));
            sig1.Label = "Sin";

            var sig2 = myPlot.Add.Signal(Generate.Cos(51));
            sig2.Label = "Cos";

            var legend = myPlot.GetLegend();
            legend.OutlineStyle.Color = Colors.Navy;
            legend.OutlineStyle.Width = 2;
            legend.BackgroundFill.Color = Colors.LightBlue;
            legend.ShadowFill.Color = Colors.Blue.WithOpacity(.5);
            legend.Font.Size = 16;
            legend.Font.Name = FontService.SerifFontName;
            legend.Alignment = Alignment.UpperCenter;
        }
    }

    internal class ManualLegend : RecipeTestBase
    {
        public override string Name => "Manual Legend";
        public override string Description => "Legends may be constructed manually.";

        [Test]
        public override void Recipe()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            LegendItem item1 = new();
            item1.Line.Color = Colors.Magenta;
            item1.Line.Width = 2;
            item1.Label = "Alpha";

            LegendItem item2 = new();
            item2.Line.Color = Colors.Green;
            item2.Line.Width = 4;
            item2.Label = "Beta";

            var legend = myPlot.GetLegend();
            legend.ManualLegendItems = new[] { item1, item2 };
        }
    }
}
