namespace ScottPlot;

/// <summary>
/// <see cref="IComparer{T}"/> for various types provided by ScottPlot
/// </summary>
public sealed class BinarySearchComparer : IComparer<Coordinates>, IComparer<double>, IComparer<RootedCoordinateVector>, IComparer<RootedPixelVector>
{
    /// <summary>
    /// Thread-Safe singleton
    /// </summary>
    public static readonly BinarySearchComparer Instance = new();
    private BinarySearchComparer() { }

    public static IComparer<T> GetComparer<T>() => GenericComparer<T>.Default;
    public int Compare(Coordinates a, Coordinates b) => a.X.CompareTo(b.X);
    public int Compare(double a, double b) => a.CompareTo(b);
    public int Compare(RootedPixelVector x, RootedPixelVector y) => x.Point.X.CompareTo(y.Point.X);
    public int Compare(RootedCoordinateVector x, RootedCoordinateVector y) => x.Point.X.CompareTo(y.Point.X);
}
