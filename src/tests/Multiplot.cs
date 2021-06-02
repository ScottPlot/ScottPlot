using NUnit.Framework;
using System;

#pragma warning disable IDE0063 // Use simple 'using' statement

namespace ScottPlotTests
{
    class Multiplot
    {
        [Test]
        public void Test_MultiPlot_MatchAxis()
        {
            // render each subplot as a Bitmap
            var plt1 = new ScottPlot.Plot(300, 250);
            plt1.AddSignal(ScottPlot.DataGen.Sin(51));
            plt1.Title("Subplot A");
            System.Drawing.Bitmap bmp1 = plt1.Render();

            var plt2 = new ScottPlot.Plot(300, 250);
            plt2.AddSignal(ScottPlot.DataGen.Cos(51));
            plt2.Title("Subplot B");
            System.Drawing.Bitmap bmp2 = plt2.Render();

            // combine subplot bitmaps into one large bitmap
            using (var bmp = new System.Drawing.Bitmap(600, 250))
            using (var gfx = System.Drawing.Graphics.FromImage(bmp))
            {
                gfx.DrawImage(bmp1, 0, 0);
                gfx.DrawImage(bmp2, 300, 0);
                //bmp.Save("test.bmp");
                TestTools.SaveFig(bmp);
            }
        }
    }
}
