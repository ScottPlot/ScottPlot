namespace ScottPlot.Plottables;

public class IsoLines : IPlottable, IHasLine
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;
    public Label TickLabelStyle = new();
    public bool RotateLabels { get; set; } = true;

    public readonly List<(double, string)> ManualPositions = [];

    public Func<double, string> TickLabelFormatter = DefaultTickFormatter;
    public bool ExteriorTickLabels { get; set; } = false;

    public LineStyle LineStyle { get; set; } = new()
    {
        Width = 1,
        Color = Colors.Black.WithAlpha(.2),
        Pattern = LinePattern.DenselyDashed,
    };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public static string DefaultTickFormatter(double yIntercept)
    {
        return Math.Round(yIntercept, 3).ToString();
    }

    public virtual void Render(RenderPack rp)
    {
        List<PixelLine> lines = ManualPositions.Any()
            ? GetLinesManual(rp)
            : GetLinesAutomatic(rp);

        using SKPaint paint = new();
        RenderLines(rp, paint, lines);

        rp.CanvasState.DisableClipping();
        if (RotateLabels)
        {
            RenderLabelsRotated(rp, paint, lines);
        }
        else
        {
            RenderLabelsFixed(rp, paint, lines);
        }
    }

    private void RenderLines(RenderPack rp, SKPaint paint, List<PixelLine> lines)
    {
        LineStyle.ApplyToPaint(paint);
        lines.ForEach(line => LineStyle.Render(rp.Canvas, line, paint));
    }

    private void RenderLabelsFixed(RenderPack rp, SKPaint paint, List<PixelLine> lines)
    {
        float padding = 5;

        for (int i = 0; i < lines.Count; i++)
        {
            PixelLine line = lines[i];
            bool topEdge = line.Pixel2.X < rp.DataRect.Right;

            Pixel px;
            if (ExteriorTickLabels)
            {
                px = topEdge
                    ? new(line.Pixel2.X, line.Pixel2.Y - padding)
                    : new(rp.DataRect.Right + padding, line.Y(rp.DataRect.Right));

                TickLabelStyle.Alignment = topEdge
                    ? Alignment.LowerCenter
                    : Alignment.MiddleLeft;
            }
            else
            {
                px = topEdge
                    ? new(line.Pixel2.X, line.Pixel2.Y + padding)
                    : new(rp.DataRect.Right - padding, line.Y(rp.DataRect.Right));

                TickLabelStyle.Alignment = topEdge
                    ? Alignment.UpperCenter
                    : Alignment.MiddleRight;
            }

            // TODO: pass a CoordinateLine instead of a PixelLine
            Coordinates c1 = Axes.GetCoordinates(line.Pixel1);
            Coordinates c2 = Axes.GetCoordinates(line.Pixel2);
            CoordinateLine cLine = new(c1, c2);

            TickLabelStyle.Text = ManualPositions.Count == 0
                ? TickLabelFormatter.Invoke(cLine.YIntercept)
                : ManualPositions[i].Item2;

            TickLabelStyle.Render(rp.Canvas, px, paint);
        }
    }

    private void RenderLabelsRotated(RenderPack rp, SKPaint paint, List<PixelLine> lines)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            PixelLine line = lines[i];

            bool topEdge = line.Pixel2.X < rp.DataRect.Right;

            Pixel px;
            if (ExteriorTickLabels)
            {
                px = topEdge
                   ? new(
                       x: line.Pixel2.X + 15,
                       y: line.Pixel2.Y - 15)
                   : new(
                       x: rp.DataRect.Right + 10,
                       y: line.Y(rp.DataRect.Right) - 10);

                TickLabelStyle.Alignment = Alignment.MiddleLeft;
            }
            else
            {
                px = topEdge
                   ? new(
                       x: line.Pixel2.X,
                       y: line.Pixel2.Y + 5)
                   : new(
                       x: rp.DataRect.Right - 10,
                       y: line.Y(rp.DataRect.Right) + 10);

                TickLabelStyle.Alignment = Alignment.UpperRight;

                // don't render labels too close to the left edge
                if (topEdge && (px.X < (rp.DataRect.Left + 50)))
                    continue;

                // don't render labels too close to the bottom edge
                if (!topEdge && (px.Y > (rp.DataRect.Bottom - 40)))
                    continue;
            }

            if (px.Y > rp.DataRect.Bottom || px.X < rp.DataRect.Left)
                continue;

            // TODO: pass a CoordinateLine instead of a PixelLine
            Coordinates c1 = Axes.GetCoordinates(line.Pixel1);
            Coordinates c2 = Axes.GetCoordinates(line.Pixel2);
            CoordinateLine cLine = new(c1, c2);

            string label = ManualPositions.Count == 0
                ? TickLabelFormatter.Invoke(cLine.YIntercept)
                : ManualPositions[i].Item2;

            TickLabelStyle.Text = label;
            TickLabelStyle.Rotation = (float)line.SlopeDegrees;
            TickLabelStyle.Render(rp.Canvas, px, paint);
        }
    }

    private List<PixelLine> GetLinesAutomatic(RenderPack rp)
    {
        AxisLimits limits = rp.Plot.Axes.GetLimits();
        bool isWider = limits.HorizontalSpan > limits.VerticalSpan;
        return isWider ? GetLinesByX(rp) : GetLinesByY(rp);
    }

    private List<PixelLine> GetLinesByX(RenderPack rp)
    {
        List<PixelLine> lines = [];

        // determine where X ticks are placed and how far apart they are
        var xTickPositions = Axes.XAxis.TickGenerator.Ticks
            .Where(x => x.IsMajor)
            .Select(x => x.Position)
            .Reverse()
            .ToArray();

        if (xTickPositions.Length < 2)
            return lines;

        double xLast = xTickPositions[0];
        double xDelta = xTickPositions[0] - xTickPositions[1];
        xLast += xDelta * 5;

        // determine X positions (many start off the screen to the left)
        AxisLimits limits = rp.Plot.Axes.GetLimits();
        double[] isoLineXs = Enumerable.Range(0, 50)
            .Select(x => xLast - x * xDelta)
            .ToArray();

        // create isolines
        foreach (double x in isoLineXs)
        {
            // create a small piece of an isoline
            double slope = 1.0;
            Coordinates pt1 = new(x, 0);
            Coordinates pt2 = new(x + 1, slope);
            CoordinateLine line = new(pt1, pt2);
            line = line.ExtendTo(limits.Rect);
            if (line.End.X < limits.Left)
                return lines;

            // extend it to fit the data area
            PixelLine pxLine = Axes.GetPixelLine(line);
            lines.Add(pxLine);
        }

        return lines;
    }

    private List<PixelLine> GetLinesByY(RenderPack rp)
    {
        List<PixelLine> lines = [];

        // determine where X ticks are placed and how far apart they are
        var yTickPositions = Axes.YAxis.TickGenerator.Ticks
            .Where(x => x.IsMajor)
            .Select(x => x.Position)
            .Reverse()
            .ToArray();

        if (yTickPositions.Length < 2)
            return lines;

        double yLast = yTickPositions[0];
        double yDelta = yTickPositions[0] - yTickPositions[1];
        yLast += yLast * 5;

        // determine Y positions (many start off the screen to the bottom)
        AxisLimits limits = rp.Plot.Axes.GetLimits();
        double[] isoLineYs = Enumerable.Range(0, 50)
            .Select(y => yLast - y * yDelta)
            .ToArray();

        // create isolines
        foreach (double y in isoLineYs)
        {
            // create a small piece of an isoline
            double slope = 1.0;
            Coordinates pt1 = new(0, y);
            Coordinates pt2 = new(1, y + slope);
            CoordinateLine line = new(pt1, pt2);
            line = line.ExtendTo(limits.Rect);
            if (line.Start.X > limits.Right)
                return lines;

            // extend it to fit the data area
            PixelLine pxLine = Axes.GetPixelLine(line);
            lines.Add(pxLine);
        }

        return lines;
    }

    private List<PixelLine> GetLinesManual(RenderPack rp)
    {
        List<PixelLine> lines = [];

        AxisLimits limits = rp.Plot.Axes.GetLimits();

        for (int i = 0; i < ManualPositions.Count; i++)
        {
            (double y, _) = ManualPositions[i];

            // create a small piece of an isoline
            double slope = 1.0;
            Coordinates pt1 = new(0, y);
            Coordinates pt2 = new(1, y + slope);
            CoordinateLine line = new(pt1, pt2);
            line = line.ExtendTo(limits.Rect);
            if (line.Start.X > limits.Right)
                return lines;

            // extend it to fit the data area
            PixelLine pxLine = Axes.GetPixelLine(line);
            lines.Add(pxLine);
        }

        return lines;
    }
}
