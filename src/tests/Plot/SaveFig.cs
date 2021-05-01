using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Plot
{
    class SaveFig
    {
        [Test]
        public void Test_SaveFig_OutputScaling()
        {
            var plt = new ScottPlot.Plot();
            plt.AddSignal(DataGen.Sin(51), label: "sin");
            plt.AddSignal(DataGen.Cos(51), label: "cos");
            plt.Title("Scaled Figure Demo");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");
            var leg = plt.Legend();

            plt.SaveFig("Test_Scaled_A.png", 400, 300);
            plt.SaveFig("Test_Scaled_B.png", 400, 300, scale: 0.5);
            plt.SaveFig("Test_Scaled_C.png", 400, 300, scale: 5);
            plt.SaveFig("Test_Scaled_D.png", 300, 400, scale: 5);

            System.Drawing.Bitmap legBmp = leg.GetBitmap(lowQuality: false, scale: 5);
            legBmp.Save("Test_Scaled_E.png", System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
