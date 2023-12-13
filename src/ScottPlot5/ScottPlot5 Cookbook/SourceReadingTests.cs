namespace ScottPlotCookbook;

internal class SourceReadingTests
{
    readonly static List<WebRecipe> Recipes = SourceReading.GetWebRecipes();

    [Test]
    public static void Test_Recipe_Sources_Found()
    {
        Recipes.Should().NotBeEmpty();
        Recipes.Should().HaveCount(Cookbook.GetRecipes().Count);
    }

    [Test]
    public static void Test_WebRecipes_HaveAllData()
    {
        foreach (WebRecipe recipe in Recipes)
        {
            recipe.Chapter.Should().NotBeNullOrWhiteSpace();
            recipe.Category.Should().NotBeNullOrWhiteSpace();
            recipe.Name.Should().NotBeNullOrWhiteSpace();
            recipe.Description.Should().NotBeNullOrWhiteSpace();
            recipe.Source.Should().NotBeNullOrWhiteSpace();
        }
    }

    [Test]
    public static void Test_WebRecipes_HaveUniqueUrls()
    {
        Recipes.Select(x => x.RecipeUrl).Should().OnlyHaveUniqueItems();
        Recipes.Select(x => x.ImageUrl).Should().OnlyHaveUniqueItems();
    }
}
