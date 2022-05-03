using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    class TooltipQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Tooltip();
        public string ID => "tooltip_quickstart";
        public string Title => "Tooltip Quickstart";
        public string Description =>
            "Tooltips are annotations that point to an X/Y coordinate on the plot";

        public void ExecuteRecipe(Plot plt)
        {
            double[] ys = DataGen.Sin(50);
            plt.AddSignal(ys);

            plt.AddTooltip(label: "Special Point", x: 17, y: ys[17]);
        }
    }

    class TooltipFont : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Tooltip();
        public string ID => "tooltip_font";
        public string Title => "Tooltip Font";
        public string Description =>
            "Tooltips fonts can be customized";

        public void ExecuteRecipe(Plot plt)
        {
            double[] ys = DataGen.Sin(50);
            plt.AddSignal(ys);

            var tt1 = plt.AddTooltip("Top", 12, ys[12]);
            tt1.Font.Color = System.Drawing.Color.Magenta;
            tt1.Font.Size = 24;

            var tt2 = plt.AddTooltip("Negative Slope", 25, ys[25]);
            tt2.Font.Name = "Comic Sans MS";
            tt2.Font.Bold = true;
        }
    }

    class TooltipColors : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Tooltip();
        public string ID => "tooltip_colors";
        public string Title => "Tooltip Colors";
        public string Description =>
            "Tooltips border and fill styles can be customized";

        public void ExecuteRecipe(Plot plt)
        {
            double[] ys = DataGen.Sin(50);
            plt.AddSignal(ys);

            var tt = plt.AddTooltip("This point has\na negative slope", 25, ys[25]);
            tt.Font.Size = 24;
            tt.Font.Color = System.Drawing.Color.White;
            tt.FillColor = System.Drawing.Color.Blue;
            tt.BorderWidth = 5;
            tt.BorderColor = System.Drawing.Color.Navy;
            tt.ArrowSize = 15;
        }
    }
}
