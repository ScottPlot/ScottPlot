namespace ScottPlot.Cookbook.Website
{
    class Image : IPageElement
    {
        public string Markdown { get; private set; }

        public Image(string url, bool linkToItself, bool center)
        {
            Markdown = linkToItself ? $"[![]({url})]({url})" : $"![]({url})";

            if (center)
                Markdown = $"<div class='text-center'>\n\n{Markdown}\n\n</div>";
        }
    }
}
