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
    public enum CellAlignment
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
        public CellAlignment CellAlignment { get; set; } = CellAlignment.Center;
        public CoordinateRect? Extent { get; set; }
        public bool FlipVertically { get; set; } = false;
        public bool Smooth { get; set; } = false;

        private CoordinateRect ExtentOrDefault => Extent ?? new CoordinateRect(0, Intensities.GetLength(0), 0, Intensities.GetLength(1));
        private CoordinateRect AlignedExtent => ExtentOrDefault.WithTranslation(new(CellAlignmentFraction * CellWidth, CellAlignmentFraction * CellHeight));
        private double CellAlignmentFraction => CellAlignment switch
        {
            CellAlignment.Start => 0,
            CellAlignment.Center => -0.5,
            CellAlignment.End => -1,
            _ => throw new NotImplementedException(),
        };
        private double CellWidth => ExtentOrDefault.Width / Intensities.GetLength(0);
        private double CellHeight => ExtentOrDefault.Height / Intensities.GetLength(1);


        public double[,] Intensities { get; set; }

        private SKBitmap? bitmap = null;

        public Heatmap(double[,] intensities)
        {
            Intensities = intensities;
        }

        ~Heatmap()
        {
            bitmap?.Dispose();
        }

        public void Update()
        {
            bitmap?.Dispose();
            SKImageInfo imageInfo = new(Intensities.GetLength(0), Intensities.GetLength(1));

            bitmap = new(imageInfo);
            var intensitiesFlat = Intensities.Cast<double>();
            var range = RangeExcludingNaN(intensitiesFlat);
            uint[] rgba = intensitiesFlat.Select(i => Colormap.GetColor(i, range).ARGB).ToArray();

            GCHandle handle = GCHandle.Alloc(rgba, GCHandleType.Pinned);
            bitmap.InstallPixels(imageInfo, handle.AddrOfPinnedObject(), imageInfo.RowBytes, (IntPtr _, object _) => handle.Free());
        }

        private Range RangeExcludingNaN(IEnumerable<double> input)
        {
            double min = double.PositiveInfinity;
            double max = double.NegativeInfinity;
            foreach (var curr in input)
            {
                if (double.IsNaN(curr))
                    continue;

                min = Math.Min(min, curr);
                max = Math.Max(max, curr);
            }

            return new(min, max);
        }

        public AxisLimits GetAxisLimits()
        {
            return new(AlignedExtent);
        }

        public void Render(SKSurface surface)
        {
            if (bitmap is not null)
            {
                using SKPaint paint = new SKPaint()
                {
                    FilterQuality = Smooth ? SKFilterQuality.High : SKFilterQuality.None
                };

                CoordinateRect bounds = new(Axes.GetPixelX(AlignedExtent.XMin), Axes.GetPixelX(AlignedExtent.XMax), Axes.GetPixelY(AlignedExtent.YMax), Axes.GetPixelY(AlignedExtent.YMin));

                surface.Canvas.Translate((float)bounds.XMin, (float)bounds.YMin);
                if (FlipVertically)
                {
                    surface.Canvas.Scale(1, -1);
                }

                SKRect rect = FlipVertically ? new(0, (float)-bounds.Height, (float)bounds.Width, 0) : new(0, 0, (float)bounds.Width, (float)bounds.Height);
                surface.Canvas.DrawBitmap(bitmap, rect, paint);
            }
        }
    }
}
