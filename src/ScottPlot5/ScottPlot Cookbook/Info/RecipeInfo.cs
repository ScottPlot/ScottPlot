namespace ScottPlotCookbook.Info;

public class RecipeInfo
{
    public string Name => Recipe.Name;
    public string Description => Recipe.Description;
    public string SourceCode { get; } = string.Empty;
    public IRecipe Recipe { get; }

    internal RecipeInfo(IRecipe recipe)
    {
        Recipe = recipe;
    }
}
