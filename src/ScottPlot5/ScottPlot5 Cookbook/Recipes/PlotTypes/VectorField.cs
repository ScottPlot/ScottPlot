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
        public override string Description => "Vectors (representing a magnitude and direction) " +
            "can be placed at specific points in coordinate space to display as a vector field.";

        [Test]
        public override void Execute()
        {
            // generate a grid of positions
            double[] xs = Generate.Consecutive(10);
            double[] ys = Generate.Consecutive(10);

            // create a collection of vectors
            List<RootedCoordinateVector> vectors = new();
            for (int i = 0; i < xs.Length; i++)
            {
                for (int j = 0; j < ys.Length; j++)
                {
                    // point on the grid
                    Coordinates pt = new(xs[i], ys[j]);

                    // direction & magnitude
                    float dX = (float)ys[j];
                    float dY = -9.81f / 0.5f * (float)Math.Sin(xs[i]);
                    System.Numerics.Vector2 v = new(dX, dY);

                    // add to the collection
                    RootedCoordinateVector vector = new(pt, v);
                    vectors.Add(vector);
                }
            }

            // plot the collection of rooted vectors as a vector field
            myPlot.Add.VectorField(vectors);
        }
    }

    public class VectorFieldColormap : RecipeBase
    {
        public override string Name => "Vector Field Colormap";
        public override string Description => "Vector field arrows can be " +
            "colored according to their magnitude.";

        [Test]
        public override void Execute()
        {
            RootedCoordinateVector[] vectors = Generate.SampleVectors();
            var vf = myPlot.Add.VectorField(vectors);
            vf.Colormap = new ScottPlot.Colormaps.Turbo();
        }
    }
}
