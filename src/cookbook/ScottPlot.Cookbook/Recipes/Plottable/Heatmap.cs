using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class HeatmapQuickstart : IRecipe
    {
        public string Category => "Plottable: Heatmap";
        public string ID => "heatmap_quickstart";
        public string Title => "Heatmap Quickstart";
        public string Description =>
            "Annotations are labels placed at a X/Y location on the figure (not coordinates of the data area). " +
            "Unlike the Text plottable, annotations do not move as the axes are adjusted.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] imageData = { { 1, 2, 3 },
                                    { 4, 5, 6 } };

            plt.AddHeatmap(imageData);

            plt.YAxis2.SetSizeLimit(min: 100); // add space the right for the scalebar
        }
    }
}
