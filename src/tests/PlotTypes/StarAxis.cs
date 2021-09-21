using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlotTypes
{
    class StarAxis
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
                { "coxcomb_quickstart", "8F386F841382685D175B739FCE379477" },
                { "coxcomb_iconValue", "E6504FFF11C3310CD74C0F05402B15C6" },
                { "radar_quickstart", "0D24BFC2C9C7887B056D9881DD46049C" },
                { "radar_straightLines", "65DA3E1259822A66BBE62C7B45084643" },
                { "radar_noLines", "58B43CF578B7E7BD371EFE32F8C86901" },
                { "radar_labelCategory", "733F21B957DFFAD4381381870E2884D3" },
                { "radar_labelValue", "3FF9D22EC0A93C5C232B6ADF316C8772" },
                { "radar_iconValue", "90DB412916B0093E79F66ECF04FDD944" },
                { "radar_axisScaling", "28FB757063B3541C3E1CA9EFD9FD74FB" },
                { "radar_axisLimits", "E712589DD13EC1FE7113798C06E47009" },
            };

            var recepies = ScottPlot.Cookbook.Locate.GetRecipes("Plottable: Coxcomb")
                .Concat(ScottPlot.Cookbook.Locate.GetRecipes("Plottable: Radar"))
                .ToArray();

            foreach (var recipe in recepies)
            {
                var plt = new ScottPlot.Plot(400, 400);
                recipe.ExecuteRecipe(plt);
                string hash = ScottPlot.Tools.BitmapHash(plt.GetBitmap());
                TestTools.SaveFig(plt, recipe.ID);

                Console.WriteLine($"{{\"{recipe.ID}\", \"{hash}\"}},");
                Assert.AreEqual(hashesByID[recipe.ID], hash);
            }
        }
    }
}
