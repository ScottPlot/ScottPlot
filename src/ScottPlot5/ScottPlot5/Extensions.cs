namespace ScottPlot;

internal static class Extensions
{
    internal static bool IsInfiniteOrNaN(this double x)
    {
        return !IsFinite(x);
    }

    internal static bool IsFinite(this double x)
    {
        if (double.IsInfinity(x))
            return false;

        if (double.IsNaN(x))
            return false;

        return true;
    }
}
