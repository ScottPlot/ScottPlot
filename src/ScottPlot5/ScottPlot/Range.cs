using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public struct Range
    {
        public static readonly Range UnitRange = new(0, 1);
        public double Min { get; }
        public double Max { get; }

        public Range(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public double Normalize(double value)
        {
            return (value - Min) / (Max - Min);
        }

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

        public double NormalizeAndClampToUnitRange(double value)
        {
            return UnitRange.Clamp(Normalize(value));
        }
    }
}
