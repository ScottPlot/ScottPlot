using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook;

public static class Website
{
    public static void Generate(string OutputFolderPath, RecipeSource[] Recipes)
    {
        MakeIndexPage(OutputFolderPath, Recipes);
        MakeCategoryPages(OutputFolderPath, Recipes);
        MakeIndividualRecipePages(OutputFolderPath, Recipes);
        MakeColorsPage(OutputFolderPath);
        MakeColormapsPage(OutputFolderPath);
    }

    private static void MakeColormapsPage(string OutputFolderPath)
    {
        Console.WriteLine("Creating Colormaps Page...");

        StringBuilder sb = new();
        sb.AppendLine("# Colormaps");
        sb.AppendLine("Colormaps define a smooth gradient of colors. ");
        sb.AppendLine("Colormaps can be passed into plottable objects that use them (e.g., Colorbar), ");
        sb.AppendLine("or they can be instantiated directly so users can access the colors they produce. ");
        sb.AppendLine("Viridis and Turbo are typically recommended as the best colormaps to use for scientific data.");
        sb.AppendLine();
        sb.AppendLine("{{< code-sp4 >}}");
        sb.AppendLine();
        sb.AppendLine("```cs");
        sb.AppendLine($"var cmap = new ScottPlot.Drawing.Colormaps.Viridis();");
        sb.AppendLine("(byte r, byte g, byte b) = cmap.GetRGB(123);");
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("{{< /code-sp4 >}}");
        sb.AppendLine();

        foreach (var cmap in ScottPlot.Drawing.Colormap.GetColormaps())
        {
            ScottPlot.Plottable.Colorbar cbar = new(cmap);
            System.Drawing.Bitmap bmp = cbar.GetBitmap(1000, 20, vertical: false);

            string imageFilename = "colormap_" + cmap.Name.ToLower() + ".png";
            string imageFolderPath = Path.Combine(OutputFolderPath, "images");
            string imageFilePath = Path.Combine(imageFolderPath, imageFilename);
            bmp.Save(imageFilePath, System.Drawing.Imaging.ImageFormat.Png);

            sb.AppendLine();
            sb.AppendLine($"### {cmap.Name}");
            sb.AppendLine();
            //sb.AppendLine("```cs");
            //sb.AppendLine($"var cmap = new ScottPlot.Drawing.Colormaps.{cmap.Name}();");
            //sb.AppendLine("(byte r, byte g, byte b) = cmap.GetRGB(123);");
            //sb.AppendLine("```");
            //sb.AppendLine();
            sb.AppendLine($"<img class='w-100' height='100' src='../images/{imageFilename}'>");
            sb.AppendLine();
        }

        Template.CreateMarkdownPage(
            mdFilePath: Path.Combine(OutputFolderPath, "colormaps.md"),
            body: sb.ToString(),
            title: "Colormaps - ScottPlot 4.1 Cookbook",
            description: "List of Colormaps used to represent continuous data",
            url: "/cookbook/4.1/colormaps/");
    }

