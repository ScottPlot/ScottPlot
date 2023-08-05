using ScottPlot.Axis;

namespace ScottPlot.Plottables
{
    public class ErrorBar : IPlottable
    {
        public bool IsVisible { get; set; } = true;
        public IAxes Axes { get; set; } = Axis.Axes.Default;

        public IReadOnlyList<double> Xs { get; set; }
        public IReadOnlyList<double> Ys { get; set; }
        public IReadOnlyList<double>? XErrorsPositive { get; set; }
        public IReadOnlyList<double>? XErrorsNegative { get; set; }
        public IReadOnlyList<double>? YErrorsPositive { get; set; }
        public IReadOnlyList<double>? YErrorsNegative { get; set; }

        public LineStyle LineStyle { get; set; } = new();
        public float CapSize { get; set; } = 3;

        public ErrorBar(IReadOnlyList<double> xs, IReadOnlyList<double> ys, IReadOnlyList<double>? xErrorsPositive, IReadOnlyList<double>? xErrorsNegative, IReadOnlyList<double>? yErrorsPositive, IReadOnlyList<double>? yErrorsNegative, Color color)
        {
            Xs = xs;
            Ys = ys;
            XErrorsPositive = xErrorsPositive;
            XErrorsNegative = xErrorsNegative;
            YErrorsPositive = yErrorsPositive;
            YErrorsNegative = yErrorsNegative;
            LineStyle.Color = color;
        }

        public IEnumerable<LegendItem> LegendItems => Enumerable.Empty<LegendItem>();

        public AxisLimits GetAxisLimits()
        {
            ExpandingAxisLimits limits = new();

            for (int i = 0; i < Xs.Count; i++)
            {
                double x = Xs[i];
                double y = Ys[i];

                limits.ExpandX(x - XErrorsNegative?[i] ?? 0);
                limits.ExpandX(x + XErrorsPositive?[i] ?? 0);
                limits.ExpandY(y - YErrorsNegative?[i] ?? 0);
                limits.ExpandY(y + YErrorsPositive?[i] ?? 0);
            }

            return limits.AxisLimits;
        }

        public void Render(RenderPack rp)
        {
            RenderErrorBars(rp.Canvas, Xs, Ys, YErrorsPositive, YErrorsNegative);
            RenderErrorBars(rp.Canvas, Ys, Xs, XErrorsPositive, XErrorsNegative, true);
        }

        private void RenderErrorBars(SKCanvas canvas, IReadOnlyList<double> positions, IReadOnlyList<double> vals, IReadOnlyList<double>? errorPositive, IReadOnlyList<double>? errorNegative, bool horizontal = false)
        {
            using SKPaint paint = new();
            using SKPath path = new();

            LineStyle.ApplyToPaint(paint);

            for (int i = 0; i < vals.Count; i++)
            {
                double bottom = vals[i] - (errorNegative?[i] ?? 0);
                double top = vals[i] + (errorPositive?[i] ?? 0);

                if (bottom == top && bottom == vals[i])
                    continue;

                Coordinates centreBottom = horizontal ? new Coordinates(bottom, positions[i]) : new Coordinates(positions[i], bottom);
                Coordinates centreTop = horizontal ? new Coordinates(top, positions[i]) : new Coordinates(positions[i], top);


                Pixel capOffset = horizontal ? new(0, CapSize) : new(CapSize, 0);

                Pixel centreBottomPx = Axes.GetPixel(centreBottom);
                Pixel leftBottomPx = centreBottomPx - capOffset;
                Pixel rightBottomPx = centreBottomPx + capOffset;

                Pixel centreTopPx = Axes.GetPixel(centreTop);
                Pixel leftTopPx = centreTopPx - capOffset;
                Pixel rightTopPx = centreTopPx + capOffset;

                path.MoveTo(leftBottomPx.ToSKPoint());
                path.LineTo(rightBottomPx.ToSKPoint());

                path.MoveTo(centreBottomPx.ToSKPoint());
                path.LineTo(centreTopPx.ToSKPoint());

                path.MoveTo(leftTopPx.ToSKPoint());
                path.LineTo(rightTopPx.ToSKPoint());

            }

            canvas.DrawPath(path, paint);
        }
    }
}
