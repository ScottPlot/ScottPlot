namespace ScottPlot.Cookbook
{
    public struct RecipeSource
    {
        public readonly string ID;
        public readonly string Category;
        public readonly string Title;
        public readonly string Description;
        public readonly string Code;

        public RecipeSource(IRecipe recipe, string source)
        {
            ID = recipe.ID;
            Category = recipe.Category;
            Title = recipe.Title;
            Description = recipe.Description;
            Code = source;
        }
    }
}
