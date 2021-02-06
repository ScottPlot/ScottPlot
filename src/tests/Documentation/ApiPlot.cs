using NUnit.Framework;
using ScottPlot.Cookbook;
using ScottPlot.Plottable;
using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace ScottPlotTests.Documentation
{
    /// <summary>
    /// Tests to ensure the Plot API is properly documented
    /// </summary>
    class ApiPlot
    {
        string XmlDocPath = "../../../../../src/ScottPlot/ScottPlot.xml";
        XDocument Doc;
        XmlDocOld XmlDoc;
        MethodInfo[] PlotMethods;
        FieldInfo[] PlotFields;
        Type[] PlottableTypes;

        [OneTimeSetUp]
        public void LoadDocs()
        {
            Doc = XDocument.Load(XmlDocPath);
            XmlDoc = new XmlDocOld(XmlDocPath);

            PlotMethods = typeof(ScottPlot.Plot)
                          .GetMethods()
                          .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                          .Where(x => x.Name != "GetType")
                          .ToArray();

            PlotFields = typeof(ScottPlot.Plot)
                         .GetFields()
                         .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                         .ToArray();

            PlottableTypes = ScottPlot.Cookbook.Locate.GetPlottableTypes();
        }

        private XElement GetXmlFor(FieldInfo fi) =>
            GetXmlFor(ScottPlot.Cookbook.XmlDocOld.XmlName(fi));

        private XElement GetXmlFor(Type type) =>
            GetXmlFor(ScottPlot.Cookbook.XmlDocOld.XmlName(type));

        private XElement GetXmlFor(MethodInfo mi) =>
            GetXmlFor(ScottPlot.Cookbook.XmlDocOld.XmlName(mi));

        private XElement GetXmlFor(string memberName)
        {
            foreach (XElement member in Doc.Element("doc").Element("members").Elements())
            {
                if (member.Attribute("name").Value == memberName)
                    return member;
            }
            throw new InvalidOperationException($"member '{memberName}' not found in XML doc");
        }

        /// <summary>
        /// Throw if the XML does not have a <summary> (it can be empty)
        /// </summary>
        private static void AssertDocumentedSummary(MethodInfo mi, XElement xml)
        {
            string name = mi.DeclaringType.Name + "." + mi.Name;
            if (xml.Element("summary") is null)
                throw new InvalidOperationException($"XML documentation for '{name}' lacks <summary>");
        }

        /// <summary>
        /// Throw if the XML does not have a <returns> (it can be empty)
        /// </summary>
        private static void AssertDocumentedReturns(MethodInfo mi, XElement xml)
        {
            // don't require return documentation for void methods
            if (mi.ReturnType == typeof(void))
                return;

            // don't require return documentation for auto-properties
            if (mi.Name.StartsWith("get_"))
                return;

            string name = mi.DeclaringType.Name + "." + mi.Name;
            if (xml.Element("returns") is null)
                throw new InvalidOperationException($"XML documentation for '{name}' lacks <returns>");
        }

        /// <summary>
        /// Throw if the XML does not have a <param> for every parameter (they can be empty)
        /// </summary>
        private static void AssertDocumentedParams(MethodInfo mi, XElement xml)
        {
            string name = mi.DeclaringType.Name + "." + mi.Name;
            var documentedParams = xml.Elements("param");
            foreach (ParameterInfo pi in mi.GetParameters())
            {
                if (documentedParams.Where(x => x.Attribute("name").Value == pi.Name).Any() == false)
                    throw new InvalidOperationException($"XML documentation for '{name}' lacks <param> for '{pi.Name}'");
            }
        }

        [Test]
        public void Test_Plot_Methods_HaveXmlDocumentation()
        {
            foreach (MethodInfo mi in PlotMethods)
            {
                if (mi.Name.StartsWith("Add"))
                    continue;

                XElement xml = GetXmlFor(mi);
                AssertDocumentedSummary(mi, xml);
                AssertDocumentedReturns(mi, xml);
                AssertDocumentedParams(mi, xml);
            }
        }

        [Test]
        public void Test_Plot_Fields_HaveXmlDocumentation()
        {
            // note: at the time this test was created, the Plot module has no public fields
            foreach (FieldInfo fi in PlotFields)
            {
                XElement xml = GetXmlFor(fi);
                Console.WriteLine(fi);
            }
        }

        [Test]
        public void Test_Plottable_Classes_HaveXmlDocumentation()
        {
            foreach (Type plottableType in PlottableTypes)
            {
                string classSummary = XmlDoc.GetSummary(plottableType);
                Assert.IsNotNull(classSummary, $"Plottable {plottableType} requires class-level <summary> documentation");
            }
        }

        [Test]
        public void Test_Plottable_Methods_HaveXmlDocumentation()
        {
            foreach (Type plottableType in PlottableTypes)
            {
                foreach (var mi in Locate.GetNotablePlottableMethods(plottableType))
                {
                    // TODO: figure out how to do XML name lookups for inherited generics!

                    if (mi.DeclaringType.FullName != null &&
                        mi.DeclaringType.FullName.Contains("SignalPlot"))
                        continue;

                    if (mi.ReflectedType.FullName != null &&
                        mi.ReflectedType.FullName.Contains("SignalPlot"))
                        continue;

                    XElement xml = GetXmlFor(mi);
                    AssertDocumentedSummary(mi, xml);
                }
            }
        }
    }
}
