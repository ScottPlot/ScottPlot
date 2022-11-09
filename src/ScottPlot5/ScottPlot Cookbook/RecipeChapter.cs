namespace ScottPlotCookbook;

/// <summary>
/// Chatpers are the top level of organization for cookbook recipes.
/// A chapter contains multiple recipe pages (<see cref="RecipePage"/>).
/// </summary>
internal class RecipeChapter
{
    internal string Title { get; set; } = string.Empty;
    internal string Description { get; set; } = string.Empty;
}
