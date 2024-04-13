namespace ScottPlotCookbook;

// a recipe does not know about its category, chapter, or source code
public interface IRecipe
{
    public string Name { get; }
    public string Description { get; }
    public void Execute(Plot plot);
}
