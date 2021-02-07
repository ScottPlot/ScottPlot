using ScottPlot.Cookbook.XmlDocumentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace ScottPlot.Cookbook.Website.Pages
{
    public class ApiIndexPage : Page
    {
        public ApiIndexPage()
        {
            Title = $"ScottPlot {Plot.Version} API";
            Description = $"API documentation for ScottPlot {Plot.Version}";

            string xmlPath = "../../../../../src/ScottPlot/ScottPlot.xml";
            XDocument loadedXmlDocument = XDocument.Load(xmlPath);

            Add("<div class='display-5 my-3'><a href='../' style='color: black;'>" +
                $"ScottPlot {Plot.Version}</a> API</div>");

            AddVersionWarning();

            AddHeading("The Plot Module", 1);

            foreach (FieldInfo info in Locate.GetPlotFields())
                Add(OneLineInfo(info, loadedXmlDocument, "api/plot/"));
            foreach (PropertyInfo info in Locate.GetPlotProperties())
                Add(OneLineInfo(info, loadedXmlDocument, "api/plot/"));
            foreach (MethodInfo info in Locate.GetPlotMethods().Where(x => !x.Name.StartsWith("Add")))
                Add(OneLineInfo(info, loadedXmlDocument, "api/plot/"));

            AddHeading("Plot Methods to Add Plottables", 1);
            foreach (MethodInfo info in Locate.GetPlotMethods().Where(x => x.Name.StartsWith("Add")))
                Add(OneLineInfo(info, loadedXmlDocument, "api/plot/"));

            AddHeading("Plottable Types", 1);
            foreach (Type plottableType in Locate.GetPlottableTypes())
            {
                var classInfo = new DocumentedClass(plottableType, loadedXmlDocument);
                AddHeading(classInfo.Name, 2);
                Add(classInfo.Summary);

                // don't try to reflect complex class types
                if (plottableType.IsAbstract)
                    continue;

                string baseUrl = $"api/plottable/{Sanitize(classInfo.Name)}/";

                foreach (MethodInfo info in Locate.GetNotablePlottableMethods(plottableType))
                {
                    // don't try to figure out where documentation for inherited methods is
                    if (info.DeclaringType.FullName is null)
                        continue;

                    Add(OneLineInfo(info, loadedXmlDocument, baseUrl));
                }

                foreach (FieldInfo info in Locate.GetNotablePlottableFields(plottableType))
                {
                    // don't try to figure out where documentation for inherited methods is
                    if (info.DeclaringType.FullName is null)
                        continue;

                    Add(OneLineInfo(info, loadedXmlDocument, baseUrl));
                }

                foreach (PropertyInfo info in Locate.GetNotablePlottableProperties(plottableType))
                {
                    // don't try to figure out where documentation for inherited methods is
                    if (info.DeclaringType.FullName is null)
                        continue;

                    Add(OneLineInfo(info, loadedXmlDocument, baseUrl));
                }
            }
        }

        private string OneLineInfo(MethodInfo info, XDocument loadedXmlDocument, string baseUrl)
        {
            var method = new DocumentedMethod(info, loadedXmlDocument);
            string url = Sanitize(method.Name);
            StringBuilder sb = new StringBuilder();
            sb.Append($"<a href='{baseUrl}#{url}'><strong>{method.Name}()</strong></a>");
            if (!string.IsNullOrWhiteSpace(method.Summary))
                sb.Append(" - " + method.Summary);
            return sb.ToString();
        }

        private string OneLineInfo(FieldInfo info, XDocument loadedXmlDocument, string baseUrl)
        {
            var field = new DocumentedField(info, loadedXmlDocument);
            string url = Sanitize(field.Name);
            StringBuilder sb = new StringBuilder();
            sb.Append($"<a href='{baseUrl}#{url}'><strong>{field.Name}</strong></a>");
            if (!string.IsNullOrWhiteSpace(field.Summary))
                sb.Append(" - " + field.Summary);
            return sb.ToString();
        }

        private string OneLineInfo(PropertyInfo info, XDocument loadedXmlDocument, string baseUrl)
        {
            var property = new DocumentedProperty(info, loadedXmlDocument);
            string url = Sanitize(property.Name);
            StringBuilder sb = new StringBuilder();
            sb.Append($"<a href='{baseUrl}#{url}'><strong>{property.Name}</strong></a>");
            if (!string.IsNullOrWhiteSpace(property.Summary))
                sb.Append(" - " + property.Summary);
            return sb.ToString();
        }
    }
}
