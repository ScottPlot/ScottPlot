namespace ScottPlot.DataSources;

public class SignalConstSource<T> : SignalSourceGenericArray<T>
    where T : struct, IComparable
{
    public readonly SegmentedTree<T> SegmentedTree = new();

    public SignalConstSource(T[] ys, double period) : base(ys, period)
    {
        SegmentedTree.SourceArray = ys;
    }

    public override SignalRangeY GetLimitsY(int firstIndex, int lastIndex)
    {
        SegmentedTree.MinMaxRangeQuery(firstIndex, lastIndex, out double min, out double max);
        return new(min, max);
    }
}
