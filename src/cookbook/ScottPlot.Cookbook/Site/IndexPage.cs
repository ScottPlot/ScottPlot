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

            AddDiv("<a href='all_cookbook_recipes.html' style='font-size: 200%;'>View all Cookbook Recipes</a>");

        }

        private void AddRecipeGroup(string groupName, IRecipe[] recipes)
        {
            // section header 
            string titleID = Sanitize(groupName);
            SB.AppendLine($"<div style='margin-top: 20px; font-size: 200%;'><a href='{titleID}{ExtPage}'>{groupName}</a></div>");

            // bullet list of links to recipes
            SB.AppendLine("<ul>");
            foreach (IRecipe recipe in recipes)
            {
                string pageUrl = $"{titleID}{ExtPage}#{recipe.ID}";
                SB.AppendLine($"<li><a href='{pageUrl}'>{recipe.Title}</a> - {recipe.Description}</li>");
            }
            SB.AppendLine("</ul>");

            // thumbnails
            foreach (IRecipe recipe in recipes)
            {
                string pageUrl = $"{titleID}{ExtPage}#{recipe.ID}";
                string imageUrl = $"source/{recipe.ID}{ExtImage}";
                SB.AppendLine($"<a href='{pageUrl}'><img src='{imageUrl}' style='height: 150px;'/></a>");
            }
        }
    }
}
