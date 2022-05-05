using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class CoxcombQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Coxcomb();
        public string ID => "coxcomb_quickstart";
        public string Title => "Coxcomb Chart";
        public string Description =>
            "A Pie chart where the angle of slices is constant but the radii are not.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 11, 16, 7, 3, 14 };
            var coxcomb = plt.AddCoxcomb(values);
            coxcomb.FillColors = plt.Palette.GetColors(5, 0, .5);
            coxcomb.SliceLabels = new string[] { "bikes", "blimps", "subs", "saucers", "rockets" };
            plt.Legend();
        }
    }
    public class CoxcombWithIcons : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Coxcomb();
        public string ID => "coxcomb_iconValue";
        public string Title => "Coxcomb Chart with icons";
        public string Description =>
            "A Pie chart where the angle of slices is constant but the radii are not, icons are used for quick reference.";

        public System.Drawing.Image[] CategoryImages = new[]
        {
            System.Drawing.Image.FromFile("Images/bicycle.png"),
            System.Drawing.Image.FromFile("Images/zeppelin.png"),
            System.Drawing.Image.FromFile("Images/submarine.png"),
            System.Drawing.Image.FromFile("Images/ufo.png"),
            System.Drawing.Image.FromFile("Images/rocket.png")
        };

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 11, 16, 7, 3, 14 };
            var coxcomb = plt.AddCoxcomb(values);
            coxcomb.CategoryImages = CategoryImages;
            coxcomb.FillColors = plt.Palette.GetColors(5, 0, .5);
            plt.Legend();
        }
    }
}
