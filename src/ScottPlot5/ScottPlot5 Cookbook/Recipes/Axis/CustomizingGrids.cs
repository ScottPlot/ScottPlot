using System.ComponentModel;

namespace ScottPlotCookbook.Recipes.Axis;

public class CustomizingGrids : ICategory
{
    public string Chapter => "Axis";
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
}
