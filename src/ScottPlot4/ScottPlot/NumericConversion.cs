using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ScottPlot
{
    /* See discussion in https://github.com/ScottPlot/ScottPlot/pull/1927 */

    /// <summary>
    /// This class contains type-specific methods to convert between generic values and doubles
    /// optimized for performance using platform-specific features.
    /// </summary>
    public static class NumericConversion
    {
        private const MethodImplOptions ImplOptions =
#if NETCOREAPP
            MethodImplOptions.AggressiveOptimization |
#endif
            MethodImplOptions.AggressiveInlining;

        /// <summary>
        /// Returns the double value of a <typeparamref name="T"/> 
        /// using a conversion technique optimized for the platform.
        /// </summary>
        [MethodImpl(ImplOptions)]
        public static double GenericToDouble<T>(ref T value)
        {
            return value switch
            {
#if NETCOREAPP
                double vDouble => vDouble,
                float vSingle => Convert.ToDouble(vSingle),
                int vInt32 => Convert.ToDouble(vInt32),
                uint vUint32 => Convert.ToDouble(vUint32),
                long vInt64 => Convert.ToDouble(vInt64),
                ulong vUint64 => Convert.ToDouble(vUint64),
                short vInt16 => Convert.ToDouble(vInt16),
                ushort vUint16 => Convert.ToDouble(vUint16),
                decimal vDecimal => Convert.ToDouble(vDecimal),
#endif
                _ => Convert.ToDouble(value),
            };
        }

        /// <summary>
        /// Returns the double value of the <typeparamref name="T"/> at position <paramref name="i"/> in <paramref name="list"/>
        /// using a conversion technique optimized for the platform.
        /// </summary>
        [MethodImpl(ImplOptions)]
        public static double GenericToDouble<T>(List<T> list, int i)
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
        /// Returns a <typeparamref name="T"/> for a given double <paramref name="value"/>
        /// using a conversion technique optimized for the platform.
        /// </summary>
        [MethodImpl(ImplOptions)]
        public static void DoubleToGeneric<T>(double value, out T v)
        {
#if NETCOREAPP
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
            else
#endif
            {
                v = (T)Convert.ChangeType(value, typeof(T));
            }
        }
    }
}
