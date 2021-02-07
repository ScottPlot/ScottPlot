using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace ScottPlot.Cookbook.XmlDocumentation
{
    public class DocumentedProperty : DocumentedItem
    {
        public DocumentedProperty(PropertyInfo info, XDocument doc)
        {
            XmlName = GetXmlName(info);
            Name = info.Name;
            FullName = info.DeclaringType.FullName + "." + info.Name;

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
