namespace ScottPlot.Plottables;

public class Callout : LabelStyleProperties, IPlottable, IHasArrow, IHasLabel
{
    public Text LabelPlottable { get; } = new() { LabelPadding = 5 };
    public Arrow ArrowPlottable { get; } = new();
    public override Label LabelStyle { get => LabelPlottable.LabelStyle; set => LabelPlottable.LabelStyle = value; }
    public string Text { get => LabelStyle.Text; set => LabelStyle.Text = value; }

    public ArrowStyle ArrowStyle { get => ArrowPlottable.ArrowStyle; set => ArrowPlottable.ArrowStyle = value; }
    public Color ArrowLineColor { get => ArrowStyle.LineStyle.Color; set => ArrowStyle.LineStyle.Color = value; }
    public float ArrowLineWidth { get => ArrowStyle.LineStyle.Width; set => ArrowStyle.LineStyle.Width = value; }
    public Color ArrowFillColor { get => ArrowStyle.FillStyle.Color; set => ArrowStyle.FillStyle.Color = value; }
    public float ArrowMinimumLength { get => ArrowStyle.MinimumLength; set => ArrowStyle.MinimumLength = value; }
    public float ArrowMaximumLength { get => ArrowStyle.MaximumLength; set => ArrowStyle.MaximumLength = value; }
    public float ArrowOffset { get => ArrowStyle.Offset; set => ArrowStyle.Offset = value; }
    public ArrowAnchor ArrowAnchor { get => ArrowStyle.Anchor; set => ArrowStyle.Anchor = value; }
    public float ArrowWidth { get => ArrowStyle.ArrowWidth; set => ArrowStyle.ArrowWidth = value; }
    public float ArrowheadAxisLength { get => ArrowStyle.ArrowheadAxisLength; set => ArrowStyle.ArrowheadAxisLength = value; }
    public float ArrowheadLength { get => ArrowStyle.ArrowheadLength; set => ArrowStyle.ArrowheadLength = value; }
    public float ArrowheadWidth { get => ArrowStyle.ArrowheadWidth; set => ArrowStyle.ArrowheadWidth = value; }

    [Obsolete("use ArrowLineColor and ArrowFillColor", true)]
    public Color ArrowColor { get; set; }

    public Color TextColor
    {
        get => LabelPlottable.LabelFontColor;
        set => LabelPlottable.LabelFontColor = value;
    }

    public bool Bold
    {
        get => LabelPlottable.LabelBold;
        set => LabelPlottable.LabelBold = value;
    }

    public float FontSize
    {
        get => LabelPlottable.LabelFontSize;
        set => LabelPlottable.LabelFontSize = value;
    }

    public Color TextBorderColor
    {
        get => LabelPlottable.LabelBorderColor;
        set => LabelPlottable.LabelBorderColor = value;
    }

    public float TextBorderWidth
    {
        get => LabelPlottable.LabelBorderWidth;
        set => LabelPlottable.LabelBorderWidth = value;
    }

    public Color TextBackgroundColor
    {
        get => LabelPlottable.LabelBackgroundColor;
        set => LabelPlottable.LabelBackgroundColor = value;
    }

    public Coordinates TextCoordinates { get; set; }
    public Pixel TextPixel { get; private set; }

    public Coordinates TipCoordinates { get; set; }
    public Pixel TipPixel { get; private set; }
    public PixelRect LastRenderRect => LabelPlottable.LabelStyle.LastRenderPixelRect;

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
        using SKPaint paint = new();
        PixelSize labelSize = LabelStyle.Measure(LabelStyle.Text, paint).Size;

        Pixel[] attachPoints = [
            new(TextPixel.X - LabelStyle.PixelPadding.Left, TextPixel.Y + labelSize.Height / 2), // west
            new(TextPixel.X + labelSize.Width + LabelStyle.PixelPadding.Right, TextPixel.Y + labelSize.Height / 2), //east
            new(TextPixel.X + labelSize.Width / 2, TextPixel.Y - LabelStyle.PixelPadding.Top), //north
            new(TextPixel.X + labelSize.Width / 2, TextPixel.Y + labelSize.Height + LabelStyle.PixelPadding.Bottom), // south
            new(TextPixel.X - LabelStyle.PixelPadding.Left, TextPixel.Y - LabelStyle.PixelPadding.Top), // northWest
            new(TextPixel.X + labelSize.Width + LabelStyle.PixelPadding.Right, TextPixel.Y - LabelStyle.PixelPadding.Top), // northEast
            new(TextPixel.X - LabelStyle.PixelPadding.Left, TextPixel.Y + labelSize.Height + LabelStyle.PixelPadding.Bottom), // southWest
            new(TextPixel.X + labelSize.Width + LabelStyle.PixelPadding.Right, TextPixel.Y + labelSize.Height + LabelStyle.PixelPadding.Bottom) // southEast
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

    public virtual void Render(RenderPack rp)
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
