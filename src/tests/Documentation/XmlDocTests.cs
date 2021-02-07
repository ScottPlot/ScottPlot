using NUnit.Framework;
using System;
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
            foreach (MethodInfo info in ScottPlot.Cookbook.Locate.GetPlotMethods())
            {
                string summary = Doc.Lookup(info).Summary;
                Console.WriteLine($"{info}: {summary}");
                Assert.IsNotNull(summary);
            }
        }

        [Test]
        public void Text_PlotProperties_AllHaveSummaries()
        {
            foreach (PropertyInfo info in ScottPlot.Cookbook.Locate.GetPlotProperties())
            {
                string summary = Doc.Lookup(info).Summary;
                Console.WriteLine($"{info}: {summary}");
                Assert.IsNotNull(summary);
            }
        }

        [Test]
        public void Text_PlotFields_AllHaveSummaries()
        {
            foreach (FieldInfo info in ScottPlot.Cookbook.Locate.GetPlotFields())
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
