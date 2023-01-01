namespace ScottPlot;

/// <summary>
/// Represents a range of pixels between two pixels on the vertical axis used in Signal plots.
/// The value of <see cref="Top"/> will be SMALLER than the value of <see cref="Bottom"/>.
/// </summary>
public struct PixelRangeY
{
    public float Top { get; private set; }
    public float Bottom { get; private set; }
    public float Span => Bottom - Top;

    public PixelRangeY(float y1, float y2)
    {
        Top = Math.Min(y1, y2);
        Bottom = Math.Max(y1, y2);
    }

    public void Expand(float value)
    {
        if (value < Top)
            Top = value;
        if (value > Bottom)
            Bottom = value;
    }

    public override string ToString()
    {
        return $"Top={Top}, Bottom={Bottom}, Span={Span}";
    }

    public bool Contains(float position)
    {
        return position >= Top && position <= Bottom;
    }
}
