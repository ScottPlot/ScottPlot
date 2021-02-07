using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Website.Pages
{
    public class CookbookCategoryPage : Page
    {
        public CookbookCategoryPage(string categoryName)
        {
            Title = categoryName;
            Description = $"Example plots and source code demonstrating {categoryName}";

            IRecipe[] recipes = Locate.GetRecipes(categoryName);
            if (recipes.Length == 0)
                throw new InvalidOperationException($"no recipes found of category {categoryName}");

            Add($"<div class='display-5 mt-3'>" +
                $"<a href='../../' style='color: black;'>ScottPlot {Plot.Version} Cookbook</a>" +
                $"</div>" +
                $"<div class='fs-1 fw-light mb-4'>" +
                $"<a href='./' style='color: black;'>{categoryName}</a>" +
                "</div>");

            AddVersionWarning("cookbook page");

            foreach (IRecipe recipe in recipes)
                AddRecipe(recipe);
        }

        private void AddRecipe(IRecipe recipe)
        {
            AddHeading(recipe.Title, 2);
            Add(recipe.Description);
            AddCodeBlock(Locate.RecipeSourceCode(recipe), "cs");
            AddImage($"../../images/{Sanitize(recipe.ID)}.png", center: true);
        }
    }
}
