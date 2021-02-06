namespace ScottPlot.Cookbook.Website
{
    class Image : IPageElement
    {
        public string Markdown { get; private set; }
        public string Html { get; private set; }

        /// <summary>
        /// A thumbnail image that links to a full-size image
        /// </summary>
        public Image(string url, string thumbUrl, bool center)
        {
            Markdown = $"[![]({thumbUrl})]({url})";
            Html = $"<a href='{url}'><img src='{thumbUrl}' /></a>";

            if (center)
            {
                Markdown = $"<div class='text-center'>\n\n{Markdown}\n\n</div>";
                Html = $"<div class='text-center'>{Html}</div>";
            }
        }

        /// <summary>
        /// A full-size image that optionally links to itself
        /// </summary>
        public Image(string url, bool linkToItself, bool center)
        {
            Markdown = linkToItself ?
                $"[![]({url})]({url})" :
                $"![]({url})";

            Html = linkToItself ?
                $"<a href='{url}'><img src='{url}' class='mw-100' /></a>" :
                $"<img src='{url}' class='mw-100' />";

            if (center)
            {
                Markdown = $"<div class='text-center'>\n\n{Markdown}\n\n</div>";
                Html = $"<div class='text-center'>{Html}</div>";
            }
        }
    }
}
