using SkiaSharp;

namespace ScottPlot.Axis.StandardAxes;

public abstract class YAxisBase
{
    public double Bottom
    {
        get => Range.Min;
        set => Range.Min = value;
    }

    public double Top
    {
        get => Range.Max;
        set => Range.Max = value;
    }

    public double Height => Range.Span;

    public CoordinateRange Range { get; private set; } = CoordinateRange.NotSet;

    public float Offset { get; set; } = 0;

    public float PixelSize { get; private set; } = 50;

    public float PixelWidth => PixelSize;

    public ITickGenerator TickGenerator { get; set; } = null!;

    public Label Label { get; private set; } = new()
    {
        Text = "Vertical Axis",
        Bold = true,
        FontSize = 16,
        Rotation = -90
    };

    public float GetPixel(double position, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Height / Height;
        double unitsFromMinValue = position - Bottom;
        float pxFromEdge = (float)(unitsFromMinValue * pxPerUnit);
        return dataArea.Bottom - pxFromEdge;
    }

    public double GetCoordinate(float pixel, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Height / Height;
        float pxFromMinValue = pixel - dataArea.Bottom;
        double unitsFromMinValue = pxFromMinValue / pxPerUnit;
        return Bottom - unitsFromMinValue;
    }

    public void Measure()
    {
        float labelWidth = Label.FontSize;
        float largestTickWidth = 0;

        using SKPaint paint = Label.MakePaint();

        foreach (Tick tick in TickGenerator.GetVisibleTicks(Range))
        {
            PixelSize tickLabelSize = Drawing.MeasureString(tick.Label, paint);
            largestTickWidth = Math.Max(largestTickWidth, tickLabelSize.Width + 10);
        }

        PixelSize = labelWidth + largestTickWidth;
    }
}
