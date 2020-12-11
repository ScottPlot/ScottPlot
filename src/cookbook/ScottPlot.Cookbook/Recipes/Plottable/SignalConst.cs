using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    // TODO: need an example showing how to update data
    public class SignalConstQuickstart : IRecipe
    {
        public string Category => "Plottable: SignalConst";
        public string ID => "signalconst_quickstart";
        public string Title => "SignalConst Quickstart";
        public string Description =>
            "SignalConst plots pre-processes data to render much faster than Signal plots. " +
            "Pre-processing takes a little time up-front and requires 4x the memory of Signal.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = DataGen.RandomWalk(1_000_000);
            plt.AddSignal(values);
            plt.Title("One Million Points");
            plt.Benchmark();
        }
    }
}
