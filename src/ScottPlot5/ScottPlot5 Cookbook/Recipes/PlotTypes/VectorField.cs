namespace ScottPlotCookbook.Recipes.PlotTypes;

public class VectorField : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Vector Field";
    public string CategoryDescription => "Vector fields display a collection of " +
        "vectors rooted at points in coordinate space";

    public class VectorFieldQuickstart : RecipeBase
    {
        public override string Name => "Vector Field Quickstart";
        public override string Description => "Vectors.";

        [Test]
        public override void Execute()
        {
            List<RootedCoordinateVector> vectors = new();

            // generate a grid of positions
            double[] xs = Generate.Consecutive(10);
            double[] ys = Generate.Consecutive(10);

            // add a vector rooted at each position on the grid
            for (int i = 0; i < xs.Length; i++)
            {
                for (int j = 0; j < ys.Length; j++)
                {
                    float dX = (float)ys[j];
                    float dY = -9.81f / 0.5f * (float)Math.Sin(xs[i]);
                    System.Numerics.Vector2 v = new(dX, dY);
                    Coordinates pt = new(xs[i], ys[j]);
                    RootedCoordinateVector vector = new(pt, v);
                    vectors.Add(vector);
                }
            }

            ScottPlot.DataSources.VectorFieldDataSourceCoordinatesList vs = new(vectors);
            ScottPlot.Plottables.VectorField field = new(vs);
            myPlot.PlottableList.Add(field);
        }
    }
}