    private static void MakeColorsPage(string OutputFolderPath)
    {
        Console.WriteLine("Creating Colors Page...");

        StringBuilder sb = new();
        sb.AppendLine("# Color Palettes");
        sb.AppendLine("Palettes are collections of colors. ");
        sb.AppendLine("The palette in `Plot.Palette` defines default colors for new objects added to plots. ");
        sb.AppendLine("Users can access palettes directly to get color values for any use. ");
        sb.AppendLine();
        sb.AppendLine("{{< code-sp4 >}}");
        sb.AppendLine();
        sb.AppendLine("```cs");
        sb.AppendLine($"var pal = ScottPlot.Palette.Category10;");
        sb.AppendLine("for (int i = 0; i < pal.Count(); i++)");
        sb.AppendLine("{");
        sb.AppendLine("    var color = pal.GetColor(i);");
        sb.AppendLine("    Console.WriteLine(color);");
        sb.AppendLine("}");
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("{{< /code-sp4 >}}");
        sb.AppendLine();

        foreach (IPalette palette in ScottPlot.Palette.GetPalettes())
        {
            sb.AppendLine();
            sb.AppendLine($"### {palette.Name}");
            sb.AppendLine();
            sb.AppendLine(palette.Description);
            sb.AppendLine();
            sb.AppendLine("{{< code-sp4 >}}");
            sb.AppendLine();
            sb.AppendLine("```cs");
            sb.AppendLine($"var myPalette = new {palette}();");
            sb.AppendLine($"foreach (var color in myPalette)");
            sb.AppendLine($"    Console.WriteLine(color);");
            sb.AppendLine("```");
            sb.AppendLine();
            sb.AppendLine("{{< /code-sp4 >}}");
            sb.AppendLine();

            sb.AppendLine("<div class='d-flex flex-wrap'>");
            for (int i = 0; i < palette.Count(); i++)
            {
                System.Drawing.Color color = palette.GetColor(i);
                string hex = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
                sb.AppendLine($"<div class='px-3 py-2' style='background-color: {hex};'>{hex}</div>");
            }
            sb.AppendLine("</div>");
            sb.AppendLine();
        }

        Template.CreateMarkdownPage(
            mdFilePath: Path.Combine(OutputFolderPath, "colors.md"),
            body: sb.ToString(),
            title: "Colors - ScottPlot 4.1 Cookbook",
            description: "List of Colors from all ScottPlot Palettes",
            url: "/cookbook/4.1/colors/");
    }

    private static void MakeCategoryPages(string OutputFolderPath, RecipeSource[] Recipes)
    {
        Console.WriteLine($"Creating category pages...");

        string categoryFolderPath = Path.Combine(OutputFolderPath, "category");
        if (!Directory.Exists(categoryFolderPath))
            Directory.CreateDirectory(categoryFolderPath);

        List<KeyValuePair<string, IRecipe[]>> recipesByCategory = Locate.GetCategorizedRecipes();

        foreach (KeyValuePair<string, IRecipe[]> pair in recipesByCategory)
        {
            string categoryFolderName = pair.Value.First().Category.Folder;

            string thisCategoryFolderPath = Path.Combine(categoryFolderPath, categoryFolderName);
            if (!Directory.Exists(thisCategoryFolderPath))
                Directory.CreateDirectory(thisCategoryFolderPath);

            MakeCategoryPage(thisCategoryFolderPath, Recipes);
        }
    }

    private static void MakeIndividualRecipePages(string OutputFolderPath, RecipeSource[] Recipes)
    {
        string recipeFolder = Path.Combine(OutputFolderPath, "recipes");
        Directory.CreateDirectory(recipeFolder);

        foreach (RecipeSource recipe in Recipes)
        {
            string bcName1 = "ScottPlot 4.1 Cookbook";
            string bcUrl1 = $"/cookbook/4.1/";

            string bcName2 = recipe.Category;
            string bcUrl2 = recipe.CategoryUrl;

            string bcName3 = recipe.Title;
            string bcUrl3 = recipe.Url;

            string searchUrl = "/cookbook/4.1/search/";

            string[] fm = new string[]
            {
                $"BreadcrumbNames: [\"{bcName1}\", \"{bcName2}\", \"{bcName3}\"]",
                $"BreadcrumbUrls: [\"{bcUrl1}\", \"{bcUrl2}\", \"{bcUrl3}\"]",
                $"SearchUrl: \"{searchUrl}\"",
                $"OgImage: \"{recipe.ImageUrl}\"",
            };

            string saveAs = Path.Combine(recipeFolder, $"{recipe.ID.ToLower()}.md");

            Template.CreateMarkdownPage(
                mdFilePath: saveAs,
                body: GetRecipeMarkdown(recipe),
                title: $"{recipe.Title} - ScottPlot 4.1 Cookbook",
                description: recipe.Description,
                url: recipe.Url,
                frontmatter: fm);
        }
    }

