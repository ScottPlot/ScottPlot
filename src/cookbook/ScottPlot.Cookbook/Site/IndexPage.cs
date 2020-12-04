using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Site
{
    /// <summary>
    /// The cookbook index page has links to recipes located on individual category pages
    /// </summary>
    public class IndexPage : Page
    {
        public IndexPage(string cookbookSiteFolder) : base(cookbookSiteFolder) { }

        public void AddLinksToRecipes()
        {
            foreach (var stuff in Locate.GetCategorizedRecipes())
                AddRecipeGroup(stuff.Key, stuff.Value);

            DivStart("categorySection");
            AddGroupHeader("View all Cookbook Recipes", "all_recipes.html");
            DivEnd();
        }

        private void AddRecipeGroup(string groupName, IRecipe[] recipes)
        {
            DivStart("categorySection");
            AddGroupHeader(groupName);
            AddRecipeLinks(recipes);
            AddRecipeThumbnails(recipes);
            DivEnd();
        }

        private void AddGroupHeader(string title, string manualUrl = null)
        {
            string url = manualUrl ?? Sanitize(title) + ExtPage;
            DivStart("categoryTitle");
            SB.AppendLine($"<a href='{url}'>{title}</a>");
            DivEnd();
        }

        private void AddRecipeLinks(IRecipe[] recipes)
        {
            //SB.AppendLine("<ul>");
            foreach (IRecipe recipe in recipes)
            {
                string categoryPageName = $"{Sanitize(recipe.Category)}{ExtPage}";
                string recipeUrl = $"{categoryPageName}#{recipe.ID}";
                SB.AppendLine($"<p><a href='{recipeUrl}'>{recipe.Title}</a> - {recipe.Description}</p>");
            }
            //SB.AppendLine("</ul>");
        }

        private void AddRecipeThumbnails(IRecipe[] recipes)
        {
            foreach (IRecipe recipe in recipes)
            {
                string categoryPageName = $"{Sanitize(recipe.Category)}{ExtPage}";
                string recipeUrl = $"{categoryPageName}#{recipe.ID}";
                string imageUrl = $"source/{recipe.ID}{ExtImage}";
                SB.AppendLine($"<a href='{recipeUrl}'><img src='{imageUrl}' style='height: 150px;'/></a>");
            }
        }
    }
}
