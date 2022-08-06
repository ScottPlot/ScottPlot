namespace ScottPlot;

public struct PixelSize
{
    public float Width;
    public float Height;
    public float Area => Width * Height;

    public PixelSize(float width, float height)
    {
        Width = width;
        Height = height;
    }
}
