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
                { "radialgauge_quickstart", "7DADD0FF9E3C8665AB7C37C74753442C" },
                { "radialgauge_negative", "A412A15974E8292593538A66AB2DB8F0" },
                { "radialgauge_mode", "E6031401433AAB2104E23A48415048BC" },
                { "radialgauge_single", "913F824E76A5EC2FBE7E0687FDA57A3C" },
                { "radialgauge_direction", "D618F1C370C7EFA1FF93830612F6AB8B" },
                { "radialgauge_size", "59E3FA1BD594F89C8F8409A3D74F4D1F" },
                { "radialgauge_caps", "743FC4A378006B711328F74C2B8DCEF0" },
                { "radialgauge_start", "EEC1E7678C4CCD442AC32FB70A1992F5" },
                { "radialgauge_range", "7D80A0239C37B254C2B65702FE646D75" },
                { "radialgauge_labels", "0D47E5AD3B48594417EE97582CA712B9" },
                { "radialgauge_legend", "FED31EFA0FEEFC5C5D96F1463C78DAE7" },
                { "radialgauge_labelpos", "2C5456E6AB6617A30C1D098A921BFEC4" },
                { "radialgauge_labelfontpct", "237A90626F9F704393E68EEDAB5FBF0B" },
                { "radialgauge_labelcolor", "92745AB300107F9848210445F372652B" },
                { "radialgauge_backdim", "678DBF75D6CAD14163F5C47A2742A092" },
                { "radialgauge_backnorm", "CE83805F9BB956B6004F040463687992" },
            };

            foreach (var recipe in ScottPlot.Cookbook.Locate.GetRecipes("Plottable: RadialGauge"))
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
