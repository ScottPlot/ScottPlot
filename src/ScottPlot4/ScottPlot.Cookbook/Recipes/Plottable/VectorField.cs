using ScottPlot.Drawing;
using ScottPlot.Statistics;
using System;
using System.Collections.Generic;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class VectorFieldQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.VectorField();
        public string ID => "vectorField_quickstart";
        public string Title => "Quickstart";
        public string Description => "A vector field can be useful to show data explained by differential equations";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xPositions = DataGen.Range(0, 10);
            double[] yPositions = DataGen.Range(0, 10);
            Vector2[,] vectors = new Vector2[xPositions.Length, yPositions.Length];

            for (int x = 0; x < xPositions.Length; x++)
                for (int y = 0; y < yPositions.Length; y++)
                    vectors[x, y] = new Vector2(
                        x: Math.Sin(xPositions[x]),
                        y: Math.Sin(yPositions[y]));

            plt.AddVectorField(vectors, xPositions, yPositions);
        }
    }

    public class WithChangeingMagnitude : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.VectorField();
        public string ID => "vectorField_angleMag";
        public string Title => "Angle and Magnitude";
        public string Description => "This example demonstrates how to define vectors according to a given angle and magnitude.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs = DataGen.Range(-5, 6);
            double[] ys = DataGen.Range(-5, 6);
            Vector2[,] vectors = new Vector2[xs.Length, ys.Length];

            for (int i = 0; i < xs.Length; i++)
            {
                for (int j = 0; j < ys.Length; j++)
                {
                    double slope = -xs[i];
                    double magnitude = Math.Abs(xs[i]);
                    double angle = Math.Atan(slope);

                    vectors[i, j] = new Vector2(Math.Cos(angle) * magnitude, Math.Sin(angle) * magnitude);
                }
            }

            plt.AddVectorField(vectors, xs, ys);
        }
    }

    public class Pendulum : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.VectorField();
        public string ID => "vectorField_colormap";
        public string Title => "Custom Colormap";
        public string Description => "A colormap can be supplied to color arrows according to their magnitude";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs = DataGen.Range(-5, 5, .5);
            double[] ys = DataGen.Range(-5, 5, .5);
            Vector2[,] vectors = new Vector2[xs.Length, ys.Length];
            double r = 0.5;


            for (int i = 0; i < xs.Length; i++)
            {
                for (int j = 0; j < ys.Length; j++)
                {
                    double x = ys[j];
                    double y = -9.81 / r * Math.Sin(xs[i]);

                    vectors[i, j] = new Vector2(x, y);
                }
            }

            plt.AddVectorField(vectors, xs, ys, colormap: Drawing.Colormap.Turbo);
            plt.XLabel("θ");
            plt.YLabel("dθ/dt");
        }
    }

    public class CustomScaleFactor : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.VectorField();
        public string ID => "vectorField_scaleFactor";
        public string Title => "Custom Scale Factor";
        public string Description => "A custom scale factor can adjust the length of the arrows.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs = DataGen.Range(-1.5, 1.5, .25);
            double[] ys = DataGen.Range(-1.5, 1.5, .25);
            Vector2[,] vectors = new Vector2[xs.Length, ys.Length];

            for (int i = 0; i < xs.Length; i++)
            {
                for (int j = 0; j < ys.Length; j++)
                {
                    double x = xs[i];
                    double y = ys[j];
                    var e = Math.Exp(-x * x - y * y);
                    var dx = (1 - 2 * x * x) * e;
                    var dy = -2 * x * y * e;

                    vectors[i, j] = new Vector2(dx, dy);
                }
            }

            plt.AddVectorField(vectors, xs, ys, scaleFactor: 0.3);
        }
    }

    public class FancyVectorField : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.VectorField();
        public string ID => "vectorField_fancytips";
        public string Title => "Scaled Arrowheads";
        public string Description => "Use a slower drawing method that draws tips that are proportional to the length of the arrows.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xPositions = DataGen.Range(0, 10);
            double[] yPositions = DataGen.Range(0, 10);
            Vector2[,] vectors = new Vector2[xPositions.Length, yPositions.Length];

            for (int x = 0; x < xPositions.Length; x++)
                for (int y = 0; y < yPositions.Length; y++)
                    vectors[x, y] = new Vector2(
                          x: Math.Sin(xPositions[x]),
                          y: Math.Sin(yPositions[y]));

            var vf = plt.AddVectorField(vectors, xPositions, yPositions);
            vf.ScaledArrowheads = true;
            vf.Anchor = ArrowAnchor.Base;
            vf.MarkerSize = 3;
        }
    }

    public class VectorFieldArbitrary : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.VectorField();
        public string ID => "vectorField_arbitrary";
        public string Title => "Arbitrary Vectors";
        public string Description => "Vectors can be placed arbitrarily in coordiante space " +
            "(they are not required to be in a grid)";

        public void ExecuteRecipe(Plot plt)
        {
            // Create a vector field list and add it to the plot
            var field = plt.AddVectorFieldList();

            // Optionally color arrows using a colormap and custom scaling function
            field.Colormap = Colormap.Turbo;
            field.ColormapScaler = (magnitude) => magnitude * 5;

            // Additional arrow styling options are here
            field.ArrowStyle.ScaledArrowheads = true;
            field.ArrowStyle.Anchor = ArrowAnchor.Base;
            field.ArrowStyle.LineWidth = 3;
            field.ArrowStyle.MarkerSize = 10;
            field.ArrowStyle.MarkerShape = MarkerShape.filledDiamond;

            // Generate random vectors
            Random rand = new(0);
            int pointCount = 20;
            for (int i = 0; i < pointCount + 1; i++)
            {
                double x = i / (double)pointCount;
                double y = Math.Sin(i * 2 * Math.PI / pointCount);
                Coordinate coordinate = new(x, y);

                double dX = i / (double)pointCount * .15;
                double dY = i / (double)pointCount * .15;
                CoordinateVector vector = new(dX, dY);

                var rootedVector = (coordinate, vector);
                field.RootedVectors.Add(rootedVector);
            }
        }
    }
}
