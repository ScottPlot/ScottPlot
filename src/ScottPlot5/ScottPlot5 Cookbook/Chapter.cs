namespace ScottPlotCookbook;

/// <summary>
/// Recipes are collected in categories, and categories are grouped into chapters.
/// The main purpose of this struct is to indicate whether a recipe demonstrates a plot type or not.
/// </summary>
public enum Chapter
{
    General,
    PlotTypes,
    Misc,
}

public static class ChapterExtensions
{
    public static string PrettyName(this Chapter chapter)
    {
        return chapter switch
        {
            Chapter.General => "General",
            Chapter.PlotTypes => "Plot Types",
            Chapter.Misc => "Miscellaneous",
            _ => throw new NotImplementedException(),
        };
    }
}
