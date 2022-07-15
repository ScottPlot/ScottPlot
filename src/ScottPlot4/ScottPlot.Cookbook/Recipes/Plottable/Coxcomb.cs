using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    public class CoxcombHatch : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Coxcomb();
        public string ID => "coxcomb_hatch";
        public string Title => "Custom Hatching (patterns)";
        public string Description =>
            "Coxcomb charts allow custom hatching of their slices.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 11, 16, 7, 3, 14 };
            var coxcomb = plt.AddCoxcomb(values);
            coxcomb.HatchOptions = new HatchOptions[] {
                new () { Pattern = HatchStyle.StripedUpwardDiagonal, Color = Color.FromArgb(100, Color.Gray) },
                new () { Pattern = HatchStyle.StripedDownwardDiagonal, Color = Color.FromArgb(100, Color.Gray) },
                new () { Pattern = HatchStyle.LargeCheckerBoard, Color = Color.FromArgb(100, Color.Gray) },
                new () { Pattern = HatchStyle.SmallCheckerBoard, Color = Color.FromArgb(100, Color.Gray) },
                new () { Pattern = HatchStyle.LargeGrid, Color = Color.FromArgb(100, Color.Gray) },
            };
            coxcomb.OutlineWidth = 1;

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
