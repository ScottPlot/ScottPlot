using NUnit.Framework;
using ScottPlot.Config.DateTimeTickUnits;
using System;
using System.Globalization;
using System.Linq;

namespace ScottPlotTests.Ticks
{
    [TestFixture]
    public class DateTimeTicksTests
    {
        [Test, Explicit]
        public void GetTicksAndLabels_AllUnitsAndAllCultures_AllLabelsContains2Lines()
        {
            DateTime from = DateTime.Now;
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                                    // This cultures support only short date range (100 Year) and throws on test
                                    .Where(culture => culture.ToString() != "ar" && culture.ToString() != "ar-SA")
                                    .ToArray();
            DateTimeUnitFactory factory = new DateTimeUnitFactory();
            for (DateTime to = from + TimeSpan.FromDays(1000 * 365); to - from > TimeSpan.FromMilliseconds(1); to -= (to - from) / 2)
            {
                foreach (var culture in cultures)
                {
                    var unit = factory.CreateBestUnit(from, to, culture, 10);
                    var labels = unit.GetTicksAndLabels(from, to, null);
                    Assert.False(labels.Labels.Any(l => l.Count(c => c == '\n') != 1));
                }
            }
        }
    }
}
