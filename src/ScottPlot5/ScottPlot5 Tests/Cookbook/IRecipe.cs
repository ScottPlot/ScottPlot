namespace ScottPlot_Tests.Cookbook;

public interface IRecipe
{
    public string Title { get; }
    public string Description { get; }
    public Category Category { get; }
    public void Recipe(Plot plot);
}
