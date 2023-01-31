namespace ScottPlot.TickGenerators.TimeUnits;

internal static class StandardDivisors
{
    public static readonly IReadOnlyList<int> Decimal = new int[] { 1, 5, 10 };
    public static readonly IReadOnlyList<int> Sexagesimal = new int[] { 1, 5, 10, 15, 20, 30, 60 };
    public static readonly IReadOnlyList<int> Dozenal = new int[] { 1, 2, 3, 4, 6, 12 };
    public static readonly IReadOnlyList<int> Hexadecimal = new int[] { 1, 2, 3, 4, 6, 8, 16 };
    public static readonly IReadOnlyList<int> Days = new int[] { 1, 3, 7, 14, 28 };
    public static readonly IReadOnlyList<int> Months = new int[] { 1, 3, 6 };
    public static readonly IReadOnlyList<int> Years = new int[] { 1, 2, 3, 4, 5, 10 };
}
