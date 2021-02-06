namespace ScottPlot.Cookbook.Website
{
    class Paragraph : IPageElement
    {
        public string Markdown { get; private set; }

        public Paragraph(string text)
        {
            Markdown = text;
        }
    }
}
