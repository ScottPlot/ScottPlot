using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Extensions
{
    internal static class InternationalizationExtensions
    {
        public static bool Uses24HourClock(this CultureInfo culture) => culture.DateTimeFormat.LongTimePattern.Contains("H");
    }
}
