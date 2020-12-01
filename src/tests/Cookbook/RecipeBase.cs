using NUnit.Framework;
using System;

namespace ScottPlotTests.Cookbook
{
    public abstract class RecipeTester
    {
        public abstract RecipeCategory Category { get; }
        public abstract string Section { get; }
        public abstract string Title { get; }
        public abstract string Description { get; }
        public abstract void ExecuteRecipe(ScottPlot.Plot plt);

        int Width = 600;
        int Height = 400;
        bool SaveToDisk = true;

        [Test]
        public void TestRecipe()
        {
            var plt = new ScottPlot.Plot(Width, Height);

            ExecuteRecipe(plt);

            if (SaveToDisk)
            {
                string fileName = $"{Category}.{Section}.{Title}.png";
                string filePath = System.IO.Path.GetFullPath(fileName);
                Console.WriteLine($"Saved: {filePath}");
                plt.SaveFig(fileName);
            }
            else
            {
                plt.Render();
            }
        }
    }
}
