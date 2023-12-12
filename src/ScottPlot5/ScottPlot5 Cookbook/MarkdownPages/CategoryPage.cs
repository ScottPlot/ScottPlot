namespace ScottPlotCookbook.MarkdownPages;

internal class CategoryPage : PageBase
{
    private readonly IEnumerable<RecipeInfo> Recipes;
    private readonly CategoryInfo Category;

    internal CategoryPage(IEnumerable<RecipeInfo> allRecipes, CategoryInfo category)
    {
        Recipes = allRecipes.Where(x => x.Category == category.Name);
        if (!allRecipes.Any())
            throw new InvalidOperationException("no recipes match category");

        Category = category;
    }

    public void Generate()
    {
        SB.AppendLine($"# {Category.Name}");
        SB.AppendLine();

        AddVersionInformation();

        foreach (RecipeInfo recipe in Recipes)
        {
            SB.AppendLine();
            SB.AppendLine($"## {recipe.Name}");
            SB.AppendLine();
            SB.AppendLine(recipe.Description);
            SB.AppendLine();
            SB.AppendLine($"[![]({recipe.ImageFilename})]({recipe.ImageFilename})");
            SB.AppendLine();
            SB.AppendLine("```cs");
            SB.AppendLine(recipe.SourceCode);
            SB.AppendLine("```");
            SB.AppendLine();
        }

        string breadcrumbName1 = "ScottPlot 5.0 Cookbook";
        string breadcrumbUrl1 = "/cookbook/5.0/";

        string breadcrumbName2 = Category.Name;
        string breadcrumbUrl2 = $"/cookbook/5.0/{Category.FolderName}/";

        string[] fm =
        {
            $"BreadcrumbNames: [\"{breadcrumbName1}\", \"{breadcrumbName2}\"]",
            $"BreadcrumbUrls: [\"{breadcrumbUrl1}\", \"{breadcrumbUrl2}\"]",
        };

        string outputFolder = Path.Combine(Cookbook.OutputFolder, "category");

        Save(outputFolder,
            title: Category.Name + " - ScottPlot 5.0 Cookbook",
            description: Category.Description,
            filename: $"{Category.FolderName}.md",
            url: $"/cookbook/5.0/{Category.FolderName}/",
            fm);
    }
}
