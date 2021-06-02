using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Recipes
{
    public class StyleDefault : IRecipe
    {
        public string Category => "Style";
        public string ID => "style_Default";
        public string Title => "Default";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // customize the plot style
            plt.Style(Style.Default);
        }
    }

    public class StyleControl : IRecipe
    {
        public string Category => "Style";
        public string ID => "style_Control";
        public string Title => "Control";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // customize the plot style
            plt.Style(Style.Control);
        }
    }

    public class StyleBlue1 : IRecipe
    {
        public string Category => "Style";
        public string ID => "style_blue1";
        public string Title => "Blue1";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // customize the plot style
            plt.Style(Style.Blue1);
        }
    }

    public class StyleBlue2 : IRecipe
    {
        public string Category => "Style";
        public string ID => "style_blue2";
        public string Title => "Blue2";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // customize the plot style
            plt.Style(Style.Blue2);
        }
    }

    public class StyleBlue3 : IRecipe
    {
        public string Category => "Style";
        public string ID => "style_blue3";
        public string Title => "Blue3";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // customize the plot style
            plt.Style(Style.Blue3);
        }
    }

    public class StyleLight1 : IRecipe
    {
        public string Category => "Style";
        public string ID => "style_light1";
        public string Title => "Light1";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // customize the plot style
            plt.Style(Style.Light1);
        }
    }

    public class StyleLight2 : IRecipe
    {
        public string Category => "Style";
        public string ID => "style_light2";
        public string Title => "Light2";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // customize the plot style
            plt.Style(Style.Light2);
        }
    }

    public class StyleGray1 : IRecipe
    {
        public string Category => "Style";
        public string ID => "style_Gray1";
        public string Title => "Gray1";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // customize the plot style
            plt.Style(Style.Gray1);
        }
    }

    public class StyleGray2 : IRecipe
    {
        public string Category => "Style";
        public string ID => "style_Gray2";
        public string Title => "Gray2";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // customize the plot style
            plt.Style(Style.Gray2);
        }
    }

    public class StyleBlack : IRecipe
    {
        public string Category => "Style";
        public string ID => "style_Black";
        public string Title => "Black";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // customize the plot style
            plt.Style(Style.Black);
        }
    }

    public class StyleSeaborn : IRecipe
    {
        public string Category => "Style";
        public string ID => "style_Seaborn";
        public string Title => "Seaborn";
        public string Description => "Customize many plot features using style presets";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // customize the plot style
            plt.Style(Style.Seaborn);
        }
    }
}
