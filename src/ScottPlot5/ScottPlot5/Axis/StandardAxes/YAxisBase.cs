namespace ScottPlot.Axis.StandardAxes;

// TODO: move shared code into a common AxisBase class

public abstract class YAxisBase : IAxis
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

    public double Height => Range.Span;

    public virtual CoordinateRange Range { get; private set; } = CoordinateRange.NotSet;

    public ITickGenerator TickGenerator { get; set; } = null!;

    public Label Label { get; private set; } = new()
    {
        Text = string.Empty,
        FontName = FontService.SansFontName,
        FontSize = 16,
        Bold = true,
        Rotation = -90,
    };
    public Style.StyledSKFont TickFont { get; set; } = new();

    public abstract Edge Edge { get; }

    public bool ShowDebugInformation { get; set; } = false;

    public float MajorTickLength { get; set; } = 4;
    public float MajorTickLineWidth { get; set; } = 1;
    public Color MajorTickColor { get; set; } = Colors.Black;
    public TickStyle MajorTickStyle => new()
    {
        Length = MajorTickLength,
        LineWidth = MajorTickLineWidth,
        Color = MajorTickColor
    };

    public float MinorTickLength { get; set; } = 2;
    public float MinorTickLineWidth { get; set; } = 1;
    public Color MinorTickColor { get; set; } = Colors.Black;
    public TickStyle MinorTickStyle => new()
    {
        Length = MinorTickLength,
        LineWidth = MinorTickLineWidth,
        Color = MinorTickColor
    };

    public float FrameLineWidth { get; set; } = 1;
    public Color FrameColor { get; set; } = Colors.Black;

    public float GetPixel(double position, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Height / Height;
        double unitsFromMinValue = position - Min;
        float pxFromEdge = (float)(unitsFromMinValue * pxPerUnit);
        return dataArea.Bottom - pxFromEdge;
    }

    public double GetCoordinate(float pixel, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Height / Height;
        float pxFromMinValue = pixel - dataArea.Bottom;
        double unitsFromMinValue = pxFromMinValue / pxPerUnit;
        return Min - unitsFromMinValue;
    }

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
        using SKPaint paint = new(TickFont.GetFont());
        float largestTickWidth = 0;

        foreach (Tick tick in TickGenerator.GetVisibleTicks(Range))
        {
            PixelSize tickLabelSize = Drawing.MeasureString(tick.Label, paint);
            largestTickWidth = Math.Max(largestTickWidth, tickLabelSize.Width + 10);
        }

        return largestTickWidth;
    }

    private PixelRect GetPanelRectangleLeft(PixelRect dataRect, float size, float offset)
    {
        return new PixelRect(
            left: dataRect.Left - offset - size,
            right: dataRect.Left - offset,
            bottom: dataRect.Bottom,
            top: dataRect.Top);
    }

    private PixelRect GetPanelRectangleRight(PixelRect dataRect, float size, float offset)
    {
        return new PixelRect(
            left: dataRect.Right + offset,
            right: dataRect.Right + offset + size,
            bottom: dataRect.Bottom,
            top: dataRect.Top);
    }

    public PixelRect GetPanelRect(PixelRect dataRect, float size, float offset)
    {
        return Edge == Edge.Left
            ? GetPanelRectangleLeft(dataRect, size, offset)
            : GetPanelRectangleRight(dataRect, size, offset);
    }

    public void Render(SKSurface surface, PixelRect dataRect, float size, float offset)
    {
        if (!IsVisible)
            return;

        PixelRect panelRect = GetPanelRect(dataRect, size, offset);

        float textDistanceFromEdge = 10;
        Pixel labelPoint = new(panelRect.Left + textDistanceFromEdge, dataRect.VerticalCenter);

        if (ShowDebugInformation)
        {
            Drawing.DrawDebugRectangle(surface.Canvas, panelRect, labelPoint, Label.Color);
        }

        Label.Alignment = Edge == Edge.Left ? Alignment.UpperCenter : Alignment.LowerCenter;
        Label.Rotation = Edge == Edge.Left ? -90 : 90;
        Label.Draw(surface.Canvas, labelPoint);

        using SKFont tickFont = TickFont.GetFont();
        var ticks = TickGenerator.GetVisibleTicks(Range);
        AxisRendering.DrawTicks(surface, tickFont, panelRect, ticks, this, MajorTickStyle, MinorTickStyle);
        AxisRendering.DrawFrame(surface, panelRect, Edge, FrameLineWidth, FrameColor);
    }

    public double GetPixelDistance(double distance, PixelRect dataArea)
    {
        return distance * dataArea.Height / Height;
    }

    public double GetCoordinateDistance(double distance, PixelRect dataArea)
    {
        return distance / (dataArea.Height / Height);
    }
}
