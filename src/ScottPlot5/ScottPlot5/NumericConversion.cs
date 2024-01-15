using System.Runtime.CompilerServices;

namespace ScottPlot;

/// <summary>
/// This class contains type-specific methods to convert between generic values and doubles
/// optimized for performance using platform-specific features.
/// See discussion in https://github.com/ScottPlot/ScottPlot/pull/1927 
/// </summary>
public static class NumericConversion
{
    private const MethodImplOptions ImplOptions =
#if NETCOREAPP
        MethodImplOptions.AggressiveOptimization |
#endif
        MethodImplOptions.AggressiveInlining;

    [MethodImpl(ImplOptions)]
    public static double[] GenericToDoubleArray<T>(T[] values)
    {
        double[] values2 = new double[values.Length];

        for (int i = 0; i < values.Length; i++)
        {
            values2[i] = GenericToDouble(values, i);
        }

        return values2;
    }

    [MethodImpl(ImplOptions)]
    public static double[] GenericToDoubleArray<T>(IEnumerable<T> values)
    {
        return values.Select(value => GenericToDouble(ref value)).ToArray();
    }

    /// <summary>
    /// Returns the double value of a <typeparamref name="T"/> 
    /// using a conversion technique optimized for the platform.
    /// </summary>
    [MethodImpl(ImplOptions)]
    public static double GenericToDouble<T>(ref T value)
    {
        return value switch
        {
            double vDouble => vDouble,
            float vSingle => Convert.ToDouble(vSingle),
            int vInt32 => Convert.ToDouble(vInt32),
            uint vUint32 => Convert.ToDouble(vUint32),
            long vInt64 => Convert.ToDouble(vInt64),
            ulong vUint64 => Convert.ToDouble(vUint64),
            short vInt16 => Convert.ToDouble(vInt16),
            ushort vUint16 => Convert.ToDouble(vUint16),
            decimal vDecimal => Convert.ToDouble(vDecimal),
            byte vByte => Convert.ToDouble(vByte),
            DateTime vDateTime => vDateTime.ToOADate(),
            _ => Convert.ToDouble(value),
        };
    }

    [MethodImpl(ImplOptions)]
    public static Coordinates GenericToCoordinates<T1, T2>(ref T1 x, ref T2 y)
    {
        return new Coordinates(GenericToDouble(ref x), GenericToDouble(ref y));
    }

    /// <summary>
    /// Returns the double value of the <typeparamref name="T"/> at position <paramref name="i"/> in <paramref name="list"/>
    /// using a conversion technique optimized for the platform.
    /// </summary>
    [MethodImpl(ImplOptions)]
    public static double GenericToDouble<T>(IReadOnlyList<T> list, int i)
    {
        var v = list[i];
        return GenericToDouble(ref v);
    }

    /// <summary>
    /// Returns the double value of the <typeparamref name="T"/> at position <paramref name="i"/> in <paramref name="array"/>
    /// using a conversion technique optimized for the platform.
    /// </summary>
    [MethodImpl(ImplOptions)]
    public static double GenericToDouble<T>(T[] array, int i)
    {
        var v = array[i];
        return GenericToDouble(ref v);
    }

    /// <summary>
    /// Creates a <typeparamref name="T"/> for a given double <paramref name="value"/>
    /// using a conversion technique optimized for the platform.
    /// </summary>
    [MethodImpl(ImplOptions)]
    public static void DoubleToGeneric<T>(double value, out T v)
    {
        if (typeof(T) == typeof(double))
            v = (T)(object)value;
        else if (typeof(T) == typeof(float))
            v = (T)(object)Convert.ToSingle(value);
        else if (typeof(T) == typeof(int))
            v = (T)(object)Convert.ToInt32(value);
        else if (typeof(T) == typeof(uint))
            v = (T)(object)Convert.ToUInt32(value);
        else if (typeof(T) == typeof(long))
            v = (T)(object)Convert.ToInt64(value);
        else if (typeof(T) == typeof(ulong))
            v = (T)(object)Convert.ToUInt64(value);
        else if (typeof(T) == typeof(short))
            v = (T)(object)Convert.ToInt16(value);
        else if (typeof(T) == typeof(ushort))
            v = (T)(object)Convert.ToUInt16(value);
        else if (typeof(T) == typeof(decimal))
            v = (T)(object)Convert.ToDecimal(value);
        else if (typeof(T) == typeof(byte))
            v = (T)(object)Convert.ToByte(value);
        else if (typeof(T) == typeof(DateTime))
            v = (T)(object)DateTime.FromOADate(value);
        else
            v = (T)Convert.ChangeType(value, typeof(T));
    }

