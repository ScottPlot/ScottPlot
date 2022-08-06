namespace ScottPlot;

/// <summary>
/// Represents a distance in pixel units
/// </summary>
public readonly struct PixelLength
{
    public readonly float Length;

    public PixelLength(float length)
    {
        Length = length;
    }

    public static implicit operator PixelLength(float length)
    {
        return new PixelLength(length);
    }

    public override string ToString()
    {
        return $"{Length} pixels";
    }
}
