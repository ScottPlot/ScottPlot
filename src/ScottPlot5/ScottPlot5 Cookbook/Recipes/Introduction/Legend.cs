namespace ScottPlotCookbook.Recipes.Introduction;

public class Legend : ICategory
{
    public string Chapter => "Introduction";
    public string CategoryName => "Configuring Legends";
    public string CategoryDescription => "A legend is a key typically displayed in the corner of a plot";

    public class LegendQuickstart : RecipeBase
    {
        public override string Name => "Legend Quickstart";
        public override string Description => "Many plottables have a Label property " +
            "that can be set so they appear in the legend.";

        [Test]
        public override void Execute()
        {
            var sig1 = myPlot.Add.Signal(Generate.Sin(51));
            sig1.Label = "Sin";

            var sig2 = myPlot.Add.Signal(Generate.Cos(51));
            sig2.Label = "Cos";

            myPlot.ShowLegend();
        }
    }

    public class ManualLegend : RecipeBase
    {
        public override string Name => "Manual Legend Items";
        public override string Description => "Legends may be constructed manually.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));
            myPlot.Legend.IsVisible = true;

            LegendItem item1 = new()
            {
                LineColor = Colors.Magenta,
                MarkerColor = Colors.Magenta,
                LineWidth = 2,
                Label = "Alpha"
            };

            LegendItem item2 = new()
            {
                LineColor = Colors.Green,
                MarkerColor = Colors.Green,
                LineWidth = 4,
                Label = "Beta"
            };

            LegendItem[] items = { item1, item2 };
            myPlot.ShowLegend(items);
        }
    }

    public class LegendStyle : RecipeBase
    {
        public override string Name => "Legend Customization";
        public override string Description => "Access the Legend object directly " +
            "for advanced customization options.";

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
            myPlot.Legend.Location = Alignment.UpperCenter;
        }
    }

    public class LegendOrientation : RecipeBase
    {
        public override string Name => "Legend Orientation";
        public override string Description => "Legend items may be arranged horizontally instead of vertically";

        [Test]
        public override void Execute()
        {
            var sig1 = myPlot.Add.Signal(Generate.Sin(51, phase: .2));
            var sig2 = myPlot.Add.Signal(Generate.Sin(51, phase: .4));
            var sig3 = myPlot.Add.Signal(Generate.Sin(51, phase: .6));

            sig1.Label = "Signal 1";
            sig2.Label = "Signal 2";
            sig3.Label = "Signal 3";

            myPlot.Legend.IsVisible = true;
            myPlot.Legend.Orientation = Orientation.Horizontal;
        }
    }

    public class LegendWrapping : RecipeBase
    {
        public override string Name => "Legend Wrapping";
        public override string Description => "Legend items may wrap to improve display for a large number of items";

        [Test]
        public override void Execute()
        {
            for (int i = 1; i <= 10; i++)
            {
                var sig = myPlot.Add.Signal(Generate.Sin(51, phase: .02 * i));
                sig.Label = $"Signal #{i}";
            }

            myPlot.Legend.IsVisible = true;
            myPlot.Legend.Orientation = Orientation.Horizontal;
            myPlot.Legend.AllowMultiline = true;
        }
    }
}
