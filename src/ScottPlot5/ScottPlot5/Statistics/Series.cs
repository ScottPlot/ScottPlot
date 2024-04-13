namespace ScottPlot.Statistics;

public static class Series
{
    /// <summary>
    /// Return a moving window average of the given data.
    /// If original length is true, data will be padded with NaN.
    /// </summary>
    public static double[] MovingAverage(double[] values, int window, bool preserveLength = false)
    {
        if (window < 2)
            throw new ArgumentException("period must be 2 or greater");

        if (window > values.Length)
            throw new ArgumentException("period cannot be longer than number of values");

        double[] sma = new double[values.Length];

        for (int i = 0; i < values.Length; i++)
        {
            if (i < window)
            {
                sma[i] = double.NaN;
            }
            else
            {
                var periodValues = new double[window];
                Array.Copy(values, i - window + 1, periodValues, 0, window);
                sma[i] = Descriptive.Mean(periodValues);
            }
        }

        if (preserveLength == false)
        {
            sma = sma.Skip(window).ToArray();
        }

        return sma;
    }

    /// <summary>
    /// Return a moving window standard deviation of the given data.
    /// If original length is true, data will be padded with NaN.
    /// </summary>
    public static double[] SimpleMovingStandardDeviation(double[] values, int window, bool preserveLength = false)
    {
        if (window < 2)
            throw new ArgumentException("period must be 2 or greater");

        if (window > values.Length)
            throw new ArgumentException("period cannot be longer than number of values");

        double[] stDev = new double[values.Length];

        for (int i = 0; i < values.Length; i++)
        {
            if (i < window)
            {
                stDev[i] = double.NaN;
                continue;
            }
            else
            {
                var periodValues = new double[window];
                Array.Copy(values, i - window + 1, periodValues, 0, window);
                stDev[i] = Descriptive.StandardDeviation(periodValues);
            }
        }

        if (preserveLength == false)
        {
            stDev = stDev.Skip(window).ToArray();
        }

        return stDev;
    }
}
