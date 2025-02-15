namespace ScottPlotCookbook.Recipes.Introduction;

public class Styling : ICategory
{
    public Chapter Chapter => Chapter.General;
    public string CategoryName => "Styling Plots";
    public string CategoryDescription => "How to customize appearance of plots";

    public class BackgroundColors : RecipeBase
    {
        public override string Name => "Background Colors";
        public override string Description => "Background color for the entire figure or just the " +
            "data area may be individually controlled. When using dark figure backgrounds it may be " +
            "necessary to configure axes to use light colors";

        [Test]
        public override void Execute()
        {
            // setup a plot with sample data
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));
            myPlot.XLabel("Horizontal Axis");
            myPlot.YLabel("Vertical Axis");

            // some items must be styled directly
            myPlot.FigureBackground.Color = Colors.Navy;
            myPlot.DataBackground.Color = Colors.Navy.Darken(0.1);
            myPlot.Grid.MajorLineColor = Colors.Navy.Lighten(0.1);

            // some items have helper methods to configure multiple properties at once
            myPlot.Axes.Color(Colors.Navy.Lighten(0.8));
        }
    }

    public class AxisCustom : RecipeBase
    {
        public override string Name => "Axis Customization";
        public override string Description => "Axis labels, tick marks, and frame can all be customized.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            myPlot.Axes.Title.Label.Text = "Plot Title";
            myPlot.Axes.Title.Label.ForeColor = Colors.RebeccaPurple;
            myPlot.Axes.Title.Label.FontSize = 32;
            myPlot.Axes.Title.Label.FontName = Fonts.Serif;
            myPlot.Axes.Title.Label.Rotation = -5;
            myPlot.Axes.Title.Label.Bold = false;

            myPlot.Axes.Left.Label.Text = "Vertical Axis";
            myPlot.Axes.Left.Label.ForeColor = Colors.Magenta;
            myPlot.Axes.Left.Label.Italic = true;

            myPlot.Axes.Bottom.Label.Text = "Horizontal Axis";
            myPlot.Axes.Bottom.Label.Bold = false;
            myPlot.Axes.Bottom.Label.FontName = Fonts.Monospace;

            myPlot.Axes.Bottom.MajorTickStyle.Length = 10;
            myPlot.Axes.Bottom.MajorTickStyle.Width = 3;
            myPlot.Axes.Bottom.MajorTickStyle.Color = Colors.Magenta;
            myPlot.Axes.Bottom.MinorTickStyle.Length = 5;
            myPlot.Axes.Bottom.MinorTickStyle.Width = 0.5f;
            myPlot.Axes.Bottom.MinorTickStyle.Color = Colors.Green;
            myPlot.Axes.Bottom.FrameLineStyle.Color = Colors.Blue;
            myPlot.Axes.Bottom.FrameLineStyle.Width = 3;

            myPlot.Axes.Right.FrameLineStyle.Width = 0;
        }
    }

    public class Palette : RecipeBase
    {
        public override string Name => "Palettes";
        public override string Description => "A palette is a set of colors, and the Plot's palette " +
            "defines the default colors to use when adding new plottables. ScottPlot comes with many " +
            "standard palettes, but users may also create their own.";

        [Test]
        public override void Execute()
        {
            // change the default palette used when adding new plottables
            myPlot.Add.Palette = new ScottPlot.Palettes.Nord();

            for (int i = 0; i < 5; i++)
            {
                double[] data = Generate.Sin(100, phase: -i / 20.0f);
                var sig = myPlot.Add.Signal(data);
                sig.LineWidth = 3;
            }
        }
    }

    public class ArrowShapeNames : RecipeBase
    {
        public override string Name => "Arrow Shapes";
        public override string Description => "Many standard arrow shapes are available";

        [Test]
        public override void Execute()
        {
            ArrowShape[] arrowShapes = Enum.GetValues<ArrowShape>().ToArray();

            for (int i = 0; i < arrowShapes.Length; i++)
            {
                Coordinates arrowTip = new(0, -i);
                Coordinates arrowBase = arrowTip.WithDelta(1, 0);

                var arrow = myPlot.Add.Arrow(arrowBase, arrowTip);
                arrow.ArrowShape = arrowShapes[i].GetShape();

                var txt = myPlot.Add.Text(arrowShapes[i].ToString(), arrowBase.WithDelta(.1, 0));
                txt.LabelFontColor = arrow.ArrowLineColor;
                txt.LabelAlignment = Alignment.MiddleLeft;
                txt.LabelFontSize = 18;
            }

            myPlot.Axes.SetLimits(-1, 3, -arrowShapes.Length, 1);
            myPlot.HideGrid();
        }
    }

    public class LineStyles : RecipeBase
    {
        public override string Name => "Line Styles";
        public override string Description => "Many plot types have a LineStyle which can be customized.";

        [Test]
        public override void Execute()
        {
            List<LinePattern> patterns = [];
            patterns.AddRange(LinePattern.GetAllPatterns());
            patterns.Add(new([2, 2, 5, 10], 0, "Custom"));

            for (int i = 0; i < patterns.Count; i++)
            {
                LinePattern pattern = patterns[i];

                var line = myPlot.Add.Line(0, -i, 1, -i);
                line.LinePattern = pattern;
                line.LineWidth = 2;
                line.Color = Colors.Black;

                var txt = myPlot.Add.Text(patterns[i].Name, 1.1, -i);
                txt.LabelFontSize = 18;
                txt.LabelBold = true;
                txt.LabelFontColor = Colors.Black;
                txt.LabelAlignment = Alignment.MiddleLeft;
            }

            myPlot.Axes.Margins(right: 1);
            myPlot.HideGrid();
            myPlot.Layout.Frameless();

            myPlot.ShowLegend();
        }
    }

    public class Scaling : RecipeBase
    {
        public override string Name => "Scale Factor";
        public override string Description => "All components of an image can be scaled up or down in size " +
            "by adjusting the ScaleFactor property. This is very useful for creating images that look nice " +
            "on high DPI displays with display scaling enabled.";

        [Test]
        public override void Execute()
        {
            myPlot.ScaleFactor = 2;
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());
        }
    }

    public class Hairline : RecipeBase
    {
        public override string Name => "Hairline Mode";
        public override string Description => "Hairline mode allows axis frames, tick marks, and grid lines " +
            "to always be rendered a single pixel wide regardless of scale factor. Enable hairline mode to allow " +
            "interactive plots to feel smoother when a large scale factor is in use.";

        [Test]
        public override void Execute()
        {
            myPlot.ScaleFactor = 2;
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            myPlot.Axes.Hairline(true);
        }
    }

    public class DarkMode : RecipeBase
    {
        public override string Name => "Dark Mode";
        public override string Description => "Plots can be created using dark mode " +
            "by setting the colors of major plot components to ones consistent with a dark theme.";

        [Test]
        public override void Execute()
        {
            // set the color palette used when coloring new items added to the plot
            myPlot.Add.Palette = new ScottPlot.Palettes.Penumbra();

            // add things to the plot
            for (int i = 0; i < 5; i++)
            {
                var sig = myPlot.Add.Signal(Generate.Sin(51, phase: -.05 * i));
                sig.LineWidth = 3;
                sig.LegendText = $"Line {i + 1}";
            }
            myPlot.XLabel("Horizontal Axis");
            myPlot.YLabel("Vertical Axis");
            myPlot.Title("ScottPlot 5 in Dark Mode");
            myPlot.ShowLegend();

            // change figure colors
            myPlot.FigureBackground.Color = Color.FromHex("#181818");
            myPlot.DataBackground.Color = Color.FromHex("#1f1f1f");

            // change axis and grid colors
            myPlot.Axes.Color(Color.FromHex("#d7d7d7"));
            myPlot.Grid.MajorLineColor = Color.FromHex("#404040");

            // change legend colors
            myPlot.Legend.BackgroundColor = Color.FromHex("#404040");
            myPlot.Legend.FontColor = Color.FromHex("#d7d7d7");
            myPlot.Legend.OutlineColor = Color.FromHex("#d7d7d7");
        }
    }

    public class ColormapColorSteps : RecipeBase
    {
        public override string Name => "Colormap Steps";
        public override string Description => "Colormaps can be used to generate " +
            "a collection of discrete colors that can be applied to plottable objects.";

        [Test]
        public override void Execute()
        {
            IColormap colormap = new ScottPlot.Colormaps.Turbo();

            for (int count = 1; count < 10; count++)
            {
                double[] xs = Generate.Consecutive(count);
                double[] ys = Generate.Repeating(count, count);
                Color[] colors = colormap.GetColors(count);

                for (int i = 0; i < count; i++)
                {
                    var circle = myPlot.Add.Circle(xs[i], ys[i], 0.45);
                    circle.FillColor = colors[i];
                    circle.LineWidth = 0;
                }
            }

            myPlot.YLabel("number of colors");
        }
    }

    public class ColormapFromColors : RecipeBase
    {
        public override string Name => "Colormap Gradient from Colors";
        public override string Description => "Colormaps can be created as a gradient between a collection of colors.";

        [Test]
        public override void Execute()
        {
            Color[] colors = [Colors.Red, Colors.Magenta, Colors.DarkGreen];
            IColormap myColormap = Colormap.FromColors(colors);

            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);
            var markers = myPlot.Add.Markers(xs, ys);
            markers.Colormap = myColormap;
        }
    }

    public class HandDrawn : RecipeBase
    {
        public override string Name => "Hand Drawn Line Style";
        public override string Description => "Enabling hand-drawn line style allows creation " +
            "of charts that mimic XKCD style graphs which use squiggly lines for comedic effect.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(100);
            double[] values1 = Generate.Sigmoidal(xs.Length, -1, 2);

            // create a hand drawn scatter plot
            var sp = myPlot.Add.ScatterLine(xs, values1);
            sp.LineStyle.HandDrawn = true;
            sp.LineStyle.HandDrawnJitter = 2;
            sp.LineWidth = 3;
            sp.LineColor = Colors.Black;

            // configure axis frames to appear hand drawn
            myPlot.HideGrid();
            myPlot.Axes.GetAxes().ToList().ForEach(x => x.FrameLineStyle.HandDrawn = true);

            // use a comedic font for axis titles and tick labels
            myPlot.Title("Answers");
            myPlot.YLabel("Utility");
            myPlot.XLabel("Time Taken to Respond");
            myPlot.Axes.Title.Label.FontName = "Comic Sans MS";
            myPlot.Axes.Left.Label.FontName = "Comic Sans MS";
            myPlot.Axes.Bottom.Label.FontName = "Comic Sans MS";
            myPlot.Axes.Bottom.TickLabelStyle.FontName = "Comic Sans MS";

            // use manually placed horizontal axis ticks
            myPlot.Axes.Left.TickGenerator = new ScottPlot.TickGenerators.EmptyTickGenerator();
            myPlot.Axes.Bottom.SetTicks([10, 50, 75], ["Minutes", "Days", "Weeks"]);
        }
    }

    public class TitleAlignment : RecipeBase
    {
        public override string Name => "Title Alignment";
        public override string Description => "The title is centered over the data area by default, " +
            "but a flag allows users to center it relative to the figure instead";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51, mult: 1e9));
            myPlot.Title("This title is centered in the figure");
            myPlot.Axes.Title.FullFigureCenter = true;
        }
    }
}
