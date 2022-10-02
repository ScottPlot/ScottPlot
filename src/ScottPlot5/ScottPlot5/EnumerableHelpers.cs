using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    internal static class EnumerableHelpers
    {
        public static IEnumerable<T> One<T>(T item) { yield return item; }
    }
}
