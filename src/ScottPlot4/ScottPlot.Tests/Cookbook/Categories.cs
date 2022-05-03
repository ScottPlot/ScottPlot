using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Cookbook
{
    internal class Categories
    {
        [Test]
        public void Test_Categories_HaveValidText()
        {
            foreach (ScottPlot.Cookbook.ICategory category in ScottPlot.Cookbook.Category.GetCategories())
            {
                Console.WriteLine(category);

                Assert.IsNotNull(category.Name, category.ToString());
                Assert.IsNotEmpty(category.Name, category.ToString());
                Assert.AreEqual(category.Name.Trim(), category.Name, category.ToString());

                Assert.IsNotNull(category.Description, category.ToString());
                Assert.IsNotEmpty(category.Description, category.ToString());
                Assert.AreEqual(category.Description.Trim(), category.Description, category.ToString());

                Assert.That(category.Description.EndsWith("."),
                    $"{category} description must end with a period");

                Console.WriteLine(category.Name);
            }
        }

        [Test]
        public void Test_CategoryFolderNames_AreLowercase()
        {
            foreach (var cat in ScottPlot.Cookbook.Category.GetCategories())
                Assert.AreEqual(cat.Folder.ToLower(), cat.Folder);
        }

        [Test]
        public void Test_CategoryFolderNames_AreUnique()
        {
            HashSet<string> names = new();

            foreach (var cat in ScottPlot.Cookbook.Category.GetCategories())
            {
                if (names.Contains(cat.Folder))
                {
                    Assert.Fail($"{cat} has duplicate subfolder name: {cat.Folder}");
                }
                else
                {
                    names.Add(cat.Folder);
                }
            }
        }

        [Test]
        public void Test_Categories_HtmlIndex()
        {
            StringBuilder sb = new();

            string categoryHeaderTemplate =
                "<div class='fs-1' style='font-weight: 500;'>{{TITLE}}</div>" +
                "<div class='fs-4 mb-5'>{{SUBTITLE}}</div>";

            string recipeTemplate =
                "<div class='row py-3'>" +
                "  <div class='col-3'>" +
                "    <img src='{{IMAGEURL}}'>" +
                "  </div>" +
                "  <div class='col'>" +
                "    <div class='fw-bold'>{{TITLE}}</div>" +
                "    <div>{{DESCRIPTION}}</div>" +
                "  </div>" +
                "</div>";

            ScottPlot.Cookbook.IRecipe[] recipes = ScottPlot.Cookbook.Locate.GetRecipes();

            foreach (ScottPlot.Cookbook.ICategory category in ScottPlot.Cookbook.Category.GetCategories())
            {

                sb.AppendLine(categoryHeaderTemplate
                    .Replace("{{TITLE}}", category.Name)
                    .Replace("{{SUBTITLE}}", category.Description));

                foreach (var recipe in recipes.Where(x => x.Category.Folder == category.Folder))
                {
                    sb.AppendLine(recipeTemplate
                        .Replace("{{IMAGEURL}}", "https://scottplot.net/cookbook/4.1/images/" + recipe.ID.ToLower() + "_thumb.jpg")
                        .Replace("{{TITLE}}", recipe.Title)
                        .Replace("{{DESCRIPTION}}", recipe.Description));
                }
            }

            string filePath = System.IO.Path.GetFullPath("CategoryIndex.html");
            System.IO.File.WriteAllText(filePath, sb.ToString());
            Console.WriteLine(filePath);
        }
    }
}
