using ScottPlot.Axis;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.Colormaps;

namespace ScottPlot.Plottables
{
    public enum CellAlignment // TODO: replace this with Alignment
    {
        Start,
        Center,
        End
    }

    public class Heatmap : IPlottable
    {
        public bool IsVisible { get; set; } = true;
        public IAxes Axes { get; set; } = Axis.Axes.Default;
        public IColormap Colormap { get; set; } = new Viridis();

        /// <summary>
        /// Indicates position of the data point relative to the rectangle used to represent it.
        /// An alignment of upper right means the rectangle will appear to the lower left of the point itself.
        /// </summary>
        public CellAlignment CellAlignment { get; set; } = CellAlignment.Center;

        /// <summary>
        /// If defined, the this rectangle sets the boundaries of colormap data.
        /// Note that the actual heatmap area is 1 cell larger than this rectangle.
        /// </summary>
        public CoordinateRect? Extent { get; set; }

        /// <summary>
        /// By default heatmaps are drawn in the same vertical orientation as vectors: row 0 is at the top.
        /// If this is true, the vertical orientation will be flipped so row 0 is on the bottom.
        /// </summary>
        public bool FlipVertically { get; set; } = false;

        /// <summary>
        /// If true, the heatmap will be blurred.
        /// If false, the heatmap will appear as individual rectangles with sharp edges.
        /// </summary>
        public bool Smooth { get; set; } = false;

        /// <summary>
        /// Actual extent of the heatmap bitmap after alignment has been applied
        /// </summary>
        private CoordinateRect AlignedExtent
        {
            get
            {
                double cellOffsetX = CellAlignmentFraction * CellWidth;
                double cellOffsetY = CellAlignmentFraction * CellHeight;
                Coordinates cellOffset = new(cellOffsetX, cellOffsetY);
                return ExtentOrDefault.WithTranslation(cellOffset);
            }
        }

        /// <summary>
        /// Extent used at render time.
        /// Supplies the user-provided extent if available, 
        /// otherwise a heatmap centered at the origin with cell size 1.
        /// </summary>
        private CoordinateRect ExtentOrDefault
        {
            get
            {
                if (Extent.HasValue)
                    return Extent.Value;

                return new CoordinateRect(
                    xMin: 0,
                    xMax: Intensities.GetLength(0),
                    yMin: 0,
                    yMax: Intensities.GetLength(1));
            }
        }

        private double CellAlignmentFraction => CellAlignment switch
        {
            CellAlignment.Start => 0,
            CellAlignment.Center => -0.5,
            CellAlignment.End => -1,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Width of a single cell from the heatmap (in coordinate units)
        /// </summary>
        private double CellWidth => ExtentOrDefault.Width / Intensities.GetLength(0);

        /// <summary>
        /// Height of a single cell from the heatmap (in coordinate units)
        /// </summary>
        private double CellHeight => ExtentOrDefault.Height / Intensities.GetLength(1);

        /// <summary>
        /// This object holds data values for the heatmap.
        /// After editing contents users must call <see cref="Update"/> before changes
        /// appear on the heatmap.
        /// </summary>
        public readonly double[,] Intensities; // TODO: consider data class

        /// <summary>
        /// Height of the heatmap data (rows)
        /// </summary>
        int Height => Intensities.GetLength(0);

        /// <summary>
        /// Width of the heatmap data (columns)
        /// </summary>
        int Width => Intensities.GetLength(1);

        /// <summary>
        /// Generated and stored when <see cref="Update"/> is called
        /// </summary>
        private SKBitmap? Bitmap = null;

        public Heatmap(double[,] intensities)
        {
            Intensities = intensities;
        }

        ~Heatmap()
        {
            Bitmap?.Dispose();
        }

        /// <summary>
        /// Return heatmap as an array of ARGB values,
        /// scaled according to the heatmap setting,
        /// and in the order necessary to create a bitmap.
        /// </summary>
        private uint[] GetArgbValues()
        {
            Range range = Range.GetRange(Intensities);

            uint[] rgba = new uint[Intensities.Length];
            for (int y = 0; y < Height; y++)
            {
                int rowOffset = FlipVertically ? (Height - 1 - y) * Width : y * Width;
                for (int x = 0; x < Width; x++)
                {
                    rgba[rowOffset + x] = Colormap.GetColor(Intensities[y, x], range).ARGB;
                }
            }

            return rgba;
        }

        public void Update()
        {
            uint[] rgba = GetArgbValues();
            GCHandle handle = GCHandle.Alloc(rgba, GCHandleType.Pinned);
            SKImageInfo imageInfo = new(Width, Height);
            Bitmap?.Dispose();
            Bitmap = new SKBitmap(imageInfo);
            Bitmap.InstallPixels(
                info: imageInfo,
                pixels: handle.AddrOfPinnedObject(),
                rowBytes: imageInfo.RowBytes,
                releaseProc: (IntPtr _, object _) => handle.Free());
        }

        public AxisLimits GetAxisLimits()
        {
            return new(AlignedExtent);
        }

        public void Render(SKSurface surface)
        {
            if (Bitmap is null)
                Update(); // automatically generate the bitmap on first render if it was not generated manually

            using SKPaint paint = new()
            {
                FilterQuality = Smooth ? SKFilterQuality.High : SKFilterQuality.None
            };

            SKRect rect = Axes.GetPixelRect(AlignedExtent).ToSKRect();

            surface.Canvas.DrawBitmap(Bitmap, rect, paint);
        }
    }
}
