using NUnit.Framework;
using ScottPlot.Cookbook;
using System.IO;

namespace ScottPlotTests.Cookbook
{
    class Generate
    {
        const string COOKBOOK_PROJECT_FOLDER = "../../../../cookbook/ScottPlot.Cookbook";
        const string COOKBOOK_OUTPUT_FOLDER = "../../../../cookbook/output";

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            if (Directory.Exists(COOKBOOK_OUTPUT_FOLDER))
                Directory.Delete(COOKBOOK_OUTPUT_FOLDER, recursive: true);
            Directory.CreateDirectory(COOKBOOK_OUTPUT_FOLDER);
        }

        [Test]
        public void Test_Generate_RecipeImages()
        {
            Chef.CreateCookbookImages(Path.Join(COOKBOOK_OUTPUT_FOLDER, "images"));
        }

        [Test]
        public void Test_Generate_RecipeSourceJson()
        {
            Chef.CreateRecipesJson(COOKBOOK_PROJECT_FOLDER, Path.Join(COOKBOOK_OUTPUT_FOLDER, "recipes.json"));
        }
    }
}
