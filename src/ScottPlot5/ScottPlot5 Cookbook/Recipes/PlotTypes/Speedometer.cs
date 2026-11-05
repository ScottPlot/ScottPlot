namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Speedometer : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
    public string CategoryName => "Speedometer";
    public string CategoryDescription => "A speedometer shows a single value with a needle on a scale of colored sections.";

    public class SpeedometerQuickstart : RecipeBase
    {
        public override string Name => "Speedometer Quickstart";
        public override string Description => "A speedometer can be created from a value and a scale. " +
            "The bar beneath the dial shows the value and a label on the color of the section the needle points into.";

        [Test]
        public override void Execute()
        {
            var speedometer = myPlot.Add.Speedometer(value: 72, min: 0, max: 100);
            speedometer.Label = "km/h";
        }
    }

    public class SpeedometerColors : RecipeBase
    {
        public override string Name => "Speedometer Colors";
        public override string Description => "The number of colors sets the number of sections. " +
            "Colors may be taken from a colormap, and the band can blend smoothly from one color to the next.";

        [Test]
        public override void Execute()
        {
            var speedometer = myPlot.Add.Speedometer(value: 4200, min: 0, max: 6000);
            speedometer.Colors = new ScottPlot.Colormaps.Turbo().GetColors(6);
            speedometer.SmoothColors = true;
            speedometer.Label = "RPM";
        }
    }

    public class SpeedometerExpandToFit : RecipeBase
    {
        public override string Name => "Expand to Fit Value";
        public override string Description => "The scale can expand to fit values beyond it, " +
            "a little past the value and on to round numbers so the needle never rests at the end of the band.";

        [Test]
        public override void Execute()
        {
            var speedometer = myPlot.Add.Speedometer(value: 0, min: 0, max: 0);
            speedometer.ExpandToFitValue = true;
            speedometer.Value = 279_123;
            speedometer.Label = "Trees per Hour";
        }
    }

    public class SpeedometerGlitter : RecipeBase
    {
        public override string Name => "Speedometer Hatch";
        public override string Description => "A hatch pattern can be drawn over the band and the value bar, " +
            "and the needle's fill style can have a hatch of its own.";

        [Test]
        public override void Execute()
        {
            var speedometer = myPlot.Add.Speedometer(value: 185, min: 0, max: 250);
            speedometer.Colors = [new("#2F7BD0"), new("#1FA86E"), new("#FDD20E"), new("#F7941D"), new("#E5197E")];
            speedometer.SmoothColors = true;
            speedometer.Hatch = new ScottPlot.Hatches.Glitter();
            speedometer.NeedleFillStyle.Color = new("#16161A");
            speedometer.NeedleFillStyle.Hatch = new ScottPlot.Hatches.Glitter();
            speedometer.NeedleFillStyle.HatchColor = Colors.Silver;
            speedometer.Label = "Trees per Hour";
        }
    }

    public class SpeedometerMultiple : RecipeBase
    {
        public override string Name => "Multiple Speedometers";
        public override string Description => "Each speedometer can be placed in a fraction of the data area " +
            "so several of them can share one plot.";

        [Test]
        public override void Execute()
        {
            double[] values = [35, 60, 88];
            string[] labels = ["CPU %", "Memory %", "Disk %"];

            for (int i = 0; i < values.Length; i++)
            {
                var speedometer = myPlot.Add.Speedometer(values[i]);
                speedometer.Label = labels[i];
                speedometer.FractionRect = FractionRect.Column(i, values.Length);
            }
        }
    }
}
