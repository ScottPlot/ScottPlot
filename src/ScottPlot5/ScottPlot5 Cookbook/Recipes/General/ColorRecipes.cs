namespace ScottPlotCookbook.Recipes.General;

public class ColorRecipes : ICategory
{
    public Chapter Chapter => Chapter.General;

    public string CategoryName => "Color";

    public string CategoryDescription => "Color tools and features built into ScottPlot";

    public class ColorQuickstart : RecipeBase
    {
        public override string Name => "Color Quickstart";
        public override string Description => "ScottPlot.Colors contains many colors";

        [Test]
        public override void Execute()
        {
            var circle1 = myPlot.Add.Circle(0, 0, 1);
            var circle2 = myPlot.Add.Circle(1, 0, 1);
            var circle3 = myPlot.Add.Circle(2, 0, 1);

            circle1.FillColor = Colors.Red;
            circle2.FillColor = Colors.Green;
            circle3.FillColor = Colors.Blue;

            // set outline style for all circles on the plot
            foreach (var circle in myPlot.GetPlottables<ScottPlot.Plottables.Ellipse>())
                circle.LineColor = Colors.Black;
        }
    }

    public class ColorHex : RecipeBase
    {
        public override string Name => "Creating Colors";
        public override string Description => "ScottPlot.Colors can be constructed from " +
            "RGB values (0-255), HTML style hexadecimal color codes (00-FF), or System.Drawing.Color objects.";

        [Test]
        public override void Execute()
        {
            var circle1 = myPlot.Add.Circle(0, 0, 1);
            var circle2 = myPlot.Add.Circle(1, 0, 1);
            var circle3 = myPlot.Add.Circle(2, 0, 1);

            circle1.FillColor = new Color(red: 255, green: 0, blue: 0);
            circle2.FillColor = new Color(System.Drawing.Color.Green);
            circle3.FillColor = new Color("#0000FF");

            // set outline style for all circles on the plot
            foreach (var circle in myPlot.GetPlottables<ScottPlot.Plottables.Ellipse>())
                circle.LineColor = Colors.Black;
        }
    }

    public class ColorTransparency : RecipeBase
    {
        public override string Name => "Alpha Channel";
        public override string Description => "The Alpha channel sets transparency of a color";

        [Test]
        public override void Execute()
        {
            var circle1 = myPlot.Add.Circle(0, 0, 1);
            var circle2 = myPlot.Add.Circle(1, 0, 1);
            var circle3 = myPlot.Add.Circle(2, 0, 1);

            circle1.FillColor = new Color(red: 255, green: 0, blue: 0, alpha: 128);
            circle2.FillColor = Colors.Green.WithAlpha(.5);
            circle3.FillColor = Colors.Blue.WithAlpha(.5);

            // set outline style for all circles on the plot
            foreach (var circle in myPlot.GetPlottables<ScottPlot.Plottables.Ellipse>())
                circle.LineColor = Colors.Black;
        }
    }

    public class ColorMixing : RecipeBase
    {
        public override string Name => "Color Mixing";
        public override string Description => "Colors have a MixWith() method that can be used to blend two colors";

        [Test]
        public override void Execute()
        {
            Color color1 = Colors.Blue;
            Color color2 = Colors.Green;

            for (int i = 0; i < 10; i++)
            {
                var circle = myPlot.Add.Circle(i, 0, 1);
                double fraction = (double)i / 10;
                circle.FillColor = color1.MixedWith(color2, fraction);
                circle.LineColor = Colors.Black;
            }
        }
    }

    public class ColorHSL : RecipeBase
    {
        public override string Name => "Color HSL";
        public override string Description => "Colors may be generated from " +
            "HSL (hue, saturation, luminosity) values.";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 10; i++)
            {
                var circle = myPlot.Add.Circle(i, 0, 1);
                float fraction = (float)i / 10;
                circle.FillColor = Color.FromHSL(hue: fraction, saturation: 1, luminosity: .5f);
                circle.LineColor = Colors.Black;
            }
        }
    }

    public class ColorInterpolate : RecipeBase
    {
        public override string Name => "Interpolating Colors";
        public override string Description => "A collection of colors can be generated " +
            "from the linear interpolation between two colors.";

        [Test]
        public override void Execute()
        {
            Color[] colors = Color.InterpolateRgbArray(Colors.Blue, Colors.Green, steps: 20);

            for (int i = 0; i < colors.Length; i++)
            {
                var sig = myPlot.Add.Signal(Generate.Sin());
                sig.Data.XOffset = i * 3;
                sig.Data.YOffset = i * .1;
                sig.LineWidth = 3;
                sig.LineColor = colors[i];
            }
        }
    }

    public class ColorRandom : RecipeBase
    {
        public override string Name => "Random Colors";
        public override string Description => "The simplest way to generate random colors is " +
            "to create colors which have same saturation and luminosity but random hue.";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 20; i++)
            {
                var sig = myPlot.Add.Signal(Generate.Sin());
                sig.Data.XOffset = i * 3;
                sig.Data.YOffset = i * .1;
                sig.LineWidth = 3;
                sig.LineColor = Color.RandomHue();
            }
        }
    }

    public class ColorsFromColormap : RecipeBase
    {
        public override string Name => "Colors from Colormap";
        public override string Description => "Colormaps may be used to source a collection of colors.";

        [Test]
        public override void Execute()
        {
            IColormap colormap = new ScottPlot.Colormaps.Viridis();
            Color[] colors = colormap.GetColors(20);

            for (int i = 0; i < colors.Length; i++)
            {
                var sig = myPlot.Add.Signal(Generate.Sin());
                sig.Data.XOffset = i * 3;
                sig.Data.YOffset = i * .1;
                sig.LineWidth = 3;
                sig.LineColor = colors[i];
            }
        }
    }

    public class ColorLightenAndDarken : RecipeBase
    {
        public override string Name => "Color Lighten and Darken";
        public override string Description => "Helper methods make it easy to lighten or darken colors.";

        [Test]
        public override void Execute()
        {
            Color color1 = Colors.Blue;
            Color color2 = Colors.Blue;

            for (int i = 0; i < 10; i++)
            {
                var circle1 = myPlot.Add.Circle(i, 3, 1);
                var circle2 = myPlot.Add.Circle(i, 0, 1);
                circle1.FillColor = color1;
                circle2.FillColor = color2;
                color1 = color1.Lighten(.2);
                color2 = color2.Darken(.2);
            }

            // set outline style for all circles on the plot
            foreach (var circle in myPlot.GetPlottables<ScottPlot.Plottables.Ellipse>())
                circle.LineColor = Colors.Black;
        }
    }
}
