using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Recipes
{
    public class StyleDefault : IRecipe
    {
        public ICategory Category => new Categories.Style();
        public string ID => "style_Default";
        public string Title => "Default Style";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.Style(Style.Default);
            plt.Title("Style.Default");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");
        }
    }

    public class StyleMonospace : IRecipe
    {
        public ICategory Category => new Categories.Style();
        public string ID => "style_monospace";
        public string Title => "Monospace Style";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.Style(Style.Monospace);
            plt.Title("Style.Monospace");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");
        }
    }

    public class StyleBlue1 : IRecipe
    {
        public ICategory Category => new Categories.Style();
        public string ID => "style_blue1";
        public string Title => "Blue1 Style";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.Style(Style.Blue1);
            plt.Title("Style.Blue1");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");
        }
    }

    public class StyleBlue2 : IRecipe
    {
        public ICategory Category => new Categories.Style();
        public string ID => "style_blue2";
        public string Title => "Blue2 Style";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.Style(Style.Blue2);
            plt.Title("Style.Blue2");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");
        }
    }

    public class StyleLight1 : IRecipe
    {
        public ICategory Category => new Categories.Style();
        public string ID => "style_light1";
        public string Title => "Light1 Style";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.Style(Style.Light1);
            plt.Title("Style.Light1");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");
        }
    }

    public class StyleGray1 : IRecipe
    {
        public ICategory Category => new Categories.Style();
        public string ID => "style_Gray1";
        public string Title => "Gray1 Style";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.Style(Style.Gray1);
            plt.Title("Style.Gray1");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");
        }
    }

    public class StyleBlack : IRecipe
    {
        public ICategory Category => new Categories.Style();
        public string ID => "style_Black";
        public string Title => "Black Style";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.Style(Style.Black);
            plt.Title("Style.Black");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");
        }
    }

    public class StyleSeaborn : IRecipe
    {
        public ICategory Category => new Categories.Style();
        public string ID => "style_Seaborn";
        public string Title => "Seaborn Style";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.Style(Style.Seaborn);
            plt.Title("Style.Seaborn");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");
        }
    }
}
