using NUnit.Framework;
using ScottPlot.Config;
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
            string defaultFontSans = Fonts.GetSansFontName();
            string defaultFontSerif = Fonts.GetSerifFontName();
            string defaultFontMonospace = Fonts.GetMonospaceFontName();

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
                Assert.That(defaultFontSerif == "Times");
                Assert.That(defaultFontMonospace == "Courier");
            }
        }
    }
}
