using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ScottPlot
{


    public class PlottableHeatmap : Plottable
    {
        public enum ColorMap
        {
            grayscale
        }

        private int width;
        private int height;
        private double[] intensitiesNormalized;
        private ColorMap colorMap;
        public string label;

        private Bitmap bmp;
        private double minAxisScale;

        public PlottableHeatmap(double[,] intensities, ColorMap colorMap, string label)
        {
            this.width = intensities.GetUpperBound(1) + 1;
            this.height = intensities.GetUpperBound(0) + 1;
            double[] intensitiesFlattened = Flatten(intensities);
            this.intensitiesNormalized = intensitiesFlattened.Select(i => (i - intensitiesFlattened.Min()) / (intensitiesFlattened.Max() - intensitiesFlattened.Min())).ToArray();
            this.colorMap = colorMap;
            this.label = label;
            double max = intensitiesNormalized.Max();
            double min = intensitiesNormalized.Min();

            byte[,] rgb = IntensityToColor(this.intensitiesNormalized, colorMap);

            int[] flatRGBA = ToRGB(rgb);
            bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            Marshal.Copy(flatRGBA, 0, bmpData.Scan0, flatRGBA.Length);
            bmp.UnlockBits(bmpData);
        }

        private int[] ToRGB(byte[,] byteArr)
        {
            int[] rgb = new int[byteArr.GetUpperBound(0) + 1];
            for (int i = 0; i < rgb.Length; i++)
            {
                rgb[i] = Color.FromArgb(byteArr[i, 0], byteArr[i, 1], byteArr[i, 2]).ToArgb();
            }
            return rgb;
        }
        private T[] Flatten<T>(T[,] toFlatten)
        {
            return toFlatten.Cast<T>().ToArray();
        }


        public override LegendItem[] GetLegendItems()
        {
            var singleLegendItem = new LegendItem(label, System.Drawing.Color.Gray, lineWidth: 10, markerShape: MarkerShape.none); //Colours in the legend is kinda difficult...
            return new LegendItem[] { singleLegendItem };
        }

        public override AxisLimits2D GetLimits()
        {
            return new AxisLimits2D(0, bmp.Width, 0, bmp.Height);
        }

        public override int GetPointCount()
        {
            return intensitiesNormalized.Length;
        }

        private byte[,] IntensityToColor(double[] intensities, ColorMap colorMap)
        {
            switch (colorMap)
            {
                case ColorMap.grayscale:
                    byte[,] output = new byte[intensities.Length, 3];
                    for (int i = 0; i < intensities.Length; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            output[i, j] = (byte)(intensities[i] * 255);
                        }
                    }
                    return output;
                    break;
                default:
                    throw new ArgumentException("Colormap not supported");
            }
        }

        public override void Render(Settings settings)
        {
            var interpMode = settings.gfxData.InterpolationMode;
            settings.gfxData.InterpolationMode = InterpolationMode.NearestNeighbor; //This is really important for heatmaps
            double scaleFactor = new double[] { settings.xAxisScale, settings.yAxisScale }.Min();
            settings.gfxData.DrawImage(bmp, (int)settings.GetPixelX(0), (int)(settings.GetPixelY(0) - (height * scaleFactor)), (int)(width * scaleFactor), (int)(height * scaleFactor));
            settings.gfxData.InterpolationMode = interpMode;
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableHeatmap{label} with {GetPointCount()} points";
        }
    }
}
