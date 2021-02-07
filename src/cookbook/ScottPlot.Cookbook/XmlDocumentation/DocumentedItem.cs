using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ScottPlot.Cookbook.XmlDocumentation
{
    public abstract class DocumentedItem
    {
        public string XmlName { get; protected set; }
        public bool HasXmlDocumentation { get; protected set; }
        public string Name { get; protected set; }
        public string FullName { get; protected set; }
        public string Summary { get; protected set; }

        protected static XElement GetMemberXml(XDocument doc, string xmlName)
        {
            var members = doc.Element("doc").Element("members").Elements("member")
                .Where(x => x.Attribute("name").Value == xmlName);

            if (members.Count() == 0)
                return null;
            else if (members.Count() == 1)
                return members.First();
            else
                throw new InvalidOperationException($"More than 1 member named '{xmlName}'");
        }

        protected static string CleanSummary(string summary)
        {
            summary = summary.Replace('\n', ' ');
            summary = summary.Replace('\r', ' ');
            while (summary.Contains("  "))
                summary = summary.Replace("  ", " ");
            return summary.Trim();
        }

        protected static string PrettyType(Type type)
        {
            if (type.Name.StartsWith("System.Nullable") || type.Name.StartsWith("Nullable"))
            {
                if (type.IsArray)
                {
                    int dimensions = type.GetArrayRank();
                    if (dimensions == 1)
                        return "?[]";
                    else
                        return "[" + new string(',', dimensions) + "]";
                }
                else
                {
                    return PrettyType(type.GenericTypeArguments[0]) + "?";
                }
            }

            return type.Name.Trim() switch
            {
                "System.Void" => "void",
                "Void" => "void",
                "System.Double" => "double",
                "Double" => "double",
                "System.Boolean" => "bool",
                "Boolean" => "bool",
                "Int32" => "int",
                _ => type.Name.Trim()
            };
        }
    }
}
