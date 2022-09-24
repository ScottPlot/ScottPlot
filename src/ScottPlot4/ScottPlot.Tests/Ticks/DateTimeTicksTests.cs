using NUnit.Framework;
using ScottPlot.Ticks;
using ScottPlot.Ticks.DateTimeTickUnits;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ScottPlotTests.Ticks
{
    [TestFixture]
    public class DateTimeTicksTests
    {
        [Test]
        public void GetTicksAndLabels_AllUnitsAndAllCultures_AllLabelsContains2Lines()
        {
            DateTime dateLower = new DateTime(1900, 1, 1);
            DateTime dateUpper = new DateTime(3000, 1, 1);

            List<CultureInfo> supportedCultures = new List<CultureInfo>();
            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                try
                {
                    dateUpper.ToString("yyyy", culture);
                    supportedCultures.Add(culture);
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }
            Assert.Greater(supportedCultures.Count, 100);

            DateTimeUnitFactory factory = new DateTimeUnitFactory();
            while ((dateUpper - dateLower).TotalMilliseconds > 1)
            {
                TimeSpan span = dateUpper - dateLower;
                Console.WriteLine($"Testing span: {span}");

                Parallel.ForEach(supportedCultures, culture =>
                {
                    var unit = factory.CreateBestUnit(dateLower, dateUpper, culture, 10);
                    var labels = unit.GetTicksAndLabels(dateLower, dateUpper, null);
                    Assert.False(labels.Labels.Any(l => l.Count(c => c == '\n') != 1));
                });

                dateUpper -= span / 2;
            }
        }

        [Test]
        public void TicksRespectLocale24HourClock()
        {
            DateTime almostMidnight = new DateTime(2000, 1, 1, 23, 59, 59);
            CultureInfo culture12HourClock = new CultureInfo("en-US");
            culture12HourClock.DateTimeFormat.ShortTimePattern = "hh:mm:ss";
            culture12HourClock.DateTimeFormat.LongTimePattern = "hh:mm:ss";

            CultureInfo culture24HourClock = new CultureInfo("en-US");
            culture24HourClock.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
            culture24HourClock.DateTimeFormat.LongTimePattern = "HH:mm:ss";

            DateTimeUnitFactory factory = new DateTimeUnitFactory();
            // Yes I know this is janky, I was feeling lazy
            IEnumerable<DateTimeUnit> dateTimeKinds = Enumerable.Range((int)DateTimeUnit.Hour, DateTimeUnit.Millisecond - DateTimeUnit.Hour + 1).Select(i => (DateTimeUnit)i);

            var ticks12Hour = dateTimeKinds.Select(kind => factory.Create(kind, culture12HourClock, 1, null).GetTicksAndLabels(almostMidnight, almostMidnight, null));
            var ticks24Hour = dateTimeKinds.Select(kind => factory.Create(kind, culture24HourClock, 1, null).GetTicksAndLabels(almostMidnight, almostMidnight, null));

            foreach (var label in ticks12Hour.SelectMany(c => c.Labels))
            {
                // The first line contains the date, the second line contains the time
                StringAssert.StartsWith("11:59:59", label.Split('\n')[1]);
            }

            foreach (var label in ticks24Hour.SelectMany(c => c.Labels))
            {
                StringAssert.StartsWith("23:59:59", label.Split('\n')[1]);
            }
        }
    }
}
