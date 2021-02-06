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
        const string SourceFolder = "../../../../cookbook/ScottPlot.Cookbook";

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
            var chef = new ScottPlot.Cookbook.Chef();
            var sources = chef.GetRecipeSources(SourceFolder);

            // ensure every recipe has source code
            foreach (ScottPlot.Cookbook.IRecipe recipe in recipes)
                Assert.AreEqual(1, sources.Where(x => x.id == recipe.ID).Count());

            // ensure there are no hanging { or }
            foreach (ScottPlot.Cookbook.IRecipe recipe in recipes)
            {
                string source = sources.Where(x => x.id == recipe.ID).First().source;
                int openCount = source.Count(f => f == '{');
                int closeCount = source.Count(f => f == '}');
                Assert.AreEqual(openCount, closeCount, message: $"{recipe.Category} : {recipe.ID} format error - empty lines after a class?");
            }
        }

        [Test]
        public void Test_CookbookRecipes_DescriptorsAreValid()
        {
            string allowedSpecialCharacters = " _'()[];.,+/-";
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
            foreach (var cat in ScottPlot.Cookbook.Locate.GetCategorizedRecipes())
            {
                HashSet<string> seen = new HashSet<string>();
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

            foreach (string id in ids)
                if (ids.Where(x => x == id).Count() > 1)
                    Assert.Fail($"duplicate id: {id}");
        }

        [Test]
        public void Test_CookbookRecipes_CategoriesRemainUniqueAfterSanitization()
        {
            var recipes = ScottPlot.Cookbook.Locate.GetRecipes();
            int uniqueFull = recipes.Select(x => x.Category).Distinct().Count();
            int uniqueClean = recipes.Select(x => x.Category)
                .Select(x => ScottPlot.Cookbook.Website.Page.Sanitize(x)).Distinct().Count();
            Assert.AreEqual(uniqueClean, uniqueFull);
        }
    }
}
