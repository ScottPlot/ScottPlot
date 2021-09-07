using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ScottPlotTests.PlotTypes
{
    class RadialGaugePlot
    {
        //[Test]
        public void Test_RecipeHashes_DoNotChange()
        {
            // only run hash tests on Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
            {
                Assert.Pass();
                return;
            }

            Dictionary<string, string> hashesByID = new()
            {
                { "radialgauge_quickstart", "15F40FD2C04204460436CC440EDEF0B8" },
                { "radialgauge_negative", "407082D25ADD5606DACF3E59EAB3692D" },
                { "radialgauge_mode", "EBF3E9EEF3722C5FEB99E857C87F827A" },
                { "radialgauge_single", "913F824E76A5EC2FBE7E0687FDA57A3C" },
                { "radialgauge_direction", "E482CA5CF086B900E2F14F5C1F4128C0" },
                { "radialgauge_size", "AD19C59C9E17657DFD23E026E4C9A09B" },
                { "radialgauge_caps", "37BED86728A34CBF5330BA4CC047C0B3" },
                { "radialgauge_start", "C47FF9F0EBA69C9FDE41212401DDB2BA" },
                { "radialgauge_range", "6973BE48800E1B3E669271305502E6AF" },
                { "radialgauge_labels", "41EAFDCBE619224D4E085E1E1AD501F7" },
                { "radialgauge_legend", "1764942DC129F9537A3B35049351207A" },
                { "radialgauge_labelpos", "A26080ADBFA3ECD546F26B24A56C253F" },
                { "radialgauge_labelfontpct", "6F91B87BC5D151199CBED071111BA541" },
                { "radialgauge_labelcolor", "F03DDE126E37962CC01F2CF92A796EB2" },
                { "radialgauge_backdim", "E1B37D2858BC4A1BBB694863F65AAEC2" },
                { "radialgauge_backnorm", "CA28123999587FD41676D2328863CB9A" },
            };

            foreach (var recipe in ScottPlot.Cookbook.Locate.GetRecipes("Plottable: RadialGauge"))
            {
                var plt = new ScottPlot.Plot(400, 400);
                recipe.ExecuteRecipe(plt);
                string hash = ScottPlot.Tools.BitmapHash(plt.GetBitmap());
                TestTools.SaveFig(plt, recipe.ID);

                Console.WriteLine($"{{\"{recipe.ID}\", \"{hash}\"}},");
                //Assert.AreEqual(hashesByID[recipe.ID], hash);
            }
        }
    }
}
