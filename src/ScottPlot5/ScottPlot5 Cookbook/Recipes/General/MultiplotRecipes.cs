namespace ScottPlotCookbook.Recipes.Miscellaneous;

public class MultiplotRecipes : ICategory
{
    public Chapter Chapter => Chapter.General;
    public string CategoryName => "Multiplot";
    public string CategoryDescription => "Use Multiplot to create figures with multiple subplots";

    public class MultiplotQuickstart : MultiplotRecipeBase
    {
        public override string Name => "Multiplot Quickstart";
        public override string Description => "Use the Multiplot class to create figures with multiple subplots.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Plot plot1 = new();
            plot1.Add.Signal(Generate.Sin());

            ScottPlot.Plot plot2 = new();
            plot2.Add.Signal(Generate.Cos());

            multiplot.AddPlot(plot1);
            multiplot.AddPlot(plot2);
        }
    }

    public class MultiplotColumns : MultiplotRecipeBase
    {
        public override string Name => "Multiplot Columns";
        public override string Description => "The Multiplot's Layout property " +
            "may be customized to achieve a column layout.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Plot plot1 = new();
            plot1.Add.Signal(Generate.Sin());

            ScottPlot.Plot plot2 = new();
            plot2.Add.Signal(Generate.Cos());

            multiplot.AddPlot(plot1);
            multiplot.AddPlot(plot2);
            multiplot.Layout = new ScottPlot.MultiplotLayouts.Columns();
        }
    }

    public class MultiplotGrid : MultiplotRecipeBase
    {
        public override string Name => "Multiplot Grid";
        public override string Description => "The Multiplot's Layout property " +
            "may be customized to achieve a grid layout.";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 6; i++)
            {
                ScottPlot.Plot plot = new();
                double[] ys = Generate.Sin(oscillations: i + 1);
                plot.Add.Signal(ys);
                multiplot.AddPlot(plot);
            }

            multiplot.Layout = new ScottPlot.MultiplotLayouts.Grid(rows: 2, columns: 3);
        }
    }

    public class MultiplotCustom : MultiplotRecipeBase
    {
        public override string Name => "Multiplot Custom Layout";
        public override string Description => "The Multiplot's Layout property " +
            "may be configured to achieve a fully custom layout.";

        [Test]
        public override void Execute()
        {
            // create 3 plots
            for (int i = 0; i < 3; i++)
            {
                ScottPlot.Plot plot = new();
                double[] ys = Generate.Sin(oscillations: i + 1);
                plot.Add.Signal(ys);
                multiplot.AddPlot(plot);
            }

            // create a custom grid layout
            ScottPlot.MultiplotLayouts.CustomGrid gridLayout = new();
            gridLayout.Set(multiplot.GetPlot(0), new GridCell(0, 0, 2, 1)); // double wide
            gridLayout.Set(multiplot.GetPlot(1), new GridCell(1, 0, 2, 2)); // bottom left
            gridLayout.Set(multiplot.GetPlot(2), new GridCell(1, 1, 2, 2)); // bottom right

            // user the custom layout in our multiplot
            multiplot.Layout = gridLayout;
        }
    }
}
