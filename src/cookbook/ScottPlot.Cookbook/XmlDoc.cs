using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace ScottPlot.Cookbook
{
    public class XmlMethodInfo
    {
        public string Name;
        public string Summary;
    }

    public class XmlDoc
    {
        public readonly List<XmlMethodInfo> Methods = new List<XmlMethodInfo>();

        public XmlDoc(string xmlFilePath)
        {
            // validate input
            xmlFilePath = Path.GetFullPath(xmlFilePath);
            if (!File.Exists(xmlFilePath))
                throw new ArgumentException($"XML documentation file does not exist: {xmlFilePath}");

            // identify XML information by parsing the file 
            XDocument doc = XDocument.Load(xmlFilePath);
            foreach (XElement element in doc.Element("doc").Element("members").Elements())
            {
                string xmlName = element.Attribute("name").Value;
                string xmlSummary = element.Element("summary").Value;
                xmlSummary = xmlSummary.Replace("\n", " ").Replace("\r", " ");
                while (xmlSummary.Contains("  "))
                    xmlSummary = xmlSummary.Replace("  ", " ").Trim();
                Methods.Add(new XmlMethodInfo() { Name = xmlName, Summary = xmlSummary });
            }
        }

        /// <summary>
        /// Return the XML summary of the given method, or NULL if it doesn't exist
        /// </summary>
        public string GetSummary(MethodInfo mi)
        {
            string xmlName = XmlName(mi);
            foreach (var m in Methods)
            {
                if (m.Name == xmlName)
                    return m.Summary;
            }
            return null;
        }

        /// <summary>
        /// Return the member name of a method as it would appear in the XML documentation
        /// </summary>
        public static string XmlName(MethodInfo mi)
        {
            // start with the method name
            string name = "M:" + mi.DeclaringType.FullName + "." + mi.Name;

            // determine XML names for each parameter
            ParameterInfo[] parameters = mi.GetParameters();
            string[] parameterNames = new string[parameters.Length];
            int genericParameterIndex = 0;
            for (int i = 0; i < parameters.Length; i++)
            {
                Type pt = parameters[i].ParameterType;
                parameterNames[i] = pt.FullName;

                // special formatting for generics
                if (parameterNames[i] is null)
                {
                    parameterNames[i] = $"``{genericParameterIndex++}";
                    parameterNames[i] += pt.IsArray ? "[]" : "";
                }

                // special formatting for nullable generic types
                if (parameterNames[i].StartsWith("System.Nullable"))
                    parameterNames[i] = "System.Nullable{" + pt.GetGenericArguments()[0] + "}";
            }

            // add XML parameters to the method name
            if (parameters.Length > 0)
                name += "(" + string.Join(",", parameterNames) + ")";

            // special replacement for generic return types
            if (mi.IsGenericMethod)
                name = name.Replace("(", $"``{genericParameterIndex}(");

            return name;
        }

        /// <summary>
        /// Return common forms of common types
        /// </summary>
        /// <returns></returns>
        public static string PrettyType(Type type)
        {
            string pretty = type.FullName ?? type.Name;

            // special case for nullables
            if (pretty.Contains("Nullable"))
                pretty = type.FullName.Split(',')[0].Split('[').Last() + "?";

            // special case for generics
            if (pretty.Contains("`") && !pretty.Contains("``") && !pretty.Contains("Nullable"))
                pretty = pretty.Split('`')[0] + "<T>";

            // TODO: add more replacements for language shortcuts
            pretty = pretty.Replace("System.", "");
            pretty = pretty switch
            {
                "String" => "string",
                "Double" => "double",
                "Single" => "float",
                "Byte" => "byte",
                "Int32" => "int",
                _ => pretty
            };

            return pretty;
        }

        /// <summary>
        /// Return a pretty formatted function signature
        /// </summary>
        public static string PrettySignature(MethodInfo mi, bool returnType = false)
        {
            string funcName = returnType ? PrettyType(mi.ReturnType) + " " : "";
            funcName = funcName + mi.DeclaringType.Name + "." + mi.Name;
            if (mi.IsGenericMethod)
                funcName += "<T>";
            string ps = string.Join(", ", mi.GetParameters().Select(p => $"{PrettyType(p.ParameterType)} {p.Name}"));
            funcName += $"({ps})";
            return funcName;
        }
    }
}
