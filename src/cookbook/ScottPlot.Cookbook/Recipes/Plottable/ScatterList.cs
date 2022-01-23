using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class ScatterListQuickstart : IRecipe
    {
        public string Category => "Plottable: Scatter Plot List";
        public string ID => "scatterList_quickstart";
        public string Title => "Scatter List Quickstart";
        public string Description =>
            "This experimental plot type has add/remove/clear methods like typical lists.";

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
        public string Category => "Plottable: Scatter Plot List";
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
}
