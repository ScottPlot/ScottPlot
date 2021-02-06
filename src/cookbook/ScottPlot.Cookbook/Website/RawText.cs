using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Website
{
    class RawText : IPageElement
    {
        public string Markdown { get; private set; }

        public RawText(string str)
        {
            Markdown = str;
        }
    }
}
