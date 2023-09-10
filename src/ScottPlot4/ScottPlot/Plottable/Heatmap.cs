using ScottPlot.Drawing;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A heatmap displays a 2D array of intensities as small rectangles on the plot
    /// colored according to their intensity value according to a colormap.
    /// </summary>
    public class Heatmap : IPlottable, IHasColormap
    {
        /// <summary>
        /// Minimum heatmap value
        /// </summary>
        private double Min;

        /// <summary>
        /// Maximum heatmap value
        /// </summary>
        private double Max;

        /// <summary>
        /// Number of columns in the heatmap data
        /// </summary>
        private int DataWidth;

        /// <summary>
        /// Number of rows in the heatmap data
        /// </summary>
        private int DataHeight;

        /// <summary>
        /// Pre-rendered heatmap image
        /// </summary>
        protected Bitmap BmpHeatmap { get; private set; }

        /// <summary>
        /// Horizontal location of the lower-left cell
        /// </summary>
        public double OffsetX { get; set; } = 0;

        /// <summary>
        /// Vertical location of the lower-left cell
        /// </summary>
        public double OffsetY { get; set; } = 0;

        /// <summary>
        /// Width of each cell composing the heatmap
        /// </summary>
        public double CellWidth { get; set; } = 1;

        /// <summary>
        /// Height of each cell composing the heatmap
        /// </summary>
        public double CellHeight { get; set; } = 1;

        /// <summary>
        /// Position of the left edge of the heatmap
        /// </summary>
        public double XMin
        {
            get => OffsetX;
            set => OffsetX = value;
        }

        /// <summary>
        /// Position of the right edge of the heatmap
        /// </summary>
        public double XMax
        {
            get => OffsetX + DataWidth * CellWidth;
            set => CellWidth = (value - OffsetX) / DataWidth;
        }

        public double YMin
        {
            get => OffsetY;
            set => OffsetY = value;
        }

        public double YMax
        {
            get => OffsetY + DataHeight * CellHeight;
            set => CellHeight = (value - OffsetY) / DataHeight;
        }

        /// <summary>
        /// Indicates whether the heatmap's size or location has been modified by the user
        /// </summary>
        public bool IsDefaultSizeAndLocation => OffsetX == 0 && OffsetY == 0 && CellHeight == 1 && CellWidth == 1;

        /// <summary>
        /// Text to appear in the legend
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Colormap used to translate heatmap values to colors
        /// </summary>
        public Colormap Colormap { get; private set; } = Colormap.Viridis;

        /// <summary>
        /// If defined, colors will be "clipped" to this value such that lower values (lower colors) will not be shown
        /// </summary>
        public double? ScaleMin { get; set; }

        /// <summary>
        /// If defined, colors will be "clipped" to this value such that greater values (higher colors) will not be shown
        /// </summary>
        public double? ScaleMax { get; set; }

        /// <summary>
        /// Heatmap values below this number (if defined) will be made transparent
        /// </summary>
        public double? TransparencyThreshold { get; set; }

        [Obsolete("This feature has been deprecated. Use AddImage() to place a bitmap beneath or above the heatmap.", true)]
        public Bitmap BackgroundImage { get; set; }

        [Obsolete("This feature has been deprecated. Use AddImage() to place a bitmap beneath or above the heatmap.", true)]
        public bool DisplayImageAbove { get; set; }

        [Obsolete("This feature has been deprecated. Use Plot.AddText() to add text to the plot.", true)]
        public bool ShowAxisLabels;

        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        /// <summary>
        /// Value of the the lower edge of the colormap
        /// </summary>
        public double ColormapMin => ScaleMin ?? Min;

        /// <summary>
        /// Value of the the upper edge of the colormap
        /// </summary>
        public double ColormapMax => ScaleMax ?? Max;

        /// <summary>
        /// Indicates whether values extend beyond the lower edge of the colormap
        /// </summary>
        public bool ColormapMinIsClipped { get; private set; } = false;

        /// <summary>
        /// Indicates whether values extend beyond the upper edge of the colormap
        /// </summary>
        public bool ColormapMaxIsClipped { get; private set; } = false;

        /// <summary>
        /// Enable multi-threaded parallel processing which may improve performance for large datasets.
        /// </summary>
        public bool UseParallel { get; set; } = false;

        /// <summary>
        /// If true, heatmap squares will be smoothed using high quality bicubic interpolation.
        /// If false, heatmap squares will look like sharp rectangles (nearest neighbor interpolation).
        /// </summary>
        public bool Smooth
        {
            get => Interpolation != InterpolationMode.NearestNeighbor;
            set => Interpolation = value ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;
        }

        /// <summary>
        /// Controls which interpolation mode is used when zooming into the heatmap.
        /// </summary>
        public InterpolationMode Interpolation { get; set; } = InterpolationMode.NearestNeighbor;

        /// <summary>
        /// By default the first row of the 2D array represents the top of the heatmap.
        /// If this is true, the first row of the 2D array represents the bottom of the heatmap.
        /// </summary>
        public bool FlipVertically { get; set; } = false;

        /// <summary>
        /// By default the first column of the 2D array represents the left side of the heatmap.
        /// If this is true, the first column of the 2D array represents the right side of the heatmap.
        /// </summary>
        public bool FlipHorizontally { get; set; } = false;

        /// <summary>        
        /// Amount of rotation (degrees) clockwise around the point described by <see cref="CenterOfRotation"/>
        /// </summary>
        public double Rotation { get; set; }

        /// <summary>
        /// Indicates which corner will be the axis of Rotation.
        /// </summary>
        public Alignment CenterOfRotation { get; set; }

        public Coordinate[] ClippingPoints { get; set; } = Array.Empty<Coordinate>();

        public LegendItem[] GetLegendItems() => LegendItem.None;

        /// <summary>
        /// Heatmap transparency from 0 (transparent) to 1 (opaque).
        /// </summary>
        public double Opacity = 1;

        /// <summary>
        /// This method analyzes the intensities and colormap to create a bitmap
        /// with a single pixel for every intensity value. The bitmap is stored
        /// and displayed (without anti-alias interpolation) when Render() is called.
        /// </summary>
        /// <param name="intensities">2D array of data for the heatmap (null values are not shown)</param>
        /// <param name="colormap">update the Colormap to use this colormap</param>
        /// <param name="min">minimum intensity (according to the colormap)</param>
        /// <param name="max">maximum intensity (according to the colormap)</param>
        /// <param name="opacity">If defined, this mask indicates the opacity of each cell in the heatmap from 0 (transparent) to 1 (opaque).
        /// If defined, this array must have the same dimensions as the heatmap array. Null values are not shown.</param>
        public void Update(double?[,] intensities, Colormap colormap = null, double? min = null, double? max = null, double?[,] opacity = null)
        {
            DataWidth = intensities.GetLength(1);
            DataHeight = intensities.GetLength(0);

            // limit edge size due to System.Drawing rendering artifacts
            // https://github.com/ScottPlot/ScottPlot/issues/2119
            int maxEdgeLength = 1 << 15;
            if (DataWidth > maxEdgeLength || DataHeight > maxEdgeLength)
            {
                throw new ArgumentException("Due to limitations in rendering large bitmaps, " +
                    $"heatmaps cannot have more than {maxEdgeLength:N0} rows or columns");
            }

            // limit total size due to System.Drawing rendering artifacts
            // https://github.com/ScottPlot/ScottPlot/issues/772
            int maxTotalValues = 10_000_000;
            if (DataWidth * DataHeight > maxTotalValues)
            {
                throw new ArgumentException($"Heatmaps may be unreliable for 2D arrays " +
                    $"with more than {maxTotalValues:N0} values");
            }

            Colormap = colormap ?? Colormap;
            ScaleMin = min;
            ScaleMax = max;

            double?[] intensitiesFlattened = Flatten(intensities, UseParallel);

            double?[] opacityFlattened = opacity is null ? null : Flatten(opacity, UseParallel);

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

            ColormapMinIsClipped = ScaleMin.HasValue && ScaleMin > Min;
            ColormapMaxIsClipped = ScaleMax.HasValue && ScaleMax < Max;

            double minimumIntensity = ScaleMin ?? Min;
            double maximumIntensity = ScaleMax ?? Max;

            if (TransparencyThreshold.HasValue)
            {
                TransparencyThreshold = Normalize(TransparencyThreshold, Min, Max, ScaleMin, ScaleMax);
                minimumIntensity = TransparencyThreshold.Value;
            }

            double?[] NormalizedIntensities = Normalize(intensitiesFlattened, minimumIntensity, maximumIntensity, ScaleMin, ScaleMax);

            int[] flatARGB;
            if (opacity != null)
            {
                flatARGB = Colormap.GetRGBAs(NormalizedIntensities, opacityFlattened, Colormap);
            }
            else if (TransparencyThreshold.HasValue)
            {
                flatARGB = Colormap.GetRGBAs(NormalizedIntensities, Colormap, minimumIntensity);
            }
            else
            {
                flatARGB = Colormap.GetRGBAs(NormalizedIntensities, Colormap, double.NegativeInfinity);
            }

            UpdateBitmap(flatARGB);
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
        /// <param name="opacity">If defined, this mask indicates the opacity of each cell in the heatmap from 0 (transparent) to 1 (opaque).
        /// If defined, this array must have the same dimensions as the heatmap array.</param>
        public void Update(double[,] intensities, Colormap colormap = null, double? min = null, double? max = null, double[,] opacity = null)
        {
            double?[,] finalIntensity = new double?[intensities.GetLength(0), intensities.GetLength(1)];
            double?[,] finalOpacity = opacity is null ? null : new double?[opacity.GetLength(0), opacity.GetLength(1)];

            Copy2D(intensities, finalIntensity, UseParallel);

            if (opacity is not null)
            {
                Copy2D(opacity, finalOpacity, UseParallel);
            }

            Update(finalIntensity, colormap, min, max, finalOpacity);
        }

        /// <summary>
        /// Update the heatmap where every cell is given the same color, but with various opacities.
        /// </summary>
        /// <param name="color">Single color used for all cells</param>
        /// <param name="opacity">Opacities (ranging 0-1) for all cells</param>
        public void Update(Color color, double?[,] opacity)
        {
            // limit edge size due to System.Drawing rendering artifacts
            // https://github.com/ScottPlot/ScottPlot/issues/2119
            int maxEdgeLength = 1 << 15;
            if (opacity.GetLength(1) > maxEdgeLength || opacity.GetLength(0) > maxEdgeLength)
            {
                throw new ArgumentException("Due to limitations in rendering large bitmaps, " +
                    $"heatmaps cannot have more than {maxEdgeLength:N0} rows or columns");
            }

            // limit total size due to System.Drawing rendering artifacts
            // https://github.com/ScottPlot/ScottPlot/issues/772
            int maxTotalValues = 10_000_000;
            if (opacity.GetLength(1) * opacity.GetLength(0) > maxTotalValues)
            {
                throw new ArgumentException($"Heatmaps may be unreliable for 2D arrays " +
                    $"with more than {maxTotalValues:N0} values");
            }

            DataWidth = opacity.GetLength(1);
            DataHeight = opacity.GetLength(0);

            double?[] opacityFlattened = Flatten(opacity, UseParallel);

            int[] flatARGB = Colormap.GetRGBAs(opacityFlattened, color);
            UpdateBitmap(flatARGB);
        }

        /// <summary>
        /// Update the heatmap where every cell is given the same color, but with various opacities.
        /// </summary>
        /// <param name="color">Single color used for all cells</param>
        /// <param name="opacity">Opacities (ranging 0-1) for all cells</param>
        public void Update(Color color, double[,] opacity)
        {
            // limit edge size due to System.Drawing rendering artifacts
            // https://github.com/ScottPlot/ScottPlot/issues/2119
            int maxEdgeLength = 1 << 15;
            if (opacity.GetLength(1) > maxEdgeLength || opacity.GetLength(0) > maxEdgeLength)
            {
                throw new ArgumentException("Due to limitations in rendering large bitmaps, " +
                    $"heatmaps cannot have more than {maxEdgeLength:N0} rows or columns");
            }

            // limit total size due to System.Drawing rendering artifacts
            // https://github.com/ScottPlot/ScottPlot/issues/772
            int maxTotalValues = 10_000_000;
            if (opacity.GetLength(1) * opacity.GetLength(0) > maxTotalValues)
            {
                throw new ArgumentException($"Heatmaps may be unreliable for 2D arrays " +
                    $"with more than {maxTotalValues:N0} values");
            }

            DataWidth = opacity.GetLength(1);
            DataHeight = opacity.GetLength(0);

            double[] opacityFlattened = Flatten(opacity, UseParallel);

            int[] flatARGB = Colormap.GetRGBAs(opacityFlattened, color);
            UpdateBitmap(flatARGB);
        }

        /// <summary>
        /// This should be the only method which creates or modifies <see cref="BmpHeatmap"/>
        /// </summary>
        /// <param name="flatARGB"></param>
        private void UpdateBitmap(int[] flatARGB)
        {
            Bitmap bmp = new(DataWidth, DataHeight, PixelFormat.Format32bppArgb);
            Rectangle rect = new(0, 0, DataWidth, DataHeight);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            Marshal.Copy(flatARGB, 0, bmpData.Scan0, flatARGB.Length);
            bmp.UnlockBits(bmpData);

            Bitmap originalBmp = BmpHeatmap;
            BmpHeatmap = bmp;
            originalBmp?.Dispose();
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

            min ??= input.Min();
            max ??= input.Max();

            min = (scaleMin.HasValue && scaleMin.Value < min) ? scaleMin.Value : min;
            max = (scaleMax.HasValue && scaleMax.Value > max) ? scaleMax.Value : max;

            double?[] normalized = input.AsParallel().AsOrdered().Select(i => NormalizePreserveNull(i)).ToArray();

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

        public virtual AxisLimits GetAxisLimits()
        {
            if (BmpHeatmap is null)
                return AxisLimits.NoLimits;

            return new AxisLimits(
                xMin: OffsetX,
                xMax: OffsetX + DataWidth * CellWidth,
                yMin: OffsetY,
                yMax: OffsetY + DataHeight * CellHeight);
        }

        /// <summary>
        /// Return the position in the 2D array corresponding to the given coordinate.
        /// Returns null if the coordinate is not over the heatmap.
        /// </summary>
        public (int? xIndex, int? yIndex) GetCellIndexes(double x, double y)
        {
            int? xIndex = (int)((x - OffsetX) / CellWidth);
            int? yIndex = (int)((y - OffsetY) / CellHeight);

            if (xIndex < 0 || xIndex >= DataWidth)
                xIndex = null;

            if (yIndex < 0 || yIndex >= DataHeight)
                yIndex = null;

            return (xIndex, yIndex);
        }

        /// <summary>
        /// Returns a copy of the heatmap image as a <see cref="Bitmap"/>.
        /// Dimensions of the image will be equal to those of the source data used to create it.
        /// </summary>
        public Bitmap GetBitmap()
        {
            Rectangle fullSizeRect = new(0, 0, BmpHeatmap.Width, BmpHeatmap.Height);
            Bitmap bmp = BmpHeatmap.Clone(fullSizeRect, BmpHeatmap.PixelFormat);
            return bmp;
        }

        public void ValidateData(bool deepValidation = false)
        {
            if (BmpHeatmap is null)
                throw new InvalidOperationException("Update() was not called prior to rendering");
            if (double.IsNaN(Rotation) || double.IsInfinity(Rotation))
                throw new InvalidOperationException("rotation must be a real value");
        }

        private PointF ImageLocationOffset(float width, float height)
        {
            return CenterOfRotation switch
            {
                Alignment.LowerCenter => new PointF(-width / 2, -height),
                Alignment.LowerLeft => new PointF(0, -height),
                Alignment.LowerRight => new PointF(-width, -height),
                Alignment.MiddleLeft => new PointF(0, -height / 2),
                Alignment.MiddleRight => new PointF(-width, -height / 2),
                Alignment.UpperCenter => new PointF(-width / 2, 0),
                Alignment.UpperLeft => new PointF(0, 0),
                Alignment.UpperRight => new PointF(-width, 0),
                Alignment.MiddleCenter => new PointF(-width / 2, -height / 2),
                _ => throw new InvalidEnumArgumentException(),
            };
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            RenderHeatmap(dims, bmp, lowQuality);
        }

        protected virtual void RenderHeatmap(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);
            GDI.ClipIntersection(gfx, dims, ClippingPoints);

            gfx.InterpolationMode = Interpolation;
            gfx.PixelOffsetMode = PixelOffsetMode.Half;

            int fromX = (int)Math.Round(dims.GetPixelX(OffsetX));
            int fromY = (int)Math.Round(dims.GetPixelY(OffsetY + DataHeight * CellHeight));
            int width = (int)Math.Round(dims.GetPixelX(OffsetX + DataWidth * CellWidth) - fromX);
            int height = (int)Math.Round(dims.GetPixelY(OffsetY) - fromY);

            ImageAttributes attr = new();
            attr.SetWrapMode(WrapMode.TileFlipXY);

            gfx.TranslateTransform(fromX, fromY);

            gfx.ScaleTransform(
                sx: FlipHorizontally ? -1 : 1,
                sy: FlipVertically ? -1 : 1);

            Rectangle destRect = new(
                x: FlipHorizontally ? -width : 0,
                y: FlipVertically ? -height : 0,
                width: width,
                height: height);

            if (Rotation != 0)
            {
                // Translate to center of image (relative to its position), rotate, translate back
                var offsetPoint = ImageLocationOffset(width, height);
                gfx.TranslateTransform(-offsetPoint.X, -offsetPoint.Y);
                gfx.RotateTransform((float)Rotation);
                gfx.TranslateTransform(offsetPoint.X, offsetPoint.Y);
            }

            ColorMatrix cm = new() { Matrix33 = (float)Opacity };
            ImageAttributes att = new();
            attr.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            gfx.DrawImage(
                    image: BmpHeatmap,
                    destRect: destRect,
                    srcX: 0,
                    srcY: 0,
                    BmpHeatmap.Width,
                    BmpHeatmap.Height,
                    GraphicsUnit.Pixel,
                    attr);
        }

        public override string ToString() => $"PlottableHeatmap ({BmpHeatmap.Size})";

        /// <summary>
        /// Return values of a 2D array flattened as a 1D array.
        /// Multi-threaded parallel processing may improve performance for large datasets.
        /// </summary>
        private static T[] Flatten<T>(T[,] values, bool parallel)
        {
            int width = values.GetLength(1);
            int height = values.GetLength(0);

            T[] flat = new T[height * width];

            if (parallel)
            {
                Parallel.For(0, height, i =>
                {
                    for (int j = 0; j < width; j++)
                    {
                        flat[i * width + j] = values[i, j];
                    }
                });
            }
            else
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        flat[i * width + j] = values[i, j];
                    }
                }
            }

            return flat;
        }

        /// <summary>
        /// Copy values from <paramref name="source"/> into <paramref name="destination"/>.
        /// Multi-threaded parallel processing may improve performance for large datasets.
        /// </summary>
        private static void Copy2D(double[,] source, double?[,] destination, bool parallel)
        {
            int height = source.GetLength(0);
            int width = source.GetLength(1);

            if (parallel)
            {
                Parallel.For(0, height, i =>
                {
                    for (int j = 0; j < width; j++)
                    {
                        destination[i, j] = source[i, j];
                    }
                });
            }
            else
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        destination[i, j] = source[i, j];
                    }
                }
            }
        }
    }
}
