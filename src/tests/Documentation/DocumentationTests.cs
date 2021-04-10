using NUnit.Framework;
using System;
using ScottPlot.Cookbook.XmlDocumentation;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace ScottPlotTests.Documentation
{
    class DocumentationTests
    {
        XDocument LoadedXmlDocument;

        [OneTimeSetUp]
        public void Setup()
        {
            string xmlPath = "../../../../../src/ScottPlot/ScottPlot.xml";
            if (!File.Exists(xmlPath))
                throw new InvalidOperationException("XML documentation file not found. Rebuild solution.");
            xmlPath = Path.GetFullPath(xmlPath);
            LoadedXmlDocument = XDocument.Load(xmlPath);
        }

        [Test]
        public void Text_PlotMethods_AllHaveSummaries()
        {
            foreach (MethodInfo info in ScottPlot.Cookbook.Locate.GetPlotMethods())
            {
                var method = new DocumentedMethod(info, LoadedXmlDocument);
                Assert.IsTrue(method.HasXmlDocumentation);
                Assert.IsNotNull(method.Summary);
            }
        }

        [Test]
        public void Text_PlotProperties_AllHaveSummaries()
        {
            foreach (PropertyInfo info in ScottPlot.Cookbook.Locate.GetPlotProperties())
            {
                var p = new DocumentedProperty(info, LoadedXmlDocument);
                Assert.IsNotNull(p.Summary);
            }
        }

        [Test]
        public void Text_PlotFields_AllHaveSummaries()
        {
            foreach (FieldInfo info in ScottPlot.Cookbook.Locate.GetPlotFields())
            {
                var f = new DocumentedField(info, LoadedXmlDocument);
                Assert.IsNotNull(f.Summary);
            }
        }

        [Test]
        public void Text_Plottables_HaveSummaries()
        {
            foreach (Type plottableType in ScottPlot.Cookbook.Locate.GetPlottableTypes())
            {
                var t = new DocumentedClass(plottableType, LoadedXmlDocument);
                Assert.IsNotNull(t.Summary);
            }
        }

        [Test]
        public void Text_Plottables_MethodsHaveSummaries()
        {
            foreach (Type plottableType in ScottPlot.Cookbook.Locate.GetPlottableTypes())
            {
                foreach (MethodInfo info in ScottPlot.Cookbook.Locate.GetNotablePlottableMethods(plottableType))
                {
                    // skip abstract methods
                    if (info.DeclaringType.IsAbstract)
                        continue;

                    // skip inherited methods
                    if (info.DeclaringType.FullName is null)
                        continue;

                    var m = new DocumentedMethod(info, LoadedXmlDocument);
                    Assert.IsNotNull(m.Summary);
                }
            }
        }

        [Test]
        public void Text_Datagen_MethodsHaveSummaries()
        {
            foreach (MethodInfo info in typeof(ScottPlot.DataGen).GetMethods())
            {
                // skip abstract methods
                if (info.DeclaringType.IsAbstract && info.DeclaringType != typeof(ScottPlot.DataGen)) // Static classes count as abstract since they cannot be instantiated.....
                    continue;

                // skip inherited methods
                if (info.DeclaringType.FullName is null || info.DeclaringType.FullName == "System.Object")
                    continue;

                var m = new DocumentedMethod(info, LoadedXmlDocument);
                Assert.IsNotNull(m.Summary, $"{m.FullName} has no summary.");
            }
        }
    }
}
