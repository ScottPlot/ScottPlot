namespace ScottPlotTests.Cookbook
{
    public interface IRecipe
    {
        RecipeCategory Category { get; }
        string Section { get; }
        string Title { get; }
        string Description { get; }
        void ExecuteRecipe(ScottPlot.Plot plt);
    }
}
