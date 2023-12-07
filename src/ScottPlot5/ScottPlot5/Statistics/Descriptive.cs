using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Statistics;

public static class Descriptive
{
    public static double Mean<T>(IEnumerable<T> values)
    {
        IEnumerable<double> data = values.Select(value => Convert.ToDouble(value));
        return data.Average();
    }

    public static double StDev<T>(IEnumerable<T> values, bool asSample = false)
    {
        return StDev(values, Mean(values), asSample);
    }

    public static double StDev<T>(IEnumerable<T> values, double mean, bool asSample = false)
    {
        double[] data = values.Select(value => Convert.ToDouble(value)).ToArray();
        double sumVariancesSquared = data.Sum(x => (x - mean) * (x - mean));
        double denominator = data.Length - (asSample ? 1 : 0);
        return denominator > 0.0 ? Math.Sqrt(sumVariancesSquared / denominator) : -1.0;

        //for (int i = 0; i < data.Length; i++)
        //{
        //    double pointVariance = Math.Abs(mean - data[i]);
        //    double pointVarianceSquared = Math.Pow(pointVariance, 2);
        //    sumVariancesSquared += pointVarianceSquared;
        //}
        //double meanVarianceSquared = sumVariancesSquared / (data.Length - (asSample ? 1 : 0));
        //double stDev = Math.Sqrt(meanVarianceSquared);
        //return stDev;
    }
    
    public static double StdErr<T>(IEnumerable<T> values, bool asSample = false)
    {
        return StdErr(values, Mean(values), asSample);
    }

    public static double StdErr<T>(IEnumerable<T> values, double mean, bool asSample = false)
    {
        double denominator = values.Count() - (asSample ? 1 : 0);
        return denominator > 0.0 ? StDev(values, mean, asSample) / Math.Sqrt(denominator) : -1.0;
    }
}