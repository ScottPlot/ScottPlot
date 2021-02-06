using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Website.Pages
{
    public class CookbookIndex : MarkdownPage
    {
        public CookbookIndex()
        {
            Title = $"ScottPlot {Plot.Version} Cookbook";
            Description = $"Code examples and API documentation for ScottPlot {Plot.Version}";

            Add("<div class='display-5 my-3'><a href='./' style='color: black;'>" +
                $"ScottPlot {Plot.Version} Cookbook</a></div>");

            AddVersionWarning();

            AddRecipeCards();
        }

        private void AddRecipeCards()
        {
            string[] categoryNames = Locate.GetCategoriesInDisplayOrder();
            string[] plottableCategories = categoryNames.Where(x => x.StartsWith("Plottable:")).ToArray();
            string[] nonPlottableCategories = categoryNames.Where(x => !x.StartsWith("Plottable:")).ToArray();

            AddHeading("Recipes", 2);

            Add($"<strong>Concepts:</strong> " + string.Join(", ",
                nonPlottableCategories.Select(x => $"<a href='#{Sanitize(x)}'>{x}</a>")));

            Add($"<strong>Plottables:</strong> " + string.Join(", ",
                plottableCategories.Select(x => $"<a href='#{Sanitize(x)}'>{x.Split(':')[1]}</a>")));

            AddSpacer();

            foreach (string categoryName in categoryNames)
            {
                AddHeading(categoryName, 2);
                Add("<div class='row'>");
                foreach (IRecipe recipe in Locate.GetRecipes(categoryName))
                    AddRecipeCard(recipe);
                Add("</div>");
                AddSpacer();
            }
        }
    }
}
