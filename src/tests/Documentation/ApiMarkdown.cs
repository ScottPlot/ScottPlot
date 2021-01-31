using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace ScottPlotTests.Documentation
{
    class ApiMarkdown
    {
        private ScottPlot.Cookbook.XmlDoc XD;
        string XmlDocPath = "../../../../../src/ScottPlot/ScottPlot.xml";
        string ApiMarkdownPath = "../../../../../dev/API-Plot.md";

        [OneTimeSetUp]
        public void LoadDocs() => XD = new ScottPlot.Cookbook.XmlDoc(XmlDocPath);

        [Test]
        public void Test_API_Plot()
        {
            MethodInfo[] plotMethods = typeof(ScottPlot.Plot).GetMethods().Where(x => x.IsPublic).OrderBy(x => x.Name).ToArray();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("# ScottPlot.Plot API");
            sb.AppendLine("Virtually all functionality in ScottPlot is achieved by calling methods of the Plot module.");
            sb.AppendLine();
            sb.AppendLine($"This document was generated for `ScottPlot {ScottPlot.Plot.Version}`");

            // add a table at the top
            sb.AppendLine("### Methods to Manipulate the Plot");
            sb.AppendLine("Method | Summary");
            sb.AppendLine("---|---");
            foreach (MethodInfo mi in plotMethods)
            {
                bool isAddPlottableMethod = mi.Name.StartsWith("Add") && mi.Name.Length > 3 && !mi.Name.Contains("Axis");
                if (isAddPlottableMethod)
                    continue;
                bool obsolete = mi.GetCustomAttributes<ObsoleteAttribute>().Any();
                if (obsolete)
                    continue;
                string summary = XD.GetSummary(mi);
                sb.AppendLine($"[**{mi.Name}()**](#{mi.Name}())|{summary}");
            }

            //
            sb.AppendLine("### Shortcuts for Adding Plottables");
            sb.AppendLine("Method | Summary");
            sb.AppendLine("---|---");
            foreach (MethodInfo mi in plotMethods)
            {
                bool isAddPlottableMethod = mi.Name.StartsWith("Add") && mi.Name.Length > 3 && !mi.Name.Contains("Axis");
                if (!isAddPlottableMethod)
                    continue;
                bool obsolete = mi.GetCustomAttributes<ObsoleteAttribute>().Any();
                if (obsolete)
                    continue;
                string summary = XD.GetSummary(mi);
                sb.AppendLine($"[**{mi.Name}()**](#{mi.Name}())|{summary}");
            }

            // add a section for each method
            foreach (MethodInfo mi in plotMethods)
            {
                bool obsolete = mi.GetCustomAttributes<ObsoleteAttribute>().Any();
                if (obsolete)
                    continue;

                sb.AppendLine();
                sb.AppendLine($"## {mi.Name}()");

                //sb.AppendLine($"\n**Signature:** `{ScottPlot.Cookbook.XmlDoc.PrettyMethod(mi)}`");

                string summary = XD.GetSummary(mi);
                if (string.IsNullOrWhiteSpace(summary))
                    sb.AppendLine("\n> **WARNING:** This method does not have XML documentation");
                else
                    sb.AppendLine($"\n**Summary:** {summary}");

                sb.AppendLine($"\n**Parameters:**");
                foreach (var p in mi.GetParameters())
                    sb.AppendLine($"* `{ScottPlot.Cookbook.XmlDoc.PrettyType(p.ParameterType)}` {p.Name}");

                sb.AppendLine($"\n**Returns:**");
                sb.AppendLine($"* `{ScottPlot.Cookbook.XmlDoc.PrettyType(mi.ReturnType)}`");
            }

            File.WriteAllText(ApiMarkdownPath, sb.ToString());
            Console.WriteLine(Path.GetFullPath(ApiMarkdownPath));
        }
    }
}