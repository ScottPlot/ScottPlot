using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Site
{
    /// <summary>
    /// This class reads content of the recipe folder to generate HTML from IDs.
    /// It does not store structures (like what IDs go on what pages).
    /// </summary>
    public class RecipesPage : Page
    {
        public RecipesPage(string cookbookSiteFolder, string resourceFolder) : base(cookbookSiteFolder, resourceFolder) { }

        public void AddRecipiesFromCategory(string category) =>
            AddRecipes(Locate.GetRecipes().Where(x => x.Category == category).ToArray());

        public void AddAllRecipies() =>
            AddRecipes(Locate.GetRecipes());

        private void AddRecipes(IRecipe[] recipes)
        {
            foreach (IRecipe recipe in recipes)
                AddRecipe(recipe);
        }

        private void AddRecipe(IRecipe recipe)
        {
            string id = recipe.ID;
            string recipeFolder = Path.Combine(SiteFolder, "source");
            string codeFilePath = Path.Combine(recipeFolder, id + ExtCode);
            string imageUrl = id + ExtImage;
            string[] raw = File.ReadAllLines(codeFilePath);
            string code = string.Join("\n", raw.Skip(2));

            AddHTML($"<h2><a href='#{id}' name='{id}' style='color: black;'>{recipe.Title}</a></h2>");
            AddHTML(recipe.Description);
            AddCode(code);

            SB.AppendLine($"<div class='center'><img src='source/{imageUrl}' /></div>");
        }
    }
}
