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
            "The Image plottable places a Bitmap at a location in X/Y space." +
            "The image's position will move in space as the axes move, but the " +
            "size of the bitmap will always be the same (matched to the display resolution). ";

        public void ExecuteRecipe(Plot plt)
        {
            // display some sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // create the bitmap we want to display
            Bitmap monaLisa = DataGen.SampleImage();

            // create the image plottable and add it to the plot
            var imagePlot = new ScottPlot.Plottable.Image() { Bitmap = monaLisa, X = 10, Y = .5 };

            plt.Add(imagePlot);
        }
    }

    public class ImageAlign : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Image();
        public string ID => "image_alignment";
        public string Title => "Image Alignment";
        public string Description =>
            "By default the X/Y coordinates define the upper left position of the image, " +
            "but alignment can be customized.";

        public void ExecuteRecipe(Plot plt)
        {
            // display some sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // display an image with 3 different alignments
            Bitmap monaLisa = DataGen.SampleImage();
            var ip1 = new ScottPlot.Plottable.Image() { Bitmap = monaLisa, X = 10 };
            var ip2 = new ScottPlot.Plottable.Image() { Bitmap = monaLisa, X = 25, Alignment = Alignment.MiddleCenter };
            var ip3 = new ScottPlot.Plottable.Image() { Bitmap = monaLisa, X = 40, Alignment = Alignment.LowerRight };

            plt.Add(ip1);
            plt.Add(ip2);
            plt.Add(ip3);

            plt.AddPoint(ip1.X, ip1.Y, Color.Magenta, size: 20);
            plt.AddPoint(ip2.X, ip2.Y, Color.Magenta, size: 20);
            plt.AddPoint(ip3.X, ip3.Y, Color.Magenta, size: 20);
        }
    }

    public class ImageRotate : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Image();
        public string ID => "image_rotation";
        public string Title => "Image Rotation";
        public string Description =>
            "Images can be rotated, but rotation is always relative to the upper left corner.";

        public void ExecuteRecipe(Plot plt)
        {
            // display some sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // place a rotated image on the plot
            Bitmap monaLisa = DataGen.SampleImage();
            var ip1 = new ScottPlot.Plottable.Image() { Bitmap = monaLisa, X = 10, Y = .5, Rotation = 30 };
            plt.Add(ip1);
            plt.AddPoint(ip1.X, ip1.Y, color: Color.Magenta, size: 20);
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
            // display some sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // place an image on the plot
            plt.Add(new ScottPlot.Plottable.Image()
            {
                Bitmap = DataGen.SampleImage(),
                X = 10,
                Y = .5,
                Rotation = 30,
                BorderColor = Color.Magenta,
                BorderSize = 5,
            });
        }
    }
}
