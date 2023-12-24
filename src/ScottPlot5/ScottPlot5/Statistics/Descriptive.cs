namespace ScottPlot.Statistics;

public static class Descriptive
{
    /* NOTES: 
     *   The core functions in this class should support double[].
     *   Overloads may support <T> but they just convert to double[] then call the other function.
     *   IReadOnlyList is favored to IEnumerable to avoid Count().
     *   Code here may benefit from benchmarking to confirm which strategies are most performant.
     *   Additional discussion: https://github.com/ScottPlot/ScottPlot/pull/3071.
     */

    /// <summary>
    /// Return the sample sum.
    /// </summary>
    public static double Sum(double[] values)
    {
        if (!values.Any())
            throw new ArgumentException($"{nameof(values)} cannot be empty");

        double sum = 0;

        for (int i = 0; i < values.Length; i++)
        {
            sum += values[i];
        }

        return sum;
    }

    /// <summary>
    /// Return the sample sum.
    /// </summary>
    public static double Sum<T>(IReadOnlyList<T> values)
    {
        double[] values2 = NumericConversion.GenericToDoubleArray(values);
        return Sum(values2);
    }

    /// <summary>
    /// Return the sample mean.
    /// </summary>
    public static double Mean(double[] values)
    {
        if (!values.Any())
            throw new ArgumentException($"{nameof(values)} cannot be empty");

        return Sum(values) / values.Length;
    }

    /// <summary>
    /// Return the sample mean.
    /// </summary>
    public static double Mean<T>(IEnumerable<T> values)
    {
        double[] values2 = NumericConversion.GenericToDoubleArray(values);
        return Mean(values2);
    }

    /// <summary>
    /// Return the sample variance (second moment about the mean) of data.
    /// Input must contain at least two values. 
    /// Use this function when your data is a sample from a population.
    /// To calculate the variance from the entire population use <see cref="VarianceP(double[])"/>.
    /// </summary>
    public static double Variance(double[] values)
    {
        if (values.Length < 2)
            throw new ArgumentException($"{nameof(values)} must have at least 2 values");

        // TODO: benchmark and see if this can be optimized by not using LINQ
        double mean = Mean(values);
        double variance = values.Select(x => x - mean).Select(x => x * x).Sum();
        return variance / (values.Length - 1);
    }

    /// <summary>
    /// Return the sample variance (second moment about the mean) of data.
    /// Input must contain at least two values. 
    /// Use this function to calculate the variance from the entire population. 
    /// To estimate the variance from a sample, use <see cref="VarianceP{T}(IReadOnlyList{T})()"/>.
    /// </summary>
    public static double Variance<T>(IReadOnlyList<T> values)
    {
        double[] values2 = NumericConversion.GenericToDoubleArray(values);
        return Variance(values2);
    }

    /// <summary>
    /// Return the sample variance (second moment about the mean) of data.
    /// Input must contain at least two values. 
    /// Use this function to calculate the variance from the entire population. 
    /// To estimate the variance from a sample, use <see cref="Variance(double[])"/>.
    /// </summary>
    public static double VarianceP(double[] values)
    {
        if (values.Length < 1)
            throw new ArgumentException($"{nameof(values)} must have at least 1 value");

        // TODO: benchmark and see if this can be optimized by not using LINQ
        double mean = Mean(values);
        double variance = values.Select(x => x - mean).Select(x => x * x).Sum();
        return variance / (values.Length);
    }

    /// <summary>
    /// Return the sample variance (second moment about the mean) of data.
    /// Input must contain at least two values. 
    /// Use this function to calculate the variance from the entire population. 
    /// To estimate the variance from a sample, use <see cref="Variance{T}(IReadOnlyList{T})"/>.
    /// </summary>
    public static double VarianceP<T>(IReadOnlyList<T> values)
    {
        double[] values2 = NumericConversion.GenericToDoubleArray(values);
        return VarianceP(values2);
    }

    /// <summary>
    /// Return the sample standard deviation (the square root of the sample variance).
    /// </summary>
    public static double StandardDeviation(double[] values)
    {
        return Math.Sqrt(Variance(values));
    }

    /// <summary>
    /// Return the sample standard deviation (the square root of the sample variance).
    /// </summary>
    public static double StandardDeviation<T>(IEnumerable<T> values)
    {
        if (!values.Any())
            throw new ArgumentException($"{nameof(values)} cannot be empty");

        double[] values2 = NumericConversion.GenericToDoubleArray(values);
        return StandardDeviation(values2);
    }

    /// <summary>
    /// Return the population standard deviation (the square root of the population variance).
    /// See VarianceP() for more information.
    /// </summary>
    public static double StandardDeviationP(double[] values)
    {
        return Math.Sqrt(VarianceP(values));
    }

    /// <summary>
    /// Return the population standard deviation (the square root of the population variance).
    /// See VarianceP() for more information.
    /// </summary>
    public static double StandardDeviationP<T>(IEnumerable<T> values)
    {
        if (!values.Any())
            throw new ArgumentException($"{nameof(values)} cannot be empty");

        double[] values2 = NumericConversion.GenericToDoubleArray(values);
        return StandardDeviationP(values2);
    }

