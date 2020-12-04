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
        public RecipesPage(string cookbookSiteFolder) : base(cookbookSiteFolder) { }

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
            string code = string.Join("<br>\n", raw.Skip(2));

            DivStart("recipeSection");

            AddDiv($"<a href='#{id}' name='{id}'>{recipe.Title}</a>", "recipeTitle");
            AddDiv(recipe.Description, "recipeDescription");
            AddCode(code);

            DivStart("recipeImages");
            SB.AppendLine($"<img src='source/{imageUrl}' />");
            DivEnd();

            DivEnd();
        }
    }
}
