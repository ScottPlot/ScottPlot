namespace ScottPlotCookbook;

/// <summary>
/// A page of recipes groups similar recipes together and displays them in order.
/// </summary>
public abstract class RecipePage
{
    internal abstract string PageName { get; }
    internal abstract string PageDescription { get; }
    internal abstract RecipeChapter Chapter { get; }
}
