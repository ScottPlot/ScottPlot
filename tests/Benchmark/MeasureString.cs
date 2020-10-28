using NUnit.Framework;
using ScottPlot.Drawing;
using ScottPlotTests.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ScottPlotTests.Benchmark
{
    class MeasureString
    {
        //[Test]
        public void Test_MeasureString_SpeedWithoutFont()
        {
            var font = GDI.Font();
            //var bmp = new System.Drawing.Bitmap(1, 1);
            //var gfx = GDI.Graphics(bmp, lowQuality: true);

            Stopwatch sw = Stopwatch.StartNew();
            int reps = 1000;
            for (int i = 0; i < reps; i++)
            {
                var sz = GDI.MeasureString("Random", font);
                Assert.Greater(sz.Width, 0);
            }

            sw.Stop();
            Console.WriteLine($"{reps} string measurements took {sw.Elapsed.TotalMilliseconds} ms");

            /*
             * reusing a font: 1000 string measurements took 17.035 ms
             * newing a font each time: 1000 string measurements took 340.4981 ms
             * reusing a gfx: 1000 string measurements took 4.1549 ms
             */
        }
    }
}
