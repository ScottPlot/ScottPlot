using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Website.Pages
{
    public class RecipeCategoryPage : Page
    {
        public RecipeCategoryPage(string categoryName)
        {
            Title = categoryName;
            Description = $"Example plots and source code demonstrating {categoryName}";

            IRecipe[] recipes = Locate.GetRecipes(categoryName);
            if (recipes.Length == 0)
                throw new InvalidOperationException($"no recipes found of category {categoryName}");

            AddHtml($"<div class='display-5 my-3'>{categoryName}</div>");

            foreach (IRecipe recipe in recipes)
                AddRecipe(recipe);
        }

        private void AddRecipe(IRecipe recipe)
        {
            AddHeading(recipe.Title, 2);
            AddParagraph(recipe.Description);
            AddCodeBlock(Locate.RecipeSourceCode(recipe), "cs");
            AddImage($"images/{recipe.ID}.png", center: true);
        }
    }
}
