using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Recipes
{
    public class PaletteCategory10 : IRecipe
    {
        public ICategory Category => new Categories.Palette();
        public string ID => "palette_Category10";
        public string Title => "Category10";
        public string Description => "This 10-color palette is the default colorset used by ScottPlot. " +
            "It is the same default colorset used by modern versions of Matplotlib " +
            "(https://matplotlib.org/2.0.2/users/dflt_style_changes.html)";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Category10;

            for (int i = 0; i < plt.Palette.Count(); i++)
            {
                double[] xs = DataGen.Consecutive(100);
                double[] ys = DataGen.Sin(100, phase: -i * .5 / plt.Palette.Count());
                plt.AddScatterLines(xs, ys, lineWidth: 3);
            }

            plt.Title($"{plt.Palette}");
            plt.AxisAuto(0, 0.1);
        }
    }

    public class PaletteCategory20 : IRecipe
    {
        public ICategory Category => new Categories.Palette();
        public string ID => "palette_Category20";
        public string Title => "Category20";
        public string Description => "This 20-color palette is similar to the default, but optimized " +
            "for situations where more than 10 plottables are required. " +
            "Every second color is a lighter version of the color before it. " +
            "This palette was sourced from Matplotlib.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Category20;

            for (int i = 0; i < plt.Palette.Count(); i++)
            {
                double[] xs = DataGen.Consecutive(100);
                double[] ys = DataGen.Sin(100, phase: -i * .5 / plt.Palette.Count());
                plt.AddScatterLines(xs, ys, lineWidth: 3);
            }

            plt.Title($"{plt.Palette}");
            plt.AxisAuto(0, 0.1);
        }
    }

    public class PaletteAurora : IRecipe
    {
        public ICategory Category => new Categories.Palette();
        public string ID => "palette_Aurora";
        public string Title => "Aurora";
        public string Description => "Aurora is a 5-color palette sourced from Nord.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Aurora;

            for (int i = 0; i < plt.Palette.Count(); i++)
            {
                double[] xs = DataGen.Consecutive(100);
                double[] ys = DataGen.Sin(100, phase: -i * .5 / plt.Palette.Count());
                plt.AddScatterLines(xs, ys, lineWidth: 3);
            }

            plt.Title($"{plt.Palette}");
            plt.AxisAuto(0, 0.1);
        }
    }

    public class PaletteFrost : IRecipe
    {
        public ICategory Category => new Categories.Palette();
        public string ID => "palette_Frost";
        public string Title => "Frost";
        public string Description => "Frost is a 4-color palette sourced from Nord.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Frost;

            for (int i = 0; i < plt.Palette.Count(); i++)
            {
                double[] xs = DataGen.Consecutive(100);
                double[] ys = DataGen.Sin(100, phase: -i * .5 / plt.Palette.Count());
                plt.AddScatterLines(xs, ys, lineWidth: 3);
            }

            plt.Title($"{plt.Palette}");
            plt.AxisAuto(0, 0.1);
        }
    }

    public class PaletteNord : IRecipe
    {
        public ICategory Category => new Categories.Palette();
        public string ID => "palette_Nord";
        public string Title => "Nord";
        public string Description => "Nord is a 7-color palette derived from Aurora source from NordConEmu.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Nord;

            for (int i = 0; i < plt.Palette.Count(); i++)
            {
                double[] xs = DataGen.Consecutive(100);
                double[] ys = DataGen.Sin(100, phase: -i * .5 / plt.Palette.Count());
                plt.AddScatterLines(xs, ys, lineWidth: 3);
            }

            plt.Title($"{plt.Palette}");
            plt.AxisAuto(0, 0.1);
        }
    }

    public class PalettePolarNight : IRecipe
    {
        public ICategory Category => new Categories.Palette();
        public string ID => "palette_PolarNight";
        public string Title => "PolarNight";
        public string Description => "PolarNight is a 4-color palette sourced from Nord. " +
            "This palette is optimized for a dark background.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.PolarNight;

            for (int i = 0; i < plt.Palette.Count(); i++)
            {
                double[] xs = DataGen.Consecutive(100);
                double[] ys = DataGen.Sin(100, phase: -i * .5 / plt.Palette.Count());
                plt.AddScatterLines(xs, ys, lineWidth: 3);
            }

            plt.Title($"{plt.Palette}");
            plt.AxisAuto(0, 0.1);
            plt.Style(ScottPlot.Style.Blue2);
        }
    }

    public class PaletteSnowStorm : IRecipe
    {
        public ICategory Category => new Categories.Palette();
        public string ID => "palette_SnowStorm";
        public string Title => "SnowStorm";
        public string Description => "SnowStorm is a 3-color palette sourced from Nord.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.SnowStorm;

            for (int i = 0; i < plt.Palette.Count(); i++)
            {
                double[] xs = DataGen.Consecutive(100);
                double[] ys = DataGen.Sin(100, phase: -i * .5 / plt.Palette.Count());
                plt.AddScatterLines(xs, ys, lineWidth: 3);
            }

            plt.Title($"{plt.Palette}");
            plt.AxisAuto(0, 0.1);
        }
    }

    public class PaletteOneHalf : IRecipe
    {
        public ICategory Category => new Categories.Palette();
        public string ID => "palette_OneHalf";
        public string Title => "OneHalf";
        public string Description => "OneHalf is a 7-color palette sourced from Sublime";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.OneHalf;

            for (int i = 0; i < plt.Palette.Count(); i++)
            {
                double[] xs = DataGen.Consecutive(100);
                double[] ys = DataGen.Sin(100, phase: -i * .5 / plt.Palette.Count());
                plt.AddScatterLines(xs, ys, lineWidth: 3);
            }

            plt.Title($"{plt.Palette}");
            plt.AxisAuto(0, 0.1);
        }
    }

    public class PaletteOneHalfDark : IRecipe
    {
        public ICategory Category => new Categories.Palette();
        public string ID => "palette_OneHalfDark";
        public string Title => "OneHalfDark";
        public string Description => "OneHalfDark is a 7-color palette of colors complimentary to the OneHalf palette " +
            "desaturated and optimized for a dark background. #2e3440 is a recommended background color with this palette.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.OneHalfDark;

            for (int i = 0; i < plt.Palette.Count(); i++)
            {
                double[] xs = DataGen.Consecutive(100);
                double[] ys = DataGen.Sin(100, phase: -i * .5 / plt.Palette.Count());
                plt.AddScatterLines(xs, ys, lineWidth: 3);
            }

            plt.Title($"{plt.Palette}");
            plt.AxisAuto(0, 0.1);
            plt.Style(ScottPlot.Style.Gray1);
            var bnColor = System.Drawing.ColorTranslator.FromHtml("#2e3440");
            plt.Style(figureBackground: bnColor, dataBackground: bnColor);
        }
    }

    public class PaletteCustom : IRecipe
    {
        public ICategory Category => new Categories.Palette();
        public string ID => "palette_Custom";
        public string Title => "Custom";
        public string Description => "A custom palette can be created from an array of HTML color values. " +
            "These colors will be used as the default colors for new plottables added to the plot.";

        public void ExecuteRecipe(Plot plt)
        {
            // custom colors generated using "i want hue" http://medialab.github.io/iwanthue/
            string[] customColors = { "#019d9f", "#7d3091", "#57e075", "#e5b5fa", "#009118" };

            // create a custom palette and set it in the plot module
            plt.Palette = ScottPlot.Palette.FromHtmlColors(customColors);

            for (int i = 0; i < plt.Palette.Count(); i++)
            {
                double[] xs = DataGen.Consecutive(100);
                double[] ys = DataGen.Sin(100, phase: -i * .5 / plt.Palette.Count());
                plt.AddScatterLines(xs, ys, lineWidth: 3);
            }

            plt.Title($"{plt.Palette}");
            plt.AxisAuto(0, 0.1);
        }
    }

    public class PaletteMicrocharts : IRecipe
    {
        public ICategory Category => new Categories.Palette();
        public string ID => "palette_Microcharts";
        public string Title => "Microcharts";
        public string Description => "This is the default 12-color palette used by Microcharts.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Microcharts;

            for (int i = 0; i < plt.Palette.Count(); i++)
            {
                double[] xs = DataGen.Consecutive(100);
                double[] ys = DataGen.Sin(100, phase: -i * .5 / plt.Palette.Count());
                plt.AddScatterLines(xs, ys, lineWidth: 3);
            }

            plt.Title($"{plt.Palette}");
            plt.AxisAuto(0, 0.1);
        }
    }

    public class PaletteColorblindFriendly : IRecipe
    {
        public ICategory Category => new Categories.Palette();
        public string ID => "palette_ColorblindFriendly";
        public string Title => "Colorblind Friendly";
        public string Description => "8-color palette that has good overall variability and " +
            "can be differentiated by individuals with red-green color blindness. " +
            "Colors originated from Wong 2011, https://www.nature.com/articles/nmeth.1618.pdf";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.ColorblindFriendly;

            for (int i = 0; i < plt.Palette.Count(); i++)
            {
                double[] xs = DataGen.Consecutive(100);
                double[] ys = DataGen.Sin(100, phase: -i * .5 / plt.Palette.Count());
                plt.AddScatterLines(xs, ys, lineWidth: 3);
            }

            plt.Title($"{plt.Palette}");
            plt.AxisAuto(0, 0.1);
        }
    }
}
