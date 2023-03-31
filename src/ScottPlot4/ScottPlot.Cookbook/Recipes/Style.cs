using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes
{
    public class StyleDefault : IRecipe
    {
        public ICategory Category => new Categories.Style();
        public string ID => "style_Default";
        public string Title => "Default Plot Style";
        public string Description => "This example demonstrates the default plot style.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));
            plt.Title("Default Style");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");
        }
    }

    public class StyleBackground : IRecipe
    {
        public ICategory Category => new Categories.Style();
        public string ID => "style_background";
        public string Title => "Background Color";
        public string Description =>
            "Plots have two background colors that can be individually customized. " +
            "The figure background is the background of the whole image. " +
            "The data background is the background of the rectangle that contains the data. " +
            "Both background types support transparency, although PNG file export is required.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.Style(
                figureBackground: Color.LightSkyBlue,
                dataBackground: Color.Salmon);
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

    class DataBackgroundImage : IRecipe
    {
        public ICategory Category => new Categories.Style();
        public string ID => "misc_background_image_data";
        public string Title => "Data Background Image";
        public string Description =>
            "A backgorund image can be drawn behind the data area. " +
            "Users to do this may want to make grid lines semitransparent.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51), 1, Color.Yellow);
            plt.AddSignal(DataGen.Cos(51), 1, Color.Magenta);

            Bitmap monaLisaBmp = ScottPlot.DataGen.SampleImage();

            plt.Style(
                grid: Color.FromArgb(50, Color.White),
                dataBackgroundImage: monaLisaBmp);
        }
    }

    class FigureBackgroundImage : IRecipe
    {
        public ICategory Category => new Categories.Style();
        public string ID => "misc_background_image_figure";
        public string Title => "Figure Background Image";
        public string Description =>
            "A backgorund image can be drawn behind the entire figure. " +
            "If you do this you likely want to make your data background transparent.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51), 1, Color.Yellow);
            plt.AddSignal(DataGen.Cos(51), 1, Color.Magenta);

            Bitmap monaLisaBmp = ScottPlot.DataGen.SampleImage();

            plt.Style(
                grid: Color.FromArgb(50, Color.White),
                tick: Color.White,
                dataBackground: Color.FromArgb(50, Color.White),
                figureBackgroundImage: monaLisaBmp);
        }
    }
}
