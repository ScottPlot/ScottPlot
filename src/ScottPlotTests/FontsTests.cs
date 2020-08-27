using NUnit.Framework;
using ScottPlot.Config;
using System;
using System.Runtime.InteropServices;

namespace ScottPlotTests
{
    [TestFixture]
    public class FontsTests
    {
        [Test]
        public void Test_DefaultFont_MatchesOsExpectations()
        {

            string defaultFont = Fonts.GetDefaultFontName();
            Console.WriteLine($">>> Default font: {defaultFont}");

            string defaultFontSans = Fonts.GetSansFontName();
            Console.WriteLine($">>> Default sans font: {defaultFontSans}");

            string defaultFontSerif = Fonts.GetSerifFontName();
            Console.WriteLine($">>> Default serif font: {defaultFontSerif}");

            string defaultFontMonospace = Fonts.GetMonospaceFontName();
            Console.WriteLine($">>> Default monospace font: {defaultFontMonospace}");

            Assert.That(defaultFont == defaultFontSans);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.That(defaultFontSans == "Segoe UI");
                Assert.That(defaultFontSerif == "Times New Roman");
                Assert.That(defaultFontMonospace == "Consolas");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Assert.That(defaultFontSans == "DejaVu Sans");
                Assert.That(defaultFontSerif == "DejaVu Serif");
                Assert.That(defaultFontMonospace == "DejaVu Sans Mono");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Assert.That(defaultFontSans == "Helvetica");
                Assert.That(defaultFontSerif.StartsWith("Times"));
                Assert.That(defaultFontMonospace == "Courier");
            }
        }
    }
}
