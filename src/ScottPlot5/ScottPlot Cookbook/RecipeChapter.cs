namespace ScottPlotCookbook;

/// <summary>
/// Chatpers are the top level of organization for cookbook recipes.
/// A chapter contains multiple recipe pages (<see cref="RecipePage"/>).
/// </summary>
public class RecipeChapter
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
