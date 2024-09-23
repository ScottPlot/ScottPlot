namespace ScottPlot.DataSources;

public class SignalSourceGenericArray<T> : SignalSourceBase, ISignalSource, IDataSource
{
    private readonly T[] Ys;
    public override int Length => Ys.Length;

    bool IDataSource.PreferCoordinates => false;

    public SignalSourceGenericArray(T[] ys, double period)
    {
        Ys = ys;
        Period = period;
    }

    public IReadOnlyList<double> GetYs()
    {
        return NumericConversion.GenericToDoubleArray(Ys);
    }

    public IEnumerable<double> GetYs(int i1, int i2)
    {
        for (int i = i1; i <= i2; i++)
        {
            yield return NumericConversion.GenericToDouble(ref Ys[i]);
        }
    }

    public double GetY(int index)
    {
        return NumericConversion.GenericToDouble(ref Ys[index]);
    }

    public override SignalRangeY GetLimitsY(int firstIndex, int lastIndex)
    {
        double min = double.PositiveInfinity;
        double max = double.NegativeInfinity;

        for (int i = firstIndex; i <= lastIndex; i++)
        {
            double value = NumericConversion.GenericToDouble(ref Ys[i]);
            min = Math.Min(min, value);
            max = Math.Max(max, value);
        }

        return new SignalRangeY(min, max);
    }

    public PixelColumn GetPixelColumn(IAxes axes, int xPixelIndex)
    {
        float xPixel = axes.DataRect.Left + xPixelIndex;
        double xRangeMin = axes.GetCoordinateX(xPixel);
        float xUnitsPerPixel = (float)(axes.XAxis.Width / axes.DataRect.Width);
        double xRangeMax = xRangeMin + xUnitsPerPixel;

        // add slight overlap to prevent floating point errors from missing points
        // https://github.com/ScottPlot/ScottPlot/issues/3665
        xRangeMax += xUnitsPerPixel * .01;

        if (RangeContainsSignal(xRangeMin, xRangeMax) == false)
            return PixelColumn.WithoutData(xPixel);

        // determine column limits horizontally
        int i1 = GetIndex(xRangeMin, true);
        int i2 = GetIndex(xRangeMax, true);
        float yEnter = axes.GetPixelY(NumericConversion.GenericToDouble(Ys, i1) * YScale + YOffset);
        float yExit = axes.GetPixelY(NumericConversion.GenericToDouble(Ys, i2) * YScale + YOffset);

        // determine column span vertically
        double yMin = double.PositiveInfinity;
        double yMax = double.NegativeInfinity;
        for (int i = i1; i <= i2; i++)
        {
            double value = NumericConversion.GenericToDouble(Ys, i);
            yMin = Math.Min(yMin, value);
            yMax = Math.Max(yMax, value);
        }
        yMin = yMin * YScale + YOffset;
        yMax = yMax * YScale + YOffset;

        float yBottom = axes.GetPixelY(yMin);
        float yTop = axes.GetPixelY(yMax);

        return new PixelColumn(xPixel, yEnter, yExit, yBottom, yTop);
    }

    int IDataSource.GetXClosestIndex(Coordinates mouseLocation)
    {
        return GetIndex(mouseLocation.X, true);
    }

    Coordinates IDataSource.GetCoordinate(int index)
    {
        return new Coordinates(index * Period, NumericConversion.GenericToDouble(Ys, index));
    }

    Coordinates IDataSource.GetCoordinateScaled(int index)
    {
        return new Coordinates(index * Period + XOffset, DataSourceUtilities.ScaleXY(Ys, index, YScale, YOffset));
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
        return NumericConversion.GenericToDouble(Ys, index);
    }

    double IDataSource.GetYScaled(int index)
    {
        return DataSourceUtilities.ScaleXY(Ys, index, YScale, YOffset);
    }
    bool IDataSource.IsSorted() => true;

}
