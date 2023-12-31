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
            myPlot.Add.Signal(ScottPlot.Generate.Sin(51));
            myPlot.Add.Signal(ScottPlot.Generate.Cos(51));

            ScottPlot.Grids.DefaultGrid grid = myPlot.GetDefaultGrid();

            grid.MajorLineStyle.Color = Colors.Green.WithOpacity(.5);
            grid.MinorLineStyle.Color = Colors.Green.WithOpacity(.1);
            grid.MinorLineStyle.Width = 1;
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

            ScottPlot.Grids.DefaultGrid grid = myPlot.GetDefaultGrid();
            grid.MajorLineStyle.Width = 3;
            grid.MajorLineStyle.Color = Colors.WhiteSmoke;
            grid.IsBeneathPlottables = false;
        }
    }
}
