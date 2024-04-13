namespace ScottPlot.Extensions;

internal static class EnumerableExtensions
{
    public static IEnumerable<T> One<T>(T item) { yield return item; }
}
