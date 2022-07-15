namespace ScottPlot.Common;

/// <summary>
/// This class contains methods for converting and performing math on generic and numeric types
/// </summary>
public static class NumericConversion
{
    public static T[] ToGenericArray<T>(this double[] input)
    {
        T[] result = new T[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            result[i] = (T)Convert.ChangeType(input[i], typeof(T));
        }
        return result;
    }
}
