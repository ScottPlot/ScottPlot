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

        public PlottableHeatmap(double[][] intensities, ColorMap colorMap, string label)
        {
            this.width = intensities[0].Length;
            this.height = intensities.Length;
            double[] intensitiesFlattened = Flatten(intensities);
            this.intensitiesNormalized = intensitiesFlattened.Select(i => (i - intensitiesFlattened.Min()) / (intensitiesFlattened.Max() - intensitiesFlattened.Min())).ToArray();
            this.colorMap = colorMap;
            this.label = label;
            double max = intensitiesNormalized.Max();
            double min = intensitiesNormalized.Min();

            byte[][] rgb = IntensityToColor(this.intensitiesNormalized, colorMap);

            int[] flatRGBA = ToRGB(rgb);
            bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            Marshal.Copy(flatRGBA, 0, bmpData.Scan0, flatRGBA.Length);
            bmp.UnlockBits(bmpData);
        }

        private int[] ToRGB(byte[][] byteArr)
        {
            int[] rgb = new int[byteArr.Length];
            for (int i = 0; i < byteArr.Length; i++)
            {
                rgb[i] = Color.FromArgb(byteArr[i][0], byteArr[i][1], byteArr[i][2]).ToArgb();
            }
            return rgb;
        }
        private T[] Flatten<T>(T[][] toFlatten)
        {
            T[] flattened = new T[toFlatten.Length * toFlatten[0].Length];
            for (int i = 0; i < toFlatten.Length; i++)
            {
                for (int j = 0; j < toFlatten[i].Length; j++)
                {
                    flattened[i * toFlatten[i].Length + j] = toFlatten[i][j];
                }
            }
            return flattened;
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

        private byte[][] IntensityToColor(double[] intensities, ColorMap colorMap)
        {
            switch (colorMap)
            {
                case ColorMap.grayscale:
                    return intensities.Select(i => new byte[] { (byte)(i * 255), (byte)(i * 255), (byte)(i * 255) }).ToArray();
                    break;
                default:
                    throw new ArgumentException("Colormap not supported");
            }
        }

        public override void Render(Settings settings)
        {
            var interpMode = settings.gfxData.InterpolationMode;
            settings.gfxData.InterpolationMode = InterpolationMode.NearestNeighbor; //This is really important for heatmaps
            double scaleFactor = new double[] { settings.axes.x.span * settings.xAxisScale / width, settings.axes.y.span * settings.yAxisScale / height }.Min();
            settings.gfxData.DrawImage(bmp, 0, 0, (int)(width * scaleFactor), (int)(height * scaleFactor));
            settings.gfxData.InterpolationMode = interpMode;
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableHeatmap{label} with {GetPointCount()} points";
        }
    }
}
