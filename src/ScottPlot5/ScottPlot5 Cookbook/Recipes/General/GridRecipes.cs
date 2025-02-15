namespace ScottPlotCookbook.Recipes.Axis;

public class CustomizingGrids : ICategory
{
    public Chapter Chapter => Chapter.General;
    public string CategoryName => "Customizing Grids";
    public string CategoryDescription => "Advanced customization of grid lines";

    public class HideGrid : RecipeBase
    {
        public override string Name => "Hide Grid";
        public override string Description => "Grid lines can be hidden.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            myPlot.HideGrid();
        }
    }

    public class GridCustom : RecipeBase
    {
        public override string Name => "Grid Customization";
        public override string Description => "Grid lines can be customized. " +
            "Custom grid systems can be created to give developers full control of grid rendering, " +
            "but the default grid can be interacted with to customize its appearance.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            myPlot.Grid.MajorLineColor = Colors.Green.WithOpacity(.3);
            myPlot.Grid.MajorLineWidth = 2;

            myPlot.Grid.MinorLineColor = Colors.Gray.WithOpacity(.1);
            myPlot.Grid.MinorLineWidth = 1;
        }
    }

    public class GridCustomAxis : RecipeBase
    {
        public override string Name => "Axis Specific Grid Customization";
        public override string Description => "Axis-specific styling properties are available " +
            "for extensive axis-specific customization of grid line styling.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos(51));

            myPlot.Grid.XAxisStyle.MajorLineStyle.Color = Colors.Magenta.WithAlpha(.1);
            myPlot.Grid.XAxisStyle.MajorLineStyle.Width = 5;

            myPlot.Grid.YAxisStyle.MajorLineStyle.Color = Colors.Green.WithAlpha(.3);
            myPlot.Grid.YAxisStyle.MajorLineStyle.Width = 2;
        }
    }

    public class GridAbove : RecipeBase
    {
        public override string Name => "Grid Above Data";
        public override string Description => "Grid lines are typically drawn beneath " +
            "data, but grids can be configured to render on top of plottables too.";

        [Test]
        public override void Execute()
        {
            var sig = myPlot.Add.Signal(ScottPlot.Generate.Sin());
            sig.LineWidth = 10;

            myPlot.Grid.MajorLineWidth = 3;
            myPlot.Grid.MajorLineColor = Colors.Black.WithAlpha(.2);
            myPlot.Grid.IsBeneathPlottables = false;
        }
    }

    public class GridWithTopAxis : RecipeBase
    {
        public override string Name => "Grid with Top Axis";
        public override string Description => "Grid lines use the bottom and left axes by default, " +
            "but this behavior can be customized for plots which use other axes.";

        [Test]
        public override void Execute()
        {
            var sig = myPlot.Add.Signal(ScottPlot.Generate.Sin());
            sig.Axes.XAxis = myPlot.Axes.Top;
            myPlot.Grid.XAxis = myPlot.Axes.Top;
        }
    }

    public class GridAlternatingColors : RecipeBase
    {
        public override string Name => "Grid Fill Colors";
        public override string Description => "Regions between alternating pairs of major grid lines " +
            "may be filled with a color specified by the user";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            // shade regions between major grid lines
            myPlot.Grid.XAxisStyle.FillColor1 = Colors.Gray.WithOpacity(0.1);
            myPlot.Grid.XAxisStyle.FillColor2 = Colors.Gray.WithOpacity(0.2);
            myPlot.Grid.YAxisStyle.FillColor1 = Colors.Gray.WithOpacity(0.1);
            myPlot.Grid.YAxisStyle.FillColor2 = Colors.Gray.WithOpacity(0.2);

            // show minor grid lines too
            myPlot.Grid.XAxisStyle.MinorLineStyle.Width = 1;
            myPlot.Grid.YAxisStyle.MinorLineStyle.Width = 1;
        }
    }

    public class GridAlternatingDarkMode : RecipeBase
    {
        public override string Name => "Grid Fill Color in Dark Mode";
        public override string Description => "Grid and plot styling can be customize to achieve a pleasing effect in dark mode";

        [Test]
        public override void Execute()
        {
            // add a green data line
            var sig = myPlot.Add.Signal(Generate.SquareWaveFromSines());
            sig.LineWidth = 3;
            sig.Color = new("#2b9433");
            sig.AlwaysUseLowDensityMode = true;

            // give the plot a dark background with light text
            myPlot.FigureBackground.Color = new("#1c1c1e");
            myPlot.Axes.Color(new("#888888"));

            // shade regions between major grid lines
            myPlot.Grid.XAxisStyle.FillColor1 = new Color("#888888").WithAlpha(10);
            myPlot.Grid.YAxisStyle.FillColor1 = new Color("#888888").WithAlpha(10);

            // set grid line colors
            myPlot.Grid.XAxisStyle.MajorLineStyle.Color = Colors.White.WithAlpha(15);
            myPlot.Grid.YAxisStyle.MajorLineStyle.Color = Colors.White.WithAlpha(15);
            myPlot.Grid.XAxisStyle.MinorLineStyle.Color = Colors.White.WithAlpha(5);
            myPlot.Grid.YAxisStyle.MinorLineStyle.Color = Colors.White.WithAlpha(5);

            // enable minor grid lines by defining a positive width
            myPlot.Grid.XAxisStyle.MinorLineStyle.Width = 1;
            myPlot.Grid.YAxisStyle.MinorLineStyle.Width = 1;

        }
    }
}