    /// <summary>
    /// Standard error of the mean.
    /// </summary>
    public static double StandardError<T>(IReadOnlyList<T> values)
    {
        return StandardDeviation(values) / Math.Sqrt(values.Count);
    }


    public static IReadOnlyList<double> RemoveNaN<T>(IReadOnlyList<T> values)
    {
        double[] values2 = NumericConversion.GenericToDoubleArray(values);
        List<double> values3 = new();

        foreach (double value in values2)
        {
            if (!double.IsNaN(value))
                values3.Add(value);
        }

        return values3;
    }

    public static double[] RemoveNaN(double[] values)
    {
        List<double> values2 = new();

        foreach (double value in values)
        {
            if (!double.IsNaN(value))
                values2.Add(value);
        }

        return values2.ToArray();
    }

    /// <summary>
    /// Return the sample mean. NaN values are ignored.
    /// </summary>
    public static double NaNMean<T> (IReadOnlyList<T> values)
    {
        return Mean(RemoveNaN(values));
    }

    /// <summary>
    /// Return the sample variance (second moment about the mean) of data.
    /// Input must contain at least two values. NaN values are ignored.
    /// Use this function when your data is a sample from a population.
    /// To calculate the variance from the entire population use <see cref="VarianceP(double[])"/>.
    /// </summary>
    public static double NaNVariance<T>(IReadOnlyList<T> values)
    {
        return Variance(RemoveNaN(values));
    }

    /// <summary>
    /// Return the sample variance (second moment about the mean) of data.
    /// Input must contain at least two values. NaN values are ignored.
    /// Use this function when your data is a sample from a population.
    /// To calculate the variance from the entire population use <see cref="VarianceP(double[])"/>.
    /// </summary>
    public static double NaNVarianceP<T>(IReadOnlyList<T> values)
    {
        return VarianceP(RemoveNaN(values));
    }

    /// <summary>
    /// Return the sample standard deviation (the square root of the sample variance).
    /// NaN values are ignored.
    /// </summary>
    public static double NaNStandardDeviation<T>(IReadOnlyList<T> values)
    {
        return StandardDeviation(RemoveNaN(values));
    }

    /// <summary>
    /// Return the population standard deviation (the square root of the sample variance).
    /// NaN values are ignored.
    /// </summary>
    public static double NaNStandardDeviationP<T>(IReadOnlyList<T> values)
    {
        return StandardDeviationP(RemoveNaN(values));
    }

    /// <summary>
    /// Standard error of the mean.
    /// NaN values are ignored.
    /// </summary>
    public static double NaNStandardError<T>(IReadOnlyList<T> values)
    {
        return StandardError(RemoveNaN(values));
    }

    /// <summary>
    /// Transpose a multidimensional (not jagged) array
    /// </summary>
    public static double[,] ArrayTranspose(double[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        double[,] result = new double[cols, rows];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
                result[j, i] = matrix[i, j];

        }
        return result;
    }

    /// <summary>
    /// Extracts a row or column from a 2D array
    /// </summary>
    public static double[] ArrayToVector(double[,] values, uint? row = 0, uint? column = null)
    {
        if (values.GetLength(0) == 0)
            throw new ArgumentException($"Array {nameof(values)} cannot be empty");

        double[] vector = Array.Empty<double>();
        if (row is not null)
        {
            vector = new double[values.GetLength(1)];
            Array.Copy(values, (int)row * values.GetLength(1), vector, 0, values.GetLength(1));
        }
        else if (column is not null)
        {
            vector = ArrayToVector(ArrayTranspose(values), column);
        }

        return vector;
    }

    public static double NaNMean(double[,] values, uint row = 0, uint? column = null)
    {
        double[] vector = ArrayToVector(values, row, column);
        return NaNMean(vector);
    }

    public static double NaNVariance<T>(double[,] values, uint row = 0, uint? column = null)
    {
        double[] vector = ArrayToVector(values, row, column);
        return Variance(RemoveNaN(vector));
    }

    public static double NaNVarianceP<T>(double[,] values, uint row = 0, uint? column = null)
    {
        double[] vector = ArrayToVector(values, row, column);
        return VarianceP(RemoveNaN(vector));
    }

    public static double NaNStandardDeviation<T>(double[,] values, uint row = 0, uint? column = null)
    {
        double[] vector = ArrayToVector(values, row, column);
        return StandardDeviation(RemoveNaN(vector));
    }

    public static double NaNStandardDeviationP<T>(double[,] values, uint row = 0, uint? column = null)
    {
        double[] vector = ArrayToVector(values, row, column);
        return StandardDeviationP(RemoveNaN(vector));
    }

    public static double NaNStandardError<T>(double[,] values, uint row = 0, uint? column = null)
    {
        double[] vector = ArrayToVector(values, row, column);
        return NaNStandardError(RemoveNaN(vector));
    }
}
