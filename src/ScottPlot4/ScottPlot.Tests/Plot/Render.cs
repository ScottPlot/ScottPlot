﻿using NUnit.Framework;

namespace ScottPlotTests.Plot
{
    internal class Render
    {
        [Test]
        public void Test_Render_Scaling()
        {
            System.Drawing.Bitmap bmp = new(600, 400);

            var plt = new ScottPlot.Plot();
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));

            plt.Render(bmp, scale: 1.5);
            TestTools.SaveBitmap(bmp);
        }

        [Test]
        public void Test_Render_Html()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));

            string b64 = plt.GetImageBase64();
            Assert.Greater(b64.Length, 1000);

            string img = plt.GetImageHTML();
            Assert.Greater(img.Length, b64.Length);

            TestTools.SaveHtml(img);
        }
    }
}
