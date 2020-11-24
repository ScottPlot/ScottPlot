﻿using NUnit.Framework;
using ScottPlot.Ticks;
using ScottPlot.Drawing;
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

            string defaultFont = InstalledFont.Default();
            Console.WriteLine($">>> Default font: {defaultFont}");

            string defaultFontSans = InstalledFont.Sans();
            Console.WriteLine($">>> Default sans font: {defaultFontSans}");

            string defaultFontSerif = InstalledFont.Serif();
            Console.WriteLine($">>> Default serif font: {defaultFontSerif}");

            string defaultFontMonospace = InstalledFont.Monospace();
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
