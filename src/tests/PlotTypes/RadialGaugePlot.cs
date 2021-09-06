using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ScottPlotTests.PlotTypes
{
    class RadialGaugePlot
    {
        [Test]
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
                { "radialgauge_quickstart", "B68EC793BB724E7BA8F38D46A4647B0F" },
                { "radialgauge_negative", "8121D0F1B1D1206E0ADF8F86D36FEC8C" },
                { "radialgauge_mode", "D574820285FBDF6907DC3CCA83033A6E" },
                { "radialgauge_single", "904D563886949C1A2DE671F1026A81C2" },
                { "radialgauge_direction", "FC968088AE1A1C97013E50085889D5D2" },
                { "radialgauge_order", "47E59F0DEF41C67B5C51C02F632DC5E3" },
                { "radialgauge_line", "F286CD5B1A8E9D315278C5B3C851E087" },
                { "radialgauge_size", "B5D95CA6CFAEC8797700D4B5585F3661" },
                { "radialgauge_caps", "95C2FC1C7C01714FBC405A30DB64D35D" },
                { "radialgauge_start", "0721BAFDA21ED65A7DD63FA49B4F9706" },
                { "radialgauge_range", "0FEF6C842831BF58A0F6FF111D2BA1EC" },
                { "radialgauge_labels", "2C1C0310462E05A0A25326D82AD15CF5" },
                { "radialgauge_labelpos", "753458E32A6A427DFA19727909C9F3C4" },
                { "radialgauge_labelfontpct", "7F16EF435B3D7B5D88C39D823167F7A1" },
                { "radialgauge_labelcolor", "5173393FD875CDDCBCE481DA2F30C377" },
                { "radialgauge_backdim", "8DA1E966A416465BBDF35692E4C63800" },
                { "radialgauge_backnorm", "00125CA2A66C509257A38170887A930D" },
                { "radialgauge_legend", "D2E0883ED03523BE5EB94B79E3604E01" },
            };

            foreach (var recipe in ScottPlot.Cookbook.Locate.GetRecipes("Plottable: RadialGauge"))
            {
                var plt = new ScottPlot.Plot(400, 400);
                recipe.ExecuteRecipe(plt);
                string hash = ScottPlot.Tools.BitmapHash(plt.GetBitmap());
                TestTools.SaveFig(plt, recipe.ID);

                Console.WriteLine($"{recipe.ID} {hash}");
                Assert.AreEqual(hashesByID[recipe.ID], hash);
            }
        }
    }
}
