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
    class API
    {
        internal static Dictionary<string, string> xmlDoc = new Dictionary<string, string>();

        [OneTimeSetUp]
        public static void LoadXmlDoc()
        {
            string xmlPath = @"../../../../../src/ScottPlot/ScottPlot.xml";
            string xmlText = File.ReadAllText(xmlPath);
            using XmlReader xmlReader = XmlReader.Create(new StringReader(xmlText));
            while (xmlReader.Read())
                if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "member")
                    xmlDoc[xmlReader["name"]] = xmlReader.ReadInnerXml();
        }

        [Test]
        public void Test_API_Plot()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("# ScottPlot.Plot API");

            Type plotType = typeof(ScottPlot.Plot);
            MethodInfo[] methods = plotType.GetMethods().OrderBy(x => x.Name).ToArray();

            foreach (var method in methods)
            {
                // don't show private methods
                if (method.Attributes == MethodAttributes.Private)
                    continue;

                // don't show obsolete methods
                object[] attributes = method.GetCustomAttributes(false);
                if (attributes.OfType<ObsoleteAttribute>().Any())
                    continue;

                string[] paramTypes = method.GetParameters().Select(x => x.ParameterType.ToString()).ToArray();
                for (int i = 0; i < paramTypes.Length; i++)
                {
                    paramTypes[i] = paramTypes[i].Replace("`1[", "{").Replace("]", "}");
                    paramTypes[i] = paramTypes[i].Replace("`2[", "{").Replace("]", "}");
                    paramTypes[i] = paramTypes[i].Replace("[}", "[]");
                    paramTypes[i] = paramTypes[i].Replace("[,}", "[0:,0:]");
                    paramTypes[i] = paramTypes[i].Replace("[,}", "[0:,0:]");
                }
                string csParams = string.Join(",", paramTypes);
                string key = $"M:ScottPlot.Plot.{method.Name}({csParams})";

                // Special replacements required for certain methods with complex XML keys.
                // These were determined by manually inspecting the XML file.
                if (method.Name == "AddSignalConst")
                    key = "M:ScottPlot.Plot.AddSignalConst``1(``0[],System.Double,System.Nullable{System.Drawing.Color},System.String)";
                if (method.Name == "AddSignalXYConst")
                    key = "M:ScottPlot.Plot.AddSignalXYConst``2(``0[],``1[],System.Nullable{System.Drawing.Color},System.String)";

                string summary;
                if (xmlDoc.ContainsKey(key))
                {
                    string xmlText = "<member>" + xmlDoc[key] + "</member>";
                    var xd = XDocument.Parse(xmlText);
                    summary = xd.Element("member").Element("summary").Value.Trim();
                    summary = $"_{summary}_";
                }
                else
                {
                    summary = "ERROR: XML DOCS NOT FOUND!";
                    summary = $"***{summary}***";
                    Console.WriteLine($"ERROR: XML not found for {method.Name}");
                }

                sb.AppendLine($"## {method.Name}()");
                sb.AppendLine(summary);

                // string conversions for standard types
                string returns = method.ReturnType.ToString();
                if (!returns.Contains("Drawing."))
                    returns = returns.Replace("System.", "");

                sb.AppendLine($"* Returns `{returns}`");
                sb.AppendLine($"* Parameters");
                foreach (var param in method.GetParameters())
                {
                    string pName = param.Name;
                    string pType = param.ParameterType.ToString();
                    if (!pType.Contains("Drawing."))
                        pType = pType.Replace("System.", "");

                    // string conversions for standard types
                    if (pType == "Double[]") pType = "double[]";
                    if (pType == "Double") pType = "double";
                    if (pType == "Int32") pType = "int";
                    if (pType == "Single") pType = "float";
                    if (pType == "String") pType = "string";
                    if (pType == "String[]") pType = "string[]";

                    if (pType.StartsWith("Nullable"))
                        pType = pType.Substring(11, pType.Length - 12) + "?";

                    if (pType.StartsWith("System.Nullable"))
                        pType = pType.Substring(18, pType.Length - 19) + "?";

                    sb.AppendLine($"  * `{pType}` **`{pName}`**");
                }
            }

            string filePath = "../../../../../dev/API-Plot.md";
            filePath = System.IO.Path.GetFullPath(filePath);
            System.IO.File.WriteAllText(filePath, sb.ToString());
            Console.WriteLine(filePath);
        }
    }
}
