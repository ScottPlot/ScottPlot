using ScottPlot.Axis;
using ScottPlot.Style;
using SkiaSharp;

namespace ScottPlot.Plottables
{
    public class PieSlice
    {
        public Label? Label { get; set; } = null; // TODO: render label
        public double Value { get; set; }
        public Fill Fill { get; set; }

        public PieSlice() { }

        public PieSlice(double value, Color color)
        {
            Value = value;
            Fill = new Fill(color);
        }
    }

    public class Pie : IPlottable
    {
        public IList<PieSlice> Slices { get; set; }
        public Stroke Stroke { get; set; } = new() { Width = 0 };
        public bool IsVisible { get; set; }

        public IAxes Axes { get; set; } = Axis.Axes.Default;

        public Pie(IList<PieSlice> slices)
        {
            Slices = slices;
        }

        public AxisLimits GetAxisLimits()
        {
            return new AxisLimits(-1, 1, -1, 1);
        }

        public void Render(SKSurface surface)
        {
            double total = Slices.Sum(s => s.Value);
            float[] sweeps = Slices.Select(x => (float)(x.Value / total) * 360).ToArray();

            Pixel origin = Axes.GetPixel(Coordinates.Origin);

            float minX = Math.Abs(Axes.GetPixelX(1) - origin.X);
            float minY = Math.Abs(Axes.GetPixelY(1) - origin.Y);
            float radius = Math.Min(minX, minY);
            float explosionRadius = 0.03f * radius;
            SKRect rect = new(-radius, -radius, radius, radius);

            using SKPath path = new();
            using SKPaint paint = new() { IsAntialias = true };

            float sweepStart = 0;
            for (int i = 0; i < Slices.Count(); i++)
            {
                int savePoint = surface.Canvas.Save();
                surface.Canvas.Translate(origin.X, origin.Y);
                surface.Canvas.RotateDegrees(sweepStart + sweeps[i] / 2);
                surface.Canvas.Translate(explosionRadius, 0);

                path.MoveTo(0, 0);
                path.ArcTo(rect, -sweeps[i] / 2, sweeps[i], false);
                path.Close();

                paint.SetFill(Slices[i].Fill);
                surface.Canvas.DrawPath(path, paint);

                paint.SetStroke(Stroke);
                surface.Canvas.DrawPath(path, paint);

                path.Reset();
                surface.Canvas.RestoreToCount(savePoint);

                sweepStart += sweeps[i];
            }
        }
    }
}
