using System.Globalization;

namespace ScottPlot.Config.DateTimeTickUnits
{
    class DateTimeTickSecond : DateTimeTickUnitBase
    {
        public DateTimeTickSecond(CultureInfo culture, int maxTickCount, int? manualSpacing) : base(culture, maxTickCount, manualSpacing)
        {
        }
    }
}
