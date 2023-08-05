using ScottPlot.Axis;

namespace ScottPlot.Plottables;

/// <summary>
/// Holds values (not styling information) for a single box
/// </summary>
public struct Box
{
    public double Position { get; set; }
    public double BoxMin { get; set; }
    public double BoxMiddle { get; set; }
    public double BoxMax { get; set; }
    public double? WhiskerMin { get; set; }
    public double? WhiskerMax { get; set; }
}

/// <summary>
/// Describes a group of boxes which are all styled the same
/// </summary>
public class BoxGroup
{
    public IList<Box> Boxes { get; set; } = Array.Empty<Box>();
    public string? Label { get; set; }
    public FillStyle Fill { get; set; } = new();
    public LineStyle Stroke { get; set; } = new();
}

/// <summary>
/// Describes multiple groups of boxes, each group with its own styling information
/// </summary>
public class BoxGroups
{
    public IList<BoxGroup> Series { get; set; } = new List<BoxGroup>();
    public Orientation Orientation { get; set; } = Orientation.Vertical;
    public double Padding { get; set; } = 0.05;
    private double MaxBoxWidth => 1 - Padding * 2;

    public bool GroupBoxesWithSameXPosition = true;

    public IEnumerable<LegendItem> LegendItems => Series
        .Where(x => !string.IsNullOrWhiteSpace(x.Label))
        .Select(x => new LegendItem()
        {
            Label = x.Label,
            Fill = x.Fill,
        });

    public AxisLimits GetAxisLimits()
    {
        ExpandingAxisLimits limits = new();

        foreach (var s in Series)
        {
            foreach (var b in s.Boxes)
            {
                if (Orientation == Orientation.Vertical)
                {
                    limits.ExpandX(b.Position);
                    limits.ExpandY(b.BoxMin);
                    limits.ExpandY(b.BoxMiddle);
                    limits.ExpandY(b.BoxMax);
                    limits.ExpandY(b.WhiskerMin ?? limits.YMin);
                    limits.ExpandY(b.WhiskerMax ?? limits.YMin);
                }
                else
                {
                    limits.ExpandY(b.Position);
                    limits.ExpandX(b.BoxMin);
                    limits.ExpandX(b.BoxMiddle);
                    limits.ExpandX(b.BoxMax);
                    limits.ExpandX(b.WhiskerMin ?? limits.YMin);
                    limits.ExpandX(b.WhiskerMax ?? limits.YMin);
                }
            }
        }

        limits.XMin -= MaxBoxWidth / 2;
        limits.XMax += MaxBoxWidth / 2;
        limits.YMin -= MaxBoxWidth / 2;
        limits.YMax += MaxBoxWidth / 2;

        return limits.AxisLimits;
    }
}

