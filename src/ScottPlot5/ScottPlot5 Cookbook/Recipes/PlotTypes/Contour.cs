using ScottPlot.DataSources;

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
        public override string Name => "Contour Plot Quickstart";
        public override string Description => "A contour plot can be created from a collection of 2D data points.";

        [Test]
        public override void Execute()
        {
            Coordinates3d[] cs = new Coordinates3d[500];
            for (int i = 0; i < 500; i++)
            {
                double x = Generate.RandomNumber(0, Math.PI * 2);
                double y = Generate.RandomNumber(0, Math.PI * 2);
                double z = Math.Sin(x) + Math.Cos(y);
                cs[i] = new(x, y, z);
            }

            TINSourceCoordinates3dArray data = new(cs.ToArray());
            ScottPlot.Plottables.TINPlot contour = new(data);
            myPlot.PlottableList.Add(contour);

            myPlot.HideGrid();
        }
    }
}
