using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Website
{
    class ListItem : IPageElement
    {
        public string Markdown { get; private set; }
        public string Html { get; private set; }

        public ListItem(string text)
        {
            Markdown = $"* {text}";
            Html = $"<li>{text}</li>";
        }
    }
}
