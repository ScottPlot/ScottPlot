namespace ScottPlot.Plottables
{
    public class ErrorBar : IPlottable, IHasLine, IHasLegendText
    {
        // TODO: use an errorbar source instead of so many lists

        public bool IsVisible { get; set; } = true;
        public IAxes Axes { get; set; } = new Axes();

        public IReadOnlyList<double> Xs { get; set; }
        public IReadOnlyList<double> Ys { get; set; }
        public IReadOnlyList<double>? XErrorPositive { get; set; }
        public IReadOnlyList<double>? XErrorNegative { get; set; }
        public IReadOnlyList<double>? YErrorPositive { get; set; }
        public IReadOnlyList<double>? YErrorNegative { get; set; }

        public LineStyle LineStyle { get; set; } = new() { Width = 1 };
        public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
        public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
        public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

        public float CapSize { get; set; } = 3;
        public Color Color
        {
            get => LineStyle.Color;
            set => LineStyle.Color = value;
        }

        public ErrorBar(IReadOnlyList<double> xs, IReadOnlyList<double> ys, IReadOnlyList<double>? xErrorsPositive, IReadOnlyList<double>? xErrorsNegative, IReadOnlyList<double>? yErrorsPositive, IReadOnlyList<double>? yErrorsNegative)
        {
            Xs = xs;
            Ys = ys;
            XErrorPositive = xErrorsPositive;
            XErrorNegative = xErrorsNegative;
            YErrorPositive = yErrorsPositive;
            YErrorNegative = yErrorsNegative;
        }

        public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, LineStyle);

        [Obsolete("use LegendText")]
        public string Label { get => LegendText; set => LegendText = value; }
        public string LegendText { get; set; } = string.Empty;

        public AxisLimits GetAxisLimits()
        {
            ExpandingAxisLimits limits = new();

            for (int i = 0; i < Xs.Count; i++)
            {
                double xMin = XErrorNegative is null ? Xs[i] : Xs[i] - XErrorNegative[i];
                double xMax = XErrorPositive is null ? Xs[i] : Xs[i] + XErrorPositive[i];
                double yMin = YErrorNegative is null ? Ys[i] : Ys[i] - YErrorNegative[i];
                double yMax = YErrorPositive is null ? Ys[i] : Ys[i] + YErrorPositive[i];

                limits.ExpandX(xMin);
                limits.ExpandX(xMax);
                limits.ExpandY(yMin);
                limits.ExpandY(yMax);
            }

            return limits.AxisLimits;
        }

        public virtual void Render(RenderPack rp)
        {
            RenderErrorBars(rp.Canvas, Xs, Ys, YErrorPositive, YErrorNegative);
            RenderErrorBars(rp.Canvas, Ys, Xs, XErrorPositive, XErrorNegative, true);
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
