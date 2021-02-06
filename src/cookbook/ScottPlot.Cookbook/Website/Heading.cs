namespace ScottPlot.Cookbook.Website
{
    class Heading : IPageElement
    {
        public string Markdown { get; private set; }
        public string Html { get; private set; }

        public Heading(string text, int level)
        {
            string url = Page.Sanitize(text);
            Markdown = new string('#', level) + " " + text;
            Html = $"<h{level}><a href='#{url}' name='{url}' " +
                "style='text-decoration: inherit; color: inherit'" +
                $">{text}</a></h{level}>";
        }
    }
}
