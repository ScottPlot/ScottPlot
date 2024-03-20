namespace ScottPlot.Plottables;

public class IsoLines : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;
    LineStyle LineStyle { get; set; } = new()
    {
        Width = 3,
        Color = Colors.Blue.WithAlpha(.2),
    };

    public void Render(RenderPack rp)
    {
        AxisLimits limits = rp.Plot.Axes.GetLimits();
        bool isWider = limits.HorizontalSpan > limits.VerticalSpan;
        List<PixelLine> lines = isWider ? GetLinesByX(rp) : GetLinesByY(rp);

        using SKPaint paint = new();
        LineStyle.ApplyToPaint(paint);
        lines.ForEach(line => Drawing.DrawLine(rp.Canvas, paint, line));
    }

    private List<PixelLine> GetLinesByX(RenderPack rp)
    {
        List<PixelLine> lines = new();

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
        List<PixelLine> lines = new();

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
