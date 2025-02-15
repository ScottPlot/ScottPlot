namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Contour : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
    public string CategoryName => "Contour Plot";
    public string CategoryDescription => "A contour plot is a graphical representation that shows the " +
        "three-dimensional surface of a function on a two-dimensional plane " +
        "by connecting points of equal value with contour lines";

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

            var contour = myPlot.Add.ContourLines(cs);
            contour.LineColor = Colors.Black.WithAlpha(.5);
            contour.LinePattern = LinePattern.Dotted;

            myPlot.Axes.TightMargins();
            myPlot.HideGrid();
        }
    }

    public class IrregularContour : RecipeBase
    {
        public override string Name => "Irregular Contour Plot";
        public override string Description => "A contour plot can be created from " +
            "a collection of 3D data points placed arbitrarily in X/Y plane.";

        [Test]
        public override void Execute()
        {
            // generate irregularly spaced X/Y/Z data points
            Coordinates3d[] cs = new Coordinates3d[1000];
            for (int i = 0; i < cs.Length; i++)
            {
                double x = Generate.RandomNumber(0, Math.PI * 2);
                double y = Generate.RandomNumber(0, Math.PI * 2);
                double z = Math.Sin(x) + Math.Cos(y);
                cs[i] = new(x, y, z);
            }

            // place markers at each data point
            double minZ = cs.Select(x => x.Z).Min();
            double maxZ = cs.Select(x => x.Z).Max();
            double spanZ = maxZ - minZ;
            IColormap cmap = new ScottPlot.Colormaps.MellowRainbow();
            for (int i = 0; i < cs.Length; i++)
            {
                double fraction = (cs[i].Z - minZ) / (spanZ);
                var marker = myPlot.Add.Marker(cs[i].X, cs[i].Y);
                marker.Color = cmap.GetColor(fraction).WithAlpha(.8);
                marker.Size = 5;
            }

            // show contour lines
            var contour = myPlot.Add.ContourLines(cs);

            // style the plot
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

            var heatmap = myPlot.Add.Heatmap(cs);
            heatmap.FlipVertically = true;
            heatmap.Colormap = new ScottPlot.Colormaps.MellowRainbow();

            var contour = myPlot.Add.ContourLines(cs);
            contour.LabelStyle.Bold = true;
            contour.LinePattern = LinePattern.DenselyDashed;
            contour.LineColor = Colors.Black.WithAlpha(.5);

            myPlot.Axes.TightMargins();
            myPlot.HideGrid();
        }
    }

    public class ContourColormap : RecipeBase
    {
        public override string Name => "Contour Lines with Colormap";
        public override string Description => "If a colormap is provided it will be used " +
            "to color each line in the colormap according to its value.";

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

            var cl = myPlot.Add.ContourLines(cs, count: 25);
            cl.Colormap = new ScottPlot.Colormaps.MellowRainbow();
            cl.LineWidth = 3;
            cl.LabelStyle.IsVisible = false;

            myPlot.Axes.TightMargins();
            myPlot.HideGrid();
        }
    }
}
