namespace ScottPlot;

internal static class NumericExtensions
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
}
