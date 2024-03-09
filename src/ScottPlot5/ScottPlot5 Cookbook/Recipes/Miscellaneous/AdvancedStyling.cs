namespace ScottPlotCookbook.Recipes.Miscellaneous;

public class AdvancedStyling : ICategory
{
    public string Chapter => "Miscellaneous";
    public string CategoryName => "Advanced Styling";
    public string CategoryDescription => "Features for users seeking extensive customization options.";

    public class AdvancedStylingDataBackground : RecipeBase
    {
        public override string Name => "Data Area Background Image";
        public override string Description => "An image can be used for the background of the data area.";

        [Test]
        public override void Execute()
        {
            // plot sample data
            var sig1 = myPlot.Add.Signal(Generate.Sin());
            var sig2 = myPlot.Add.Signal(Generate.Cos());
            sig1.LineWidth = 3;
            sig2.LineWidth = 3;

            // One could load an image from a file...
            // Image bgImage = new("background.png");

            // But in this example we will use a sample image:
            Image bgImage = SampleImages.ScottPlotLogo();
            myPlot.DataBackground.Image = bgImage;
        }
    }

    public class AdvancedStylingFigureBackground : RecipeBase
    {
        public override string Name => "Figure Background Image";
        public override string Description => "An image can be used for the background of a figure.";

        [Test]
        public override void Execute()
        {
            // plot sample data
            var sig1 = myPlot.Add.Signal(Generate.Sin());
            var sig2 = myPlot.Add.Signal(Generate.Cos());

            // One could load an image from a file...
            // Image bgImage = new("background.png");

            // But in this example we will use a sample image:
            Image bgImage = SampleImages.MonaLisa();
            myPlot.FigureBackground.Image = bgImage;

            // Color the axes and data so they stand out against the dark background
            myPlot.Style.ColorAxes(Colors.White);
            sig1.Color = sig1.Color.Lighten(.2);
            sig2.Color = sig2.Color.Lighten(.2);
            sig1.LineWidth = 3;
            sig2.LineWidth = 3;

            // Shade the data area to make it stand out
            myPlot.DataBackground.Color = Colors.Black.WithAlpha(.5);
        }
    }
}
