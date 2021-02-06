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
            Title = $"ScottPlot {ScottPlotVersion} Cookbook";
            Description = $"Code examples and API documentation for ScottPlot {ScottPlotVersion}";

            // The HTML template has large text indicating this page is the cookbook.
            // Since Markdown does not use a template, this information has to be added manually.
            //AddMarkdown($"# ScottPlot {ScottPlotVersion} Cookbook", htmlToo: false);
            AddHtml("<div class='display-5 my-3'><a href='./' style='color: black;'>" +
                $"ScottPlot {ScottPlotVersion} Cookbook</a></div>", markdownToo: true);

            // The HTML template has warnings that documentation is version specific.
            // Since Markdown does not use a template, this information has to be added manually.
            AddVersionWarning();

            AddRecipeCards();
        }

        private void AddRecipeCards()
        {
            string[] categoryNames = Locate.GetCategoriesInDisplayOrder();
            string[] plottableCategories = categoryNames.Where(x => x.StartsWith("Plottable:")).ToArray();
            string[] nonPlottableCategories = categoryNames.Where(x => !x.StartsWith("Plottable:")).ToArray();

            AddHeading("Recipes", 2);

            AddParagraph($"<strong>Concepts:</strong> " + string.Join(", ",
                nonPlottableCategories.Select(x => $"<a href='#{Sanitize(x)}'>{x}</a>")));

            AddParagraph($"<strong>Plottables:</strong> " + string.Join(", ",
                plottableCategories.Select(x => $"<a href='#{Sanitize(x)}'>{x.Split(':')[1]}</a>")));

            AddSpacer();

            foreach (string categoryName in categoryNames)
            {
                AddHeading(categoryName, 2);
                AddHtml("<div class='row'>", true);
                foreach (IRecipe recipe in Locate.GetRecipes(categoryName))
                    AddRecipeCard(recipe);
                AddHtml("</div>", true);
                AddSpacer();
            }
        }
    }
}
