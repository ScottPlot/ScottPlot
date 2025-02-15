namespace ScottPlotCookbook;

/// <summary>
/// Recipes are collected in categories, and categories are grouped into chapters.
/// The main purpose of this struct is to indicate whether a recipe demonstrates a plot type or not.
/// The demo app doesn't organize categories into chapters, but the website does.
/// Also having a Chapter defined instead of relying on namespace alone is necessary because
/// category source code files are parsed to generate the cookbook JSON file and assign categories to chapters.
/// </summary>
public enum Chapter
{
    General,
    PlotTypes,
}

public static class ChapterExtensions
{
    public static string PrettyName(this Chapter chapter)
    {
        return chapter switch
        {
            Chapter.General => "General",
            Chapter.PlotTypes => "Plot Types",
            _ => throw new NotImplementedException(),
        };
    }
}
