using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class PolygonQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Polygon();
        public string ID => "polygon_quickstart";
        public string Title => "Polygon Quickstart";
        public string Description =>
            "Polygons are 2D shapes made from pairs of X/Y points. " +
            "The last point connects back to the first point, forming a closed shape. " +
            "Polygons can be optionally outlined and optionally filled. " +
            "Colors with semitransparency are especially useful for polygons.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs1 = { 2, 8, 6, 4 };
            double[] ys1 = { 3, 4, 0.5, 1 };
            plt.AddPolygon(xs1, ys1);

            double[] xs2 = { 3, 2.5, 5 };
            double[] ys2 = { 4.5, 1.5, 2.5 };
            plt.AddPolygon(xs2, ys2, plt.GetNextColor(.5), lineWidth: 2);
        }
    }

    public class PolygonFilledLinePlot : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Polygon();
        public string ID => "polygon_filledLinePlot";
        public string Title => "Filled Line Plot";
        public string Description =>
            "Polygons can be used to create 2D shapes resembling filled line plots. " +
            "When mixed with semitransprent fills, these can be useful for displaying data.";

        public void ExecuteRecipe(Plot plt)
        {
            // create sample data
            double[] xs = { 1, 2, 3, 4 };
            double[] ys1 = { 1, 3, 1, 2 };
            double[] ys2 = { 3, 7, 3, 1 };
            double[] ys3 = { 5, 2, 5, 6 };

            // pad data to turn a line into a shaded region
            xs = Tools.Pad(xs, cloneEdges: true);
            ys1 = Tools.Pad(ys1);
            ys2 = Tools.Pad(ys2);
            ys3 = Tools.Pad(ys3);

            // plot the padded data points as polygons
            plt.AddPolygon(xs, ys3, plt.GetNextColor(.7), lineWidth: 2);
            plt.AddPolygon(xs, ys2, plt.GetNextColor(.7), lineWidth: 2);
            plt.AddPolygon(xs, ys1, plt.GetNextColor(.7), lineWidth: 2);

            // use tight margins so we don't see the edges of polygons
            plt.AxisAuto(0, 0);
        }
    }

    public class PolygonFillBetween : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Polygon();
        public string ID => "polygon_fillBetween";
        public string Title => "Fill Between Curves";
        public string Description =>
            "A shaded area between two curves can be created by enclosing the area as a polygon. " +
            "For this to work the two curves must share the same X points.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new(0);
            int pointCount = 100;
            double[] xs = ScottPlot.DataGen.Consecutive(pointCount);

            // plot a shaded region
            double[] lower = ScottPlot.DataGen.Sin(pointCount, 5, offset: 3);
            double[] upper = ScottPlot.DataGen.Cos(pointCount, 5, offset: -3);
            var poly = plt.AddFill(xs, lower, upper);
            poly.FillColor = Color.FromArgb(50, Color.Green);

            // plot a line within that region
            double[] ys = ScottPlot.DataGen.Random(rand, pointCount);
            var sig = plt.AddSignal(ys);
            sig.Color = plt.Palette.GetColor(0);

            plt.Margins(0, .5);
        }
    }

    public class PolygonStackedFilledLinePlot : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Polygon();
        public string ID => "polygon_stackedFilledLinePlot";
        public string Title => "Stacked Filled Line Plot";
        public string Description =>
            "A stacked filled line plot effect can be achieved by overlapping polygons.";

        public void ExecuteRecipe(Plot plt)
        {
            // create sample data
            double[] xs = { 1, 2, 3, 4 };
            double[] ys1 = { 1, 3, 1, 2 };
            double[] ys2 = { 3, 7, 3, 1 };
            double[] ys3 = { 5, 2, 5, 6 };

            // manually stack plots
            ys2 = Enumerable.Range(0, ys2.Length).Select(x => ys2[x] + ys1[x]).ToArray();
            ys3 = Enumerable.Range(0, ys2.Length).Select(x => ys3[x] + ys2[x]).ToArray();

            // pad data to turn a line into a shaded region
            xs = Tools.Pad(xs, cloneEdges: true);
            ys1 = Tools.Pad(ys1);
            ys2 = Tools.Pad(ys2);
            ys3 = Tools.Pad(ys3);

            // plot the padded data points as polygons
            plt.AddPolygon(xs, ys3, lineWidth: 2);
            plt.AddPolygon(xs, ys2, lineWidth: 2);
            plt.AddPolygon(xs, ys1, lineWidth: 2);

            // use tight margins so we don't see the edges of polygons
            plt.AxisAuto(0, 0);
        }
    }

    public class Polygons : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Polygon();
        public string ID => "polygon_polygons";
        public string Title => "Many Polygons";
        public string Description =>
            "Special rendering optimizations are available to display a large number of polygons.";

        public void ExecuteRecipe(Plot plt)
        {
            // create a list of random polygons
            List<List<(double x, double y)>> polys = new List<List<(double x, double y)>>();
            int polygonCount = 5_000;
            int pointsPerPolygon = 100;
            Random rand = new Random(0);
            for (int i = 0; i < polygonCount; i++)
            {
                // random placement
                double polyX = rand.NextDouble() * 100;
                double polyY = rand.NextDouble() * 100;

                // points are random locations around a circle of random size
                double polyR = rand.NextDouble();
                double[] xs = Enumerable.Range(0, pointsPerPolygon).Select(x => polyR * Math.Cos(2.0 * Math.PI * x / pointsPerPolygon) + polyX).ToArray();
                double[] ys = Enumerable.Range(0, pointsPerPolygon).Select(x => polyR * Math.Sin(2.0 * Math.PI * x / pointsPerPolygon) + polyY).ToArray();

                // add this polygon to the list
                List<(double x, double y)> thisPolygon = xs.Zip(ys, (xp, yp) => (xp, yp)).ToList();
                polys.Add(thisPolygon);
            }

            // plot the list of polygons with one step
            plt.AddPolygons(polys, fillColor: Color.Green);

            // ensure X and Y pixel scales are the same (so circles aren't ovals)
            plt.AxisScaleLock(true);
        }
    }
}
