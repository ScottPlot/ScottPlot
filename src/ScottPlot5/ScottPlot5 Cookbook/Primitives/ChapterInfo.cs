using System.Linq;

namespace ScottPlotCookbook;

// TODO: collapse this datatype with Chapter

/// <summary>
/// A cookbook chapter is a group of pages.
/// Example: Plot Types (contains signal, scatter, etc., each with their own collection of recipes)
/// </summary>
public class ChapterInfo
{
    public string Name { get; }
    public List<CategoryInfo> Categories { get; }

    internal ChapterInfo(Chapter chapter)
    {
        Name = chapter.ToString();
        Categories = Cookbook.GetPagesInChapter(chapter)
            .Select(x => new CategoryInfo(x))
            .ToList();
    }

    internal ChapterInfo(Chapter chapter, List<RecipeInfo> recipes)
    {
        Name = chapter.ToString();
        Categories = Cookbook.GetPagesInChapter(chapter)
            .Select(x => new CategoryInfo(x, recipes))
            .ToList();
    }
}
