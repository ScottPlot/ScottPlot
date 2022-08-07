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

    public class Pie : PlottableBase
    {
        public IList<PieSlice> Slices { get; set; }
        public Stroke Stroke { get; set; } = new() { Width = 0 };

        public Pie(IList<PieSlice> slices)
        {
            Slices = slices;
        }

        public override AxisLimits GetAxisLimits()
        {
            return new AxisLimits(-1, 1, -1, 1);
        }

        public override void Render(SKSurface surface, PixelRect dataRect)
        {
            // TODO: Maybe this can be done in a call to base.Render()?
            if (XAxis is null || YAxis is null)
                throw new InvalidOperationException("Both axes must be set before first render");

            surface.Canvas.ClipRect(dataRect.ToSKRect()); // TODO: This too?

            double total = Slices.Sum(s => s.Value);
            float[] sweeps = Slices.Select(x => (float)(x.Value / total) * 360).ToArray();

            Pixel origin = new(
                x: XAxis.GetPixel(0, dataRect),
                y: YAxis.GetPixel(0, dataRect));

            float minX = Math.Abs(XAxis.GetPixel(1, dataRect) - origin.X);
            float minY = Math.Abs(YAxis.GetPixel(1, dataRect) - origin.Y);
            float radius = Math.Min(minX, minY);
            float explosionRadius = 0.01f * radius;
            SKRect rect = new(-radius, -radius, radius, radius);

            using SKPath path = new();
            using SKPaint paint = new() { IsAntialias = true };

            float sweepStart = 0;
            for (int i = 0; i < Slices.Count(); i++)
            {
                int savePoint = surface.Canvas.Save();
                surface.Canvas.Translate(origin.X, origin.Y);
                surface.Canvas.RotateDegrees(sweepStart + sweeps[i] / 2);
                surface.Canvas.Translate(explosionRadius, explosionRadius);

                path.MoveTo(0, 0);
                path.ArcTo(rect, - sweeps[i] / 2, sweeps[i], false);
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
