using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ScottPlotTests.Plot
{
    class YLabel
    {
        readonly string sampleLabel = "Frequency (Hz)";

        [Test]
        public void Test_Label_DefaultIsEmpty()
        {
            ScottPlot.Plot plt = TestTools.SamplePlotScatter();

            string hashDefault = TestTools.HashedFig(plt, "default label");
            plt.YLabel("");
            string hashEmpty = TestTools.HashedFig(plt, "empty label");

            Assert.That(hashDefault == hashEmpty);
        }

        [Test]
        public void Test_Label_IsSettable()
        {
            ScottPlot.Plot plt = TestTools.SamplePlotScatter();

            string hash1 = TestTools.HashedFig(plt, "default label");

            plt.YLabel(sampleLabel);
            string hash2 = TestTools.HashedFig(plt);

            Assert.That(plt.GetSettings(false).yLabel.text == sampleLabel);
            Assert.That(hash1 != hash2);
        }

        [Test]
        public void Test_Label_IsReplaceable()
        {
            ScottPlot.Plot plt = TestTools.SamplePlotScatter();

            string label1 = "first label";
            plt.YLabel(label1);
            string hash1 = TestTools.HashedFig(plt, label1);

            string label2 = "second label";
            plt.YLabel(label2);
            string hash2 = TestTools.HashedFig(plt, label2);

            Assert.That(hash1 != hash2);
        }

        [Test]
        public void Test_Label_DoesntClearWithRepeatedMethodCalls()
        {
            ScottPlot.Plot plt = TestTools.SamplePlotScatter();

            string hashDefault = TestTools.HashedFig(plt, "default");
            plt.YLabel(sampleLabel);
            string hashAfterLabel = TestTools.HashedFig(plt, "labeled");

            plt.YLabel();
            string hashAfterEmptyCall = TestTools.HashedFig(plt, "empty call");

            Assert.That(hashDefault != hashAfterLabel);
            Assert.That(hashAfterEmptyCall == hashAfterLabel);
        }

        [Test]
        public void Test_Visible_IsSettable()
        {
            ScottPlot.Plot plt = TestTools.SamplePlotScatter();

            plt.YLabel(sampleLabel);
            bool visibleByDefault = plt.GetSettings(false).yLabel.visible;
            string hashVisible = TestTools.HashedFig(plt, "visible");

            plt.YLabel(sampleLabel, enable: false);
            bool visibleAfterDisabled = plt.GetSettings(false).yLabel.visible;
            string hashInvisible = TestTools.HashedFig(plt, "invisible");

            plt.YLabel(sampleLabel, enable: true);
            bool visibleAfterEnabled = plt.GetSettings(false).yLabel.visible;
            string hashVisibleAgain = TestTools.HashedFig(plt, "visible again");

            Assert.IsTrue(visibleByDefault);
            Assert.IsFalse(visibleAfterDisabled);
            Assert.IsTrue(visibleAfterEnabled);

            Assert.That(hashVisible != hashInvisible);
            Assert.That(hashVisible == hashVisibleAgain);
        }

        [Test]
        public void Test_Color_IsSettable()
        {
            ScottPlot.Plot plt = TestTools.SamplePlotScatter();

            string hashDefault = TestTools.HashedFig(plt, "default");
            plt.YLabel(sampleLabel, color: System.Drawing.Color.Red);
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

            plt.YLabel(sampleLabel, fontName: font1);
            string hash1 = TestTools.HashedFig(plt, font1);

            plt.YLabel(sampleLabel, fontName: font2);
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

            plt.YLabel(sampleLabel);
            string hashDefault = TestTools.HashedFig(plt, "default");

            plt.YLabel(sampleLabel, bold: true);
            string hashBold = TestTools.HashedFig(plt, "bold");

            plt.YLabel(sampleLabel, bold: false);
            string hashNotBold = TestTools.HashedFig(plt, "not bold");

            Assert.That(hashDefault == hashNotBold);
            Assert.That(hashBold != hashNotBold);
        }

        [Test]
        public void Test_FontSize_IsSettable()
        {
            ScottPlot.Plot plt = TestTools.SamplePlotScatter();

            plt.YLabel(sampleLabel, fontSize: 12);
            string hashSize12 = TestTools.HashedFig(plt, "size 12");

            plt.YLabel(sampleLabel, fontSize: 16);
            string hashSize16 = TestTools.HashedFig(plt, "size 16");

            Assert.That(plt.GetSettings(false).yLabel.fontSize == 16);
            Assert.That(hashSize12 != hashSize16);
        }
    }
}
