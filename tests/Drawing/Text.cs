using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Drawing
{
    public class Text
    {
        [Test]
        public void Test_MeasureString_ShowSize()
        {
            string testString = "ScottPlot";

            var fontName = ScottPlot.Config.Fonts.GetDefaultFontName();
            float fontSize = 14;

            // an active Graphics object is required to measure a string...
            using (var bmp = new System.Drawing.Bitmap(1, 1))
            using (var gfx = System.Drawing.Graphics.FromImage(bmp))
            using (var font = new System.Drawing.Font(fontName, fontSize))
            {
                var stringSize = gfx.MeasureString(testString, font);

                var sb = new StringBuilder();
                sb.AppendLine(System.Environment.OSVersion.ToString());
                sb.AppendLine($"The string '{testString}' with font '{fontName}' (size {fontSize}) " +
                    $"measures: {stringSize.Width}px x {stringSize.Height}px");

                Console.WriteLine(sb);

                // create an intentionally-failing test to visualuze Azure Pipelines output
                Assert.AreEqual(1, 2);
            }
        }
    }
}
