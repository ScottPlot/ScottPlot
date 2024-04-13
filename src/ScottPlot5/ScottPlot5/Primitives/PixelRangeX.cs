namespace ScottPlot;

/// <summary>
/// Represents a range of pixels between two pixels on the horizontal axis.
/// The value of <see cref="Left"/> will be SMALLER than the value of <see cref="Right"/>.
/// </summary>
public struct PixelRangeX(float x1, float x2)
{
    public float Left { get; private set; } = Math.Min(x1, x2);
    public float Right { get; private set; } = Math.Max(x1, x2);
    public readonly float Span => Right - Left;

    public void Expand(float value)
    {
        if (value < Left)
            Left = value;

        if (value > Right)
            Right = value;
    }

    public override readonly string ToString()
    {
        return $"Left={Left}, Right={Right}, Span={Span}";
    }

    public readonly bool Contains(float position)
    {
        return position >= Left && position <= Right;
    }
}
