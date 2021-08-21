using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class CoxcombQuickstart : IRecipe
    {
        public string Category => "Plottable: Coxcomb";
        public string ID => "coxcomb_quickstart";
        public string Title => "Coxcomb Chart";
        public string Description =>
            "A Pie chart where the angle of slices is constant but the radii are not.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 11, 16, 7, 3, 14 };
            var coxcomb = plt.AddCoxcomb(values);
            coxcomb.SliceLabels = new string[] { "Category 1", "Category 2", "Category 3", "Category 4", "Category 5" };

            plt.Legend();
        }
    }
}
