namespace ScottPlotCookbook;

public abstract class Recipe
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract void Execute(Plot plot);
}
