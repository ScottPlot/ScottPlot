using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Website
{
    class VersionWarning : IPageElement
    {
        public string Markdown { get; private set; }
        public VersionWarning(string documentationName)
        {
            Markdown =
            "<div class='alert bg-light border my-4' role='alert'>" +
            $"<strong>⚠️ This {documentationName} is specific to ScottPlot version {Plot.Version}</strong>. " +
            "The API of different versions may vary, and newer versions of ScottPlot may be available. " +
            "Refer to the <a href='https://swharden.com/scottplot/cookbook'><strong>ScottPlot Cookbook Home Page</strong></a> " +
            "for the latest documentation. </div>";
        }
    }
}
