namespace ScottPlot;

public enum LinePattern
{
    Solid,
    Dash,
    ShortDash,
    Dot,
}

public static class LinePatternExtensions
{
    public static SKPathEffect? GetPathEffect(this LinePattern pattern)
    {
        return pattern switch
        {
            LinePattern.Solid => null,
            LinePattern.Dash => SKPathEffect.CreateDash(new float[] { 10, 10 }, 0),
            LinePattern.ShortDash => SKPathEffect.CreateDash(new float[] { 6, 6 }, 0),
            LinePattern.Dot => SKPathEffect.CreateDash(new float[] { 3, 5 }, 0),
            _ => throw new NotImplementedException($"Line pattern '{pattern}' has no matching path effect"),
        };
    }
}
