using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Website.Pages
{
    public class ApiIndexPage : Page
    {
        public ApiIndexPage()
        {
            Title = $"ScottPlot {Plot.Version} API";
            Description = $"API documentation for ScottPlot {Plot.Version}";

            string xmlPath = "../../../../../src/ScottPlot/ScottPlot.xml";
            var doc = new XmlDocumentation.XmlDoc(xmlPath);

            Add("<div class='display-5 my-3'><a href='../' style='color: black;'>" +
                $"ScottPlot {Plot.Version}</a> API</div>");

            AddVersionWarning();

            AddHeading("The Plot Module", 1);
            foreach (var info in Locate.GetPlotMethods().Where(x => !x.Name.StartsWith("Add")))
            {
                var d = doc.Lookup(info);
                d.Update(info);
                AddMethodInfo(d, 2);
            }

            AddHeading("Methods to Create Plottables", 1);
            foreach (var info in Locate.GetPlotMethods().Where(x => x.Name.StartsWith("Add")))
            {
                var d = doc.Lookup(info);
                d.Update(info);
                AddMethodInfo(d, 2);
            }

            AddHeading("Plottable Types", 1);
            foreach (Type plottableType in Locate.GetPlottableTypes())
            {
                AddHeading(plottableType.Name, 2);

                var d1 = doc.Lookup(plottableType);
                Add(d1.Summary);

                foreach (var mi in Locate.GetNotablePlottableMethods(plottableType))
                {
                    // don't reflect on inherited methods
                    if (mi.DeclaringType.FullName is null)
                        continue;

                    Console.WriteLine($"{plottableType} {mi}");
                    Console.WriteLine(string.Join(", ", mi.GetParameters().Select(x => x.Name)));
                    var d2 = doc.Lookup(mi);
                    d2.Update(mi);
                    AddMethodInfo(d2, 3);
                }
            }
        }
    }
}
