using ScottPlot.Style;

namespace ScottPlot.Axis.StandardAxes;

public abstract class XAxisBase : IAxis
{
    public bool IsVisible { get; set; } = true;

    public double Min
    {
        get => Range.Min;
        set => Range.Min = value;
    }

    public double Max
    {
        get => Range.Max;
        set => Range.Max = value;
    }

    public double Width => Range.Span;

    public virtual CoordinateRange Range { get; private set; } = CoordinateRange.NotSet;

    public ITickGenerator TickGenerator { get; set; } = null!;

    public Label Label { get; private set; } = new()
    {
        Text = string.Empty,
        Font = new() { Size = 16, Bold = true },
    };

    public FontStyle TickFont { get; set; } = new();

    public abstract Edge Edge { get; }
    public bool ShowDebugInformation { get; set; } = false;

    public float MajorTickLength { get; set; } = 4;
    public float MajorTickWidth { get; set; } = 1;

    public Color MajorTickColor { get; set; } = Colors.Black;
    public TickStyle MajorTickStyle => new()
    {
        Length = MajorTickLength,
        Width = MajorTickWidth,
        Color = MajorTickColor
    };

    public float MinorTickLength { get; set; } = 2;
    public float MinorTickWidth { get; set; } = 1;
    public Color MinorTickColor { get; set; } = Colors.Black;
    public TickStyle MinorTickStyle => new()
    {
        Length = MinorTickLength,
        Width = MinorTickWidth,
        Color = MinorTickColor
    };

    public LineStyle FrameLineStyle { get; } = new();

    public float Measure()
    {
        if (!IsVisible)
            return 0;

        float largestTickSize = MeasureTicks();
        float largestTickLabelSize = Label.Measure().Height;
        float spaceBetweenTicksAndAxisLabel = 15;
        return largestTickSize + largestTickLabelSize + spaceBetweenTicksAndAxisLabel;
    }

    private float MeasureTicks()
    {
        using SKPaint paint = TickFont.MakePaint();

        float largestTickHeight = 0;

        foreach (Tick tick in TickGenerator.GetVisibleTicks(Range))
        {
            PixelSize tickLabelSize = Drawing.MeasureString(tick.Label, paint);
            largestTickHeight = Math.Max(largestTickHeight, tickLabelSize.Height + 10);
        }

        return largestTickHeight;
    }

    public float GetPixel(double position, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Width / Width;
        double unitsFromLeftEdge = position - Min;
        float pxFromEdge = (float)(unitsFromLeftEdge * pxPerUnit);
        return dataArea.Left + pxFromEdge;
    }

    public double GetCoordinate(float pixel, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Width / Width;
        float pxFromLeftEdge = pixel - dataArea.Left;
        double unitsFromEdge = pxFromLeftEdge / pxPerUnit;
        return Min + unitsFromEdge;
    }

    private PixelRect GetPanelRectangleBottom(PixelRect dataRect, float size, float offset)
    {
        return new PixelRect(
            left: dataRect.Left,
            right: dataRect.Right,
            bottom: dataRect.Bottom + offset + size,
            top: dataRect.Bottom + offset);
    }

    private PixelRect GetPanelRectangleTop(PixelRect dataRect, float size, float offset)
    {
        return new PixelRect(
            left: dataRect.Left,
            right: dataRect.Right,
            bottom: dataRect.Top - offset,
            top: dataRect.Top - offset - size);
    }

    public PixelRect GetPanelRect(PixelRect dataRect, float size, float offset)
    {
        return Edge == Edge.Bottom
            ? GetPanelRectangleBottom(dataRect, size, offset)
            : GetPanelRectangleTop(dataRect, size, offset);
    }

    public void Render(SKSurface surface, PixelRect dataRect, float size, float offset)
    {
        if (!IsVisible)
            return;

        PixelRect panelRect = GetPanelRect(dataRect, size, offset);

        float textDistanceFromEdge = 10;
        Pixel labelPoint = new(panelRect.HorizontalCenter, panelRect.Bottom - textDistanceFromEdge);

        if (ShowDebugInformation)
        {
            Drawing.DrawDebugRectangle(surface.Canvas, panelRect, labelPoint, Label.Font.Color);
        }

        Label.Alignment = Alignment.LowerCenter;
        Label.Rotation = 0;
        Label.Draw(surface.Canvas, labelPoint);


        IEnumerable<Tick> ticks = TickGenerator.GetVisibleTicks(Range);

        AxisRendering.DrawTicks(surface, TickFont, panelRect, ticks, this, MajorTickStyle, MinorTickStyle);
        AxisRendering.DrawFrame(surface, panelRect, Edge, FrameLineStyle);
    }

    public double GetPixelDistance(double distance, PixelRect dataArea)
    {
        return distance * dataArea.Width / Width;
    }

    public double GetCoordinateDistance(double distance, PixelRect dataArea)
    {
        return distance / (dataArea.Width / Width);
    }
}
