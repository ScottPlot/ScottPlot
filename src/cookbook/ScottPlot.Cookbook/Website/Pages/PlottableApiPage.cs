using ScottPlot.Cookbook.XmlDocumentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ScottPlot.Cookbook.Website.Pages
{
    public class PlottableApiPage : Page
    {
        public PlottableApiPage(Type plottableType)
        {
            Title = $"{plottableType.Name} - ScottPlot {Plot.Version} API";
            Description = $"API documentation for ScottPlot {Plot.Version}";

            var xmlDoc = GetXmlDoc();
            var classDoc = new DocumentedClass(plottableType, xmlDoc);

            Add($"<div class='display-5 mt-3'>" +
                $"<a href='../../../' style='color: black;'>ScottPlot {Plot.Version} Cookbook</a>" +
                $"</div>" +
                $"<div class='fs-1 fw-light mb-4'>" +
                $"<a href='./' style='color: black;'>{plottableType.Name}</a>" +
                "</div>");

            AddVersionWarning("API documentation page");

            AddHeading($"Plot Type: {classDoc.Name}", 1);
            Add($"**Summary:** {classDoc.Summary}");
            Add($"**Full name:** `{classDoc.FullName}`");

            Add($"**{plottableType.Name} Public API:**");
            Add("![](TOC)");

            AddHeading("Fields", 2);
            foreach (var field in plottableType.GetFields().Select(x => new DocumentedField(x, xmlDoc)))
                AddDetailedInfo(field);

            AddHeading("Properties", 2);
            foreach (var property in plottableType.GetProperties().Select(x => new DocumentedProperty(x, xmlDoc)))
                AddDetailedInfo(property);

            AddHeading("Methods", 2);
            foreach (var method in plottableType.GetMethods()
                                                .Where(x => !x.Name.StartsWith("get_"))
                                                .Where(x => !x.Name.StartsWith("set_"))
                                                .Where(x => !x.Name.StartsWith("add_"))
                                                .Where(x => !x.Name.StartsWith("remove_"))
                                                .Where(x => x.IsPublic)
                                                .Where(x => x.DeclaringType.FullName != null)
                                                .Where(x => x.Name != "GetType")
                                                .Where(x => x.Name != "ToString")
                                                .Where(x => x.Name != "Equals")
                                                .Where(x => x.Name != "GetHashCode")
                                                .Select(x => new DocumentedMethod(x, xmlDoc)))
                AddDetailedInfo(method);
        }
    }
}
