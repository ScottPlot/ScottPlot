namespace ScottPlotCookbook;

public interface IRecipe
{
    public string Name { get; }
    public string Description { get; }
    public void Recipe(Plot plot);
}
