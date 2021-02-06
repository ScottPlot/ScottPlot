using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Website.Pages
{
    public class ApiIndexPage : Page
    {
        public ApiIndexPage()
        {
            Title = $"ScottPlot {Plot.Version} API";
            Description = $"API documentation for ScottPlot {Plot.Version}";

            Add("<div class='display-5 my-3'><a href='./' style='color: black;'>" +
                $"ScottPlot {Plot.Version} API</a></div>");

            AddVersionWarning();
        }
    }
}
