namespace ScottPlot.Cookbook.Website
{
    class CodeBlock : IPageElement
    {
        public string Markdown { get; private set; }
        public CodeBlock(string code, string language)
        {
            Markdown = $"\n```{language}\n{code}\n```\n";
        }
    }
}
