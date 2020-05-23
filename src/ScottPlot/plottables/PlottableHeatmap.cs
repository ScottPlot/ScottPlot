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
            grayscale,
            grayscaleInverted,
            viridis,
            magma,
            inferno,
            plasma
        }

        private int width;
        private int height;
        private double[] intensitiesNormalized;
        private ColorMap colorMap;
        public string label;

        private Bitmap bmp;
        private Bitmap scale;
        private double min;
        private double max;
        private SolidBrush brush;
        private Pen pen;

        public PlottableHeatmap(double[,] intensities, ColorMap colorMap, string label)
        {
            this.width = intensities.GetUpperBound(1) + 1;
            this.height = intensities.GetUpperBound(0) + 1;
            double[] intensitiesFlattened = Flatten(intensities);
            this.min = intensitiesFlattened.Min();
            this.max = intensitiesFlattened.Max();
            this.brush = new SolidBrush(Color.Black);
            this.pen = new Pen(brush);

            this.intensitiesNormalized = Normalize(intensitiesFlattened);
            this.colorMap = colorMap;
            this.label = label;

            byte[,] rgb = IntensityToColor(this.intensitiesNormalized, colorMap);

            int[] flatRGBA = ToRGB(rgb);
            bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            scale = new Bitmap(1, 200, PixelFormat.Format32bppArgb);
            int[] scaleRGBA = ToRGB(IntensityToColor(Normalize(Invert(Enumerable.Range(0, scale.Height).Select(i => (double)i).ToArray())), colorMap));

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            Rectangle rectScale = new Rectangle(0, 0, scale.Width, scale.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            BitmapData scaleBmpData = scale.LockBits(rectScale, ImageLockMode.ReadWrite, scale.PixelFormat);

            Marshal.Copy(flatRGBA, 0, bmpData.Scan0, flatRGBA.Length);
            Marshal.Copy(scaleRGBA, 0, scaleBmpData.Scan0, scaleRGBA.Length);
            bmp.UnlockBits(bmpData);
            scale.UnlockBits(scaleBmpData);
        }

        private double[] Normalize(double[] input)
        {
            return input.Select(i => (i - input.Min()) / (input.Max() - input.Min())).ToArray();
        }

        private double[] Invert(double[] input) {
            return input.Select(i => 1 - i).ToArray();
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
                    return new Config.ColorMaps.Grayscale().IntensityToRGB(intensities);
                case ColorMap.grayscaleInverted:
                    return new Config.ColorMaps.GrayscaleInverted().IntensityToRGB(intensities);
                case ColorMap.viridis:
                    return new Config.ColorMaps.Viridis().IntensityToRGB(intensities);
                case ColorMap.magma:
                    return new Config.ColorMaps.Magma().IntensityToRGB(intensities);
                case ColorMap.inferno:
                    return new Config.ColorMaps.Inferno().IntensityToRGB(intensities);
                case ColorMap.plasma:
                    return new Config.ColorMaps.Plasma().IntensityToRGB(intensities);
                default:
                    throw new ArgumentException("Colormap not supported");
            }
        }

        public override void Render(Settings settings)
        {
            var interpMode = settings.gfxData.InterpolationMode;
            settings.gfxData.InterpolationMode = InterpolationMode.Bilinear;
            double minScale = settings.xAxisScale < settings.yAxisScale ? settings.xAxisScale : settings.yAxisScale;
            settings.gfxData.DrawImage(bmp, (int)settings.GetPixelX(0), (int)(settings.GetPixelY(0) - (height * minScale)), (int)(width * minScale), (int)(height * minScale));
            RenderScale(settings);
            settings.gfxData.InterpolationMode = interpMode;
        }

        private void RenderScale(Settings settings)
        {
            Rectangle scaleRect = new Rectangle((int)(settings.figureSize.Width * 0.8), 50, 30, 200);
            Rectangle scaleRectOutline = scaleRect;
            scaleRectOutline.Width /= 2;
            settings.gfxData.InterpolationMode = InterpolationMode.NearestNeighbor; //This is necessary for the scale (as its a 1 pixel wide image)
            settings.gfxData.DrawImage(scale, scaleRect);
            settings.gfxData.DrawRectangle(pen, scaleRectOutline);
            settings.gfxData.DrawString($"{max:f3}", new Font(FontFamily.GenericSansSerif, 12), brush, new Point(scaleRect.X + 40, 50));
            settings.gfxData.DrawString($"{min:f3}", new Font(FontFamily.GenericSansSerif, 12), brush, new Point(scaleRect.X + 40, 250), new StringFormat() { LineAlignment = StringAlignment.Far});
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableHeatmap{label} with {GetPointCount()} points";
        }
    }
}
