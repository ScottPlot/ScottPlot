using ScottPlot.Axis;
using SkiaSharp;

namespace ScottPlot.Plottables
{
    public class PieSlice
    {
        public string? Label { get; set; }
        public double Value { get; set; }
        public FillStyle Fill { get; set; } = new();

        public PieSlice() { }

        public PieSlice(double value, Color color)
        {
            Value = value;
            Fill = new() { Color = color };
        }
    }

    public class Pie : IPlottable
    {
        public string? Label { get; set; }
        public IList<PieSlice> Slices { get; set; }
        public LineStyle LineStyle { get; set; } = new() { Width = 0 };
        public bool IsVisible { get; set; } = true;
        public double ExplodeFraction { get; set; } = 0;

        public IAxes Axes { get; set; } = Axis.Axes.Default;

        public Pie(IList<PieSlice> slices)
        {
            Slices = slices;
        }

        public AxisLimits GetAxisLimits()
        {
            double padding = .03;
            return new AxisLimits(
                xMin: -1 - ExplodeFraction - padding,
                xMax: 1 + ExplodeFraction + padding,
                yMin: -1 - ExplodeFraction - padding,
                yMax: 1 + ExplodeFraction + padding);
        }
        public IEnumerable<LegendItem> LegendItems => EnumerableExtensions.One(
            new LegendItem
            {
                Label = Label,
                Children = Slices.Select(slice => new LegendItem
                {
                    Label = slice.Label,
                    Fill = slice.Fill
                })
            });

        public void Render(RenderPack rp)
        {
            double total = Slices.Sum(s => s.Value);
            float[] sweeps = Slices.Select(x => (float)(x.Value / total) * 360).ToArray();

            Pixel origin = Axes.GetPixel(Coordinates.Origin);

            float minX = Math.Abs(Axes.GetPixelX(1) - origin.X);
            float minY = Math.Abs(Axes.GetPixelY(1) - origin.Y);
            float radius = Math.Min(minX, minY);
            float explosionRadius = (float)ExplodeFraction * radius;
            SKRect rect = new(-radius, -radius, radius, radius);

            using SKPath path = new();
            using SKPaint paint = new() { IsAntialias = true };

            float sweepStart = 0;
            for (int i = 0; i < Slices.Count(); i++)
            {
                using var _ = new SKAutoCanvasRestore(rp.Canvas);

                float rotation = sweepStart + sweeps[i] / 2;
                rp.Canvas.Translate(origin.X, origin.Y);
                rp.Canvas.RotateDegrees(rotation);
                rp.Canvas.Translate(explosionRadius, 0);

                if (sweeps[i] != 360)
                {
                    path.MoveTo(0, 0);
                    path.ArcTo(rect, -sweeps[i] / 2, sweeps[i], false);
                    path.Close();
                }
                else
                {
                    path.AddOval(rect);
                }

                Slices[i].Fill.ApplyToPaint(paint);
                paint.Shader = paint.Shader?.WithLocalMatrix(SKMatrix.CreateRotationDegrees(-rotation));
                rp.Canvas.DrawPath(path, paint);

                LineStyle.ApplyToPaint(paint);
                rp.Canvas.DrawPath(path, paint);

                path.Reset();
                sweepStart += sweeps[i];
            }
        }
    }
}
