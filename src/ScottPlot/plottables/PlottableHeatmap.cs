using ScottPlot.Config;
using ScottPlot.Diagnostic.Attributes;
using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace ScottPlot
{

#pragma warning disable CS0618 // Type or member is obsolete
    public class PlottableHeatmap : Plottable, IPlottable
    {
        // these fields are updated when the intensities are analyzed
        private double[] NormalizedIntensities;
        private double min;
        private double max;
        private int width;
        private int height;
        private Bitmap BmpHeatmap;
        private Bitmap BmpScale;

        // these fields are customized by the user
        public string label;
        public Colormap Colormap;
        public double[] AxisOffsets;
        public double[] AxisMultipliers;
        public double? ScaleMin;
        public double? ScaleMax;
        public double? TransparencyThreshold;
        public Bitmap BackgroundImage;
        public bool DisplayImageAbove;
        public bool ShowAxisLabels;

        // call this externally if data changes
        public void UpdateData(double[,] intensities)
        {
            width = intensities.GetLength(1);
            height = intensities.GetLength(0);

            double[] intensitiesFlattened = intensities.Cast<double>().ToArray();
            min = intensitiesFlattened.Min();
            max = intensitiesFlattened.Max();

            double normalizeMin = (ScaleMin.HasValue && ScaleMin.Value < min) ? ScaleMin.Value : min;
            double normalizeMax = (ScaleMax.HasValue && ScaleMax.Value > max) ? ScaleMax.Value : max;

            if (TransparencyThreshold.HasValue)
                TransparencyThreshold = Normalize(TransparencyThreshold.Value, min, max, ScaleMin, ScaleMax);

            NormalizedIntensities = Normalize(intensitiesFlattened, null, null, ScaleMin, ScaleMax);

            int[] flatARGB = Colormap.GetRGBAs(NormalizedIntensities, Colormap, minimumIntensity: TransparencyThreshold ?? 0);
            double[] normalizedValues = Normalize(Enumerable.Range(0, 256).Select(i => (double)i).Reverse().ToArray(), null, null, ScaleMin, ScaleMax);
            int[] scaleRGBA = Colormap.GetRGBAs(normalizedValues, Colormap);

            BmpHeatmap?.Dispose();
            BmpScale?.Dispose();
            BmpHeatmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
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

        public override LegendItem[] GetLegendItems()
        {
            var singleLegendItem = new LegendItem(label, Color.Gray, lineWidth: 10, markerShape: MarkerShape.none);
            return new LegendItem[] { singleLegendItem };
        }

        public override AxisLimits2D GetLimits() =>
            ShowAxisLabels ?
            new AxisLimits2D(-10, BmpHeatmap.Width, -5, BmpHeatmap.Height) :
            new AxisLimits2D(-3, BmpHeatmap.Width, -3, BmpHeatmap.Height);

        public override int GetPointCount() => NormalizedIntensities.Length;

        public string ValidationErrorMessage { get; private set; }
        public bool IsValidData(bool deepValidation = false)
        {
            if (NormalizedIntensities is null || BmpHeatmap is null)
            {
                ValidationErrorMessage = "Call UpdateData() to process data";
                return false;
            }

            ValidationErrorMessage = "";
            return true;
        }

        public override void Render(Settings settings) =>
            throw new NotImplementedException("use new Render method");

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            RenderHeatmap(dims, bmp, lowQuality);
            RenderScale(dims, bmp, lowQuality);
            if (ShowAxisLabels)
                RenderAxis(dims, bmp, lowQuality);
        }

        private void RenderHeatmap(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            using (Graphics gfx = GDI.Graphics(bmp, lowQuality))
            {
                gfx.InterpolationMode = InterpolationMode.NearestNeighbor;

                double minScale = Math.Min(dims.PxPerUnitX, dims.PxPerUnitY);

                if (BackgroundImage != null && !DisplayImageAbove)
                    gfx.DrawImage(
                        BackgroundImage,
                        dims.GetPixelX(0),
                        dims.GetPixelY(0) - (float)(height * minScale),
                        (float)(width * minScale), (float)(height * minScale));

                gfx.DrawImage(
                    BmpHeatmap,
                    dims.GetPixelX(0),
                    dims.GetPixelY(0) - (float)(height * minScale),
                    (float)(width * minScale),
                    (float)(height * minScale));

                if (BackgroundImage != null && DisplayImageAbove)
                    gfx.DrawImage(BackgroundImage,
                        dims.GetPixelX(0),
                        dims.GetPixelY(0) - (float)(height * minScale),
                        (float)(width * minScale),
                        (float)(height * minScale));
            }
        }

        private void RenderScale(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (Graphics gfx = GDI.Graphics(bmp, lowQuality))
            using (var pen = GDI.Pen(Color.Black))
            using (var brush = GDI.Brush(Color.Black))
            using (Font font = GDI.Font(null, 12))
            using (var sf2 = new StringFormat() { LineAlignment = StringAlignment.Far })
            {
                gfx.InterpolationMode = InterpolationMode.NearestNeighbor;

                float pxFromBottom = 30;
                float pxFromRight = dims.Width - 150;
                float pxWidth = 30;
                RectangleF scaleRect = new RectangleF(pxFromRight, pxFromBottom, pxWidth, dims.DataHeight);
                gfx.DrawImage(BmpScale, scaleRect);
                gfx.DrawRectangle(pen, pxFromRight, pxFromBottom, pxWidth / 2, dims.DataHeight);

                string maxString = ScaleMax.HasValue ? $"{(ScaleMax.Value < max ? "≥ " : "")}{ ScaleMax.Value:f3}" : $"{max:f3}";
                string minString = ScaleMin.HasValue ? $"{(ScaleMin.Value > min ? "≤ " : "")}{ScaleMin.Value:f3}" : $"{min:f3}";
                gfx.DrawString(maxString, font, brush, new PointF(scaleRect.X + 30, scaleRect.Top));
                gfx.DrawString(minString, font, brush, new PointF(scaleRect.X + 30, scaleRect.Bottom), sf2);
            }
        }

        private void RenderAxis(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            using (Graphics gfx = GDI.Graphics(bmp, lowQuality))
            using (Pen pen = GDI.Pen(Color.Black))
            using (Brush brush = GDI.Brush(Color.Black))
            using (Font axisFont = GDI.Font(null, 12))
            using (StringFormat right_centre = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center })
            using (StringFormat centre_top = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near })
            {
                double offset = -2;
                double minScale = Math.Min(dims.PxPerUnitX, dims.PxPerUnitY);
                gfx.DrawString($"{AxisOffsets[0]:f3}", axisFont, brush, dims.GetPixelX(0), dims.GetPixelY(offset), centre_top);
                gfx.DrawString($"{AxisOffsets[0] + AxisMultipliers[0]:f3}", axisFont, brush, new PointF((float)((width * minScale) + dims.GetPixelX(0)), dims.GetPixelY(offset)), centre_top);
                gfx.DrawString($"{AxisOffsets[1]:f3}", axisFont, brush, dims.GetPixelX(offset), dims.GetPixelY(0), right_centre);
                gfx.DrawString($"{AxisOffsets[1] + AxisMultipliers[1]:f3}", axisFont, brush, new PointF(dims.GetPixelX(offset), dims.GetPixelY(0) - (float)(height * minScale)), right_centre);
            }
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableHeatmap{label} with {GetPointCount()} points";
        }
    }
}
