using ScottPlotCookbook.Recipes;
using ScottPlotCookbook.Website;

namespace ScottPlotCookbook;

internal class JsonCookbookInfoTests
{
    [Test]
    public void Test_JsonCookbook_Loads()
    {
        string json = JsonFile.Generate();

        JsonCookbookInfo cb = new(json);
        cb.Version.Should().NotBeNullOrWhiteSpace();

        cb.Chapters.Should().NotBeEmpty();
        cb.Chapters.Should().OnlyHaveUniqueItems();

        cb.Categories.Should().NotBeEmpty();
        cb.Categories.Select(x => x.Name).Should().OnlyHaveUniqueItems();
        cb.Categories.Select(x => x.Description).Should().OnlyHaveUniqueItems();
        cb.Categories.Select(x => x.Url).Should().OnlyHaveUniqueItems();

        cb.Recipes.Should().NotBeEmpty();
        cb.Recipes.Select(x => x.RecipeUrl).Should().OnlyHaveUniqueItems();
    }
}
