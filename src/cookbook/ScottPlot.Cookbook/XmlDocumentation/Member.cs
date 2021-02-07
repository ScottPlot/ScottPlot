using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace ScottPlot.Cookbook.XmlDocumentation
{
    public class Member
    {
        public string XmlName { get; private set; }
        public string Summary { get; private set; }

        public string ShortName { get; private set; }

        public string FullName { get; private set; }

        public string ReturnType { get; private set; }

        /// <summary>
        /// Names in the order they appear in the argument list
        /// </summary>
        public string[] ParamNames { get; private set; }

        public readonly Dictionary<string, string> ParamNotes = new Dictionary<string, string>();

        public readonly Dictionary<string, string> ParamTypes = new Dictionary<string, string>();

        public void Update(XElement element)
        {
            XmlName = element.Attribute("name").Value;
            Summary = element.Element("summary").Value;
            Summary = Summary.Replace("\n", " ").Replace("\r", " ");
            while (Summary.Contains("  "))
                Summary = Summary.Replace("  ", " ").Trim();

            ParamNotes.Clear();
            foreach (var e in element.Elements("param"))
                ParamNotes.Add(e.Attribute("name").Value, e.Value);
        }

        public void Update(MethodInfo info)
        {
            ShortName = info.Name;
            FullName = info.DeclaringType.Name + "." + info.Name;
            ReturnType = PrettyType(info.ReturnType);
            ParamNames = info.GetParameters().Select(x => x.Name).ToArray();

            ParamTypes.Clear();
            foreach (ParameterInfo pi in info.GetParameters())
                ParamTypes.Add(pi.Name, PrettyType(pi.ParameterType));
        }

        public string GetSignature()
        {
            var paramStrings = ParamNames.Select(name => $"{ParamTypes[name]} {name}");
            return $"{ReturnType} {FullName}(" + string.Join(", ", paramStrings) + ")";
        }

        private string PrettyType(Type type)
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
