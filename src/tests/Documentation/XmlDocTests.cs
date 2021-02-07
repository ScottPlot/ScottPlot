using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.Cookbook.XmlDocumentation;
using System.IO;
using System.Reflection;

namespace ScottPlotTests.Documentation
{
    class XmlDocTests
    {
        XmlDoc Doc;

        [OneTimeSetUp]
        public void Setup()
        {
            string xmlPath = "../../../../../src/ScottPlot/ScottPlot.xml";
            if (!File.Exists(xmlPath))
                throw new InvalidOperationException("XML documentation file not found. Rebuild solution.");
            xmlPath = Path.GetFullPath(xmlPath);
            Doc = new XmlDoc(xmlPath);
        }

        [Test]
        public void Text_PlotMethods_AllHaveSummaries()
        {
            MethodInfo[] infos = typeof(ScottPlot.Plot)
                                 .GetMethods()
                                 .Where(x => x.IsPublic)
                                 .Where(x => !x.Name.StartsWith("get_"))
                                 .Where(x => !x.Name.StartsWith("set_"))
                                 .Where(x => x.Name != "GetType")
                                 .Where(x => x.Name != "ToString")
                                 .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                                 .ToArray();

            foreach (MethodInfo info in infos)
            {
                string summary = Doc.Lookup(info).Summary;
                Console.WriteLine($"{info}: {summary}");
                Assert.IsNotNull(summary);
            }
        }

        [Test]
        public void Text_PlotProperties_AllHaveSummaries()
        {
            PropertyInfo[] infos = typeof(ScottPlot.Plot)
                                  .GetProperties()
                                  .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                                  .ToArray();

            foreach (PropertyInfo info in infos)
            {
                string summary = Doc.Lookup(info).Summary;
                Console.WriteLine($"{info}: {summary}");
                Assert.IsNotNull(summary);
            }
        }

        [Test]
        public void Text_PlotFields_AllHaveSummaries()
        {
            FieldInfo[] infos = typeof(ScottPlot.Plot)
                                .GetFields()
                                .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                                .ToArray();

            foreach (FieldInfo info in infos)
            {
                string summary = Doc.Lookup(info).Summary;
                Console.WriteLine($"{info}: {summary}");
                Assert.IsNotNull(summary);
            }
        }

        [Test]
        public void Text_Plottables_HaveSummaries()
        {
            foreach (Type plottableType in ScottPlot.Cookbook.Locate.GetPlottableTypes())
            {
                string summary = Doc.Lookup(plottableType).Summary;
                Console.WriteLine($"{plottableType}: {summary}");
                Assert.IsNotNull(summary);
            }
        }

        [Test]
        public void Text_Plottables_MethodsHaveSummaries()
        {
            foreach (Type plottableType in ScottPlot.Cookbook.Locate.GetPlottableTypes())
            {
                foreach (MethodInfo info in ScottPlot.Cookbook.Locate.GetNotablePlottableMethods(plottableType))
                {
                    // skip inherited methods
                    if (info.DeclaringType.FullName is null)
                        continue;

                    string summary = Doc.Lookup(info).Summary;
                    Assert.IsNotNull(summary);
                }
            }
        }
    }
}
