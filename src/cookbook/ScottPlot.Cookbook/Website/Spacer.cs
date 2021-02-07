namespace ScottPlot.Cookbook.Website
{
    class Spacer : IPageElement
    {
        public string Markdown { get; private set; }

        public Spacer()
        {
            Markdown = $"\n\n<hr class='my-4 invisible'>\n\n";
        }
    }
}
