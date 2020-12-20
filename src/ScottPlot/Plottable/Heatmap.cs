using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace ScottPlot.Plottable
{
    public class Heatmap : IPlottable
    {
        // these fields are updated when the intensities are analyzed
        private double[] NormalizedIntensities;
        private double Min;
        private double Max;
        private int Width;
        private int Height;
        private Bitmap BmpHeatmap;
        private Bitmap BmpScale;

        // these fields are customized by the user
        public string Label;
        public Colormap Colormap;
        public double[] AxisOffsets;
        public double[] AxisMultipliers;
        public double? ScaleMin;
        public double? ScaleMax;
        public double? TransparencyThreshold;
        public double? TransparencyThresholdNormalized;
        public Bitmap BackgroundImage;
        public bool DisplayImageAbove;
        public bool ShowAxisLabels;
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        // call this externally if data changes
        public void UpdateData(double[,] intensities)
        {
            Width = intensities.GetLength(1);
            Height = intensities.GetLength(0);

            double[] intensitiesFlattened = intensities.Cast<double>().ToArray();
            Min = intensitiesFlattened.Min();
            Max = intensitiesFlattened.Max();

            double normalizeMin = (ScaleMin.HasValue && ScaleMin.Value < Min) ? ScaleMin.Value : Min;
            double normalizeMax = (ScaleMax.HasValue && ScaleMax.Value > Max) ? ScaleMax.Value : Max;

            if (TransparencyThreshold.HasValue)
                TransparencyThresholdNormalized = Normalize(TransparencyThreshold.Value, Min, Max, ScaleMin, ScaleMax);

            NormalizedIntensities = Normalize(intensitiesFlattened, null, null, ScaleMin, ScaleMax);

            int[] flatARGB = Colormap.GetRGBAs(NormalizedIntensities, Colormap, minimumIntensity: TransparencyThresholdNormalized ?? 0);
            double[] normalizedValues = Normalize(Enumerable.Range(0, 256).Select(i => (double)i).Reverse().ToArray(), null, null, ScaleMin, ScaleMax);
            int[] scaleRGBA = Colormap.GetRGBAs(normalizedValues, Colormap);

            BmpHeatmap?.Dispose();
            BmpScale?.Dispose();
            BmpHeatmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            BmpScale = new Bitmap(1, 256, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, BmpHeatmap.Width, BmpHeatmap.Height);
            Rectangle rectScale = new Rectangle(0, 0, BmpScale.Width, BmpScale.Height);
            BitmapData bmpData = BmpHeatmap.LockBits(rect, ImageLockMode.ReadWrite, BmpHeatmap.PixelFormat);
            BitmapData scaleBmpData = BmpScale.LockBits(rectScale, ImageLockMode.ReadWrite, BmpScale.PixelFormat);
            Marshal.Copy(flatARGB, 0, bmpData.Scan0, flatARGB.Length);
            Marshal.Copy(scaleRGBA, 0, scaleBmpData.Scan0, scaleRGBA.Length);
            BmpHeatmap.UnlockBits(bmpData);
            BmpScale.UnlockBits(scaleBmpData);
        }

        private double Normalize(double input, double? min = null, double? max = null, double? scaleMin = null, double? scaleMax = null)
            => Normalize(new double[] { input }, min, max, scaleMin, scaleMax)[0];

        private double[] Normalize(double[] input, double? min = null, double? max = null, double? scaleMin = null, double? scaleMax = null)
        {
            min = min ?? input.Min();
            max = max ?? input.Max();

            min = (scaleMin.HasValue && scaleMin.Value < min) ? scaleMin.Value : min;
            max = (scaleMax.HasValue && scaleMax.Value > max) ? scaleMax.Value : max;

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

        public LegendItem[] GetLegendItems()
        {
            var singleLegendItem = new LegendItem()
            {
                label = Label,
                color = Color.Gray,
                lineWidth = 10,
                markerShape = MarkerShape.none
            };
            return new LegendItem[] { singleLegendItem };
        }

        public AxisLimits GetAxisLimits() =>
            ShowAxisLabels ?
            new AxisLimits(-10, BmpHeatmap.Width, -5, BmpHeatmap.Height) :
            new AxisLimits(-3, BmpHeatmap.Width, -3, BmpHeatmap.Height);

        public int PointCount { get => NormalizedIntensities.Length; }

        public void ValidateData(bool deepValidation = false)
        {
            if (NormalizedIntensities is null || BmpHeatmap is null)
                throw new InvalidOperationException("UpdateData() was not called prior to rendering");
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            RenderHeatmap(dims, bmp, lowQuality);
            RenderScale(dims, bmp, lowQuality);
            if (ShowAxisLabels)
                RenderAxis(dims, bmp, lowQuality);
        }

        private void RenderHeatmap(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            {
                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                double minScale = Math.Min(dims.PxPerUnitX, dims.PxPerUnitY);

                if (BackgroundImage != null && !DisplayImageAbove)
                    gfx.DrawImage(
                        BackgroundImage,
                        dims.GetPixelX(0),
                        dims.GetPixelY(0) - (float)(Height * minScale),
                        (float)(Width * minScale), (float)(Height * minScale));

                gfx.DrawImage(
                    BmpHeatmap,
                    dims.GetPixelX(0),
                    dims.GetPixelY(0) - (float)(Height * minScale),
                    (float)(Width * minScale),
                    (float)(Height * minScale));

                if (BackgroundImage != null && DisplayImageAbove)
                    gfx.DrawImage(BackgroundImage,
                        dims.GetPixelX(0),
                        dims.GetPixelY(0) - (float)(Height * minScale),
                        (float)(Width * minScale),
                        (float)(Height * minScale));
            }
        }

        private void RenderScale(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var pen = GDI.Pen(Color.Black))
            using (var brush = GDI.Brush(Color.Black))
            using (var font = GDI.Font(null, 12))
            using (var sf2 = new StringFormat() { LineAlignment = StringAlignment.Far })
            {
                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                float pxFromBottom = 30;
                float pxFromRight = dims.Width - 150;
                float pxWidth = 30;
                RectangleF scaleRect = new RectangleF(pxFromRight, pxFromBottom, pxWidth, dims.DataHeight);
                gfx.DrawImage(BmpScale, scaleRect);
                gfx.DrawRectangle(pen, pxFromRight, pxFromBottom, pxWidth / 2, dims.DataHeight);

                string maxString = ScaleMax.HasValue ? $"{(ScaleMax.Value < Max ? "≥ " : "")}{ ScaleMax.Value:f3}" : $"{Max:f3}";
                string minString = ScaleMin.HasValue ? $"{(ScaleMin.Value > Min ? "≤ " : "")}{ScaleMin.Value:f3}" : $"{Min:f3}";
                gfx.DrawString(maxString, font, brush, new PointF(scaleRect.X + 30, scaleRect.Top));
                gfx.DrawString(minString, font, brush, new PointF(scaleRect.X + 30, scaleRect.Bottom), sf2);
            }
        }

        private void RenderAxis(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (Pen pen = GDI.Pen(Color.Black))
            using (Brush brush = GDI.Brush(Color.Black))
            using (var axisFont = GDI.Font(null, 12))
            using (StringFormat right_centre = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center })
            using (StringFormat centre_top = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near })
            {
                double offset = -2;
                double minScale = Math.Min(dims.PxPerUnitX, dims.PxPerUnitY);
                gfx.DrawString($"{AxisOffsets[0]:f3}", axisFont, brush, dims.GetPixelX(0), dims.GetPixelY(offset), centre_top);
                gfx.DrawString($"{AxisOffsets[0] + AxisMultipliers[0]:f3}", axisFont, brush, new PointF((float)((Width * minScale) + dims.GetPixelX(0)), dims.GetPixelY(offset)), centre_top);
                gfx.DrawString($"{AxisOffsets[1]:f3}", axisFont, brush, dims.GetPixelX(offset), dims.GetPixelY(0), right_centre);
                gfx.DrawString($"{AxisOffsets[1] + AxisMultipliers[1]:f3}", axisFont, brush, new PointF(dims.GetPixelX(offset), dims.GetPixelY(0) - (float)(Height * minScale)), right_centre);
            }
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableHeatmap{label} with {PointCount} points";
        }
    }
}
