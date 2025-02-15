namespace ScottPlot;

/// <summary>
/// Represents a range of indexes in an array (inclusive)
/// </summary>
public readonly record struct IndexRange(int Min, int Max)
{
    /// <summary>
    /// The IndexRange that represents an empty collection
    /// </summary>
    public static readonly IndexRange None = new IndexRange(-1, -1, 0);
    public int Length { get; } = Max - Min + 1;

    public bool IsValid => Min >= 0 && Max >= Min;

    private IndexRange(int min, int max, int length) : this(min, max) { Length = length; }
}
