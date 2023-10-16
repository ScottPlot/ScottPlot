/// <summary>
/// Represents a vertical range of pixels at a specific horizontal pixel location
/// </summary>
public readonly struct PixelColumn
{
    public readonly float YBottom;
    public readonly float YTop;
    public readonly float X;

    public PixelColumn(float x, float yBottom, float yTop)
    {
        X = x;
        YBottom = yBottom;
        YTop = yTop;
    }
}