using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public struct Range
    {
        public double Min { get; set; }
        public double Max { get; set; }

        public Range(double min, double max)
        {
            Min = min;
            Max = max; 
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
    }
}
