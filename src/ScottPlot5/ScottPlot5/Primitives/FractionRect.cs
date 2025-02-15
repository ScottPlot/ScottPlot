namespace ScottPlot;

/// <summary>
/// Describes a rectangular region of a larger rectangle using fractional units
/// </summary>
public readonly struct FractionRect
{
    public double Left { get; }
    public double Top { get; }
    public double Width { get; }
    public double Height { get; }
    public double Right => Left + Width;
    public double Bottom => Top + Height;

    public FractionRect(double left, double top, double width, double height)
    {
        if (left < 0 || left >= 1)
            throw new ArgumentOutOfRangeException(nameof(left), "must be >=0 and <1");

        if (top < 0 || top >= 1)
            throw new ArgumentOutOfRangeException(nameof(left), "must be >=0 and <1");

        Left = left;
        Top = top;
        Width = width;
        Height = height;

        if (Bottom > 1)
            throw new ArgumentException($"{nameof(top)} + {nameof(height)} must not exceed 1");

        if (Right > 1)
            throw new ArgumentException($"{nameof(left)} + {nameof(width)} must not exceed 1");
    }

    public PixelRect GetPixelRect(PixelRect fullRect)
    {
        Pixel topLeft = new(Left * fullRect.Width, Top * fullRect.Height);
        PixelSize size = new(Width * fullRect.Width, Height * fullRect.Height);
        return new PixelRect(topLeft, size);
    }

    public PixelRect GetPixelRect(float width, float height)
    {
        Pixel topLeft = new(Left * width, Top * height);
        PixelSize size = new(Width * width, Height * height);
        return new PixelRect(topLeft, size);
    }

    public static FractionRect Full => new(0, 0, 1, 1);

    public static FractionRect Column(int columnIndex, int columnCount)
    {
        double width = 1.0 / columnCount;
        double x = width * columnIndex;
        return new FractionRect(x, 0, width, 1);
    }

    public static FractionRect Row(int rowIndex, int rowCount)
    {
        double height = 1.0 / rowCount;
        double y = height * rowIndex;
        return new FractionRect(0, y, 1, height);
    }

    public static FractionRect GridCell(int columnIndex, int rowIndex, int columnCount, int rowCount)
    {
        double width = 1.0 / columnCount;
        double x = width * columnIndex;
        double height = 1.0 / rowCount;
        double y = height * rowIndex;
        return new FractionRect(x, y, width, height);
    }
}
