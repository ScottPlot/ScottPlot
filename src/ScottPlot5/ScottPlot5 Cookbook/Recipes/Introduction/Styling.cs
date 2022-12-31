using ScottPlot.Style;

namespace ScottPlotCookbook.Recipes.Introduction;

internal class Styling : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.Introduction,
        PageName = "Styling Plots",
        PageDescription = "How to customize plots",
    };

    internal class BackgroundColors : RecipeTestBase
    {
        public override string Name => "Background Colors";
        public override string Description => "Figure and data area background colors can be customized.";

        [Test]
        public override void Recipe()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            myPlot.FigureBackground = Colors.DarkGray;
            myPlot.DataBackground = Colors.LightGray;
        }
    }

    internal class AxisCustom : RecipeTestBase
    {
        public override string Name => "Axis Customization";
        public override string Description => "Axis labels, tick marks, and frame can all be customized.";

        [Test]
        public override void Recipe()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            myPlot.Title.Label.Text = "Plot Title";
            myPlot.Title.Label.Font.Color = Colors.RebeccaPurple;
            myPlot.Title.Label.Font.Size = 32;
            myPlot.Title.Label.Font.Name = FontService.SerifFontName;
            myPlot.Title.Label.Rotation = -5;
            myPlot.Title.Label.Font.Bold = false;

            myPlot.YAxis.Label.Text = "Vertical Axis";
            myPlot.YAxis.Label.Font.Color = Colors.Magenta;
            myPlot.YAxis.Label.Font.Italic = true;

            myPlot.XAxis.Label.Text = "Horizontal Axis";
            myPlot.XAxis.Label.Font.Bold = false;
            myPlot.XAxis.Label.Font.Name = FontService.MonospaceFontName;

            myPlot.XAxis.MajorTickLength = 10;
            myPlot.XAxis.MajorTickWidth = 3;
            myPlot.XAxis.MajorTickColor = Colors.Magenta;
            myPlot.XAxis.MinorTickLength = 5;
            myPlot.XAxis.MinorTickWidth = 0.5f;
            myPlot.XAxis.MinorTickColor = Colors.Green;
            myPlot.XAxis.FrameLineStyle.Color = Colors.LightBlue;
        }
    }

    internal class GridCustom : RecipeTestBase
    {
        public override string Name => "Grid Customization";
        public override string Description => "Grid lines can be customized. " +
            "Custom grid systems can be created to give developers full control of grid rendering, " +
            "but the default grid can be interacted with to customize its appearance.";

        [Test]
        public override void Recipe()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            ScottPlot.Grids.DefaultGrid grid = myPlot.GetDefaultGrid();

            grid.MajorLineStyle.Color = Colors.Green.WithOpacity(.5);
            grid.MinorLineStyle.Color = Colors.Green.WithOpacity(.1);
            grid.MinorLineStyle.Width = 1;
        }
    }

    internal class GridAbove : RecipeTestBase
    {
        public override string Name => "Grid Above Data";
        public override string Description => "Grid lines are typically drawn beneath " +
            "data, but grids can be configured to render on top of plottables too.";

        [Test]
        public override void Recipe()
        {
            var sig = myPlot.Add.Signal(Generate.Sin(51));
            sig.LineStyle.Width = 10;

            ScottPlot.Grids.DefaultGrid grid = myPlot.GetDefaultGrid();
            grid.MajorLineStyle.Width = 3;
            grid.MajorLineStyle.Color = Colors.WhiteSmoke;
            grid.IsBeneathPlottables = false;
        }
    }

    internal class Palette : RecipeTestBase
    {
        public override string Name => "Palettes";
        public override string Description => "A palette is a set of colors, and the Plot's palette " +
            "defines the default colors to use when adding new plottables. ScottPlot comes with many " +
            "standard palettes, but users may also create their own.";

        [Test]
        public override void Recipe()
        {
            IPalette palette = new ScottPlot.Palettes.Nord();
            myPlot.Palette = palette;

            for (int i = 0; i < 5; i++)
            {
                double[] data = Generate.Sin(100, phase: -i / 20.0f);
                var sig = myPlot.Add.Signal(data);
                sig.LineStyle.Width = 3;
            }
        }
    }

    internal class Markers : RecipeTestBase
    {
        public override string Name => "Markers";
        public override string Description => "Many plot types have a MarkerStyle which can be customized.";

        [Test]
        public override void Recipe()
        {
            int count = 21;
            double[] xs = Generate.Consecutive(count);
            double[] ys = Generate.Sin(count);

            MarkerShape[] markerShapes = Enum.GetValues<MarkerShape>().ToArray();

            for (int i = 0; i < markerShapes.Length; i++)
            {
                double[] data = ys.Select(y => markerShapes.Length - y + i).ToArray();

                var scatter = myPlot.Add.Scatter(xs, data);

                scatter.Label = markerShapes[i].ToString();

                scatter.MarkerStyle = new MarkerStyle(
                    shape: markerShapes[i],
                    size: 10,
                    color: scatter.LineStyle.Color);
            }
        }
    }
}
