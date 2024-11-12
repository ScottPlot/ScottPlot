namespace ScottPlot.DataSources;

public class FastSignalSourceDouble : SignalSourceBase, ISignalSource, IDataSource
{
    private readonly IReadOnlyList<double> Ys;
    private readonly MinMaxCache MinMaxCache;

    public override int Length => Ys.Count;

    bool IDataSource.PreferCoordinates => false;

    public FastSignalSourceDouble(IReadOnlyList<double> ys, double period, int cachePeriod = 1000)
    {
        Ys = ys;
        Period = period;
        MinMaxCache = new MinMaxCache(Ys, cachePeriod);
    }

    public IReadOnlyList<double> GetYs()
    {
        return Ys;
    }

    public IEnumerable<double> GetYs(int i1, int i2)
    {
        i1 = Math.Max(i1, MinRenderIndex);
        i2 = Math.Min(i2, MaxRenderIndex);
        for (int i = i1; i <= i2; i++)
        {
            yield return Ys[i];
        }
    }

    public double GetY(int index)
    {
        return Ys[index];
    }

    public override SignalRangeY GetLimitsY(int firstIndex, int lastIndex)
    {
        return MinMaxCache.GetMinMax(firstIndex, lastIndex + 1);
    }

    public PixelColumn GetPixelColumn(IAxes axes, int xPixelIndex)
    {
        float xPixel = axes.DataRect.Left + xPixelIndex;
        double xRangeMin = axes.GetCoordinateX(xPixel);
        float xUnitsPerPixel = (float)(axes.XAxis.Width / axes.DataRect.Width);
        double xRangeMax = xRangeMin + Math.Abs(xUnitsPerPixel);

        // add slight overlap to prevent floating point errors from missing points
        // https://github.com/ScottPlot/ScottPlot/issues/3665
        xRangeMax += xUnitsPerPixel * .01;

        if (RangeContainsSignal(xRangeMin, xRangeMax) == false)
            return PixelColumn.WithoutData(xPixel);

        // determine column limits horizontally
        int i1 = GetIndex(xRangeMin, true);
        int i2 = GetIndex(xRangeMax, true);

        // first and last Y vales for this column
        float yEnter = axes.GetPixelY(Ys[i1] + YOffset);
        float yExit = axes.GetPixelY(Ys[i2] + YOffset);

        // column min and max
        SignalRangeY rangeY = GetLimitsY(i1, i2);
        float yBottom = axes.GetPixelY(rangeY.Min + YOffset);
        float yTop = axes.GetPixelY(rangeY.Max + YOffset);

        return new PixelColumn(xPixel, yEnter, yExit, yBottom, yTop);
    }

    int IDataSource.GetXClosestIndex(Coordinates mouseLocation) => GetIndex(mouseLocation.X, true);

    Coordinates IDataSource.GetCoordinate(int index)
    {
        return new Coordinates(index * Period, Ys[index]);
    }

    Coordinates IDataSource.GetCoordinateScaled(int index)
    {
        return new Coordinates(index * Period + XOffset, DataSourceUtilities.ScaleXY(Ys[index], YScale, YOffset));
    }

    double IDataSource.GetX(int index)
    {
        return index * Period;
    }

    double IDataSource.GetXScaled(int index)
    {
        return index * Period + XOffset;
    }

    double IDataSource.GetY(int index)
    {
        return Ys[index];
    }

    double IDataSource.GetYScaled(int index)
    {
        return DataSourceUtilities.ScaleXY(Ys[index], YScale, YOffset);
    }

    bool IDataSource.IsSorted() => true;
}
