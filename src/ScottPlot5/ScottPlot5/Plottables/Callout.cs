namespace ScottPlot.Plottables;

public class Callout : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public Coordinates LabelCoordinates { get; set; }
    public Pixel LabelPixelLocation
    {
        get
        {
            Pixel pixelLocation = Axes.GetPixel(LabelCoordinates);

            return new Pixel(pixelLocation.X + Label.Padding, pixelLocation.Y + Label.Padding);
        }
    }

    public Coordinates LineCoordinates { get; set; }
    public Pixel LinePixelLocation => Axes.GetPixel(LineCoordinates);

    public readonly Label Label = new();
    public LineStyle LineStyle { get; set; } = new();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    /// <summary>
    /// Delta X between mouse position and label location
    /// </summary>
    private float deltaX = 0;

    /// <summary>
    /// Delta Y between mouse position and label location
    /// </summary>
    private float deltaY = 0;

    public AxisLimits GetAxisLimits()
    {
        return new AxisLimits(LabelCoordinates);
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
            new(LabelPixelLocation.X - Label.Padding, LabelPixelLocation.Y + labelSize.Height / 2), // west
            new(LabelPixelLocation.X + labelSize.Width + Label.Padding, LabelPixelLocation.Y + labelSize.Height / 2), //east
            new(LabelPixelLocation.X + labelSize.Width / 2, LabelPixelLocation.Y - Label.Padding), //north
            new(LabelPixelLocation.X + labelSize.Width / 2, LabelPixelLocation.Y + labelSize.Height + Label.Padding), // south
            new(LabelPixelLocation.X - Label.Padding, LabelPixelLocation.Y - Label.Padding), // northWest
            new(LabelPixelLocation.X + labelSize.Width + Label.Padding, LabelPixelLocation.Y - Label.Padding), // northEast
            new(LabelPixelLocation.X - Label.Padding, LabelPixelLocation.Y + labelSize.Height + Label.Padding), // southWest
            new(LabelPixelLocation.X + labelSize.Width + Label.Padding, LabelPixelLocation.Y + labelSize.Height + Label.Padding) // southEast
        ];

        Pixel closestAttachPoint = attachPoints[0];
        float distanceMin = GetDistance(LinePixelLocation, attachPoints[0]);
        for (int i = 1; i < attachPoints.Length; i++)
        {
            float distance = GetDistance(LinePixelLocation, attachPoints[i]);
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
        deltaX = x - LabelPixelLocation.X;
        deltaY = y - LabelPixelLocation.Y;
    }

    /// <summary>
    /// Updates the position of the label continuously during movement by applying the previously calculated delta values.
    /// </summary>
    /// <param name="x">The new horizontal coordinate for the label.</param>
    /// <param name="y">The new vertical coordinate for the label.</param>
    public void Move(float x, float y)
    {
        LabelCoordinates = Axes.GetCoordinates(new Pixel(x - deltaX, y - deltaY));
    }

    public void Render(RenderPack rp)
    {
        Label.Render(rp.Canvas, LabelPixelLocation);

        using SKPaint paint = new();
        CoordinateLine line = new(CalculateClosestAttachPoint(), LineCoordinates);
        PixelLine pxLine = Axes.GetPixelLine(line);
        Drawing.DrawLine(rp.Canvas, paint, pxLine, LineStyle);
    }
}
