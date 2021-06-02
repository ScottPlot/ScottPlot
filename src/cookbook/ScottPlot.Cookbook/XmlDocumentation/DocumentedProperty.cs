using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace ScottPlot.Cookbook.XmlDocumentation
{
    public class DocumentedProperty : DocumentedItem
    {
        public string Type { get; private set; }
        public bool CanRead { get; private set; }
        public bool CanWrite { get; private set; }

        public DocumentedProperty(PropertyInfo info, XDocument doc)
        {
            XmlName = GetXmlName(info);
            Name = info.Name;
            string baseType = info.DeclaringType.ToString();
            if (baseType.Contains("`"))
                baseType = baseType.Split('`')[0] + "<T>";
            FullName = baseType + "." + info.Name;
            Type = PrettyType(info.PropertyType);
            CanRead = info.CanRead;
            CanWrite = info.CanWrite;

            var memberXml = GetMemberXml(doc, XmlName);
            if (memberXml != null)
            {
                HasXmlDocumentation = true;
                Summary = CleanSummary(memberXml.Element("summary")?.Value);
            }
        }

        private string GetXmlName(PropertyInfo info)
        {
            string fullName = info.DeclaringType.FullName + "." + info.Name;
            return "P:" + fullName;
        }
    }
}
