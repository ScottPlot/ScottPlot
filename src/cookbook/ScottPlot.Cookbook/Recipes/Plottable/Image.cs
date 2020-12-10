using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class ImageQuickstart : IRecipe
    {
        public string Category => "Plottable: Image";
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
            var imagePlot = new ScottPlot.Plottable.Image() { image = monaLisa, x = 10, y = .5 };

            plt.Add(imagePlot);
        }
    }

    public class ImageAlign : IRecipe
    {
        public string Category => "Plottable: Image";
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
            var ip1 = new ScottPlot.Plottable.Image() { image = monaLisa, x = 10 };
            var ip2 = new ScottPlot.Plottable.Image() { image = monaLisa, x = 25, alignment = Alignment.MiddleCenter };
            var ip3 = new ScottPlot.Plottable.Image() { image = monaLisa, x = 40, alignment = Alignment.LowerRight };

            plt.Add(ip1);
            plt.Add(ip2);
            plt.Add(ip3);

            plt.AddPoint(ip1.x, ip1.y, Color.Magenta, size: 20);
            plt.AddPoint(ip2.x, ip2.y, Color.Magenta, size: 20);
            plt.AddPoint(ip3.x, ip3.y, Color.Magenta, size: 20);
        }
    }

    public class ImageRotate : IRecipe
    {
        public string Category => "Plottable: Image";
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
            var ip1 = new ScottPlot.Plottable.Image() { image = monaLisa, x = 10, y = .5, rotation = 30 };
            plt.Add(ip1);
            plt.AddPoint(ip1.x, ip1.y, color: Color.Magenta, size: 20);
        }
    }

    public class ImageBorder : IRecipe
    {
        public string Category => "Plottable: Image";
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
                image = DataGen.SampleImage(),
                x = 10,
                y = .5,
                rotation = 30,
                frameColor = Color.Magenta,
                frameSize = 5,
            });
        }
    }
}
