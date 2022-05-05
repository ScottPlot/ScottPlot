using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    // TODO: need an example showing how to update data
    public class SignalConstQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.SignalConst();
        public string ID => "signalconst_quickstart";
        public string Title => "SignalConst Quickstart";
        public string Description =>
            "SignalConst plots pre-processes data to render much faster than Signal plots. " +
            "Pre-processing takes a little time up-front and requires 4x the memory of Signal.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = DataGen.RandomWalk(1_000_000);
            plt.AddSignalConst(values);
            plt.Title("One Million Points");
            plt.Benchmark();
        }
    }

    public class SignalConstGeneric : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.SignalConst();
        public string ID => "signalconst_generic";
        public string Title => "Generic Data Type";
        public string Description =>
            "SignalConst supports other data types beyond just double arrays. " +
            "You can use this plot type to display data in any numerical format that can be cast to a double.";

        public void ExecuteRecipe(Plot plt)
        {
            int[] data = { 2, 6, 3, 8, 5, 6, 1, 9, 7 };
            plt.AddSignalConst(data);
            plt.Title("SignalConst Displaying int[] Data");
        }
    }
}