    private static string GetRecipeMarkdown(RecipeSource recipe)
    {
        StringBuilder sb = new();
        sb.AppendLine($"<h2><a id='{recipe.AnchorID}' href='{recipe.Url}'>{recipe.Title}</a></h2>");
        sb.AppendLine();
        sb.AppendLine(recipe.Description);
        sb.AppendLine();
        sb.AppendLine("{{< code-sp4 >}}");
        sb.AppendLine();
        sb.AppendLine("```cs");
        sb.AppendLine(recipe.Code);
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("{{< /code-sp4 >}}");
        sb.AppendLine();
        sb.AppendLine($"<img src='../../images/{recipe.ID.ToLower()}.png' class='d-block mx-auto my-5' />");
        sb.AppendLine();
        return sb.ToString();
    }

    private static void MakeCategoryPage(string thisCategoryFolderPath, RecipeSource[] Recipes)
    {
        string categoryFolderName = Path.GetFileName(thisCategoryFolderPath);

        RecipeSource[] recipes = Recipes.Where(x => x.CategoryFolder == categoryFolderName).ToArray();

        ICategory category = Category.GetCategories().Where(x => x.Folder == categoryFolderName).First();
        string categoryName = category.ToString()!.Contains("PlotType") ? "Plot Type: " + category.Name : category.Name;

        StringBuilder markdown = new();
        markdown.AppendLine($"# {categoryName}");
        markdown.AppendLine($"* This page contains recipes for the _{category.Name}_ category.");
        markdown.AppendLine($"* Visit the [Cookbook Home Page](../../) to view all cookbook recipes.");
        markdown.AppendLine($"* Generated by ScottPlot {ScottPlot.Plot.Version} on {DateTime.Now.ToShortDateString()}");
        foreach (RecipeSource recipe in recipes)
            markdown.AppendLine(GetRecipeMarkdown(recipe));

        string bcName1 = "ScottPlot 4.1 Cookbook";
        string bcUrl1 = $"/cookbook/4.1/";

        string bcName2 = category.Name;
        string bcUrl2 = $"/cookbook/4.1/category/{category.Folder}/";

        string searchUrl = "/cookbook/4.1/search/";

        string[] fm = new string[]
        {
            $"BreadcrumbNames: [\"{bcName1}\", \"{bcName2}\"]",
            $"BreadcrumbUrls: [\"{bcUrl1}\", \"{bcUrl2}\"]",
            $"SearchUrl: \"{searchUrl}\"",
        };

        Template.CreateMarkdownPage(
            mdFilePath: Path.Combine(thisCategoryFolderPath, "index.md"),
            body: markdown.ToString(),
            title: $"{categoryName} - ScottPlot 4.1 Cookbook",
            description: category.Description,
            frontmatter: fm);
    }

