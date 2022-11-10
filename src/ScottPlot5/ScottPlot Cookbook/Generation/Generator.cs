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
        FrontPageHtml page = new();
        page.Generate();
    }

    [Test]
    public void Generate_Recipe_Images()
    {
        foreach (RecipePageBase page in Cookbook.GetPages())
        {
            string pageFolderName = Output.GetPagePath(page.PageDetails);
            TestContext.WriteLine(pageFolderName);

            foreach (IRecipe recipe in page.GetRecipes())
            {
                string recipeImagePath = Output.GetBaseImagePath(page.PageDetails, recipe);
                TestContext.WriteLine("  " + Path.GetFileName(recipeImagePath));
            }
        }
    }
}
