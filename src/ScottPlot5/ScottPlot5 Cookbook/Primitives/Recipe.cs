namespace ScottPlotCookbook;

// a recipe does not know about its category or source code (that's what RecipeInfo is for)
public abstract class Recipe
{
    public abstract string Name { get; }
    public string ImageFilename => UrlTools.UrlSafe(Name) + ".png";
    public abstract string Description { get; }
    public abstract void Execute(Plot plot);
}
