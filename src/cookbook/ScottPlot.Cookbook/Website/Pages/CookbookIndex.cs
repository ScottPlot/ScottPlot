using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Website.Pages
{
    public class CookbookIndex : Page
    {
        public CookbookIndex()
        {
            Title = "All Recipes";
            Description = "List of all cookbook recipes for this version of ScottPlot";

            string[] categoryNames = Locate.GetCategoriesInDisplayOrder();
            string[] plottableCategories = categoryNames.Where(x => x.StartsWith("Plottable:")).ToArray();
            string[] nonPlottableCategories = categoryNames.Where(x => !x.StartsWith("Plottable:")).ToArray();

            AddHeading("Recipes", 2);

            AddHtml("<div class='m-1 pt-2'>");
            AddParagraph($"<strong>Concepts:</strong> " + string.Join(", ",
                nonPlottableCategories.Select(x => $"<a href='#{Sanitize(x)}'>{x}</a>")));

            AddParagraph($"<strong>Plottables:</strong> " + string.Join(", ",
                plottableCategories.Select(x => $"<a href='#{Sanitize(x)}'>{x.Split(':')[1]}</a>")));
            AddHtml("</div>");

            AddSpacer();

            foreach (string categoryName in categoryNames)
            {
                AddHeading(categoryName, 2);
                AddHtml("<div class='row'>");
                foreach (IRecipe recipe in Locate.GetRecipes(categoryName))
                    AddRecipeCard(recipe);
                AddHtml("</div>");
                AddSpacer();
            }
        }
    }
}
