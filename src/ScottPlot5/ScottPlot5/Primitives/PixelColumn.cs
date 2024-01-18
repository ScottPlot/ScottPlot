/// <summary>
/// This data structure describes a single vertical column of pixels
/// that represents the Y span of an X range of data points.
/// </summary>
public readonly struct PixelColumn
{
    public readonly float X;
    public readonly float Enter;
    public readonly float Exit;
    public readonly float Bottom;
    public readonly float Top;

    public bool HasData => !float.IsNaN(Enter);

    public PixelColumn(float x, float enter, float exit, float bottom, float top)
    {
        X = x;
        Enter = enter;
        Exit = exit;
        Bottom = bottom;
        Top = top;
    }

    public static PixelColumn WithoutData(float x)
    {
        return new PixelColumn(x, float.NaN, float.NaN, float.NaN, float.NaN);
    }

    public override string ToString()
    {
        return $"x={X} y=[{Bottom}, {Top}], edges=[{Enter}, {Exit}]";
    }
}
