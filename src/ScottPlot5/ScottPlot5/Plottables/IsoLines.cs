using ScottPlot.DataSources;

namespace ScottPlot.Plottables;

public class IsoLines : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;
    public Label TickLabelStyle = new();

    LineStyle LineStyle { get; set; } = new()
    {
        Width = 1,
        Color = Colors.Black.WithAlpha(.2),
        Pattern = LinePattern.DenselyDashed,
    };

    public void Render(RenderPack rp)
    {
        AxisLimits limits = rp.Plot.Axes.GetLimits();
        bool isWider = limits.HorizontalSpan > limits.VerticalSpan;
        List<PixelLine> lines = isWider ? GetLinesByX(rp) : GetLinesByY(rp);

        using SKPaint paint = new();
        RenderLines(rp, paint, lines);

        RenderLabels(rp, paint, lines);
    }

    private void RenderLines(RenderPack rp, SKPaint paint, List<PixelLine> lines)
    {
        LineStyle.ApplyToPaint(paint);
        lines.ForEach(line => LineStyle.Render(rp.Canvas, paint, line));
    }

    private void RenderLabels(RenderPack rp, SKPaint paint, List<PixelLine> lines)
    {
        float padding = 5;

        foreach (var line in lines)
        {
            // TODO: rotate text to match the slope of the isoline

            bool topEdge = line.Pixel2.X < rp.DataRect.Right;

            Pixel px = topEdge
                ? new(line.Pixel2.X, line.Pixel2.Y + padding)
                : new(rp.DataRect.Right - padding, line.Y(rp.DataRect.Right));

            TickLabelStyle.Alignment = topEdge
            ? Alignment.UpperCenter
            : Alignment.MiddleRight;

            // TODO: pass a CoordinateLine instead of a PixelLine
            Coordinates c1 = Axes.GetCoordinates(line.Pixel1);
            Coordinates c2 = Axes.GetCoordinates(line.Pixel2);
            CoordinateLine cLine = new(c1, c2);
            double isoValue = Math.Round(cLine.YIntercept, 5);
            TickLabelStyle.Text = $"{isoValue}";

            TickLabelStyle.Render(rp.Canvas, px, paint);
        }
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
}
