namespace ScottPlot.Plottables;

public class Callout : IPlottable
{
    public Text LabelPlottable { get; } = new() { Padding = 5 };
    public Arrow ArrowPlottable { get; } = new();
    public Label LabelStyle => LabelPlottable.Label;
    public string Text { get => LabelStyle.Text; set => LabelStyle.Text = value; }

    public Color ArrowColor
    {
        get => ArrowPlottable.Color;
        set => ArrowPlottable.Color = value;
    }

    public Color TextColor
    {
        get => LabelPlottable.FontColor;
        set => LabelPlottable.FontColor = value;
    }

    public bool Bold
    {
        get => LabelPlottable.Bold;
        set => LabelPlottable.Bold = value;
    }

    public float FontSize
    {
        get => LabelPlottable.FontSize;
        set => LabelPlottable.FontSize = value;
    }

    public Color TextBorderColor
    {
        get => LabelPlottable.BorderColor;
        set => LabelPlottable.BorderColor = value;
    }

    public float TextBorderWidth
    {
        get => LabelPlottable.BorderWidth;
        set => LabelPlottable.BorderWidth = value;
    }

    public Color TextBackgroundColor
    {
        get => LabelPlottable.BackgroundColor;
        set => LabelPlottable.BackgroundColor = value;
    }

    public Coordinates TextCoordinates { get; set; }
    public Pixel TextPixel { get; private set; }

    public Coordinates TipCoordinates { get; set; }
    public Pixel TipPixel { get; private set; }
    public PixelRect LastRenderRect => LabelPlottable.Label.LastRenderPixelRect;

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

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

    public void Render(RenderPack rp)
    {
        TextPixel = Axes.GetPixel(TextCoordinates);

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
