namespace ScottPlot;

public enum HorizontalAlignment
{
    Left,
    Center,
    Right,
}

public static class HorizontalAlignmentExtensions
{
    public static SKTextAlign ToSKTextAlign(this HorizontalAlignment ha)
    {
        return ha switch
        {
            HorizontalAlignment.Left => SKTextAlign.Left,
            HorizontalAlignment.Center => SKTextAlign.Center,
            HorizontalAlignment.Right => SKTextAlign.Right,
            _ => throw new NotSupportedException(),
        };
    }
}
