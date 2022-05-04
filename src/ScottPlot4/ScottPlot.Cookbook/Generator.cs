using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ScottPlot.Cookbook
{
    public class Generator
    {
        readonly string OUTPUT_FOLDER;
        string JSON_FILE => Path.Combine(OUTPUT_FOLDER, "recipes.json");
        string IMAGE_FOLDER => Path.Combine(OUTPUT_FOLDER, "images");
        readonly RecipeSource[] Recipes;

        public Generator(string cookbookProjectFolder, string outputFolder, bool regenerate = false)
        {
            OUTPUT_FOLDER = outputFolder;

            if (regenerate)
            {
                if (Directory.Exists(IMAGE_FOLDER))
                    Directory.Delete(IMAGE_FOLDER, recursive: true);
                Directory.CreateDirectory(IMAGE_FOLDER);

                RecipeImages.Generate(IMAGE_FOLDER);
                RecipeJson.Generate(cookbookProjectFolder, JSON_FILE);
            }

            Recipes = RecipeJson.GetRecipes(new FileInfo(JSON_FILE)).Values.Select(x => x).ToArray();
        }

        public void MakeCategoryPages()
        {
            string categoryFolderPath = Path.Combine(OUTPUT_FOLDER, "category");
            if (!Directory.Exists(categoryFolderPath))
                Directory.CreateDirectory(categoryFolderPath);

            foreach (string categoryFolderName in Recipes.Select(x => x.CategoryFolder).Distinct())
            {
                string thisCategoryFolderPath = Path.Combine(categoryFolderPath, categoryFolderName);
                if (!Directory.Exists(thisCategoryFolderPath))
                    Directory.CreateDirectory(thisCategoryFolderPath);
                MakeCategoryPage(thisCategoryFolderPath);
            }
        }

        private void MakeCategoryPage(string thisCategoryFolderPath)
        {
            string categoryFolderName = Path.GetFileName(thisCategoryFolderPath);

            RecipeSource[] recipes = Recipes.Where(x => x.CategoryFolder == categoryFolderName).ToArray();

            ICategory category = Category.GetCategories().Where(x => x.Folder == categoryFolderName).First();

            StringBuilder sb = new();
            sb.AppendLine("---");
            sb.AppendLine($"Title: {category.Name} - ScottPlot 4.1 Cookbook");
            sb.AppendLine($"Description: {category.Description}");
            sb.AppendLine("---");
            sb.AppendLine("");

            foreach (RecipeSource recipe in recipes)
            {
                sb.AppendLine($"## {recipe.Title}");
                sb.AppendLine("");
                sb.AppendLine(recipe.Description);
                sb.AppendLine("");
                sb.AppendLine("```");
                sb.AppendLine(recipe.Code);
                sb.AppendLine("```");
                sb.AppendLine("");
                sb.AppendLine($"<img src='../../images/{recipe.ID.ToLower()}.png' class='d-block mx-auto my-5' />");
                sb.AppendLine("");
            }

            string mdFilePath = Path.Combine(thisCategoryFolderPath, "index.md");
            File.WriteAllText(mdFilePath, sb.ToString());
        }

        public void MakeIndexPage()
        {
            string devPageTemplate =
                "<html>" +
                "<head>" +
                "  <link href=https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css rel=stylesheet>" +
                "</head>" +
                "<body class=bg-light>" +
                "  <div class='container' style='max-width:1000px'>" +
                "    <article class='bg-light shadow rounded my-5'>" +
                "    <div class='p-3 rounded bg-white'>" +
                "      {{CONTENT}}" +
                "    </div>" +
                "    </article>" +
                "  </div>" +
                "</body>" +
                "</html>";

            string categoryHeaderTemplate =
                "<div class='fs-1 mt-4' style='font-weight: 500;' id='{{ANCHOR}}'>" +
                "  <a href='#{{ANCHOR}}' class='text-dark'>{{TITLE}}</a>" +
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

            static string GetAnchor(string s) => s.ToLower()
                .Replace(" ", "-")
                .Replace("_", "-")
                .Replace(" ", "-");

            // TODO: figure out a better way to sort this


            // CATEGORY LIST
            sb.AppendLine("<ul>");

            sb.AppendLine("<li>Quickstart</li>");
            sb.AppendLine("<ul>");
            foreach (ICategory category in Category.GetCategories().Where(x => x.Name == "Quickstart").Take(1))
                sb.AppendLine($"<li><a href='#{GetAnchor(category.Name)}'>{category.Name}</a> - {category.Description}</li>");
            sb.AppendLine("</ul>");

            sb.AppendLine("<li>Customization</li>");
            sb.AppendLine("<ul>");
            foreach (ICategory category in Category.GetCategories().Where(x => !x.ToString().Contains("PlotTypes")))
                sb.AppendLine($"<li><a href='#{GetAnchor(category.Name)}'>{category.Name}</a> - {category.Description}</li>");
            sb.AppendLine("</ul>");

            sb.AppendLine("<li>Plot Types</li>");
            sb.AppendLine("<ul>");
            foreach (ICategory category in Category.GetCategories().Where(x => x.ToString().Contains("PlotTypes")))
                sb.AppendLine($"<li><a href='#{GetAnchor(category.Name)}'>{category.Name}</a> - {category.Description}</li>");
            sb.AppendLine("</ul>");

            sb.AppendLine("</ul>");


            // SEPARATION
            sb.AppendLine("<hr class='my-5' />");

            // EVERY RECIPE
            foreach (ICategory category in Category.GetCategories())
            {
                string categoryUrl = $"category/{category.Folder}/";

                sb.AppendLine(categoryHeaderTemplate
                    .Replace("{{ANCHOR}}", GetAnchor(category.Name))
                    .Replace("{{TITLE}}", category.Name)
                    .Replace("{{SUBTITLE}}", category.Description));

                foreach (var recipe in Recipes.Where(x => x.CategoryFolder == category.Folder))
                {
                    sb.AppendLine(recipeTemplate
                        .Replace("{{IMAGEURL}}", "images/" + recipe.ID.ToLower() + "_thumb.jpg")
                        .Replace("{{RECIPEURL}}", categoryUrl + "#" + GetAnchor(recipe.Title))
                        .Replace("{{TITLE}}", recipe.Title)
                        .Replace("{{DESCRIPTION}}", recipe.Description));
                }
            }

            string filePathHtml = Path.Combine(OUTPUT_FOLDER, "index.dev.html");
            File.WriteAllText(filePathHtml, devPageTemplate.Replace("{{CONTENT}}", sb.ToString()));
            Console.WriteLine(filePathHtml);

            string markdownFrontmatter =
                "---\n" +
                "Title: ScottPlot 4.1 Cookbook\n" +
                "Description: Example plots shown next to the code used to create them\n" +
                "url: /cookbook/4.1/\n" +
                "---\n\n";

            string filePathMd = Path.Combine(OUTPUT_FOLDER, "index_.md");
            File.WriteAllText(filePathMd, markdownFrontmatter + sb.ToString());
            Console.WriteLine(filePathMd);
        }
    }
}
