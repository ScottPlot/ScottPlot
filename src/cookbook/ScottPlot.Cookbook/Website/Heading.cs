namespace ScottPlot.Cookbook.Website
{
    class Heading : IPageElement
    {
        public string Markdown { get; private set; }

        public Heading(string text, int level)
        {
            Markdown = new string('#', level) + " " + text;
        }
    }
}
