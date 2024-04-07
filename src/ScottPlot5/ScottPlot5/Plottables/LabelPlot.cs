namespace ScottPlot.Plottables;

public class LabelPlot : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public Coordinates Location { get; set; }
    public Pixel PixelLocation => Axes.GetPixel(Location);
    public Coordinates LinkLocation { get; set; }
    public Pixel PixelLinkLocation => Axes.GetPixel(LinkLocation);

    public readonly Label Label = new();
    public LineStyle LineStyle { get; set; } = new();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    /// <summary>
    /// Delta X between mouse position and label location
    /// </summary>
    private float deltaX;

    /// <summary>
    /// Delta Y between mouse position and label location
    /// </summary>
    private float deltaY;

    public AxisLimits GetAxisLimits()
    {
        return new AxisLimits(Location);
    }

    public bool IsUnderMouse(float x, float y)
    {
        return Label.LabelRect.Contains(x, y);
    }

    /// <summary>
    /// Calculates the attachment point on the label based on the closest distance from the given pixel location.
    /// </summary>
    /// <remarks>
    /// <returns>The coordinates of the attachment point on the label.</returns>
    public Coordinates CalculateClosestAttachPoint()
    {
        PixelSize labelSize = Label.Measure();

        Pixel[] attachPoints = [
            new(PixelLocation.X - Label.Padding, PixelLocation.Y + labelSize.Height / 2), // west
            new(PixelLocation.X + labelSize.Width + Label.Padding, PixelLocation.Y + labelSize.Height / 2), //east
            new(PixelLocation.X + labelSize.Width / 2, PixelLocation.Y - Label.Padding), //north
            new(PixelLocation.X + labelSize.Width / 2, PixelLocation.Y + labelSize.Height + Label.Padding), // south
            new(PixelLocation.X - Label.Padding, PixelLocation.Y - Label.Padding), // northWest
            new(PixelLocation.X + labelSize.Width + Label.Padding, PixelLocation.Y - Label.Padding), // northEast
            new(PixelLocation.X - Label.Padding, PixelLocation.Y + labelSize.Height + Label.Padding), // southWest
            new(PixelLocation.X + labelSize.Width + Label.Padding, PixelLocation.Y + labelSize.Height + Label.Padding) // southEast
        ];

        Pixel closestAttachPoint = attachPoints[0];
        float distanceMin = GetDistance(PixelLinkLocation, attachPoints[0]);
        for (int i = 1; i < attachPoints.Length; i++)
        {
            float distance = GetDistance(PixelLinkLocation, attachPoints[i]);
            if (distance < distanceMin)
            {
                closestAttachPoint = attachPoints[i];
                distanceMin = distance;
            }
        }

        return Axes.GetCoordinates(closestAttachPoint);
    }

    /// <summary>
    /// Calculates the squared distance between two pixels.
    /// </summary>
    /// <param name="position1">The first pixel's position.</param>
    /// <param name="position2">The second pixel's position.</param>
    /// <returns>The squared distance between the two pixels.</returns>
    public float GetDistance(Pixel position1, Pixel position2)
    {
        return GetDistance(position1.X, position1.Y, position2.X, position2.Y);
    }

    /// <summary>
    /// Calculates the squared distance between two points specified by their coordinates.
    /// </summary>
    /// <param name="x1">The horizontal coordinate of the first point.</param>
    /// <param name="y1">The vertical coordinate of the first point.</param>
    /// <param name="x2">The horizontal coordinate of the second point.</param>
    /// <param name="y2">The vertical coordinate of the second point.</param>
    /// <returns>The squared distance between the two points.</returns>
    public float GetDistance(float x1, float y1, float x2, float y2)
    {
        return (float)(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    }

    /// <summary>
    /// Prepares the label for movement by calculating the difference between the mouse position and the label position.
    /// This allows maintaining the mouse position relative to the label during movement.
    /// </summary>
    /// <param name="x">The horizontal coordinate of the mouse position.</param>
    /// <param name="y">The vertical coordinate of the mouse position.</param>
    public void StartMove(float x, float y)
    {
        deltaX = x - PixelLocation.X;
        deltaY = y - PixelLocation.Y;
    }

    /// <summary>
    /// Updates the position of the label continuously during movement by applying the previously calculated delta values.
    /// </summary>
    /// <param name="x">The new horizontal coordinate for the label.</param>
    /// <param name="y">The new vertical coordinate for the label.</param>
    public void Move(float x, float y)
    {
        Location = Axes.GetCoordinates(new Pixel(x - deltaX, y - deltaY));
    }

    public void Render(RenderPack rp)
    {
        Label.Render(rp.Canvas, PixelLocation);

        using SKPaint paint = new();
        CoordinateLine line = new(CalculateClosestAttachPoint(), LinkLocation);
        PixelLine pxLine = Axes.GetPixelLine(line);
        Drawing.DrawLine(rp.Canvas, paint, pxLine, LineStyle);
    }
}
