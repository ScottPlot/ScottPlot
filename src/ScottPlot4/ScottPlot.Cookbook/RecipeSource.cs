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
        public string AnchorID => GetAnchor(Title);
        public string CategoryUrl => $"/cookbook/4.1/category/{CategoryFolder}";
        public string AnchorUrl => $"{CategoryUrl}#{AnchorID}";
        public string Url => $"/cookbook/4.1/recipes/{ID.ToLower()}/";
        public string ImageUrl => $"/cookbook/4.1/images/{ID.ToLower()}.png";

        public RecipeSource(IRecipe recipe, string source)
        {
            ID = recipe.ID;
            Category = recipe.Category.Name;
            CategoryFolder = recipe.Category.Folder;
            Title = recipe.Title;
            Description = recipe.Description;
            Code = source;
        }

        public RecipeSource(string id, string category, string categoryFolder, string title, string description, string code)
        {
            ID = id;
            Category = category;
            CategoryFolder = categoryFolder;
            Title = title;
            Description = description;
            Code = code;
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
