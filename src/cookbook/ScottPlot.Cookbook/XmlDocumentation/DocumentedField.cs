using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace ScottPlot.Cookbook.XmlDocumentation
{
    public class DocumentedField : DocumentedItem
    {
        public string Type { get; private set; }

        public DocumentedField(FieldInfo info, XDocument doc)
        {
            XmlName = GetXmlName(info);
            Name = info.Name;
            FullName = info.DeclaringType.FullName + "." + info.Name;
            Type = PrettyType(info.FieldType);

            var memberXml = GetMemberXml(doc, XmlName);
            if (memberXml != null)
            {
                HasXmlDocumentation = true;
                Summary = CleanSummary(memberXml.Element("summary")?.Value);
            }
        }

        private string GetXmlName(FieldInfo info)
        {
            string fullName = info.DeclaringType.FullName + "." + info.Name;
            return "F:" + fullName;
        }
    }
}
