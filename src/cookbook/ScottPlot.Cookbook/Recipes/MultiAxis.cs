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
            sigSmall.VerticalAxisIndex = 0;
            sigSmall.HorizontalAxisIndex = 0;
            plt.XAxis.Configure(color: sigSmall.color, ticks: true);
            plt.XAxis.Label = "Primary X Axis";
            plt.YAxis.Configure(color: sigSmall.color, ticks: true);
            plt.YAxis.Label = "Secondary Y Axis";

            // plot another set of data using the secondary axes
            var sigBig = plt.AddSignal(DataGen.Cos(51, mult: 100), sampleRate: 100);
            sigBig.VerticalAxisIndex = 1;
            sigBig.HorizontalAxisIndex = 1;
            plt.YAxis2.Configure(color: sigBig.color, ticks: true);
            plt.YAxis2.Label = "Secondary Y Axis";
            plt.XAxis2.Configure(color: sigBig.color, ticks: true);
            plt.XAxis2.ConfigureAxisLabel(fontBold: false);
            plt.XAxis2.Label = "Secondary X Axis";
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
            sigSmall.VerticalAxisIndex = 0;
            plt.YAxis.Label = "Primary Axis";
            plt.YAxis.Configure(color: sigSmall.color, ticks: true);

            // plot another set of data using an additional axis
            var sigBig = plt.AddSignal(DataGen.Cos(51, mult: 100));
            var yAxis3 = plt.AddAxis(Renderable.Edge.Left, axisIndex: 2);
            sigBig.VerticalAxisIndex = 2;
            yAxis3.Label = "Additional Axis";
            yAxis3.Configure(color: sigBig.color, ticks: true);
        }
    }
}
