using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Ticks
{
    class GridDisable : IRecipe
    {
        public string Category => "Axis";
        public string ID => "grid_disable";
        public string Title => "Disable Grid";
        public string Description => "Visibility of grid lines can be controlled for each axis.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.PlotSignal(DataGen.Sin(51));
            plt.PlotSignal(DataGen.Cos(51));

            plt.XAxis.Grid = false;
            plt.YAxis.Grid = false;
        }
    }

    class GridConfigure : IRecipe
    {
        public string Category => "Axis";
        public string ID => "grid_custom";
        public string Title => "Cursomize Grid";
        public string Description => "Grid lines can be extensively customized using various configuration methods.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.PlotSignal(DataGen.Sin(51));
            plt.PlotSignal(DataGen.Cos(51));

            plt.XAxis.ConfigureMajorGrid(color: Color.FromArgb(100, Color.Black));
            plt.XAxis.ConfigureMinorGrid(enable: true, color: Color.FromArgb(20, Color.Black));
            plt.YAxis.ConfigureMajorGrid(lineWidth: 2, lineStyle: LineStyle.Dash, color: Color.Magenta);
        }
    }
}
