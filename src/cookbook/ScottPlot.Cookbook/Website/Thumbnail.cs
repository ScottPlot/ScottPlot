namespace ScottPlot.Cookbook.Website
{
    class Thumbnail : IPageElement
    {
        public string Markdown { get; private set; }

        public Thumbnail(string url, string thumbUrl, bool center)
        {
            Markdown = $"[![]({thumbUrl})]({url})";

            if (center)
                Markdown = $"<div class='text-center'>\n\n{Markdown}\n\n</div>";
        }
    }
}
