namespace ScottPlotCookbook;

public interface IRecipe
{
    public string Title { get; }
    public string Description { get; }
    public void Recipe(Plot plot);
}
