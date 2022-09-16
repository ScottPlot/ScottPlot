namespace ScottPlot;

public struct PixelSize
{
    public readonly float Width;
    public readonly float Height;
    public float Area => Width * Height;

    public static readonly PixelSize Zero = new PixelSize(0, 0);

    public PixelSize(float width, float height)
    {
        Width = width;
        Height = height;
    }
}
