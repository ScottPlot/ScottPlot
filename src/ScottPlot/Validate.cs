using System;
using System.Drawing;

namespace ScottPlot
{
    public static class Validate
    {
        /// <summary>
        /// Throw an exception if the value is NaN or infinity
        /// </summary>
        public static void AssertIsReal(string label, double value)
        {
            if (double.IsNaN(value))
                throw new ArgumentException($"{label} is NaN");
            if (double.IsInfinity(value))
                throw new ArgumentException($"{label} is infinity");
        }

        /// <summary>
        /// Throw an exception if the array is null or contains NaN or infinity
        /// </summary>
        public static void AssertAllReal(string label, double[] values)
        {
            if (values is null)
                throw new ArgumentException($"{label} must not be null");

            for (int i = 0; i < values.Length; i++)
                if (double.IsNaN(values[i]) || double.IsInfinity(values[i]))
                    throw new ArgumentException($"{label} index {i} is invalid ({values[i]})");
        }

        /// <summary>
        /// Throw an exception if one elemnt is equal to or less than the previous element
        /// </summary>
        public static void AssertAscending(string label, double[] values)
        {
            if (values is null)
                throw new ArgumentException($"{label} must not be null");

            for (int i = 0; i < values.Length - 1; i++)
                if (values[i] >= values[i + 1])
                    throw new ArgumentException($"{label} must be ascending values (index {i} >= {i + 1}");
        }

        /// <summary>
        /// Throw an exception if the array does not contain at least one element
        /// </summary>
        public static void AssertHasElements(string label, double[] values)
        {
            if (values is null)
                throw new ArgumentException($"{label} must not be null");

            if (values.Length == 0)
                throw new ArgumentException($"{label} must contain at least one element");
        }

        /// <summary>
        /// Throw an exception if the array does not contain at least one element
        /// </summary>
        public static void AssertHasElements(string label, Color[] values)
        {
            if (values is null)
                throw new ArgumentException($"{label} must not be null");

            if (values.Length == 0)
                throw new ArgumentException($"{label} must contain at least one element");
        }

        /// <summary>
        /// Throw an exception if the array does not contain at least one element
        /// </summary>
        public static void AssertHasElements(string label, string[] values)
        {
            if (values is null)
                throw new ArgumentException($"{label} must not be null");

            if (values.Length == 0)
                throw new ArgumentException($"{label} must contain at least one element");
        }

        /// <summary>
        /// Throw an exception if non-null arrays have different lengths
        /// </summary>
        public static void AssertEqualLength(string label,
            double[] a, double[] b = null, double[] c = null,
            double[] d = null, double[] e = null, double[] f = null)
        {
            if (!IsEqualLength(a, b, c, d, e, f))
                throw new ArgumentException($"{label} must all have same length");
        }

        /// <summary>
        /// Returns true if all non-null arguments have equal length
        /// </summary>
        public static bool IsEqualLength(double[] a, double[] b = null, double[] c = null,
                                         double[] d = null, double[] e = null, double[] f = null)
        {
            if (a is null)
                throw new ArgumentException($"first array must not be null");
            if (b is object && b.Length != a.Length) return false;
            if (c is object && c.Length != a.Length) return false;
            if (d is object && d.Length != a.Length) return false;
            if (e is object && e.Length != a.Length) return false;
            if (f is object && f.Length != a.Length) return false;
            return true;
        }

        /// <summary>
        /// Throws an exception if the string is null, empty, or only contains whitespace
        /// </summary>
        public static void AssertHasText(string label, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{label} must contain text");
        }
    }
}
