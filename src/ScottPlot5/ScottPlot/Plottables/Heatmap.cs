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
    public class Heatmap : IPlottable
    {
        public bool IsVisible { get; set; } = true;
        public IAxes Axes { get; set; } = Axis.Axes.Default;
        public IColormap Colormap { get; set; } = new Viridis();

        private double[,] _intensities;
        public double[,] Intensities
        {
            get => _intensities;
            set
            {
                _intensities = value;
                Update();
            }
        }

        private SKBitmap? bitmap = null;

        public Heatmap(double[,] intensities)
        {
            Intensities = intensities;
        }

        ~Heatmap()
        {
            bitmap?.Dispose();
        }

        private void Update()
        {
            bitmap?.Dispose();
            SKImageInfo imageInfo = new(Intensities.GetLength(0), Intensities.GetLength(1));

            bitmap = new(imageInfo);
            var intensitiesFlat = Intensities.Cast<double>();
            uint[] rgba = intensitiesFlat.Select(i => Colormap.GetColor(i, new(intensitiesFlat.Min(), intensitiesFlat.Max())).ARGB).ToArray();

            GCHandle handle = GCHandle.Alloc(rgba, GCHandleType.Pinned);
            bitmap.InstallPixels(imageInfo, handle.AddrOfPinnedObject(), imageInfo.RowBytes, (IntPtr _, object _) => handle.Free());
        }

        double Normalize(double intensity, Range range)
        {
            return (intensity - range.Min) / (range.Max - range.Min);
        }

        private IEnumerable<double> Normalize(IEnumerable<double> input, Range? domain, Range? range) // Codomain might be a less repetitive but more obscure alternative?
        {
            domain ??= new(input.Min(), input.Max());
            range ??= new(domain.Value.Min, domain.Value.Max);

            domain = new(Math.Min(domain.Value.Min, range.Value.Min), Math.Max(domain.Value.Max, range.Value.Max));

            Range normalizedRange = new(Normalize(range.Value.Min, domain.Value), Normalize(range.Value.Max, domain.Value));
            return input.AsParallel().AsOrdered().Select(i => normalizedRange.Clamp(Normalize(i, domain.Value)));
        }

        public AxisLimits GetAxisLimits()
        {
            return new(-0.5, (bitmap?.Width ?? 1) - 0.5, -0.5, (bitmap?.Height ?? 1) - 0.5);
        }

        public void Render(SKSurface surface)
        {
            if (bitmap is not null)
            {
                SKRect rect = new(Axes.GetPixelX(-0.5), Axes.GetPixelY(bitmap.Height - 0.5), Axes.GetPixelX(bitmap.Width - 0.5), Axes.GetPixelY(-0.5));
                surface.Canvas.DrawBitmap(bitmap, rect);
            }
        }
    }
}
