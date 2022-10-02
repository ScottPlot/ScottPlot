using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Recipes
{
    class MultiAxisSecondaryY : IRecipe
    {
        public ICategory Category => new Categories.MultiAxis();
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
            plt.YAxis.Label("Primary Y Axis");
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
        public ICategory Category => new Categories.MultiAxis();
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
            var yAxis3 = plt.AddAxis(Renderable.Edge.Left);
            sigBig.YAxisIndex = yAxis3.AxisIndex;
            yAxis3.Label("Additional Axis");
            yAxis3.Color(sigBig.Color);
        }
    }

    public class RightAxis : IRecipe
    {
        public ICategory Category => new Categories.MultiAxis();
        public string ID => "multiAxis_right";
        public string Title => "Right Y Axis";
        public string Description =>
            "This example demonstrates how to display a Y axis on the right side of the figure. " +
            "The vertical axis to the right of the figure is index 1, so plots must be updated " +
            "to indicate they are to use a nonstandard axis index.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = DataGen.RandomWalk(100);
            var sig = plt.AddSignal(values);
            sig.YAxisIndex = 1;

            plt.YAxis.Ticks(false);
            plt.YAxis.Grid(false);
            plt.YAxis2.Ticks(true);
            plt.YAxis2.Grid(true);
            plt.YAxis2.Label("Value");
            plt.XAxis.Label("Sample Number");
        }
    }

    public class TopAxis : IRecipe
    {
        public ICategory Category => new Categories.MultiAxis();
        public string ID => "multiAxis_top";
        public string Title => "Top X Axis";
        public string Description =>
            "This example demonstrates how to display an X axis above the figure. " +
            "The horizontal axis above the figure is index 1, so plots must be updated " +
            "to indicate they are to use a nonstandard axis index.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = DataGen.RandomWalk(100);
            var sig = plt.AddSignal(values);
            sig.XAxisIndex = 1;

            plt.XAxis.Ticks(false);
            plt.XAxis.Grid(false);
            plt.XAxis2.Ticks(true);
            plt.XAxis2.Grid(true);
            plt.XAxis2.Label("Sample Number");
            plt.YAxis.Label("Value");
        }
    }

    class MultiAxisInvisible : IRecipe
    {
        public ICategory Category => new Categories.MultiAxis();
        public string ID => "multiAxis_invisible";
        public string Title => "Axis Visibility";
        public string Description =>
            "Visibility of axes can be toggled. " +
            "In this example an additional Y axis is added but the primary Y axis is hidden. " +
            "The result is a plot that appears to only have one Y axis.";

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

            // hide the primary Y axis
            plt.YAxis.IsVisible = false;
        }
    }

    class MultiAxisSetLimits : IRecipe
    {
        public ICategory Category => new Categories.MultiAxis();
        public string ID => "multiAxis_limits";
        public string Title => "Setting Multi Axis Limits";
        public string Description =>
            "Axis limits can be set for each axis by indicating which axis index " +
            "you are wish to modify when setting axis limits.";

        public void ExecuteRecipe(Plot plt)
        {
            // signal one on the primary Y axis
            var sig1 = plt.AddSignal(DataGen.Sin(51, mult: 1));
            sig1.YAxisIndex = 0;

            // signal two on the secondary Y axis
            var sig2 = plt.AddSignal(DataGen.Cos(51, mult: 100));
            sig2.YAxisIndex = 1;
            plt.YAxis2.Ticks(true);

            // set axis limits for each axis individually
            plt.SetAxisLimits(yMin: -2, yMax: 2, yAxisIndex: 0);
            plt.SetAxisLimits(yMin: -200, yMax: 200, yAxisIndex: 1);
        }
    }
}
