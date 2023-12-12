namespace ScottPlotCookbook.Recipes.Introduction;

internal class Legend : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.Customization,
        PageName = "Configuring Legends",
        PageDescription = "A legend is a key typically displayed in the corner of a plot",
    };

    internal class LegendStyle : RecipeBase
    {
        public override string Name => "Legend Customization";
        public override string Description => "The default legend can be easily accessed and customized. " +
            "It is possible to add multiple legends, including custom ones implementing ILegend.";

        [Test]
        public override void Execute()
        {
            var sig1 = myPlot.Add.Signal(Generate.Sin(51));
            sig1.Label = "Sin";

            var sig2 = myPlot.Add.Signal(Generate.Cos(51));
            sig2.Label = "Cos";

            myPlot.Legend.IsVisible = true;
            myPlot.Legend.OutlineStyle.Color = Colors.Navy;
            myPlot.Legend.OutlineStyle.Width = 2;
            myPlot.Legend.BackgroundFill.Color = Colors.LightBlue;
            myPlot.Legend.ShadowFill.Color = Colors.Blue.WithOpacity(.5);
            myPlot.Legend.Font.Size = 16;
            myPlot.Legend.Font.Name = Fonts.Serif;
            myPlot.Legend.Alignment = Alignment.UpperCenter;
        }
    }

    internal class ManualLegend : RecipeBase
    {
        public override string Name => "Manual Legend";
        public override string Description => "Legends may be constructed manually.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            myPlot.Legend.IsVisible = true;

            myPlot.Legend.ManualItems.Add(new LegendItem()
            {
                LineColor = Colors.Magenta,
                LineWidth = 2,
                Label = "Alpha"
            });

            myPlot.Legend.ManualItems.Add(new LegendItem()
            {
                LineColor = Colors.Green,
                LineWidth = 4,
                Label = "Beta"
            });
        }
    }
}
