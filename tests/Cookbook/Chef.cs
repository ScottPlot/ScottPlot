using NUnit.Framework;
using ScottPlot.Demo;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Cookbook
{
    class Chef
    {
        [Test]
        public void Test_Cookbook_Hashes()
        {
            // don't run test on MacOS
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
                return; // TODO: figure out how to get this working in MacOS

            foreach (IPlotDemo recipe in Reflection.GetPlots())
            {
                if (recipe is IBitmapDemo)
                    continue;

                var plt = new ScottPlot.Plot();
                recipe.Render(plt);
                System.Drawing.Bitmap bmp = plt.GetBitmap();
                string hash = ScottPlot.Tools.BitmapHash(bmp);
                Console.WriteLine($"{hash} {recipe.categoryMajor}.{recipe.categoryMinor}.{recipe.categoryClass}");

            }
        }
    }
}
