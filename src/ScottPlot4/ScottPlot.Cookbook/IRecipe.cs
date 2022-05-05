namespace ScottPlot.Cookbook
{
    public interface IRecipe
    {
        /// <summary>
        /// A unique identifier for each recipe. This becomes the filename and URL anchor for this example.
        /// </summary>
        string ID { get; }

        /// <summary>
        /// Every category gets its own page in the cookbook and appears as a top-level node in the demo application.
        /// </summary>
        ICategory Category { get; }

        /// <summary>
        /// The heading for the recipe should be as short as possible.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// The description for the recipe has no size limit. Line breaks are permitted.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The content of your recipe goes here. Assume the plot coming in is a new, clean plot.
        /// </summary>
        void ExecuteRecipe(Plot plt);
    }
}
