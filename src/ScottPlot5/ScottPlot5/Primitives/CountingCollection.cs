namespace ScottPlot;

internal class CountingCollection<T> where T : notnull
{
    public readonly Dictionary<T, int> Counts = [];

    public bool Any() => Counts.Any();
    public int Count => Counts.Count;

    public IEnumerable<T> SortedKeys => Counts.OrderBy(x => x.Value).Select(x => x.Key);

    public void Add(T item)
    {
        if (Counts.ContainsKey(item))
        {
            Counts[item]++;
        }
        else
        {
            Counts[item] = 1;
        }
    }

    public void AddRange(IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            Add(item);
        }
    }

    public override string ToString()
    {
        return $"CountingCollection<{typeof(T)}> with {Count} items";
    }

    public string GetLongString()
    {
        return string.Join(", ", SortedKeys.Select(x => $"{x} ({Counts[x]})"));
    }
}
