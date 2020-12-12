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
        public IndexPage(string cookbookSiteFolder, string resourceFolder) : base(cookbookSiteFolder, resourceFolder) { }

        public void AddLinksToRecipes()
        {
            foreach (var stuff in Locate.GetCategorizedRecipes())
                AddRecipeGroup(stuff.Key, stuff.Value);

            DivStart("categorySection");
            AddGroupHeader("Miscellaneous");
            AddHTML("<div style='margin-left: 1em;'><a href='all_recipes.html'>Single page with all recipes</a></div>");
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

        private void AddGroupHeader(string title)
        {
            string anchor = Sanitize(title);
            SB.AppendLine($"<h2 style='margin-top: 1em;'>");
            SB.AppendLine($"<a href='#{anchor}' name='{anchor}' style='color: black; text-decoration: none; font-weight: 600;'>{title}</a>");
            SB.AppendLine($"</h2>");
        }

        private void AddRecipeLinks(IRecipe[] recipes)
        {
            SB.Append("<div style='margin-left: 1em;'>");
            foreach (IRecipe recipe in recipes)
            {
                string categoryPageName = $"{Sanitize(recipe.Category)}{ExtPage}";
                string recipeUrl = $"{categoryPageName}#{recipe.ID}".ToLower();
                SB.AppendLine($"<p><a href='{recipeUrl}' style='font-weight: 600;'>{recipe.Title}</a> - {recipe.Description}</p>");
            }
            SB.Append("</div>");
        }

        private void AddRecipeThumbnails(IRecipe[] recipes)
        {
            foreach (IRecipe recipe in recipes)
            {
                string categoryPageName = $"{Sanitize(recipe.Category)}{ExtPage}";
                string recipeUrl = $"{categoryPageName}#{recipe.ID}".ToLower();
                string imageUrl = $"source/{recipe.ID}{ExtThumb}".ToLower();
                SB.AppendLine($"<a href='{recipeUrl}'><img src='{imageUrl}' style='padding: 10px;'/></a>");
            }
        }
    }
}
