using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class ScatterListQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatterList_quickstart";
        public string Title => "Scatter List Quickstart";
        public string Description =>
            "This plot type has add/remove/clear methods like typical lists.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs = { 1, 2, 3, 4 };
            double[] ys = { 1, 4, 9, 16 };

            var scatterList = plt.AddScatterList();
            scatterList.AddRange(xs, ys);
            scatterList.Add(5, 25);
        }
    }

    public class ScatterListGeneric : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatterList_generic";
        public string Title => "Scatter List Generic";
        public string Description =>
            "This plot type supports generics.";

        public void ExecuteRecipe(Plot plt)
        {
            int[] xs = { 1, 2, 3, 4 };
            int[] ys = { 1, 4, 9, 16 };

            var scatterList = plt.AddScatterList<int>();
            scatterList.AddRange(xs, ys);
            scatterList.Add(5, 25);
        }
    }

    public class ScatterListDraggable : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatterList_draggable";
        public string Title => "Scatter List Draggable";
        public string Description =>
            "There exists a Scatter Plot List with draggable points.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs = ScottPlot.DataGen.Consecutive(51);
            double[] ys = ScottPlot.DataGen.Sin(51);

            var scatter = new ScottPlot.Plottable.ScatterPlotListDraggable();
            scatter.AddRange(xs, ys);

            plt.Add(scatter);
        }
    }

    public class ScatterListDraggableLimits : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatterList_draggableLimits";
        public string Title => "Scatter List Draggable Limits";
        public string Description =>
            "A custom function can be used to limit the range of draggable points.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            double[] xs = ScottPlot.DataGen.Consecutive(20);
            double[] ys = ScottPlot.DataGen.Sin(20);
            var scatter = new ScottPlot.Plottable.ScatterPlotListDraggable();
            scatter.AddRange(xs, ys);
            scatter.MarkerSize = 5;
            plt.Add(scatter);

            // use a custom function to limit the movement of points
            static Coordinate MoveBetweenAdjacent(List<double> xs, List<double> ys, int index, Coordinate requested)
            {
                int leftIndex = Math.Max(index - 1, 0);
                int rightIndex = Math.Min(index + 1, xs.Count - 1);

                double newX = requested.X;
                newX = Math.Max(newX, xs[leftIndex]);
                newX = Math.Min(newX, xs[rightIndex]);

                return new Coordinate(newX, requested.Y);
            }

            scatter.MovePointFunc = MoveBetweenAdjacent;
        }
    }
}
