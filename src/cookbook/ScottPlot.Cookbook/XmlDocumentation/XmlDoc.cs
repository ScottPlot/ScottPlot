using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ScottPlot.Cookbook.XmlDocumentation
{
    public class XmlDoc
    {
        public readonly string AssemblyName;
        public readonly Dictionary<string, Member> XmlMembers = new Dictionary<string, Member>();

        public XmlDoc(string xmlFilePath)
        {
            XDocument doc = XDocument.Load(xmlFilePath);

            AssemblyName = doc.XPathSelectElement("/doc/assembly/name").Value;

            foreach (XElement elem in doc.XPathSelectElement("/doc/members").Elements())
            {
                Member member = new Member(elem);
                string xmlName = member.Name;
                XmlMembers.Add(xmlName, member);
            }
        }

        public Member Lookup(MethodInfo info) => XmlMembers[GetXmlName(info)];

        public Member Lookup(PropertyInfo info) => XmlMembers[GetXmlName(info)];

        public Member Lookup(FieldInfo info) => XmlMembers[GetXmlName(info)];

        public Member Lookup(Type type) => XmlMembers[GetXmlName(type)];

        public static string GetXmlName(PropertyInfo info)
        {
            string fullName = info.DeclaringType.FullName + "." + info.Name;
            return "P:" + fullName;
        }

        public static string GetXmlName(FieldInfo info)
        {
            string fullName = info.DeclaringType.FullName + "." + info.Name;
            return "F:" + fullName;
        }

        public static string GetXmlName(Type type)
        {
            string fullName = type.FullName;
            return "T:" + fullName;
        }

        public static string GetXmlName(MethodInfo info)
        {
            string declaringTypeName = info.DeclaringType.FullName;

            if (declaringTypeName is null)
                throw new NotImplementedException("inherited classes are not supported");

            string xmlName = "M:" + declaringTypeName + "." + info.Name;
            xmlName = string.Join("", xmlName.Split(']').Select(x => x.Split('[')[0]));
            xmlName = xmlName.Replace(",", "");

            if (info.IsGenericMethod)
                xmlName += "``#";

            int genericParameterCount = 0;
            List<string> paramNames = new List<string>();
            foreach (var parameter in info.GetParameters())
            {
                Type paramType = parameter.ParameterType;
                string paramName = GetXmlNameForMethodParameter(paramType);
                if (paramName.Contains("#"))
                    paramName = paramName.Replace("#", (genericParameterCount++).ToString());
                paramNames.Add(paramName);
            }
            xmlName = xmlName.Replace("#", genericParameterCount.ToString());

            if (paramNames.Any())
                xmlName += "(" + string.Join(",", paramNames) + ")";

            // manual tweaks for methods I can't figure out:

            // For some reason Reflection treats T as a double
            if (xmlName.Contains("M:ScottPlot.Plottable.SignalPlotBase`1.Update"))
                xmlName = xmlName.Replace("System.Double", "`0");

            return xmlName;
        }

        private static string GetXmlNameForMethodParameter(Type type)
        {
            string xmlName = type.FullName ?? type.BaseType.FullName;
            bool isNullable = xmlName.StartsWith("System.Nullable");
            Type nullableType = isNullable ? type.GetGenericArguments()[0] : null;

            // special formatting for generics (also Func, Nullable, and ValueTulpe)
            if (type.IsGenericType)
            {
                var genericNames = type.GetGenericArguments().Select(x => GetXmlNameForMethodParameter(x));
                var typeName = type.FullName.Split('`')[0];
                xmlName = typeName + "{" + string.Join(",", genericNames) + "}";
            }

            // special case for generic nullables
            if (type.IsGenericType && isNullable && type.IsArray == false)
                xmlName = "System.Nullable{" + nullableType.FullName + "}";

            // special case for multidimensional arrays
            if (type.IsArray && (type.GetArrayRank() > 1))
            {
                string arrayName = type.FullName.Split('[')[0].Split('`')[0];
                if (isNullable)
                    arrayName += "{" + nullableType.FullName + "}";
                string arrayContents = string.Join(",", Enumerable.Repeat("0:", type.GetArrayRank()));
                xmlName = arrayName + "[" + arrayContents + "]";
            }

            // special case for generic arrays
            if (type.IsArray && type.FullName is null)
                xmlName = "``#[]";

            // special case for value types
            if (xmlName.Contains("System.ValueType"))
                xmlName = "`#";

            return xmlName;
        }
    }
}
