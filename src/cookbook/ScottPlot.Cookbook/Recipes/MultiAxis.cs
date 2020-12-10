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
            plt.XAxis.SetLabel("Primary X Axis");
            plt.YAxis.SetLabel("Secondary Y Axis");
            plt.XAxis.SetColor(sigSmall.color);
            plt.YAxis.SetColor(sigSmall.color);

            // plot another set of data using the secondary axes
            var sigBig = plt.AddSignal(DataGen.Cos(51, mult: 100), sampleRate: 100);
            sigBig.VerticalAxisIndex = 1;
            sigBig.HorizontalAxisIndex = 1;
            plt.YAxis2.Configure(ticks: true); // ticks weren't visible by default
            plt.XAxis2.Configure(ticks: true); // ticks weren't visible by default
            plt.YAxis2.SetColor(sigBig.color);
            plt.XAxis2.SetColor(sigBig.color);
            plt.YAxis2.SetLabel("Secondary Y Axis");
            plt.XAxis2.SetLabel("Secondary X Axis");
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
            plt.YAxis.SetLabel("Primary Axis");
            plt.YAxis.SetColor(sigSmall.color);

            // plot another set of data using an additional axis
            var sigBig = plt.AddSignal(DataGen.Cos(51, mult: 100));
            var yAxis3 = plt.AddAxis(Renderable.Edge.Left, axisIndex: 2);
            sigBig.VerticalAxisIndex = 2;
            yAxis3.SetLabel("Additional Axis");
            yAxis3.SetColor(sigBig.color);
        }
    }
}
