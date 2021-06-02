namespace ScottPlot.Cookbook.Website
{
    interface IPageElement
    {
        public string Markdown { get; }

        // Enable this to support dual Markdown and HTML exporting
        //public string HTML { get; }
    }
}
