namespace ScottPlotCookbook.Info;

public class ChapterInfo
{
    public string Name { get; }
    public List<PageInfo> Pages { get; }

    internal ChapterInfo(Chapter chapter)
    {
        Name = chapter.ToString();
        Pages = Cookbook.GetPagesInChapter(chapter)
            .Select(x => new PageInfo(x))
            .ToList();
    }
}
