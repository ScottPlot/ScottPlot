using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ScottPlotTests.Export
{
    class CSV
    {
        [Test]
        [TestCase("en-US")]
        [TestCase("de-DE")]
        [TestCase("da-DK")]
        [TestCase("hu-HU")]
        [TestCase("ka-GE")]
        public void Test_CSV_Signal(string cultureName)
        {
            var plt = new ScottPlot.Plot();
            var sig = plt.PlotSignal(
                ys: new double[] { 1.11, 2.22, 3.33, 4.44 },
                sampleRate: 0.1);

            string expected = "0, 1.11\n10, 2.22\n20, 3.33\n30, 4.44\n";
            var culture = CultureInfo.GetCultureInfo(cultureName);
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            Assert.AreEqual(expected, sig.GetCSV());
        }

        [Test]
        [TestCase("en-US")]
        [TestCase("de-DE")]
        [TestCase("da-DK")]
        [TestCase("hu-HU")]
        [TestCase("ka-GE")]
        public void Test_CSV_SignalConst(string cultureName)
        {
            var plt = new ScottPlot.Plot();
            var sig = plt.PlotSignalConst(
                ys: new double[] { 1.11, 2.22, 3.33, 4.44 },
                sampleRate: 0.1);

            string expected = "0, 1.11\n10, 2.22\n20, 3.33\n30, 4.44\n";
            var culture = CultureInfo.GetCultureInfo(cultureName);
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            Assert.AreEqual(expected, sig.GetCSV());
        }

        [Test]
        [TestCase("en-US")]
        [TestCase("de-DE")]
        [TestCase("da-DK")]
        [TestCase("hu-HU")]
        [TestCase("ka-GE")]
        public void Test_CSV_Scatter(string cultureName)
        {
            var plt = new ScottPlot.Plot();
            var sig = plt.PlotScatter(
                xs: new double[] { 0, 10, 20, 30 },
                ys: new double[] { 1.11, 2.22, 3.33, 4.44 });

            string expected = "0, 1.11\n10, 2.22\n20, 3.33\n30, 4.44\n";
            var culture = CultureInfo.GetCultureInfo(cultureName);
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            Assert.AreEqual(expected, sig.GetCSV());
        }
    }
}
