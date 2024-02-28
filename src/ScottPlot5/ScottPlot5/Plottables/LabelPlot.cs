namespace ScottPlot.Plottables;

public class LabelPlot : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public Coordinates Location { get; set; }
    public Coordinates LinkLocation { get; set; }
    public readonly Label Label = new();
    public LineStyle LineStyle { get; set; } = new();

    public MarkerStyle MarkerStyle { get; set; } = new() { Size = 0 };
    public Color MarkerColor
    {
        get => MarkerStyle.Fill.Color;
        set
        {
            MarkerStyle.Fill.Color = value;
            MarkerStyle.Outline.Color = value;
        }
    }

    public MarkerShape MarkerShape
    {
        get => MarkerStyle.Shape;
        set => MarkerStyle.Shape = value;
    }

    public float MarkerSize
    {
        get => MarkerStyle.Size;
        set => MarkerStyle.Size = value;
    }

    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public AxisLimits GetAxisLimits()
    {
        return new AxisLimits(Location);
    }

    /// <summary>
    /// Calculates the attachment point on the label based on Calculates the attachment point on the label based on the position where the label is attached.
    /// </summary>
    /// <remarks>
    /// <returns>The coordinates of the attachment point on the label.</returns>
    public Coordinates GetAttachPoint()
    {
        var labelSize = Label.Measure();

        Pixel etiquetteLocation = Axes.GetPixel(Location);
        Pixel linkLocation = Axes.GetPixel(LinkLocation);

        // Calculate attachment points on the label's edges
        Pixel leftAttach = new(etiquetteLocation.X - Label.Padding, etiquetteLocation.Y + labelSize.Height / 2);
        Pixel RightAttach = new(etiquetteLocation.X + labelSize.Width + Label.Padding, etiquetteLocation.Y + labelSize.Height / 2);
        Pixel TopAttach = new(etiquetteLocation.X + labelSize.Width / 2, etiquetteLocation.Y - Label.Padding);
        Pixel BottomAttach = new(etiquetteLocation.X + labelSize.Width / 2, etiquetteLocation.Y + labelSize.Height + Label.Padding);

        Pixel closestPoint = leftAttach;
        float minDistance = GetDistance(linkLocation.X, linkLocation.Y, leftAttach.X, leftAttach.Y);

        float distance = GetDistance(linkLocation.X, linkLocation.Y, RightAttach.X, RightAttach.Y);
        if (distance < minDistance)
        {
            minDistance = distance;
            closestPoint = RightAttach;
        }

        distance = GetDistance(linkLocation.X, linkLocation.Y, TopAttach.X, TopAttach.Y);
        if (distance < minDistance)
        {
            minDistance = distance;
            closestPoint = TopAttach;
        }

        distance = GetDistance(linkLocation.X, linkLocation.Y, BottomAttach.X, BottomAttach.Y);
        if (distance < minDistance)
        {
            closestPoint = BottomAttach;
        }

        return Axes.GetCoordinates(closestPoint);
    }

    private static float GetDistance(float x1, float y1, float x2, float y2)
    {
        return (float)(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    }

    public void Render(RenderPack rp)
    {
        Pixel pixelLocation = Axes.GetPixel(Location);
        Label.Render(rp.Canvas, pixelLocation);
        using SKPaint paint = new();
        CoordinateLine line = new(GetAttachPoint(), LinkLocation);
        PixelLine pxLine = Axes.GetPixelLine(line);
        Drawing.DrawLine(rp.Canvas, paint, pxLine, LineStyle);
    }
}
