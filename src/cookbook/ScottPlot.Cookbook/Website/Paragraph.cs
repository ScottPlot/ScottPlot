namespace ScottPlot.Cookbook.Website
{
    class Paragraph : IPageElement
    {
        public string Markdown { get; private set; }
        public string Html { get; private set; }

        public Paragraph(string text)
        {
            Markdown = Page.ReplaceHtmlWithMarkdown(text);
            Html = $"<p>{text}</p>";
        }
    }
}
