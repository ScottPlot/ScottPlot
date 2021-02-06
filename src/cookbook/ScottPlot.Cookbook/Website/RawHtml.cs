using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Website
{
    class RawHtml : IPageElement
    {
        public string Markdown { get; private set; } = "";

        public string Html { get; private set; }

        public RawHtml(string html) => Html = html;
    }
}
