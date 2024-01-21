namespace ScottPlot;

/// <summary>
/// Represents a range of indexes in an array (inclusive)
/// </summary>
public readonly record struct IndexRange(int Min, int Max)
{
    public int Length => Max - Min + 1;
}
