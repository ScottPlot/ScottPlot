using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A heatmap displays a 2D array of intensities as small rectangles on the plot
    /// colored according to their intensity value according to a colormap.
    /// </summary>
    public class Heatmap : IPlottable
    {
        // these fields are updated when the intensities are analyzed
        private double Min;
        private double Max;
        private int Width;
        private int Height;
        protected Bitmap BmpHeatmap;

        // these fields are customized by the user
        public string Label;
        public Colormap Colormap { get; private set; }
        public double[] AxisOffsets;
        public double[] AxisMultipliers;
        public double? ScaleMin;
        public double? ScaleMax;
        public double? TransparencyThreshold;
        public Bitmap BackgroundImage;
        public bool DisplayImageAbove;
        public bool ShowAxisLabels;
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        public string ColorbarMin { get; private set; }
        public string ColorbarMax { get; private set; }

        public Heatmap()
        {
            AxisOffsets = new double[] { 0, 0 };
            AxisMultipliers = new double[] { 1, 1 };
            Colormap = Colormap.Viridis;
        }

        /// <summary>
        /// This method analyzes the intensities and colormap to create a bitmap
        /// with a single pixel for every intensity value. The bitmap is stored
        /// and displayed (without anti-alias interpolation) when Render() is called.
        /// </summary>
        /// <param name="intensities">2D array of data for the heatmap (null values are not shown)</param>
        /// <param name="colormap">update the Colormap to use this colormap</param>
        /// <param name="min">minimum intensity (according to the colormap)</param>
        /// <param name="max">maximum intensity (according to the colormap)</param>
        public void Update(double?[,] intensities, Colormap colormap = null, double? min = null, double? max = null)
        {
            Width = intensities.GetLength(1);
            Height = intensities.GetLength(0);
            Colormap = colormap ?? Colormap;
            ScaleMin = min;
            ScaleMax = max;

            double?[] intensitiesFlattened = intensities.Cast<double?>().ToArray();
            Min = double.PositiveInfinity;
            Max = double.NegativeInfinity;

            foreach (double? curr in intensitiesFlattened)
            {
                if (curr.HasValue && double.IsNaN(curr.Value))
                    throw new ArgumentException("Heatmaps do not support intensities of double.NaN");

                if (curr.HasValue && curr.Value < Min)
                    Min = curr.Value;

                if (curr.HasValue && curr.Value > Max)
                    Max = curr.Value;
            }

            // labels for colorbar ticks
            ColorbarMin = (ScaleMin.HasValue && ScaleMin > Min) ? $"≤ {ScaleMin:f3}" : $"{Min:f3}";
            ColorbarMax = (ScaleMax.HasValue && ScaleMax < Max) ? $"≥ {ScaleMax:f3}" : $"{Max:f3}";

            double normalizeMin = (ScaleMin.HasValue && ScaleMin.Value < Min) ? ScaleMin.Value : Min;
            double normalizeMax = (ScaleMax.HasValue && ScaleMax.Value > Max) ? ScaleMax.Value : Max;

            if (TransparencyThreshold.HasValue)
                TransparencyThreshold = Normalize(TransparencyThreshold.Value, Min, Max, ScaleMin, ScaleMax);

            double?[] NormalizedIntensities = Normalize(intensitiesFlattened, null, null, ScaleMin, ScaleMax);

            int[] flatARGB = Colormap.GetRGBAs(NormalizedIntensities, Colormap, minimumIntensity: TransparencyThreshold ?? 0);
            double?[] normalizedValues = Normalize(Enumerable.Range(0, 256).Select(i => (double?)i).Reverse().ToArray(), null, null, ScaleMin, ScaleMax);
            int[] scaleRGBA = Colormap.GetRGBAs(normalizedValues, Colormap);

            BmpHeatmap?.Dispose();
            BmpHeatmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, BmpHeatmap.Width, BmpHeatmap.Height);
            BitmapData bmpData = BmpHeatmap.LockBits(rect, ImageLockMode.ReadWrite, BmpHeatmap.PixelFormat);
            Marshal.Copy(flatARGB, 0, bmpData.Scan0, flatARGB.Length);
            BmpHeatmap.UnlockBits(bmpData);
        }


        /// <summary>
        /// This method analyzes the intensities and colormap to create a bitmap
        /// with a single pixel for every intensity value. The bitmap is stored
        /// and displayed (without anti-alias interpolation) when Render() is called.
        /// </summary>
        /// <param name="intensities">2D array of data for the heatmap (all values are shown)</param>
        /// <param name="colormap">update the Colormap to use this colormap</param>
        /// <param name="min">minimum intensity (according to the colormap)</param>
        /// <param name="max">maximum intensity (according to the colormap)</param>
        public void Update(double[,] intensities, Colormap colormap = null, double? min = null, double? max = null)
        {
            double?[,] tmp = new double?[intensities.GetLength(0), intensities.GetLength(1)];
            for (int i = 0; i < intensities.GetLength(0); i++)
                for (int j = 0; j < intensities.GetLength(1); j++)
                    tmp[i, j] = intensities[i, j];
            Update(tmp, colormap, min, max);
        }

        private double? Normalize(double? input, double? min = null, double? max = null, double? scaleMin = null, double? scaleMax = null)
            => Normalize(new double?[] { input }, min, max, scaleMin, scaleMax)[0];

        private double?[] Normalize(double?[] input, double? min = null, double? max = null, double? scaleMin = null, double? scaleMax = null)
        {
            double? NormalizePreserveNull(double? i)
            {
                if (i.HasValue)
                {
                    return (i.Value - min.Value) / (max.Value - min.Value);
                }
                return null;
            }

            min = min ?? input.Min();
            max = max ?? input.Max();

            min = (scaleMin.HasValue && scaleMin.Value < min) ? scaleMin.Value : min;
            max = (scaleMax.HasValue && scaleMax.Value > max) ? scaleMax.Value : max;

            double?[] normalized = input.AsParallel().AsOrdered().Select<double?, double?>(i => NormalizePreserveNull(i)).ToArray();

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

        public virtual AxisLimits GetAxisLimits()
        {
            if (BmpHeatmap is null)
                return new AxisLimits();

            return ShowAxisLabels ?
                new AxisLimits(-10, BmpHeatmap.Width, -5, BmpHeatmap.Height) :
                new AxisLimits(-3, BmpHeatmap.Width, -3, BmpHeatmap.Height);
        }

        public void ValidateData(bool deepValidation = false)
        {
            if (BmpHeatmap is null)
                throw new InvalidOperationException("Update() was not called prior to rendering");

            if (deepValidation)
            {
                if (Width > 1e6 || Height > 1e6)
                    throw new ArgumentException("Heatmaps may be unreliable for arrays with edges larger than 1 million values");
                if (Width * Height > 1e7)
                    throw new ArgumentException("Heatmaps may be unreliable for arrays with more than 10 million values");
            }
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            RenderHeatmap(dims, bmp, lowQuality);
            if (ShowAxisLabels)
                RenderAxis(dims, bmp, lowQuality);
        }

        protected virtual void RenderHeatmap(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            {
                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                gfx.PixelOffsetMode = PixelOffsetMode.Half;

                int fromX = (int)Math.Round(dims.GetPixelX(0));
                int fromY = (int)Math.Round(dims.GetPixelY(Height));
                int width = (int)Math.Round(dims.GetPixelX(Width) - fromX);
                int height = (int)Math.Round(dims.GetPixelY(0) - fromY);
                Rectangle destRect = new Rectangle(fromX, fromY, width, height);

                ImageAttributes attr = new ImageAttributes();
                attr.SetWrapMode(WrapMode.TileFlipXY);

                if (BackgroundImage != null && !DisplayImageAbove)
                    gfx.DrawImage(
                            image: BackgroundImage,
                            destRect: destRect,
                            srcX: 0,
                            srcY: 0,
                            srcWidth: BackgroundImage.Width,
                            srcHeight: BackgroundImage.Height,
                            srcUnit: GraphicsUnit.Pixel,
                            imageAttr: attr);

                gfx.DrawImage(
                        image: BmpHeatmap,
                        destRect: destRect,
                        srcX: 0,
                        srcY: 0,
                        BmpHeatmap.Width,
                        BmpHeatmap.Height,
                        GraphicsUnit.Pixel,
                        attr);

                if (BackgroundImage != null && DisplayImageAbove)
                    gfx.DrawImage(
                            image: BackgroundImage,
                            destRect: destRect,
                            srcX: 0,
                            srcY: 0,
                            srcWidth: BackgroundImage.Width,
                            srcHeight: BackgroundImage.Height,
                            srcUnit: GraphicsUnit.Pixel,
                            imageAttr: attr);
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

        public override string ToString() => $"PlottableHeatmap ({BmpHeatmap.Size})";
    }
}
