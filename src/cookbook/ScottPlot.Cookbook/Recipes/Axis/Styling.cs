using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Axis
{
    class AxisLabel : IRecipe
    {
        public string ID => "Axis_Label";
        public string Title => "Axis labels";
        public string Description => "Axis labels can be individually customized.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.PlotSignal(DataGen.Sin(51));
            plt.PlotSignal(DataGen.Cos(51));

            // simple way to set an axis label
            plt.XAxis.Label = "Horizontal Axis";

            // advanced customizations are available
            plt.YAxis.ConfigureAxisLabel(
                label: "Vertical Axis",
                color: Color.Magenta,
                fontSize: 24,
                fontName: "Comic Sans MS");
        }
    }

    class AxisColor : IRecipe
    {
        public string ID => "Axis_color";
        public string Title => "Axis color";
        public string Description =>
            "An axis has a label, tick lines, tick marks, tick mark labels, and a line along its edge. " +
            "Set the color of all of these at once by assigning to the Axis's Color field.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.PlotSignal(DataGen.Sin(51));
            plt.PlotSignal(DataGen.Cos(51));

            plt.YAxis.Label = "Vertical Axis";
            plt.YAxis.Color = Color.Magenta;
        }
    }
}
