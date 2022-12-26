namespace ScottPlot.Extensions;

public static class NumericExtensions
{
    public static bool IsInfiniteOrNaN(this double x)
    {
        return !IsFinite(x);
    }

    public static bool IsFinite(this double x)
    {
        if (double.IsInfinity(x))
            return false;

        if (double.IsNaN(x))
            return false;

        return true;
    }

    public static float ToDegrees(this double radians)
    {
        return (float)(radians * 180.0 / Math.PI);
    }

    public static float ToRadians(this double degrees)
    {
        return (float)(degrees / 180.0 * Math.PI);
    }

    public static float ToRadians(this float degrees)
    {
        return (float)(degrees / 180.0 * Math.PI);
    }

    public static float ToRadians(this int degrees)
    {
        return (float)(degrees / 180.0 * Math.PI);
    }
}