public class BoxPlot : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = Axis.Axes.Default;
    public BoxGroups Groups { get; set; } = new();
    public IEnumerable<LegendItem> LegendItems => Groups.LegendItems;
    public AxisLimits GetAxisLimits() => Groups.GetAxisLimits();

    public void Render(RenderPack rp)
    {
        using var paint = new SKPaint();
        var boxesByXCoordinate = Groups.Series
            .SelectMany(s => s.Boxes.Select(b => (Box: b, Series: s)))
            .ToLookup(t => t.Box.Position);

        int maxPerXCoordinate = boxesByXCoordinate.Max(g => g.Count());
        double widthPerGroup = 1 - (maxPerXCoordinate + 1) * Groups.Padding;
        double boxWidth = (1 - Groups.Padding) * widthPerGroup / maxPerXCoordinate;

        foreach (IGrouping<double, (Box Box, BoxGroup Series)>? group in boxesByXCoordinate)
        {
            int boxesInGroup = group.Count();
            int i = 0;
            foreach (var t in group)
            {
                double boxWidthAndPadding = boxWidth + Groups.Padding;
                double groupWidth = boxWidthAndPadding * boxesInGroup;

                double newPosition = Groups.GroupBoxesWithSameXPosition ?
                    group.Key - groupWidth / 2 + (i + 0.5) * boxWidthAndPadding :
                    group.Key;

                DrawBox(rp, paint, t.Box, t.Series, newPosition, boxWidth);

                if (t.Box.WhiskerMin.HasValue)
                    DrawWhisker(rp, paint, t.Box, t.Series, newPosition, boxWidth, t.Box.WhiskerMin.Value);

                if (t.Box.WhiskerMax.HasValue)
                    DrawWhisker(rp, paint, t.Box, t.Series, newPosition, boxWidth, t.Box.WhiskerMax.Value);

                i++;
            }
        }
    }

    /// <summary>
    /// Get the two rectangles (above and below the midline) for a box.
    /// Rectangles have the given width, centered at the given X position.
    /// </summary>
    private (PixelRect topRect, PixelRect bottomRect) GetRects(Box box, double x, double width)
    {
        if (Groups.Orientation == Orientation.Vertical)
        {
            var topLeft = Axes.GetPixel(new Coordinates(x - width / 2, box.BoxMax));
            var midRight = Axes.GetPixel(new Coordinates(x + width / 2, box.BoxMiddle));
            var botLeft = Axes.GetPixel(new Coordinates(x - width / 2, box.BoxMin));

            var topRect = new PixelRect(topLeft, midRight);
            var botRect = new PixelRect(midRight, botLeft);

            return (topRect, botRect);
        }
        else
        {
            var topLeft = Axes.GetPixel(new Coordinates(box.BoxMin, x - width / 2));
            var midRight = Axes.GetPixel(new Coordinates(box.BoxMiddle, x + width / 2));
            var botLeft = Axes.GetPixel(new Coordinates(box.BoxMax, x - width / 2));

            var topRect = new PixelRect(topLeft, midRight);
            var botRect = new PixelRect(midRight, botLeft);

            return (topRect, botRect);
        }
    }

    /// <summary>
    /// Render a single box of a series centered at the given X and with the given width.
    /// The series is passed in to reference styling information from.
    /// </summary>
    public void DrawBox(RenderPack rp, SKPaint paint, Box box, BoxGroup series, double x, double width)
    {
        (PixelRect topRect, PixelRect botRect) = GetRects(box, x, width);

        series.Fill.ApplyToPaint(paint);
        rp.Canvas.DrawRect(topRect.ToSKRect(), paint);
        rp.Canvas.DrawRect(botRect.ToSKRect(), paint);

        series.Stroke.ApplyToPaint(paint);

        // Done individually with DrawLine rather than with DrawRect to avoid double-stroking the middle line
        if (Groups.Orientation == Orientation.Vertical)
        {
            rp.Canvas.DrawLine(topRect.TopLeft.ToSKPoint(), topRect.TopRight.ToSKPoint(), paint);
            rp.Canvas.DrawLine(topRect.BottomLeft.ToSKPoint(), topRect.BottomRight.ToSKPoint(), paint);
            rp.Canvas.DrawLine(botRect.BottomLeft.ToSKPoint(), botRect.BottomRight.ToSKPoint(), paint);

            rp.Canvas.DrawLine(topRect.TopLeft.ToSKPoint(), botRect.BottomLeft.ToSKPoint(), paint);
            rp.Canvas.DrawLine(topRect.TopRight.ToSKPoint(), botRect.BottomRight.ToSKPoint(), paint);
        }
        else
        {
            // TODO: outlining doesn't look quite right
            rp.Canvas.DrawLine(botRect.TopLeft.ToSKPoint(), botRect.BottomLeft.ToSKPoint(), paint);
            rp.Canvas.DrawLine(botRect.TopRight.ToSKPoint(), botRect.BottomRight.ToSKPoint(), paint);
            rp.Canvas.DrawLine(topRect.TopRight.ToSKPoint(), topRect.BottomRight.ToSKPoint(), paint);

            rp.Canvas.DrawLine(botRect.TopLeft.ToSKPoint(), topRect.TopRight.ToSKPoint(), paint);
            rp.Canvas.DrawLine(botRect.BottomLeft.ToSKPoint(), topRect.BottomRight.ToSKPoint(), paint);
        }
    }

    private void DrawWhisker(RenderPack rp, SKPaint paint, Box box, BoxGroup series, double x, double boxWidth, double value)
    {
        Coordinates whiskerBase = value > box.BoxMax ? new(x, box.BoxMax) : new(x, box.BoxMin);
        Coordinates whiskerTip = new(x, value);

        if (Groups.Orientation == Orientation.Horizontal)
            (whiskerBase, whiskerTip) = (new(whiskerBase.Y, whiskerBase.X), new(whiskerTip.Y, whiskerTip.X));

        Pixel whiskerBasePx = Axes.GetPixel(whiskerBase);
        Pixel whiskerTipPx = Axes.GetPixel(whiskerTip);

        series.Stroke.ApplyToPaint(paint);
        rp.Canvas.DrawLine(whiskerBasePx.ToSKPoint(), whiskerTipPx.ToSKPoint(), paint);

        float whiskerWidth = Math.Max((float)Axes.XAxis.GetPixelDistance(boxWidth, rp.DataRect) / 5, 20);
        Pixel whiskerEarOffset = Groups.Orientation == Orientation.Vertical
            ? new Pixel(whiskerWidth / 2, 0)
            : new Pixel(0, whiskerWidth / 2);

        Pixel whiskerLeft = whiskerTipPx - whiskerEarOffset;
        Pixel whiskerRight = whiskerTipPx + whiskerEarOffset;

        rp.Canvas.DrawLine(whiskerLeft.ToSKPoint(), whiskerRight.ToSKPoint(), paint);
    }
}
