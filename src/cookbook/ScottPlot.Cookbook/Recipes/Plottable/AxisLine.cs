using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class AxisLine : Recipe
    {
        public override string ID => "PlotType_AxisLine_Introduction";
        public override string Title => "Axis Line Introduction";
        public override string Description => "Horizontal and vertical lines can be placed using HLine() and VLine(). Styling can be customized using arguments.";

        public override void ExecuteRecipe(Plot plt)
        {
            int pointCount = 51;
            double[] x = DataGen.Consecutive(pointCount);
            double[] sin = DataGen.Sin(pointCount);
            double[] cos = DataGen.Cos(pointCount);

            plt.PlotScatter(x, sin);
            plt.PlotScatter(x, cos);

            plt.PlotHLine(y: .85, label: "HLine");
            plt.PlotVLine(x: 23, label: "VLine");
            plt.PlotVLine(x: 33, label: "VLine too", color: Color.Magenta, lineWidth: 3, lineStyle: LineStyle.Dot);

            plt.Grid(lineStyle: LineStyle.Dot);
            plt.Legend();
        }
    }

    public class Draggable : Recipe
    {
        public override string ID => "PlotType_AxisLine_Draggable";
        public override string Title => "Draggable Axis Lines";
        public override string Description => "Use arguments to enable draggable lines (with optional limits).";

        public override void ExecuteRecipe(Plot plt)
        {
            int pointCount = 51;
            double[] x = DataGen.Consecutive(pointCount);
            double[] sin = DataGen.Sin(pointCount);
            double[] cos = DataGen.Cos(pointCount);

            plt.PlotScatter(x, sin);
            plt.PlotScatter(x, cos);

            plt.PlotHLine(y: .85, draggable: true, dragLimitLower: -1, dragLimitUpper: +1);
            plt.PlotVLine(x: 23, draggable: true, dragLimitLower: 0, dragLimitUpper: 50);

            plt.Grid(lineStyle: LineStyle.Dot);
        }
    }
}
