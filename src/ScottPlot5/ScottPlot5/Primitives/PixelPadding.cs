namespace ScottPlot;

/// <summary>
/// Represents the size (in pixels) of padding on all edges of a rectangle
/// </summary>
public struct PixelPadding
{
    public float Left;
    public float Right;
    public float Bottom;
    public float Top;

    public float Horizontal => Left + Right;
    public float Vertical => Top + Bottom;

    public PixelPadding(float padding)
    {
        Left = padding;
        Right = padding;
        Bottom = padding;
        Top = padding;
    }

    public PixelPadding(float x, float y)
    {
        Left = x;
        Right = x;
        Bottom = y;
        Top = y;
    }

    public PixelPadding(float left, float right, float bottom, float top)
    {
        Left = left;
        Right = right;
        Bottom = bottom;
        Top = top;
    }

    public PixelPadding(PixelSize figureSize, PixelRect dataArea)
    {
        Left = dataArea.Left;
        Right = figureSize.Width - dataArea.Right;
        Top = dataArea.Top;
        Bottom = figureSize.Height - dataArea.Bottom;
    }

    public void Expand(float amount)
    {
        Left += amount;
        Right += amount;
        Bottom += amount;
        Top += amount;
    }

    public void Contract(float amount) => Expand(-amount);

    public static PixelPadding Zero => new();
}
