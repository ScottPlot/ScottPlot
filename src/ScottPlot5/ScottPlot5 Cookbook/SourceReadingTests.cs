namespace ScottPlotCookbook;

internal class SourceReadingTests
{
    [Test]
    public static void Test_Recipe_Sources_Found()
    {
        List<RecipeSource> sources = SourceReading.GetRecipeSources();

        sources.Should().NotBeEmpty();
        sources.Should().HaveCount(Cookbook.GetRecipes().Count);

        foreach (RecipeInfo recipe in Query.GetRecipes())
        {
            recipe.AddSource(sources);
            recipe.SourceCode.Should().NotBeNull();
        }
    }
}
