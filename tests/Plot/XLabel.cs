using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ScottPlotTests.Plot
{
    public class XLabel
    {
        readonly string sampleLabel = "Frequency (Hz)";

        [Test]
        public void Test_Label_DefaultIsEmpty()
        {
            ScottPlot.Plot plt = TestTools.SamplePlotScatter();

            string hashDefault = TestTools.HashedFig(plt, "default label");
            plt.XLabel("");
            string hashEmpty = TestTools.HashedFig(plt, "empty label");

            Assert.That(hashDefault == hashEmpty);
        }

        [Test]
        public void Test_Label_IsSettable()
        {
            ScottPlot.Plot plt = TestTools.SamplePlotScatter();

            string hash1 = TestTools.HashedFig(plt, "default label");

            plt.XLabel(sampleLabel);
            string hash2 = TestTools.HashedFig(plt);

            Assert.That(plt.GetSettings(false).xLabel.text == sampleLabel);
            Assert.That(hash1 != hash2);
        }

        [Test]
        public void Test_Label_IsReplaceable()
        {
            ScottPlot.Plot plt = TestTools.SamplePlotScatter();

            string label1 = "first label";
            plt.XLabel(label1);
            string hash1 = TestTools.HashedFig(plt, label1);

            string label2 = "second label";
            plt.XLabel(label2);
            string hash2 = TestTools.HashedFig(plt, label2);

            Assert.That(hash1 != hash2);
        }

        [Test]
        public void Test_Color_IsSettable()
        {
            ScottPlot.Plot plt = TestTools.SamplePlotScatter();

            string hashDefault = TestTools.HashedFig(plt, "default");
            plt.XLabel(sampleLabel, color: System.Drawing.Color.Red);
            string hashModified = TestTools.HashedFig(plt, "modified");

            Assert.That(hashDefault != hashModified);
        }

        [Test]
        public void Test_FontName_IsSettable()
        {
            ScottPlot.Plot plt = TestTools.SamplePlotScatter();

            string font1 = ScottPlot.Config.Fonts.GetSerifFontName();
            string font2 = ScottPlot.Config.Fonts.GetMonospaceFontName();
            Assert.That(font1 != font2);

            plt.XLabel(sampleLabel, fontName: font1);
            string hash1 = TestTools.HashedFig(plt, font1);

            plt.XLabel(sampleLabel, fontName: font2);
            string hash2 = TestTools.HashedFig(plt, font2);

            Assert.That(hash1 != hash2);
        }

        [Test]
        public void Test_Bold_IsSettable()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("Skipping bold test (bold font rendering with System.Drawing is only supported on Windows)");
                return;
            }

            ScottPlot.Plot plt = TestTools.SamplePlotScatter();

            plt.XLabel(sampleLabel);
            string hashDefault = TestTools.HashedFig(plt, "default");

            plt.XLabel(sampleLabel, bold: true);
            string hashBold = TestTools.HashedFig(plt, "bold");

            plt.XLabel(sampleLabel, bold: false);
            string hashNotBold = TestTools.HashedFig(plt, "not bold");

            Assert.That(hashDefault == hashNotBold);
            Assert.That(hashBold != hashNotBold);
        }

        [Test]
        public void Test_FontSize_IsSettable()
        {
            ScottPlot.Plot plt = TestTools.SamplePlotScatter();

            plt.XLabel(sampleLabel, fontSize: 12);
            string hashSize12 = TestTools.HashedFig(plt, "size 12");

            plt.XLabel(sampleLabel, fontSize: 16);
            string hashSize16 = TestTools.HashedFig(plt, "size 16");

            Assert.That(plt.GetSettings(false).xLabel.fontSize == 16);
            Assert.That(hashSize12 != hashSize16);
        }
    }
}
