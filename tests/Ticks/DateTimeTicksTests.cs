using NUnit.Framework;
using ScottPlot.Config.DateTimeTickUnits;
using System;
using System.Collections.Generic;
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

                foreach (var culture in supportedCultures)
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
