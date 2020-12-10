using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Axis
{
    class AxisLabel : IRecipe
    {
        public string Category => "Axis";
        public string ID => "Axis_label";
        public string Title => "Axis labels";
        public string Description => "Axis labels can be individually customized.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // simple way to set an axis label
            plt.XAxis.SetLabel("Horizontal Axis");

            // advanced customizations are available
            plt.YAxis.ConfigureLabel(
                label: "Vertical Axis",
                color: Color.Magenta,
                fontSize: 24,
                fontName: "Comic Sans MS");
        }
    }

    class AxisColor : IRecipe
    {
        public string Category => "Axis";
        public string ID => "Axis_color";
        public string Title => "Axis color";
        public string Description =>
            "An axis has a label, tick lines, tick marks, tick mark labels, and a line along its edge. " +
            "Set the color of all of these at once by assigning to the Axis's Color field.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.YAxis.SetLabel("Vertical Axis");
            plt.YAxis.SetColor(Color.Magenta);
        }
    }

    class GridDisable : IRecipe
    {
        public string Category => "Axis";
        public string ID => "axis_gridDisable";
        public string Title => "Disable Grid";
        public string Description => "Visibility of grid lines can be controlled for each axis.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.XAxis.Grid(false);
            plt.YAxis.Grid(false);
        }
    }

    class GridConfigure : IRecipe
    {
        public string Category => "Axis";
        public string ID => "asis_gridCustom";
        public string Title => "Customize Grid Style";
        public string Description => "Grid lines can be extensively customized using various configuration methods.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.XAxis.ConfigureMajorGrid(color: Color.FromArgb(100, Color.Black));
            plt.XAxis.ConfigureMinorGrid(enable: true, color: Color.FromArgb(20, Color.Black));
            plt.YAxis.ConfigureMajorGrid(lineWidth: 2, lineStyle: LineStyle.Dash, color: Color.Magenta);
        }
    }
}
