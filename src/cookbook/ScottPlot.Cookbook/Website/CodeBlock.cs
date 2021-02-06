namespace ScottPlot.Cookbook.Website
{
    class CodeBlock : IPageElement
    {
        public string Markdown { get; private set; }
        public string Html { get; private set; }
        public CodeBlock(string code, string language)
        {
            Markdown = $"\n```{language}\n{code}\n```\n";
            Html = $"<div><pre class='prettyprint lang-{language} " +
                "border-1 border p-2 rounded bg-light" +
                $"'>{code}</pre></div>";
        }
    }
}
