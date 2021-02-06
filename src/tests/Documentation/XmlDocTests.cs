using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.Cookbook.XmlDocumentation;

namespace ScottPlotTests.Documentation
{
    class XmlDocTests
    {
        private XmlDoc Docs;

        [OneTimeSetUp]
        public void LoadDocs()
        {
            string XmlPath = "../../../../../src/ScottPlot/ScottPlot.xml";
            Docs = new XmlDoc(XmlPath);
        }
    }
}