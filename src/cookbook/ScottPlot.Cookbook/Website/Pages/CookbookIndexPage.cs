using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Website.Pages
{
    public class CookbookIndexPage : Page
    {
        public CookbookIndexPage()
        {
            Title = $"ScottPlot {Plot.Version} Cookbook";
            Description = $"Code examples and API documentation for ScottPlot {Plot.Version}";

            Add("<div class='display-5 my-3'><a href='./' style='color: black;'>" +
                $"ScottPlot {Plot.Version} Cookbook</a></div>");

            AddVersionWarning();

            AddRecipeCards();

            AddApiLinks();
        }

        private void AddApiLinks()
        {
            string xmlPath = "../../../../../src/ScottPlot/ScottPlot.xml";
            var doc = new XmlDocumentation.XmlDoc(xmlPath);

            AddHeading("Plot Module API", 2);
            foreach (var info in Locate.GetPlotMethods().Where(x => !x.Name.StartsWith("Add")))
            {
                var d = doc.Lookup(info);
                d.Update(info);
                Add($"<a href='api/#{Sanitize(d.ShortName)}'><strong>{d.ShortName}()</strong></a> - {d.Summary}");
            }

            AddHeading("Methods to Create Plottables", 2);
            foreach (var info in Locate.GetPlotMethods().Where(x => x.Name.StartsWith("Add")))
            {
                var d = doc.Lookup(info);
                d.Update(info);
                Add($"<a href='api/#{Sanitize(d.ShortName)}'><strong>{d.ShortName}()</a></strong> - {d.Summary}");
            }

            AddHeading("Plottable Types", 2);
            foreach (Type plottableType in Locate.GetPlottableTypes())
            {
                var d = doc.Lookup(plottableType);
                Add($"<a href='api/#{Sanitize(plottableType.Name)}'><strong>{plottableType.Name}</strong></a> - {d.Summary}");
            }
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
