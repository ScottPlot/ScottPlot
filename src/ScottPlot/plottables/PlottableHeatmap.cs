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
using System.Threading.Tasks;

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
            plasma,
            turbo
        }

        private int width;
        private int height;
        private double[] intensitiesNormalized;
        private ColorMap colorMap;
        public string label;
        private double[] axisOffsets;
        private double[] axisMultipliers;

        private Bitmap bmp;
        private Bitmap scale;
        private double min;
        private double max;
        private SolidBrush brush;
        private Pen pen;

        public PlottableHeatmap(double[,] intensities, ColorMap colorMap, string label, double[] axisOffsets, double[] axisMultipliers)
        {
            long start = DateTime.Now.Ticks;
            this.width = intensities.GetUpperBound(1) + 1;
            this.height = intensities.GetUpperBound(0) + 1;
            double[] intensitiesFlattened = Flatten(intensities);
            this.min = intensitiesFlattened.Min();
            this.max = intensitiesFlattened.Max();
            this.brush = new SolidBrush(Color.Black);
            this.pen = new Pen(brush);
            this.axisOffsets = axisOffsets;
            this.axisMultipliers = axisMultipliers;
            this.colorMap = colorMap;
            this.label = label;


            var intensityTask = NormalizeAsync(intensitiesFlattened);
            intensityTask.Wait();
            this.intensitiesNormalized = intensityTask.Result;

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
            Debug.WriteLine(DateTime.Now.Ticks - start);
        }

        private double[] Normalize(double[] input)
        {
            double min = input.Min(); //You would think that C# is capable of this optimization itself, but this is about 5x faster for a 100x100 image
            double max = input.Max();
            return input.Select(i => (i - min) / (max - min)).ToArray();
        }

        private async Task<double[]> NormalizeAsync(double[] input)
        {
            double min = input.Min();
            double max = input.Max();
            int threads = 12;
            int stride = (input.Length - 1) / threads;
            int remainder = (input.Length - 1) % threads;
            List<Task<List<double>>> tasks = new List<Task<List<double>>>(threads);

            for (int i = 0; i < threads; i++)
            {
                int extra = i == threads - 1 ? remainder : 0;
                tasks.Add(NormalizeSegment(input, min, max, i * stride, stride + extra));
            }

            await Task.WhenAll(tasks);
            double[] results = tasks.Select(t => t.Result).SelectMany(d => d).ToArray();
            return results;
        }

        private async Task<List<double>> NormalizeSegment(double[] input, double min, double max, int index, int stride)
        {
            List<double> output = new List<double>();
            for (int i = 0; i < stride; i++)
            {
                output.Add((input[i + index] - min) / (max - min));
            }
            return output;
        }

        private double[] Invert(double[] input)
        {
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
            return new AxisLimits2D(-10, bmp.Width, -5, bmp.Height);
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
                case ColorMap.turbo:
                    return new Config.ColorMaps.Turbo().IntensityToRGB(intensities);
                default:
                    throw new ArgumentException("Colormap not supported");
            }
        }

        public override void Render(Settings settings)
        {
            var interpMode = settings.gfxData.InterpolationMode;
            settings.gfxData.InterpolationMode = InterpolationMode.NearestNeighbor;
            double minScale = settings.xAxisScale < settings.yAxisScale ? settings.xAxisScale : settings.yAxisScale;
            settings.gfxData.DrawImage(bmp, (int)settings.GetPixelX(0), (int)(settings.GetPixelY(0) - (height * minScale)), (int)(width * minScale), (int)(height * minScale));
            RenderScale(settings);
            RenderAxis(settings, minScale);
            settings.gfxData.InterpolationMode = interpMode;
        }

        private void RenderScale(Settings settings)
        {
            Rectangle scaleRect = new Rectangle(settings.figureSize.Width - 150, settings.layout.xLabelHeight, 30, settings.layout.plot.Height - settings.layout.xLabelHeight - settings.layout.xScaleHeight - settings.layout.titleHeight);

            Rectangle scaleRectOutline = scaleRect;
            scaleRectOutline.Width /= 2;
            var interpMode = settings.gfxFigure.InterpolationMode;

            settings.gfxFigure.InterpolationMode = InterpolationMode.NearestNeighbor; //This is necessary for the scale (as its a 1 pixel wide image)
            settings.gfxFigure.DrawImage(scale, scaleRect);
            settings.gfxFigure.DrawRectangle(pen, scaleRectOutline);
            settings.gfxFigure.DrawString($"{max:f3}", new Font(FontFamily.GenericSansSerif, 12), brush, new Point(scaleRect.X + 30, scaleRect.Top));
            settings.gfxFigure.DrawString($"{min:f3}", new Font(FontFamily.GenericSansSerif, 12), brush, new Point(scaleRect.X + 30, scaleRect.Bottom), new StringFormat() { LineAlignment = StringAlignment.Far });

            settings.gfxFigure.InterpolationMode = interpMode;
        }

        private void RenderAxis(Settings settings, double minScale)
        {
            StringFormat right_centre = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };
            StringFormat centre_top = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near };
            Font axisFont = new Font(FontFamily.GenericSansSerif, 12);
            double offset = -2;

            settings.gfxData.DrawString($"{axisOffsets[0]:f3}", axisFont, brush, settings.GetPixel(0, offset), centre_top);
            settings.gfxData.DrawString($"{axisOffsets[0] + axisMultipliers[0]:f3}", axisFont, brush, new PointF((float)((width * minScale) + settings.GetPixelX(0)), (float)settings.GetPixelY(offset)), centre_top);

            settings.gfxData.DrawString($"{axisOffsets[1]:f3}", axisFont, brush, settings.GetPixel(offset, 0), right_centre);
            settings.gfxData.DrawString($"{axisOffsets[1] + axisMultipliers[1]:f3}", axisFont, brush, new PointF((float)settings.GetPixelX(offset), (float)(settings.GetPixelY(0) - (height * minScale))), right_centre);
        }
        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableHeatmap{label} with {GetPointCount()} points";
        }
    }
}
