using NUnit.Framework;
using ScottPlot.Config.DateTimeTickUnits;
using System;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace ScottPlotTests.Ticks
{
    [TestFixture]
    public class DateTimeTicksTests
    {
        [Test]
        public void GetTicksAndLabels_AllUnitsAndAllCultures_AllLabelsContains2Lines()
        {
            string[] ignoredCultures = { "ar", "ar-SA" }; // these use a 100 year date
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                                    .Where(culture => ignoredCultures.Contains(culture.ToString()) == false)
                                    .ToArray();

            DateTimeUnitFactory factory = new DateTimeUnitFactory();

            // OSX can only instantiate DateTimes between 04/30/1900 and 11/16/2077
            DateTime dateLower = new DateTime(1901, 1, 1);
            DateTime dateUpper = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ?
                 new DateTime(2070, 1, 1) : new DateTime(3000, 1, 1);

            while ((dateUpper - dateLower).TotalMilliseconds > 1)
            {
                TimeSpan span = dateUpper - dateLower;
                Console.WriteLine($"Testing span: {span}");

                foreach (var culture in cultures)
                {
                    var unit = factory.CreateBestUnit(dateLower, dateUpper, culture, 10);
                    var labels = unit.GetTicksAndLabels(dateLower, dateUpper, null);
                    Assert.False(labels.Labels.Any(l => l.Count(c => c == '\n') != 1));
                }

                dateUpper -= span / 2;
            }
        }
    }
}
