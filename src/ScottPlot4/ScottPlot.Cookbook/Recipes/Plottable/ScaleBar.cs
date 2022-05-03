using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class ScaleBarQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.ScaleBar();
        public string ID => "scalebar_quickstart";
        public string Title => "Scale Bar";
        public string Description =>
            "An L-shaped scalebar can be added in the corner of any plot. " +
            "Set the vertical or horizontal sizer to zero and the scale bar will only span one dimension.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // remove traditional scale indicators
            plt.Grid(enable: false);
            plt.Frameless();

            // add an L-shaped scalebar
            plt.AddScaleBar(5, .25, "100 ms", "250 mV");
        }
    }

    public class ScaleBarHorizontal : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.ScaleBar();
        public string ID => "scalebar_horizontal";
        public string Title => "Horizontal Scale Bar";
        public string Description =>
            "Set the vertical or horizontal sizer to zero and the scale bar will only span one dimension.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data 
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // show only the left axis
            plt.XAxis.Hide();
            plt.XAxis2.Hide();
            plt.YAxis2.Hide();
            plt.Grid(enable: false);

            // add a horizontal scale bar (no Y height)
            plt.AddScaleBar(5, 0, "100 ms", null);
        }
    }

    public class StyledScaleBar : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.ScaleBar();
        public string ID => "scalebar_styled";
        public string Title => "Styled Scale Bar";
        public string Description =>
            "An L-shaped scalebar can be added in the corner of any plot. " +
            "Set the vertical or horizontal sizer to zero and the scale bar will only span one dimension.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // remove traditional scale indicators
            plt.Grid(enable: false);
            plt.Frameless();

            // add an L-shaped scalebar
            plt.AddScaleBar(5, .25, "100 ms", "250 mV");

            // add style
            plt.Style(Style.Black);
        }
    }
}
