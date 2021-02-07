using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace ScottPlot.Cookbook.XmlDocumentation
{
    public class Member
    {
        public readonly string Name;
        public readonly string Summary;

        public Member(XElement element)
        {
            Name = GetName(element);
            Summary = GetSummary(element);
        }

        private string GetName(XElement element) => element.Attribute("name").Value.Trim();

        private string GetSummary(XElement element)
        {
            // collapse summary into a single line
            string xmlSummary = element.Element("summary").Value;
            xmlSummary = xmlSummary.Replace("\n", " ").Replace("\r", " ");
            while (xmlSummary.Contains("  "))
                xmlSummary = xmlSummary.Replace("  ", " ").Trim();
            return xmlSummary;
        }
    }
}
