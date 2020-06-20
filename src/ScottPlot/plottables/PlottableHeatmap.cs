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
using System.Transactions;
using ScottPlot.Config;
using ScottPlot.Drawing;

namespace ScottPlot
{

#pragma warning disable CS0618 // Type or member is obsolete
    public class PlottableHeatmap : Plottable
    {
        private int width;
        private int height;
        private double[] intensitiesNormalized;
        private Colormap colorMap;
        public string label;
        private double[] axisOffsets;
        private double[] axisMultipliers;
        private double? scaleMin;
        private double? scaleMax;
        private double? transparencyThreshold;
        private Bitmap backgroundImage;
        private bool displayImageAbove;
        private bool drawAxisLabels;

        private Bitmap bmp;
        private Bitmap scale;
        private double min;
        private double max;
        private SolidBrush brush;
        private Pen pen;

        public PlottableHeatmap(double[,] intensities, Colormap colormap, string label, double[] axisOffsets, double[] axisMultipliers, double? scaleMin, double? scaleMax, double? transparencyThreshold, Bitmap backgroundImage, bool displayImageAbove, bool drawAxisLabels)
        {
            this.width = intensities.GetLength(1);
            this.height = intensities.GetLength(0);
            double[] intensitiesFlattened = Flatten(intensities);
            this.min = intensitiesFlattened.Min();
            this.max = intensitiesFlattened.Max();
            this.brush = new SolidBrush(Color.Black);
            this.pen = new Pen(brush);
            this.axisOffsets = axisOffsets;
            this.axisMultipliers = axisMultipliers;
            this.colorMap = colormap;
            this.label = label;
            this.scaleMin = scaleMin;
            this.scaleMax = scaleMax;
            this.backgroundImage = backgroundImage;
            this.displayImageAbove = displayImageAbove;
            this.drawAxisLabels = drawAxisLabels;

            double normalizeMin = min;
            double normalizeMax = max;

            if (scaleMin.HasValue && scaleMin.Value < min)
                normalizeMin = scaleMin.Value;

            if (scaleMax.HasValue && scaleMax.Value > max)
                normalizeMin = scaleMax.Value;

            if (transparencyThreshold.HasValue)
                this.transparencyThreshold = Normalize(new double[] { transparencyThreshold.Value }, min, max, scaleMin, scaleMax)[0];

            intensitiesNormalized = Normalize(intensitiesFlattened, null, null, scaleMin, scaleMax);

            int[] flatARGB = Colormap.GetRGBAs(intensitiesNormalized, colormap, minimumIntensity: transparencyThreshold ?? 0);

            bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            scale = new Bitmap(1, 256, PixelFormat.Format32bppArgb);

            double[] normalizedValues = Normalize(Enumerable.Range(0, scale.Height).Select(i => (double)i).Reverse().ToArray(), null, null, scaleMin, scaleMax);
            int[] scaleRGBA = Colormap.GetRGBAs(normalizedValues, colormap);

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            Rectangle rectScale = new Rectangle(0, 0, scale.Width, scale.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            BitmapData scaleBmpData = scale.LockBits(rectScale, ImageLockMode.ReadWrite, scale.PixelFormat);

            Marshal.Copy(flatARGB, 0, bmpData.Scan0, flatARGB.Length);
            Marshal.Copy(scaleRGBA, 0, scaleBmpData.Scan0, scaleRGBA.Length);
            bmp.UnlockBits(bmpData);
            scale.UnlockBits(scaleBmpData);
        }

        private double[] Normalize(double[] input, double? min = null, double? max = null, double? scaleMin = null, double? scaleMax = null)
        {
            min = min ?? input.Min();
            max = max ?? input.Max();

            if (scaleMin.HasValue && scaleMin.Value < min)
            {
                min = scaleMin.Value;
            }

            if (scaleMax.HasValue && scaleMax.Value > max)
            {
                max = scaleMax.Value;
            }

            double[] normalized = input.AsParallel().AsOrdered().Select(i => (i - min.Value) / (max.Value - min.Value)).ToArray();
            if (scaleMin.HasValue)
            {
                double threshold = (scaleMin.Value - min.Value) / (max.Value - min.Value);
                normalized = normalized.AsParallel().AsOrdered().Select(i => i < threshold ? threshold : i).ToArray();
            }

            if (scaleMax.HasValue)
            {
                double threshold = (scaleMax.Value - min.Value) / (max.Value - min.Value);
                normalized = normalized.AsParallel().AsOrdered().Select(i => i > threshold ? threshold : i).ToArray();
            }

            return normalized;
        }

        private double[] Invert(double[] input)
        {
            return input.Select(i => 1 - i).ToArray();
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
            if (drawAxisLabels)
            {
                return new AxisLimits2D(-10, bmp.Width, -5, bmp.Height);
            }
            else
            {
                return new AxisLimits2D(-3, bmp.Width, -3, bmp.Height);
            }
        }

        public override int GetPointCount()
        {
            return intensitiesNormalized.Length;
        }

        public override void Render(Settings settings)
        {
            var interpMode = settings.gfxData.InterpolationMode;
            settings.gfxData.InterpolationMode = InterpolationMode.NearestNeighbor;
            double minScale = settings.xAxisScale < settings.yAxisScale ? settings.xAxisScale : settings.yAxisScale;
            if (backgroundImage != null && !displayImageAbove)
            {
                settings.gfxData.DrawImage(backgroundImage, (float)settings.GetPixelX(0), (float)(settings.GetPixelY(0) - (height * minScale)), (float)(width * minScale), (float)(height * minScale));
            }
            settings.gfxData.DrawImage(bmp, (float)settings.GetPixelX(0), (float)(settings.GetPixelY(0) - (height * minScale)), (float)(width * minScale), (float)(height * minScale));
            if (backgroundImage != null && displayImageAbove)
            {
                settings.gfxData.DrawImage(backgroundImage, (float)settings.GetPixelX(0), (float)(settings.GetPixelY(0) - (height * minScale)), (float)(width * minScale), (float)(height * minScale));
            }
            RenderScale(settings);
            if (drawAxisLabels)
            {
                RenderAxis(settings, minScale);
            }
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
            string maxString = scaleMax.HasValue ? $"{(scaleMax.Value < max ? "≥ " : "")}{ scaleMax.Value:f3}" : $"{max:f3}";
            string minString = scaleMin.HasValue ? $"{(scaleMin.Value > min ? "≤ " : "")}{scaleMin.Value:f3}" : $"{min:f3}";

            settings.gfxFigure.DrawString(maxString, new Font(FontFamily.GenericSansSerif, 12), brush, new Point(scaleRect.X + 30, scaleRect.Top));
            settings.gfxFigure.DrawString(minString, new Font(FontFamily.GenericSansSerif, 12), brush, new Point(scaleRect.X + 30, scaleRect.Bottom), new StringFormat() { LineAlignment = StringAlignment.Far });

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
