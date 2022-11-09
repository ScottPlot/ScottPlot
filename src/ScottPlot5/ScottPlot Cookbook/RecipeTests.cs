using FluentAssertions;
using NUnit.Framework.Internal;

namespace ScottPlotCookbook;

internal class RecipeTests
{
    // TODO: recipe descriptions should end with a period
    // TODO: page and chapter descriptions should not end with a period

    [Test]
    public void Test_Recipes_Found()
    {
        List<IRecipe> recipes = Cookbook.GetRecipes();
        recipes.Should().NotBeNull();
        recipes.Should().NotBeEmpty();
        recipes.ForEach(x => TestContext.WriteLine(x));
    }

    [Test]
    public void Test_Pages_Found()
    {
        List<RecipePage> pages = Cookbook.GetPages();
        pages.Should().NotBeNull();
        pages.Should().NotBeEmpty();
        pages.ForEach(x => TestContext.WriteLine(x));
    }

    [Test]
    public void Test_Chapters_Found()
    {
        List<RecipeChapter> chapters = Cookbook.GetChapters();
        chapters.Should().NotBeNull();
        chapters.Should().NotBeEmpty();
        chapters.ForEach(x => TestContext.WriteLine(x));
    }

    [Test]
    public void Test_Recipe_Nest()
    {
        foreach (RecipeChapter chapter in Cookbook.GetChapters())
        {
            TestContext.WriteLine($"Chapter: {chapter.Name}");
            foreach (RecipePage page in Cookbook.GetPages())
            {
                TestContext.WriteLine($"  Page: {page.PageName}");
                foreach (IRecipe recipe in Cookbook.GetRecipes(page))
                {
                    TestContext.WriteLine($"    Recipe: {recipe.Name}");
                }
            }
        }
    }
}
