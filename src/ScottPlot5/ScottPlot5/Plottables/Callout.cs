namespace ScottPlot.Plottables;

public class Callout : IPlottable
{
    public Text LabelPlottable { get; } = new();
    public Arrow ArrowPlottable { get; } = new();

    public Coordinates TextCoordinates { get; set; }
    public Pixel TextPixel { get; private set; }

    public Coordinates TipCoordinates { get; set; }
    public Pixel TipPixel { get; private set; }
    public PixelRect LastRenderRect => LabelPlottable.Label.LastRenderPixelRect;

    public Label LabelStyle => LabelPlottable.Label;

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;


    private float MouseDistanceFromLabelX { get; set; } = 0;
    private float MouseDistanceFromLabelY { get; set; } = 0;

    public AxisLimits GetAxisLimits()
    {
        ExpandingAxisLimits expandingLimits = new();
        expandingLimits.Expand(LabelPlottable);
        expandingLimits.Expand(ArrowPlottable);
        return expandingLimits.AxisLimits;
    }

    /// <summary>
    /// Calculates the attachment point on the label based on the closest distance from the given pixel location.
    /// </summary>
    /// <remarks>
    /// <returns>The coordinates of the attachment point on the label.</returns>
    public Coordinates CalculateClosestAttachPoint()
    {
        PixelSize labelSize = LabelStyle.Measure();

        Pixel[] attachPoints = [
            new(TextPixel.X - LabelStyle.Padding, TextPixel.Y + labelSize.Height / 2), // west
            new(TextPixel.X + labelSize.Width + LabelStyle.Padding, TextPixel.Y + labelSize.Height / 2), //east
            new(TextPixel.X + labelSize.Width / 2, TextPixel.Y - LabelStyle.Padding), //north
            new(TextPixel.X + labelSize.Width / 2, TextPixel.Y + labelSize.Height + LabelStyle.Padding), // south
            new(TextPixel.X - LabelStyle.Padding, TextPixel.Y - LabelStyle.Padding), // northWest
            new(TextPixel.X + labelSize.Width + LabelStyle.Padding, TextPixel.Y - LabelStyle.Padding), // northEast
            new(TextPixel.X - LabelStyle.Padding, TextPixel.Y + labelSize.Height + LabelStyle.Padding), // southWest
            new(TextPixel.X + labelSize.Width + LabelStyle.Padding, TextPixel.Y + labelSize.Height + LabelStyle.Padding) // southEast
        ];

        Pixel closestAttachPoint = attachPoints[0];
        float distanceMin = TipPixel.DistanceFrom(attachPoints[0]);

        for (int i = 1; i < attachPoints.Length; i++)
        {
            float distance = TipPixel.DistanceFrom(attachPoints[i]);
            if (distance < distanceMin)
            {
                closestAttachPoint = attachPoints[i];
                distanceMin = distance;
            }
        }

        return Axes.GetCoordinates(closestAttachPoint);
    }

    #region interactivity

    // TODO: move these methods somewhere else...

    /// <summary>
    /// Prepares the label for movement by calculating the difference between the mouse position and the label position.
    /// This allows maintaining the mouse position relative to the label during movement.
    /// </summary>
    /// <param name="x">The horizontal coordinate of the mouse position.</param>
    /// <param name="y">The vertical coordinate of the mouse position.</param>
    public void StartMove(float x, float y)
    {
        MouseDistanceFromLabelX = x - TextPixel.X;
        MouseDistanceFromLabelY = y - TextPixel.Y;
    }

    /// <summary>
    /// Updates the position of the label continuously during movement by applying the previously calculated delta values.
    /// </summary>
    /// <param name="x">The new horizontal coordinate for the label.</param>
    /// <param name="y">The new vertical coordinate for the label.</param>
    public void Move(float x, float y)
    {
        double x2 = x - MouseDistanceFromLabelX;
        double y2 = y - MouseDistanceFromLabelY;
        Pixel px = new(x2, y2);
        TextCoordinates = Axes.GetCoordinates(px);
    }

    #endregion

    public void Render(RenderPack rp)
    {
        Pixel pixelLocation = Axes.GetPixel(TextCoordinates);
        TextPixel = new Pixel(
            x: pixelLocation.X + LabelStyle.Padding,
            y: pixelLocation.Y + LabelStyle.Padding);

        TipPixel = Axes.GetPixel(TipCoordinates);

        LabelPlottable.LabelText = LabelStyle.Text;
        LabelPlottable.Axes = Axes;
        LabelPlottable.Location = TextCoordinates;
        LabelPlottable.Render(rp);


        ArrowPlottable.Axes = Axes;
        ArrowPlottable.Base = CalculateClosestAttachPoint();
        ArrowPlottable.Tip = TipCoordinates;
        ArrowPlottable.Render(rp);
    }
}