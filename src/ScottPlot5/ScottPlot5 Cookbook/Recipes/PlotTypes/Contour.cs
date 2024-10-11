namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Contour : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Contour Plot";
    public string CategoryDescription => "A contour plot is a graphical representation that shows the " +
        "three-dimensional surface of a function on a two-dimensional plane " +
        "by connecting points of equal value with contour lines";

    public class ContourQuickstart : RecipeBase
    {
        public override string Name => "Irregular Contour Plot";
        public override string Description => "A contour plot can be created from " +
            "a collection of 3D data points placed arbitrarily in X/Y plane.";

        [Test]
        public override void Execute()
        {
            Coordinates3d[] cs = new Coordinates3d[50];
            for (int i = 0; i < cs.Length; i++)
            {
                double x = Generate.RandomNumber(0, Math.PI * 2);
                double y = Generate.RandomNumber(0, Math.PI * 2);
                double z = Math.Sin(x) + Math.Cos(y);
                cs[i] = new(x, y, z);
            }
            
            myPlot.Add.ContourLines(cs);

            myPlot.Axes.TightMargins();
            myPlot.HideGrid();
        }
    }

    public class ContourGrid : RecipeBase
    {
        public override string Name => "Rectangular Contour Plot";
        public override string Description => "A rectangular contour plot with evenly spaced points " +
            "can be created from a 2D array of 3D points.";

        [Test]
        public override void Execute()
        {
            Coordinates3d[,] cs = new Coordinates3d[50, 50];
            for (int y = 0; y < cs.GetLength(0); y++)
            {
                for (int x = 0; x < cs.GetLength(1); x++)
                {
                    double z = Math.Sin(x * .1) + Math.Cos(y * .1);
                    cs[y, x] = new(x, y, z);
                }
            }

            myPlot.Add.ContourLines(cs);

            myPlot.Axes.TightMargins();
            myPlot.HideGrid();
        }
    }

    public class ContourHeatmap : RecipeBase
    {
        public override string Name => "Contour Lines with Heatmap";
        public override string Description => "Contour lines may be placed on top of heatmaps.";

        [Test]
        public override void Execute()
        {
            Coordinates3d[,] cs = new Coordinates3d[50, 50];
            for (int y = 0; y < cs.GetLength(0); y++)
            {
                for (int x = 0; x < cs.GetLength(1); x++)
                {
                    double z = Math.Sin(x * .1) + Math.Cos(y * .1);
                    cs[y, x] = new(x, y, z);
                }
            }

            myPlot.Add.Heatmap(cs);
            myPlot.Add.ContourLines(cs);

            myPlot.Axes.TightMargins();
            myPlot.HideGrid();
        }
    }
}
