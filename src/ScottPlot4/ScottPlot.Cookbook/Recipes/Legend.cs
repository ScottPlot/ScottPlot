using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Recipes
{
    public class Legend : IRecipe
    {
        public ICategory Category => new Categories.Legend();
        public string ID => "legend_quickstart";
        public string Title => "Legend Quickstart";
        public string Description => "Add labels to plot items, " +
            "then enable the legend to display a key in the corner of the data area.";

        public void ExecuteRecipe(Plot plt)
        {
            // add a label using the helper methods
            plt.AddSignal(Generate.Sin(), label: "sin");

            // or add a label manually
            var sig2 = plt.AddSignal(Generate.Cos());
            sig2.Label = "Second Label";

            // enable the legend
            plt.Legend();
        }
    }

    class LegendLocation : IRecipe
    {
        public ICategory Category => new Categories.Legend();
        public string ID => "legend_location";
        public string Title => "Legend Location";
        public string Description =>
            "Legends can be placed at various locations within the plot area";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(Generate.Sin(), label: "sin");
            plt.AddSignal(Generate.Cos(), label: "cos");

            var legend = plt.Legend();
            legend.Location = Alignment.UpperCenter;
        }
    }

    class LegendOrientation : IRecipe
    {
        public ICategory Category => new Categories.Legend();
        public string ID => "legend_orientation";
        public string Title => "Legend Orientation";
        public string Description =>
            "Legends can be customized to support horizontal orientation";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51), label: "sin");
            plt.AddSignal(DataGen.Cos(51), label: "cos");

            var legend = plt.Legend(enable: true);
            legend.Orientation = Orientation.Horizontal;
        }
    }
}
