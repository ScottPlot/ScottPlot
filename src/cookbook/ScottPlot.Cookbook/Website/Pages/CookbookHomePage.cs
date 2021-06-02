using ScottPlot.Cookbook.XmlDocumentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ScottPlot.Cookbook.Website.Pages
{
    public class CookbookHomePage : Page
    {
        public CookbookHomePage()
        {
            Title = $"ScottPlot {Plot.Version} Cookbook";
            Description = $"Code examples and API documentation for ScottPlot {Plot.Version}";

            Add("<div class='display-5 my-3'><a href='./' style='color: black;'>" +
                $"ScottPlot {Plot.Version} Cookbook</a></div>");

            AddVersionWarning("cookbook page");

            AddHeading("API Reference", 2);
            Add("* [Plot API](#the-plot-module)");
            Add("* [Methods for Creating Plottables](#methods-for-creating-plottables)");
            Add("* [Plottable Types](#plottable-types)");

            AddRecipeCards();

            AddPlotApiSummary();
        }

        private void AddPlotApiSummary()
        {
            string xmlPath = "../../../../../src/ScottPlot/ScottPlot.xml";
            XDocument xmlDoc = XDocument.Load(xmlPath);

            AddHeading("The Plot Module", 1);
            Add("The Plot module is the primary way to interct with ScottPlot. " +
                "It has helper methods to make it easy to create and add Plottable objects. " +
                "Use the Render() method to render the plot as an image.");

            //AddHeading("Fields", 3);
            //foreach (var field in Locate.GetPlotFields().Select(x => new DocumentedField(x, xmlDoc)))
            //Add(OneLineInfo(field, "plot/"));

            AddHeading("Properties", 2);
            foreach (var property in Locate.GetPlotProperties().Select(x => new DocumentedProperty(x, xmlDoc)))
                Add(OneLineInfo(property, "api/plot/"));

            AddHeading("Methods", 2);
            foreach (var method in Locate.GetPlotMethodsNoAdd().Select(x => new DocumentedMethod(x, xmlDoc)))
                Add(OneLineInfo(method, "api/plot/"));

            AddHeading("Methods for Creating Plottables", 2);
            foreach (var method in Locate.GetPlotMethodsOnlyAdd().Select(x => new DocumentedMethod(x, xmlDoc)))
                Add(OneLineInfo(method, "api/plot/"));

            AddHeading("Plottable Types", 1);
            foreach (Type plottableType in Locate.GetPlottableTypes())
            {
                var classInfo = new DocumentedClass(plottableType, xmlDoc);
                Add(OneLineInfo(classInfo, $"api/plottable/{Sanitize(plottableType.Name)}/"));
            }
        }

        private void AddRecipeCards()
        {
            string[] categoryNames = Locate.GetCategoriesInDisplayOrder();
            string[] plottableCategories = categoryNames.Where(x => x.StartsWith("Plottable:")).ToArray();
            string[] nonPlottableCategories = categoryNames.Where(x => !x.StartsWith("Plottable:")).ToArray();

            AddHeading("Cookbook Recipes", 2);

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
