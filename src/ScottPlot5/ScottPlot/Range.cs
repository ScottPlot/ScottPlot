using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    ///<summary>
    ///Represents a range between any two finite values (inclusive)
    ///</summary>
    public struct Range
    {
        /// <summary>
        /// A range representing the closed interval [0, 1]
        /// </summary>
        public static readonly Range UnitRange = new(0, 1);
        public double Min { get; }
        public double Max { get; }

        public Range(double min, double max)
        {
            if (double.IsInfinity(min) || double.IsNaN(min))
            {
                throw new ArgumentException($"Argument ${nameof(min)} must be finite and non-NaN");
            }

            if (double.IsInfinity(max) || double.IsNaN(max))
            {
                throw new ArgumentException($"Argument ${nameof(max)} must be finite and non-NaN");
            }

            if (min > max)
            {
                throw new ArgumentException($"Argument ${nameof(min)} must be less than or equal to ${nameof(max)}.");
            }

            Min = min;
            Max = max;
        }

        /// <summary>
        /// Returns the given value as a fraction of the difference between Min and Max. This is a min-max feature scaling.
        /// </summary>
        /// <param name="value">The value to normalize</param>
        /// <param name="clamp">If true, values outside of the range will be clamped onto the interval [0, 1].</param>
        /// <returns>The normalized value</returns>
        public double Normalize(double value, bool clamp = false)
        {
            if (Max == Min) // When Min == Max we would divide by zero without this check.
            {
                return Min;
            }

            double normalized = (value - Min) / (Max - Min);

            return clamp ? UnitRange.Clamp(normalized) : normalized;
        }

        ///<summary>
        ///Returns the given value clamped to the range (inclusive).
        ///</summary>
        public double Clamp(double value)
        {
            if (value < Min)
            {
                return Min;
            }
            if (value > Max)
            {
                return Max;
            }

            return value;
        }
    }
}
