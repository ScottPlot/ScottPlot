using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace ScottPlot.Cookbook.XmlDocumentation
{
    public class DocumentedMethod : DocumentedItem
    {
        public readonly string Signature;
        public readonly string ReturnType;
        public readonly DocumentedParameter[] Parameters;

        public DocumentedMethod(MethodInfo info, XDocument doc)
        {
            XmlName = GetXmlName(info);
            Name = info.IsGenericMethod ? info.Name.Split('`')[0] + "<T>" : info.Name;
            FullName = info.DeclaringType.Name + "." + Name;
            ReturnType = PrettyType(info.ReturnType);

            // LOAD PARAMETER NOTES FROM XML
            var paramNotesByName = new Dictionary<string, string>();
            var memberXml = GetMemberXml(doc, XmlName);
            if (memberXml != null)
            {
                HasXmlDocumentation = true;
                Summary = CleanSummary(memberXml.Element("summary")?.Value);
                foreach (var p in memberXml.Elements("param"))
                    paramNotesByName.Add(p.Attribute("name").Value, p.Value);
            }

            // ANALYZE REFLECTED PARAMETERS
            Parameters = info
                .GetParameters()
                .Select(p => new DocumentedParameter()
                {
                    Name = p.Name,
                    Type = PrettyType(p.ParameterType),
                    Summary = paramNotesByName.ContainsKey(p.Name) ? paramNotesByName[p.Name] : "",
                })
                .ToArray();

            // GENERATE SIGNATURE BASED ON PARAMETERS
            Signature = $"{ReturnType} {FullName}(" + string.Join(", ", Parameters.Select(p => $"{p.Type} {p.Name}")) + ")";
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
