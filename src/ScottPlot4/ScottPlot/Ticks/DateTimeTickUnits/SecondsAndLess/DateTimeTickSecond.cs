using System.Globalization;

namespace ScottPlot.Ticks.DateTimeTickUnits
{
    class DateTimeTickSecond : DateTimeTickUnitBase
    {
        public DateTimeTickSecond(CultureInfo culture, int maxTickCount, int? manualSpacing) : base(culture, maxTickCount, manualSpacing)
        {
        }
    }
}
