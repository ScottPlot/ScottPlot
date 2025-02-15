namespace ScottPlotCookbook.Recipes.Axis;

public class Miscellaneous : ICategory
{
    public Chapter Chapter => Chapter.General;
    public string CategoryName => "Miscellaneous";
    public string CategoryDescription => "Miscellaneous features and customization options";

    public class DataBackgroundImage : RecipeBase
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

    public class FigureBackgroundImage : RecipeBase
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
            myPlot.Axes.Color(Colors.White);
            sig1.Color = sig1.Color.Lighten(.2);
            sig2.Color = sig2.Color.Lighten(.2);
            sig1.LineWidth = 3;
            sig2.LineWidth = 3;

            // Shade the data area to make it stand out
            myPlot.DataBackground.Color = Colors.Black.WithAlpha(.5);
        }
    }

    public class ColorInterpolation : RecipeBase
    {
        public override string Name => "Color Interpolation";
        public override string Description => "Colors can be mixed to create" +
            "a range of colors. This strategy uses linear RGB interpolation.";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i <= 10; i++)
            {
                double fraction = (double)i / 10;
                double x = i;
                double y = Math.Sin(Math.PI * 2 * fraction);
                var circle = myPlot.Add.Circle(x, y, 2);
                circle.FillColor = Colors.Blue.MixedWith(Colors.Green, fraction);
                circle.LineColor = Colors.Black.WithAlpha(.5);
            }
        }
    }

    public class CustomFontFiles : RecipeBase
    {
        public override string Name => "Custom Font Files";
        public override string Description => "Users can apply custom typefaces by loaded from font files.";

        [Test]
        public override void Execute()
        {
            // Add a font file to use its typeface for fonts with a given name
            Fonts.AddFontFile(
                name: "Alumni Sans",
                path: Path.Combine(Paths.FontFolder, @"AlumniSans/AlumniSans-Regular.ttf"));

            // plot sample data
            var sig1 = myPlot.Add.Signal(Generate.Sin(51));
            sig1.LegendText = "Sin";
            var sig2 = myPlot.Add.Signal(Generate.Cos(51));
            sig2.LegendText = "Cos";

            // custom fonts may be used in legends
            myPlot.Legend.FontName = "Alumni Sans";
            myPlot.Legend.FontSize = 24;

            // custom fonts may be used in plottables that contain text
            var text = myPlot.Add.Text("Hello", 25, 0.5);
            text.LabelStyle.FontName = "Alumni Sans";
            text.LabelStyle.FontSize = 24;

            // Custom fonts may be used for axis labels.
            // Note that bold is disabled because support for
            // bold would require loading an additional font file.
            myPlot.Title("Custom Font Demo");
            myPlot.Axes.Title.Label.FontName = "Alumni Sans";
            myPlot.Axes.Title.Label.FontSize = 36;
            myPlot.Axes.Title.Label.Bold = false;

            myPlot.XLabel("Horizontal Axis");
            myPlot.Axes.Bottom.Label.FontName = "Alumni Sans";
            myPlot.Axes.Bottom.Label.FontSize = 24;
            myPlot.Axes.Bottom.Label.Bold = false;

            myPlot.YLabel("Vertical Axis");
            myPlot.Axes.Left.Label.FontName = "Alumni Sans";
            myPlot.Axes.Left.Label.FontSize = 24;
            myPlot.Axes.Left.Label.Bold = false;
        }
    }
}
