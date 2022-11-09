using ScottPlot_Tests.Cookbook.Recipes.Introduction;

namespace ScottPlot_Tests.Cookbook;

internal class RecipeTests
{
    private static RecipeTestBase[] GetRecipeTests()
    {
        // TODO: use reflection to return this
        return Array.Empty<RecipeTestBase>();
    }

    [Test]
    public void Test_Recipes_Have_Test_Attribute()
    {
        foreach (RecipeTestBase recipe in GetRecipeTests())
        {
            recipe.RecipeHasTestAttribute.Should().BeTrue();
        }
    }
}
