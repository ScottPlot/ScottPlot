namespace ScottPlot.Cookbook
{
    public readonly struct RecipeSource
    {
        public readonly string ID;
        public readonly string Category;
        public readonly string CategoryFolder;
        public readonly string Title;
        public readonly string Description;
        public readonly string Code;
        public readonly string AnchorID;

        public RecipeSource(IRecipe recipe, string source)
        {
            ID = recipe.ID;
            Category = recipe.Category.Name;
            CategoryFolder = recipe.Category.Folder;
            Title = recipe.Title;
            Description = recipe.Description;
            Code = source;
            AnchorID = GetAnchor(recipe.Title);
        }

        public RecipeSource(string id, string category, string categoryFolder, string title, string description, string code)
        {
            ID = id;
            Category = category;
            CategoryFolder = categoryFolder;
            Title = title;
            Description = description;
            Code = code;
            AnchorID = GetAnchor(title);
        }

        private static string GetAnchor(string s)
        {
            return s.ToLower()
                .Replace(" ", "-")
                .Replace("_", "-")
                .Replace(" ", "-");
        }
    }
}
