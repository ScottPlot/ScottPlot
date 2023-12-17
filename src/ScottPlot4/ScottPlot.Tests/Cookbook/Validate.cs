using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Cookbook
{
    class Validate
    {
        const string SourceFolder = "../../../../ScottPlot.Cookbook";

        [Test]
        public void Test_CookbookRecipes_Exist()
        {
            var recipes = ScottPlot.Cookbook.Locate.GetRecipes();
            Assert.IsNotEmpty(recipes);

            foreach (var recipe in recipes)
                Console.WriteLine(recipe);
        }

        [Test]
        public void Test_CookbookRecipes_AllHaveValidSourceCode()
        {
            var recipes = ScottPlot.Cookbook.Locate.GetRecipes();
            var sources = ScottPlot.Cookbook.SourceParsing.GetRecipeSources(SourceFolder, 333, 222);

            // ensure every recipe has source code
            foreach (ScottPlot.Cookbook.IRecipe recipe in recipes)
                Assert.AreEqual(1, sources.Where(x => x.ID == recipe.ID).Count());

            // ensure there are no hanging { or }
            foreach (ScottPlot.Cookbook.IRecipe recipe in recipes)
            {
                string source = sources.Where(x => x.ID == recipe.ID).First().Code;
                int openCount = source.Count(f => f == '{');
                int closeCount = source.Count(f => f == '}');
                Assert.AreEqual(openCount, closeCount,
                    message: $"Cookbook formatting error ({recipe.Category} / {recipe.ID}):\n" +
                    $"Classes must be separated by exactly ONE line of whitespace.");
            }
        }

        [Test]
        public void Test_CookbookRecipes_DescriptorsAreValid()
        {
            string allowedSpecialCharacters = " _'()[];.,+/-:#?";
            foreach (var recipe in ScottPlot.Cookbook.Locate.GetRecipes())
            {
                foreach (var c in recipe.Title + recipe.Description)
                {
                    if (char.IsLetterOrDigit(c) || allowedSpecialCharacters.Contains(c))
                        continue;
                    else
                        Assert.Fail($"{recipe.ID} Title or Description contains a special character: {c}");
                }
            }
        }

        [Test]
        public void Test_CookbookRecipes_IDsAreValid()
        {
            string allowedSpecialCharacters = "_";
            foreach (var recipe in ScottPlot.Cookbook.Locate.GetRecipes())
            {
                foreach (var c in recipe.ID.ToCharArray())
                {
                    if (char.IsLetterOrDigit(c) || allowedSpecialCharacters.Contains(c))
                        continue;
                    else
                        Assert.Fail("IDs must not contain special characters (except underscore)");
                }
            }
        }

        [Test]
        public void Test_CookbookRecipes_TitlesInsideACategoryAreUnique()
        {
            List<string> titles = new();
            foreach (var cat in ScottPlot.Cookbook.Locate.GetCategorizedRecipes())
            {
                HashSet<string> seen = new();
                foreach (string title in cat.Value.Select(x => x.Title))
                {
                    if (seen.Contains(title))
                        throw new InvalidOperationException($"'{cat.Key}' has multiple recipes titled '{title}'");
                    seen.Add(title);
                }
            }
        }

        [Test]
        public void Test_CookbookRecipes_IDsAreUnique()
        {
            var recipes = ScottPlot.Cookbook.Locate.GetRecipes();
            string[] ids = recipes.Select(x => x.ID.ToLower()).ToArray();
            string[] duplicates = TestTools.GetDuplicates(ids);
        }

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
        public void Test_RecipePage_UrlsAreUnique()
        {
            HashSet<string> seenImages = new();
            HashSet<string> seenAnchors = new();
            HashSet<string> seenPages = new();

            var sources = ScottPlot.Cookbook.SourceParsing.GetRecipeSources(SourceFolder, 333, 222);

            foreach (var recipe in sources)
            {
                if (seenImages.Contains(recipe.ImageUrl))
                    throw new InvalidOperationException($"duplicate recipe URL: {recipe.Category} - {recipe.Title}");

                if (seenAnchors.Contains(recipe.AnchorUrl))
                    throw new InvalidOperationException($"duplicate recipe URL: {recipe.Category} - {recipe.Title}");

                if (seenPages.Contains(recipe.Url))
                    throw new InvalidOperationException($"duplicate recipe URL: {recipe.Category} - {recipe.Title}");

                seenImages.Add(recipe.ImageUrl);
                seenAnchors.Add(recipe.AnchorUrl);
                seenPages.Add(recipe.Url);
            }

            Console.WriteLine($"recipes contain {seenPages.Count} unique URLs");
        }
    }
}
