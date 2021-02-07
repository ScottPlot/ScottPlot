using ScottPlot.Cookbook.XmlDocumentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;


namespace ScottPlot.Cookbook.Website.Pages
{
    public class PlotApiPage : Page
    {
        public PlotApiPage()
        {
            Title = $"Plot API - ScottPlot {Plot.Version} Cookbook";
            Description = $"API documentation for ScottPlot {Plot.Version}";

            XDocument xmlDoc = GetXmlDoc();

            Add($"<div class='display-5 mt-3'>" +
                $"<a href='../../' style='color: black;'>ScottPlot {Plot.Version} Cookbook</a>" +
                $"</div>" +
                $"<div class='fs-1 fw-light mb-4'>" +
                $"<a href='./' style='color: black;'>Plot API</a>" +
                "</div>");

            AddVersionWarning("API documentation page");

            AddHeading("The Plot Module", 1);

            AddHeading("Methods", 2);
            foreach (var method in GetPlotMethods(xmlDoc))
                AddDetailedInfo(method);

            AddHeading("Properties", 2);
            foreach (var property in GetPlotProperties(xmlDoc))
                AddDetailedInfo(property);
        }

        private static DocumentedProperty[] GetPlotProperties(XDocument xmlDoc) =>
            typeof(Plot).GetProperties()
                        .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                        .Select(x => new DocumentedProperty(x, xmlDoc))
                        .ToArray();

        private static DocumentedMethod[] GetPlotMethods(XDocument xmlDoc) =>
            typeof(Plot).GetMethods()
                        .Where(x => !x.Name.StartsWith("get_"))
                        .Where(x => !x.Name.StartsWith("set_"))
                        .Where(x => !x.Name.StartsWith("add_"))
                        .Where(x => !x.Name.StartsWith("remove_"))
                        .Where(x => x.IsPublic)
                        .Where(x => x.DeclaringType.FullName != null)
                        .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                        .Where(x => x.Name != "GetType")
                        .Where(x => x.Name != "ToString")
                        .Where(x => x.Name != "Equals")
                        .Where(x => x.Name != "GetHashCode")
                        .Select(x => new DocumentedMethod(x, xmlDoc))
                        .ToArray();
    }
}
