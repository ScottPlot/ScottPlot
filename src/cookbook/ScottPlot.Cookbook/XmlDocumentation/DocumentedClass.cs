using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ScottPlot.Cookbook.XmlDocumentation
{
    public class DocumentedClass : DocumentedItem
    {
        public DocumentedClass(Type type, XDocument doc)
        {
            XmlName = GetXmlName(type);
            Name = type.IsGenericType ? type.Name.Split('`')[0] + "<T>" : type.Name;
            FullName = type.IsGenericType ? type.FullName.Split('`')[0] + "<T>" : type.FullName;
            var memberXml = GetMemberXml(doc, XmlName);
            if (memberXml != null)
            {
                HasXmlDocumentation = true;
                Summary = CleanSummary(memberXml.Element("summary")?.Value);
            }
        }

        private string GetXmlName(Type type)
        {
            string fullName = type.FullName;
            return "T:" + fullName;
        }
    }
}
