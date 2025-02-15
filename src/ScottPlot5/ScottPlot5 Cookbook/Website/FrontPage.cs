namespace ScottPlotCookbook.Website;

internal class FrontPage : PageBase
{
    readonly JsonCookbookInfo CB;

    public FrontPage(JsonCookbookInfo cb)
    {
        CB = cb;
    }

    public void Generate(string outputFolder)
    {
        SB.AppendLine($"# ScottPlot 5.0 Cookbook");
        SB.AppendLine();

        // manage chapters manually as the simplest way to enforce ordered chapters
        string[] chapters = ["General", "Plot Types"];
        if (chapters.Length != CB.Chapters.Length)
            throw new InvalidOperationException("Chapter mismatch. Edit this area manually.");
        foreach (string chapter in chapters)
            if (!CB.Chapters.Contains(chapter))
                throw new InvalidOperationException("Chapter mismatch. Edit this area manually.");

        // table of contents
        foreach (string chapter in chapters)
        {
            SB.AppendLine($"<div class='mt-3 fs-4'><strong>{chapter}</strong></div>");

            SB.AppendLine("<ul>");
            foreach (var category in CB.Categories.Where(x => x.Chapter == chapter))
            {
                SB.AppendLine($"<li><a href='{category.Url}'>{category.Name}</a> - {category.Description}</li>");
            }
            if (chapter == "General")
            {
                SB.AppendLine("<hr>");
                SB.AppendLine($"<li><a href='colors'>Colors</a> - A list of all the colors included with ScottPlot</li>");
                SB.AppendLine($"<li><a href='palettes'>Color Palettes</a> - Collections of colors which can be used to represent categorical data</li>");
                SB.AppendLine($"<li><a href='colormaps'>Colormaps</a> - Color gradients available to represent continuous data</li>");
            }
            SB.AppendLine("</ul>");
        }

        // individual recipes
        foreach (string chapter in CB.Chapters)
        {
            SB.AppendLine($"<h1>{chapter}</h1>");

            foreach (var category in CB.Categories.Where(x => x.Chapter == chapter))
            {
                AddCategory(category);
            }
        }

        string breadcrumbName1 = "ScottPlot 5.0 Cookbook";
        string breadcrumbUrl1 = "/cookbook/5.0/";

        string[] fm =
        {
            $"BreadcrumbNames: [\"{breadcrumbName1}\"]",
            $"BreadcrumbUrls: [\"{breadcrumbUrl1}\"]",
        };

        Save(outputFolder,
            title: "ScottPlot 5.0 Cookbook",
            description: "Example plots shown next to the code used to create them",
            filename: "index_.md",
            url: "/cookbook/5.0/",
            frontmatter: fm);
    }

    private void AddCategory(JsonCookbookInfo.JsonCategoryInfo category)
    {
        //IEnumerable<WebRecipe> recipes = RecipesByCategory[category];
        //string categoryUrl = recipes.First().CategoryUrl;

        SB.AppendLine($"<h2 class=''><a href='{category.Url}' class='text-dark'>{category.Name}</a></h2>");
        SB.AppendLine($"<div>{category.Description}</div>");

        foreach (JsonCookbookInfo.JsonRecipeInfo recipe in CB.Recipes.Where(x => x.Category == category.Name))
        {
            AddRecipe(recipe);
        }
    }

    private void AddRecipe(JsonCookbookInfo.JsonRecipeInfo recipe)
    {
        SB.AppendLine("<div class='row my-4'>");

        SB.AppendLine("<div class='col'>");
        SB.AppendLine($"<a href='{recipe.RecipeUrl}'><img class='img-fluid' src='{recipe.ImageUrl}' /></a>");
        SB.AppendLine("</div>");

        SB.AppendLine("<div class='col'>");
        SB.AppendLine($"<div><a href='{recipe.RecipeUrl}'><b>{recipe.Name}</b></a></div>");
        SB.AppendLine($"<div>{recipe.Description}</div>");
        SB.AppendLine("</div>");

        SB.AppendLine("</div>");
    }
}
