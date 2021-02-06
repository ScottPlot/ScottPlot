using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Website
{
    class RawMarkdown : IPageElement
    {
        public string Markdown { get; private set; } = "";

        public string Html { get; private set; }

        public RawMarkdown(string md, bool htmlToo = true)
        {
            Markdown = md;
            Html = htmlToo ? $"<p>{md}</p>" : "";
        }
    }
}
