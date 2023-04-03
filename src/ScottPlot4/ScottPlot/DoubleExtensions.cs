namespace ScottPlot
{
    internal static class DoubleExtensions
    {
        public static bool IsFinite(this double x) => !double.IsNaN(x) && !double.IsInfinity(x);
    }
}