    public static T[] DoubleToGeneric<T>(this double[] input)
    {
        T[] result = new T[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            DoubleToGeneric<T>(input[i], out result[i]);
        }
        return result;
    }

    public static T[] ToGenericArray<T>(this double[] input)
    {
        T[] result = new T[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            DoubleToGeneric<T>(input[i], out result[i]);
        }
        return result;
    }

    public static byte AddBytes(byte a, byte b) => (byte)(a + b);
    public static byte Multiply(byte a, byte b) => (byte)(a * b);
    public static byte SubtractBytes(byte a, byte b) => (byte)(a - b);
    public static bool LessThanOrEqualBytes(byte a, byte b) => a <= b;

    public static Func<T, T, T> CreateAddFunction<T>()
    {
        ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
        ParameterExpression paramB = Expression.Parameter(typeof(T), "b");

        BinaryExpression body = Type.GetTypeCode(typeof(T)) switch
        {
            TypeCode.Byte => Expression.Add(paramA, paramB, typeof(NumericConversion).GetMethod(nameof(NumericConversion.AddBytes))),
            _ => Expression.Add(paramA, paramB),
        };

        return Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();
    }

    public static Func<T, T, T> CreateMultFunction<T>()
    {
        ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
        ParameterExpression paramB = Expression.Parameter(typeof(T), "b");

        BinaryExpression body = Type.GetTypeCode(typeof(T)) switch
        {
            TypeCode.Byte => Expression.Multiply(paramA, paramB, typeof(NumericConversion).GetMethod(nameof(NumericConversion.Multiply))),
            _ => Expression.Multiply(paramA, paramB),
        };

        return Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();

    }

    public static Func<T, T, T> CreateSubtractFunction<T>()
    {
        ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
        ParameterExpression paramB = Expression.Parameter(typeof(T), "b");

        BinaryExpression body = Type.GetTypeCode(typeof(T)) switch
        {
            TypeCode.Byte => Expression.Subtract(paramA, paramB, typeof(NumericConversion).GetMethod(nameof(NumericConversion.SubtractBytes))),
            _ => Expression.Subtract(paramA, paramB),
        };

        return Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();
    }

    public static Func<T, T, bool> CreateLessThanOrEqualFunction<T>()
    {
        ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
        ParameterExpression paramB = Expression.Parameter(typeof(T), "b");

        BinaryExpression body = Type.GetTypeCode(typeof(T)) switch
        {
            TypeCode.Byte => Expression.LessThanOrEqual(paramA, paramB, false, typeof(NumericConversion).GetMethod(nameof(NumericConversion.LessThanOrEqualBytes))),
            _ => Expression.LessThanOrEqual(paramA, paramB),
        };

        return Expression.Lambda<Func<T, T, bool>>(body, paramA, paramB).Compile();
    }

    public static T Clamp<T>(T input, T min, T max) where T : IComparable
    {
        if (input.CompareTo(min) < 0) return min;
        if (input.CompareTo(max) > 0) return max;
        return input;
    }

    public static bool IsReal(double x)
    {
        // implemented here because older versions of .NET do not support double.IsReal()
        return !double.IsNaN(x) && !double.IsInfinity(x);
    }

    public static bool IsReal(float x)
    {
        // implemented here because older versions of .NET do not support double.IsReal()
        return !float.IsNaN(x) && !float.IsInfinity(x);
    }
}