    private static void MakeIndexPage(string OutputFolderPath, RecipeSource[] Recipes)
    {
        Console.WriteLine("Creating index page ...");

        string categoryHeaderTemplate =
            "<div class='fs-1 mt-4' style='font-weight: 500;' id='{{ANCHOR}}'>" +
            "  <a href='{{CATEGORY_URL}}' class='text-dark'>{{TITLE}}</a>" +
            "</div>" +
            "<div class='mb-3'>{{SUBTITLE}}</div>";

        string recipeTemplate =
            "<div class='row py-3'>" +
            "  <div class='col-4'>" +
            "    <a href='{{RECIPEURL}}'><img src='{{IMAGEURL}}' style='max-width: 100%'></a>" +
            "  </div>" +
            "  <div class='col'>" +
            "    <div class='fw-bold'><a href='{{RECIPEURL}}'>{{TITLE}}</a></div>" +
            "    <div>{{DESCRIPTION}}</div>" +
            "  </div>" +
            "</div>";

        StringBuilder sb = new();
        sb.AppendLine("# ScottPlot 4.0 Cookbook");
        sb.AppendLine($"Generated using ScottPlot {ScottPlot.Plot.Version} on {DateTime.Now.ToShortDateString()} <br />");

        // CATEGORY LIST
        sb.AppendLine("<h4>Customization</h4>");
        sb.AppendLine("<ul>");
        foreach (KeyValuePair<string, IRecipe[]> pair in Locate.GetCategorizedRecipes())
        {
            if (pair.Value.First().Category.ToString()!.Contains("PlotType"))
                continue;
            ICategory category = pair.Value.First().Category;
            if (category.Name == "Miscellaneous" || category.Name == "Statistics")
                continue;
            sb.AppendLine($"<li><a href='#{category.Folder}'>{category.Name}</a> - {category.Description}</li>");
        }
        sb.AppendLine("</ul>");

        sb.AppendLine("<h4>Plot Types</h4>");
        sb.AppendLine("<ul>");
        foreach (KeyValuePair<string, IRecipe[]> pair in Locate.GetCategorizedRecipes())
        {
            if (!pair.Value.First().Category.ToString()!.Contains("PlotType"))
                continue;
            ICategory category = pair.Value.First().Category;
            sb.AppendLine($"<li><a href='#{category.Folder}'>{category.Name}</a> - {category.Description}</li>");
        }
        sb.AppendLine("</ul>");

        sb.AppendLine("<h4>Additional Examples</h4>");
        sb.AppendLine("<ul>");
        foreach (KeyValuePair<string, IRecipe[]> pair in Locate.GetCategorizedRecipes())
        {
            if (pair.Value.First().Category.ToString()!.Contains("PlotType"))
                continue;
            ICategory category = pair.Value.First().Category;
            if (category.Name == "Miscellaneous" || category.Name == "Statistics")
            {
                sb.AppendLine($"<li><a href='#{category.Folder}'>{category.Name}</a> - {category.Description}</li>");
            }
        }
        sb.AppendLine("</ul>");

        sb.AppendLine("<h4>Colors</h4>");
        sb.AppendLine("<ul>");
        sb.AppendLine("<li><a href='colors/'>Palettes</a> - Lists of colors in each color palette for representing categorical data</li>");
        sb.AppendLine("<li><a href='colormaps/'>Colormaps</a> - Color gradients available to represent continuous data</li>");
        sb.AppendLine("</ul>");

        // SEPARATION
        sb.AppendLine("<hr class='my-5' />");

        // EVERY RECIPE
        foreach (KeyValuePair<string, IRecipe[]> pair in Locate.GetCategorizedRecipes())
        {
            ICategory category = pair.Value.First().Category;
            string categoryUrl = $"category/{category.Folder}/";

            sb.AppendLine(categoryHeaderTemplate
                .Replace("{{ANCHOR}}", category.Folder)
                .Replace("{{TITLE}}", category.Name)
                .Replace("{{SUBTITLE}}", category.Description))
                .Replace("{{CATEGORY_URL}}", categoryUrl);

            foreach (var recipe in Recipes.Where(x => x.CategoryFolder == category.Folder))
            {
                sb.AppendLine(recipeTemplate
                    .Replace("{{IMAGEURL}}", "images/" + recipe.ID.ToLower() + "_thumb.jpg")
                    .Replace("{{RECIPEURL}}", categoryUrl + "#" + recipe.AnchorID)
                    .Replace("{{TITLE}}", recipe.Title)
                    .Replace("{{DESCRIPTION}}", recipe.Description));
            }
        }

        string bcName1 = "ScottPlot 4.1 Cookbook";
        string bcUrl1 = "/cookbook/4.1/";
        string searchUrl = "/cookbook/4.1/search/";

        string[] fm = new string[]
        {
            $"BreadcrumbNames: [\"{bcName1}\"]",
            $"BreadcrumbUrls: [\"{bcUrl1}\"]",
            $"SearchUrl: \"{searchUrl}\"",
        };

        Template.CreateMarkdownPage(
            mdFilePath: Path.Combine(OutputFolderPath, "index_.md"), // NOTE: must end with an underscore because subpages exist
            body: sb.ToString(),
            title: "ScottPlot 4.1 Cookbook",
            description: "Example plots shown next to the code used to create them",
            url: "/cookbook/4.1/",
            frontmatter: fm);
    }
}
