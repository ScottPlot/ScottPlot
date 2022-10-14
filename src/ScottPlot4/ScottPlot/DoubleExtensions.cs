using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    internal static class DoubleExtensions
    {
        public static bool IsFinite(this double x) => !double.IsNaN(x) && !double.IsInfinity(x);
    }
}
