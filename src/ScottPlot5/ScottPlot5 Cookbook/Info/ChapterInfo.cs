namespace ScottPlotCookbook.Info;

/// <summary>
/// A cookbook chapter is a group of pages.
/// Example: Plot Types (contains signal, scatter, etc., each with their own collection of recipes)
/// </summary>
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
