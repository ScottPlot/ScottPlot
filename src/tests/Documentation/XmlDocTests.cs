using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Documentation
{
    class XmlDocTests
    {
        private ScottPlot.Cookbook.XmlDoc XD;
        string XmlPath = "../../../../../src/ScottPlot/ScottPlot.xml";

        [OneTimeSetUp]
        public void LoadDocs() => XD = new ScottPlot.Cookbook.XmlDoc(XmlPath);

        [Test]
        public void Test_XML_Load()
        {
            Console.WriteLine($"Found {XD.Methods.Count} documented methods.");

            var plotMethods = XD.Methods.Where(x => x.Name.Contains("ScottPlot.Plot."));
            Console.WriteLine($"Found {plotMethods.Count()} documented Plot methods.");

            foreach (var dm in plotMethods)
            {
                Console.WriteLine($"");
                Console.WriteLine($"{dm.Name}");
                Console.WriteLine($"{dm.Summary}");
            }
        }

        private string PlotMethodXmlName(string methodName, int parameterCount)
        {
            MethodInfo[] infos = typeof(ScottPlot.Plot).GetMethods()
                                                       .Where(x => x.Name == methodName)
                                                       .Where(x => x.GetParameters().Length == parameterCount)
                                                       .ToArray();
            return infos.Length switch
            {
                0 => throw new InvalidOperationException($"no method named '{methodName}' has {parameterCount} parameters"),
                1 => ScottPlot.Cookbook.XmlDoc.XmlName(infos[0]),
                _ => throw new InvalidOperationException($"multiple methods named '{methodName}' with {parameterCount} parameters found")
            };
        }

        [Test]
        public void Test_XmlName_WithoutParameters() =>
            Assert.AreEqual(
                expected: "M:ScottPlot.Plot.Clear",
                actual: PlotMethodXmlName("Clear", 0)
            );

        [Test]
        public void Test_XmlName_WithParameters() =>
            Assert.AreEqual(
                expected: "M:ScottPlot.Plot.AddImage(System.Drawing.Bitmap,System.Double,System.Double)",
                actual: PlotMethodXmlName("AddImage", 3)
            );

        [Test]
        public void Test_XmlName_WithArray() =>
            Assert.AreEqual(
                expected: "M:ScottPlot.Plot.AddPie(System.Double[],System.Boolean)",
                actual: PlotMethodXmlName("AddPie", 2)
            );

        [Test]
        public void Test_XmlName_WithNullableValueType() =>
            Assert.AreEqual(
                expected: "M:ScottPlot.Plot.AddPoint(System.Double,System.Double,System.Nullable{System.Drawing.Color},System.Single,ScottPlot.MarkerShape)",
                actual: PlotMethodXmlName("AddPoint", 5)
            );

        [Test]
        public void Test_XmlName_WithGenerics() =>
            Assert.AreEqual(
                expected: "M:ScottPlot.Plot.AddSignalConst``1(``0[],System.Double,System.Nullable{System.Drawing.Color},System.String)",
                actual: PlotMethodXmlName("AddSignalConst", 4)
            );

        [Test]
        public void Test_XmlName_WithMultipleGenerics() =>
            Assert.AreEqual(
                expected: "M:ScottPlot.Plot.AddSignalXYConst``2(``0[],``1[],System.Nullable{System.Drawing.Color},System.String)",
                actual: PlotMethodXmlName("AddSignalXYConst", 4)
            );
    }
}
