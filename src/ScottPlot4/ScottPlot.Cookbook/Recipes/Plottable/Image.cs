using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class ImageQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Image();
        public string ID => "image_quickstart";
        public string Title => "Image Quickstart";
        public string Description =>
            "The Image plottable places a Bitmap at coordinte in axis space.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            Bitmap monaLisa = DataGen.SampleImage();

            plt.AddImage(monaLisa, 10, .5);
        }
    }

    public class ImageAlign : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Image();
        public string ID => "image_alignment";
        public string Title => "Image Alignment";
        public string Description =>
            "By default the X/Y coordinates define the upper left position of the image, " +
            "but alignment can be customized by defining the anchor.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            Bitmap monaLisa = DataGen.SampleImage();

            plt.AddImage(monaLisa, 10, 0);
            plt.AddPoint(10, 0, Color.Magenta, size: 20);

            plt.AddImage(monaLisa, 25, 0, anchor: Alignment.MiddleCenter);
            plt.AddPoint(25, 0, Color.Magenta, size: 20);

            plt.AddImage(monaLisa, 40, 0, anchor: Alignment.LowerRight);
            plt.AddPoint(40, 0, Color.Magenta, size: 20);
        }
    }

    public class ImageRotate : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Image();
        public string ID => "image_rotation";
        public string Title => "Image Rotation";
        public string Description =>
            "Images can be rotated around the position defined by their anchor.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            Bitmap monaLisa = DataGen.SampleImage();

            plt.AddImage(monaLisa, 10, .5, rotation: 30);
            plt.AddPoint(10, .5, color: Color.Magenta, size: 20);

            plt.AddImage(monaLisa, 25, 0, rotation: -30);
            plt.AddPoint(25, 0, color: Color.Magenta, size: 20);

            plt.AddImage(monaLisa, 45, 0, rotation: 30, anchor: Alignment.MiddleCenter);
            plt.AddPoint(45, 0, color: Color.Magenta, size: 20);
        }
    }

    public class ImageBorder : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Image();
        public string ID => "image_border";
        public string Title => "Image Border";
        public string Description =>
            "The borders of images can be customized.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            Bitmap monaLisa = DataGen.SampleImage();

            var img = plt.AddImage(monaLisa, 10, .5, rotation: 30);
            img.BorderColor = Color.Magenta;
            img.BorderSize = 5;
        }
    }

    public class ImageScaling : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Image();
        public string ID => "image_scaling";
        public string Title => "Image Scaling";
        public string Description =>
            "Size of the image (in relative pixel units) can be adjusted.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            Bitmap monaLisa = DataGen.SampleImage();

            plt.AddImage(monaLisa, 5, .5);
            plt.AddImage(monaLisa, 15, .5, scale: .5);
            plt.AddImage(monaLisa, 30, .5, scale: 2);
        }
    }

    public class ImageStretching : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Image();
        public string ID => "image_stretching";
        public string Title => "Image Stretching";
        public string Description =>
            "By default image dimensions are in pixel units so they are not stretched " +
            "as axes are manipulated. However, users have the option to define " +
            "image dimensions in axis units. In this case, corners of images will remain " +
            "fixed on the coordinate system and will get stretched as axes are stretched.";

        public void ExecuteRecipe(Plot plt)
        {
            // display some sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            Bitmap monaLisa = DataGen.SampleImage();

            var img = plt.AddImage(monaLisa, 10, .5);
            img.HeightInAxisUnits = 1;
            img.WidthInAxisUnits = 30;

            // 4 corners of the image remain fixed in coordinate space
            plt.AddPoint(10, .5, color: Color.Magenta, size: 20);
            plt.AddPoint(40, .5, color: Color.Green, size: 20);
            plt.AddPoint(10, -.5, color: Color.Green, size: 20);
            plt.AddPoint(40, -.5, color: Color.Green, size: 20);
        }
    }

    public class ImageClipping : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Image();
        public string ID => "image_clipping";
        public string Title => "Image Clipping";
        public string Description =>
            "Images can be clipped to an arbitrary polygon";

        public void ExecuteRecipe(Plot plt)
        {
            Bitmap bmp = DataGen.SampleImage();
            var img = plt.AddImage(bmp, 0, bmp.Height);
            img.HeightInAxisUnits = bmp.Height;
            img.WidthInAxisUnits = bmp.Width;
            img.ClippingPoints = new Coordinate[]
            {
                new Coordinate(30, 15),
                new Coordinate(55, 40),
                new Coordinate(60, 45),
                new Coordinate(80, 60),
                new Coordinate(40, 95),
                new Coordinate(15, 90),
                new Coordinate(5, 50),
            };
        }
    }

    public class ImageAntiAliasing : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Image();
        public string ID => "images_smooth";
        public string Title => "Image Anti-Aliasing";
        public string Description =>
            "Images have an option to enable or disable anti-aliasing";

        public void ExecuteRecipe(Plot plt)
        {
            Bitmap bmp = DataGen.SampleImage();
            var imgTop = plt.AddImage(bmp, 0, 2.2);
            imgTop.HeightInAxisUnits = 1;
            imgTop.WidthInAxisUnits = 30;
            imgTop.AntiAlias = true;

            var imgBottom = plt.AddImage(bmp, 0, 1.0);
            imgBottom.HeightInAxisUnits = 1;
            imgBottom.WidthInAxisUnits = 30;
            imgBottom.AntiAlias = false;
        }
    }
}
