using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Cookbook
{
    public static class Chef
    {
        public static void MakeCookbook(string sourceCodeFolder, string outputFolder = "./output")
        {
            outputFolder = System.IO.Path.GetFullPath(outputFolder);
            string imageFolder = System.IO.Path.Combine(outputFolder, "images");

            Console.WriteLine($"Generating cookbook in: {outputFolder}");

            if (System.IO.Directory.Exists(outputFolder))
                System.IO.Directory.Delete(outputFolder, recursive: true);
            System.IO.Directory.CreateDirectory(outputFolder);
            System.IO.Directory.CreateDirectory(imageFolder);

            foreach (IPlotDemo recipe in Reflection.GetPlots())
            {
                var plt = new Plot(600, 400);
                recipe.Render(plt);
                plt.SaveFig($"{imageFolder}/{recipe.id}.png");
                System.IO.File.WriteAllText($"{imageFolder}/{recipe.id}.cs", recipe.GetSourceCode(sourceCodeFolder));
            }
        }
    }
}
