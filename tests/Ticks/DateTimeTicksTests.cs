using NUnit.Framework;
using ScottPlot.Config.DateTimeTickUnits;
using System;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;

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

            DateTime dateLower = new DateTime(1985, 9, 24);
            DateTime dateUpper = dateLower + TimeSpan.FromDays(1000 * 365);
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
