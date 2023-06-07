using System;
using System.Drawing;

namespace ScottPlot
{
    public static class Validate
    {
        private static string ValidLabel(string label) =>
            string.IsNullOrWhiteSpace(label) ? "[unknown variable]" : label;

        /// <summary>
        /// Throw an exception if the value is NaN or infinity
        /// </summary>
        public static void AssertIsReal(string label, double value)
        {
            label = ValidLabel(label);

            if (double.IsNaN(value))
                throw new InvalidOperationException($"{label} is NaN");

            if (double.IsInfinity(value))
                throw new InvalidOperationException($"{label} is infinity");
        }

        /// <summary>
        /// Throw an exception if the array is null or contains NaN or infinity
        /// </summary>
        public static void AssertAllReal(string label, double[] values)
        {
            label = ValidLabel(label);

            if (values is null)
                throw new InvalidOperationException($"{label} must not be null");

            for (int i = 0; i < values.Length; i++)
                if (double.IsNaN(values[i]) || double.IsInfinity(values[i]))
                    throw new InvalidOperationException($"{label} index {i} is invalid ({values[i]})");
        }

        /// <summary>
        /// Throw an exception if the array is null or contains NaN or infinity
        /// </summary>
        public static void AssertAllReal<T>(string label, T[] values)
        {
            if (typeof(T) == typeof(double))
                AssertAllReal(label, (double[])(object)values);
            else if (typeof(T) == typeof(float))
                AssertAllReal(label, (float[])(object)values);
            else
                throw new InvalidOperationException("values must be float[] or double[]");
        }

        /// <summary>
        /// Throw an exception if an element is less than the previous element
        /// </summary>
        public static void AssertDoesNotDescend(double[] values) => AssertDoesNotDescend("values", values);

        /// <summary>
        /// Throw an exception if an element is less than the previous element
        /// </summary>
        public static void AssertDoesNotDescend<T>(T[] values) => AssertDoesNotDescend("values", values);

        /// <summary>
        /// Throw an exception if an element is less than the previous element
        /// </summary>
        public static void AssertDoesNotDescend<T>(string label, T[] values, int minIndex = 0, int? maxIndex = null)
        {
            if (maxIndex is null)
                maxIndex = values.Length - 1;

            label = ValidLabel(label);

            if (values is null)
                throw new InvalidOperationException($"{label} must not be null");

            for (int i = minIndex; i < maxIndex; i++)
                if (NumericConversion.GenericToDouble(ref values[i]) > NumericConversion.GenericToDouble(ref values[i + 1]))
                    throw new InvalidOperationException($"{label} must not descend: " +
                        $"{label}[{i}]={values[i]} but {label}[{i + 1}]={values[i + 1]}");
        }

        /// <summary>
        /// Throw an exception if the array does not contain at least one element
        /// </summary>
        public static void AssertHasElements(string label, double[] values)
        {
            label = ValidLabel(label);

            if (values is null)
                throw new InvalidOperationException($"{label} must not be null");

            if (values.Length == 0)
                throw new InvalidOperationException($"{label} must contain at least one element");
        }

        /// <summary>
        /// Throw an exception if the array does not contain at least one element
        /// </summary>
        public static void AssertHasElements<T>(string label, T[] values)
        {
            label = ValidLabel(label);

            if (values is null)
                throw new InvalidOperationException($"{label} must not be null");

            if (values.Length == 0)
                throw new InvalidOperationException($"{label} must contain at least one element");
        }

        /// <summary>
        /// Throw an exception if the array does not contain at least one element
        /// </summary>
        public static void AssertHasElements(string label, Color[] values)
        {
            label = ValidLabel(label);

            if (values is null)
                throw new InvalidOperationException($"{label} must not be null");

            if (values.Length == 0)
                throw new InvalidOperationException($"{label} must contain at least one element");
        }

        /// <summary>
        /// Throw an exception if the array does not contain at least one element
        /// </summary>
        public static void AssertHasElements(string label, string[] values)
        {
            label = ValidLabel(label);

            if (values is null)
                throw new InvalidOperationException($"{label} must not be null");

            if (values.Length == 0)
                throw new InvalidOperationException($"{label} must contain at least one element");
        }

        /// <summary>
        /// Throw an exception if non-null arrays have different lengths
        /// </summary>
        public static void AssertEqualLength(string label,
            double[] a, double[] b = null, double[] c = null,
            double[] d = null, double[] e = null, double[] f = null)
        {
            label = ValidLabel(label);

            if (!IsEqualLength(a, b, c, d, e, f))
                throw new InvalidOperationException($"{label} must all have same length");
        }

        /// <summary>
        /// Throw an exception if non-null arrays have different lengths
        /// </summary>
        public static void AssertEqualLength<T1, T2>(string label, T1[] a, T2[] b)
        {
            label = ValidLabel(label);

            if (a.Length != b.Length)
                throw new InvalidOperationException($"{label} must all have same length");
        }

        /// <summary>
        /// Returns true if all non-null arguments have equal length
        /// </summary>
        public static bool IsEqualLength(double[] a, double[] b = null, double[] c = null,
                                         double[] d = null, double[] e = null, double[] f = null)
        {
            if (a is null)
                throw new InvalidOperationException($"first array must not be null");
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
            label = ValidLabel(label);

            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException($"{label} must contain text");
        }

        /// <summary>
        /// Placeholder for functions which need to call a validation function
        /// </summary>
        public static void Pass() { }
    }
}
