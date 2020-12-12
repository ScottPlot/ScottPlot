using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Recipes
{
    class MultiAxisSecondaryY : IRecipe
    {
        public string Category => "Multi-Axis";
        public string ID => "multiAxis_primary";
        public string Title => "Primary Axes";
        public string Description =>
            "Plots always have 4 fundamental axes available to work with. " +
            "Primary axes (XAxis and YAxis) are axis index 0. " +
            "Secondary axes (XAxis2 and YAxis2) are axis index 1." +
            "By default primary axes are totally visible, and secondary axes have ticks hidden and no label. " +
            "Sometimes the top axis (XAxis2) is given a label to simulate a plot title.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot one set of data using the primary Y axis
            var sigSmall = plt.AddSignal(DataGen.Sin(51, mult: 1), sampleRate: 1);
            sigSmall.YAxisIndex = 0;
            sigSmall.XAxisIndex = 0;
            plt.XAxis.Label("Primary X Axis");
            plt.YAxis.Label("Secondary Y Axis");
            plt.XAxis.Color(sigSmall.Color);
            plt.YAxis.Color(sigSmall.Color);

            // plot another set of data using the secondary axes
            var sigBig = plt.AddSignal(DataGen.Cos(51, mult: 100), sampleRate: 100);
            sigBig.YAxisIndex = 1;
            sigBig.XAxisIndex = 1;
            plt.YAxis2.Ticks(true); // ticks weren't visible by default
            plt.XAxis2.Ticks(true); // ticks weren't visible by default
            plt.YAxis2.Color(sigBig.Color);
            plt.XAxis2.Color(sigBig.Color);
            plt.YAxis2.Label("Secondary Y Axis");
            plt.XAxis2.Label("Secondary X Axis");
        }
    }

    class MultiAxisAdditional : IRecipe
    {
        public string Category => "Multi-Axis";
        public string ID => "multiAxis_additional";
        public string Title => "Additional Y Axis";
        public string Description =>
            "Additional axes can be added on any edge. " +
            "Additional axes stack away from the plot area.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot one set of data using the primary Y axis
            var sigSmall = plt.AddSignal(DataGen.Sin(51, mult: 1));
            sigSmall.YAxisIndex = 0;
            plt.YAxis.Label("Primary Axis");
            plt.YAxis.Color(sigSmall.Color);

            // plot another set of data using an additional axis
            var sigBig = plt.AddSignal(DataGen.Cos(51, mult: 100));
            var yAxis3 = plt.AddAxis(Renderable.Edge.Left, axisIndex: 2);
            sigBig.YAxisIndex = 2;
            yAxis3.Label("Additional Axis");
            yAxis3.Color(sigBig.Color);
        }
    }
}
