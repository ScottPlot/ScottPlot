using FluentAssertions;
using NUnit.Framework.Internal;
using System.Reflection;

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
        List<RecipePage> recipes = Cookbook.GetPages();
        recipes.Should().NotBeNull();
        recipes.Should().NotBeEmpty();
        recipes.ForEach(x => TestContext.WriteLine(x));
    }

    [Test]
    public void Test_Chapters_Found()
    {
        List<RecipeChapter> recipes = Cookbook.GetChapters();
        recipes.Should().NotBeNull();
        recipes.Should().NotBeEmpty();
        recipes.ForEach(x => TestContext.WriteLine(x));
    }
}
