using NUnit.Framework;
using ScottPlot.Cookbook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ScottPlotTests.Cookbook
{
    [TestFixture]
    class Generate
    {
        string COOKBOOK_PROJECT_FOLDER => Path.GetFullPath("../../../../ScottPlot.Cookbook");
        string OUTPUT_FOLDER => Path.GetFullPath("../../../../ScottPlot.Cookbook/CookbookOutput");
        string JSON_FILE => Path.Join(OUTPUT_FOLDER, "recipes.json");

        [Test]
        public void Test_Generate_Cookbook()
        {
            Console.WriteLine($"Generating cookbook in:\n{OUTPUT_FOLDER}");

            // DELETE OLD COOKBOOK IMAGES
            string IMAGE_FOLDER = Path.Join(OUTPUT_FOLDER, "images");
            if (Directory.Exists(IMAGE_FOLDER))
            {
                Console.WriteLine($"Deleting old images: {IMAGE_FOLDER}");
                Directory.Delete(IMAGE_FOLDER, recursive: true);
            }

            // CREATE IMAGE FOLDER
            Console.WriteLine($"Creating images folder: {IMAGE_FOLDER}");
            Directory.CreateDirectory(IMAGE_FOLDER);

            // GENERATE IMAGES
            Console.WriteLine($"Generating PNGs...");
            Stopwatch sw = Stopwatch.StartNew();
            IRecipe[] imageRecipes = RecipeImages.Generate(IMAGE_FOLDER);
            Console.WriteLine($"Generated {imageRecipes.Length} PNGs in {sw.Elapsed.TotalSeconds:F4} sec");

            // GENERATE JSON
            Console.Write($"Generating JSON...");
            sw.Restart();
            RecipeSource[] sourceRecipes = RecipeJson.Generate(COOKBOOK_PROJECT_FOLDER, JSON_FILE);
            Console.WriteLine($" {sw.Elapsed.TotalSeconds:F4} sec");

            // READ JSON BACK
            Console.Write($"Validating JSON...");
            sw.Restart();
            List<string> readIDs = new();
            using JsonDocument document = JsonDocument.Parse(File.ReadAllText(JSON_FILE));
            string version = document.RootElement.GetProperty("version").GetString();
            string generated = document.RootElement.GetProperty("generated").GetString();
            foreach (JsonElement recipeElement in document.RootElement.GetProperty("recipes").EnumerateArray())
            {
                string id = recipeElement.GetProperty("id").GetString();
                string category = recipeElement.GetProperty("category").GetString();
                string title = recipeElement.GetProperty("title").GetString();
                string description = recipeElement.GetProperty("description").GetString();
                string code = recipeElement.GetProperty("code").GetString();
                readIDs.Add(id);
            }
            Console.WriteLine($" {sw.Elapsed.TotalSeconds:F4} sec");

            // VALIDATE
            Assert.AreEqual(imageRecipes.Length, sourceRecipes.Length);
            Assert.AreEqual(sourceRecipes.Length, readIDs.Count);
        }

        [Test]
        public void Test_GeneratePage_Index()
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
                "<div class='fs-1 mt-4' style='font-weight: 500;' id='{{ID}}'>{{TITLE}}</div>" +
                "<div class='mb-3'>{{SUBTITLE}}</div>";

            string recipeTemplate =
                "<div class='row py-3'>" +
                "  <div class='col-3'>" +
                "    <img src='{{IMAGEURL}}' style='max-width: 100%'>" +
                "  </div>" +
                "  <div class='col'>" +
                "    <div class='fw-bold'>{{TITLE}}</div>" +
                "    <div>{{DESCRIPTION}}</div>" +
                "  </div>" +
                "</div>";

            IRecipe[] recipes = Locate.GetRecipes();

            StringBuilder sb = new();

            static string GetCategoryAnchor(ICategory cat) => cat.Name.ToLower().Replace(" ", "-");

            static string GetRecipeAnchor(IRecipe recipe) => recipe.Title.ToLower().Replace("_", "-").Replace(" ", "-");

            // TODO: figure out a better way to sort this


            // CATEGORY LIST
            sb.AppendLine("<ul>");

            sb.AppendLine("<li>Quickstart</li>");
            sb.AppendLine("<ul>");
            foreach (ICategory category in Category.GetCategories().Where(x => x.Name == "Quickstart").Take(1))
                sb.AppendLine($"<li><a href='#{GetCategoryAnchor(category)}'>{category.Name}</a> - {category.Description}</li>");
            sb.AppendLine("</ul>");

            sb.AppendLine("<li>Customization</li>");
            sb.AppendLine("<ul>");
            foreach (ICategory category in Category.GetCategories().Where(x => !x.ToString().Contains("PlotTypes")))
                sb.AppendLine($"<li><a href='#{GetCategoryAnchor(category)}'>{category.Name}</a> - {category.Description}</li>");
            sb.AppendLine("</ul>");

            sb.AppendLine("<li>Plot Types</li>");
            sb.AppendLine("<ul>");
            foreach (ICategory category in Category.GetCategories().Where(x => x.ToString().Contains("PlotTypes")))
                sb.AppendLine($"<li><a href='#{GetCategoryAnchor(category)}'>{category.Name}</a> - {category.Description}</li>");
            sb.AppendLine("</ul>");

            sb.AppendLine("</ul>");


            // SEPARATION
            sb.AppendLine("<hr class='my-5' />");

            // EVERY RECIPE
            foreach (ICategory category in Category.GetCategories())
            {
                string categoryUrl = $"https://scottplot.net/cookbook/4.1/category/{category.Folder}/";

                sb.AppendLine(categoryHeaderTemplate
                    .Replace("{{ID}}", GetCategoryAnchor(category))
                    .Replace("{{TITLE}}", category.Name)
                    .Replace("{{SUBTITLE}}", category.Description));

                foreach (IRecipe recipe in recipes.Where(x => x.Category.Folder == category.Folder))
                {
                    string recipeUrl = categoryUrl + "#" + GetRecipeAnchor(recipe);

                    sb.AppendLine(recipeTemplate
                        .Replace("{{IMAGEURL}}", "images/" + recipe.ID.ToLower() + "_thumb.jpg")
                        .Replace("{{TITLE}}", $"<a href='{recipeUrl}'>{recipe.Title}</a>")
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
