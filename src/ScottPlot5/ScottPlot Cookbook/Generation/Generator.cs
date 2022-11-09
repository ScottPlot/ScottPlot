using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotCookbook.Generation;

internal class Generator
{
    [Test]
    public void Generate_Front_Page()
    {
        FrontPage pb = new();
    }

    [Test]
    public void Generate_Recipe_Pages()
    {
        foreach (RecipePage page in Cookbook.GetPages())
        {
            string pageFolderName = PageBase.UrlSafe(page.PageName);
            string pageFolderPath = Path.Combine(PageBase.OutputFolder, pageFolderName);
            Directory.CreateDirectory(pageFolderPath);

            foreach (IRecipe recipe in Cookbook.GetRecipes(page))
            {
                string recipeImageFileName = PageBase.UrlSafe(recipe.Name) + ".jpg";
                string recipeImagePath = Path.Combine(pageFolderPath, recipeImageFileName);
                TestContext.WriteLine(recipeImageFileName);
            }
        }
    }
}
