namespace ScottPlot.Cookbook
{
    public interface IRecipe
    {
        /// <summary>
        /// Filename of the example image and source code (must be unique)
        /// </summary>
        string ID { get; }

        /// <summary>
        /// The recipes with the same category appear on the same cookbook page
        /// </summary>
        string Category { get; }

        /// <summary>
        /// Heading for the recipe (as short as possible)
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Description has no word limit
        /// </summary>
        string Description { get; }

        void ExecuteRecipe(Plot plt);
    }
}
