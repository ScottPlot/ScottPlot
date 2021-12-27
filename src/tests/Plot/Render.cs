using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
