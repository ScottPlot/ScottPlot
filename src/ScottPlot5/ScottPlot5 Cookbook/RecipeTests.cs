using ScottPlotCookbook.Website;

namespace ScottPlotCookbook;

internal class RecipeTests
{
    [Test]
    public void Test_Query_Categories()
    {
        IEnumerable<ICategory> categories = Query.GetCategories();
        categories.Should().NotBeNullOrEmpty();

        categories.Select(x => x.CategoryName).Should().OnlyHaveUniqueItems();
        categories.Select(x => x.CategoryDescription).Should().OnlyHaveUniqueItems();

        foreach (ICategory category in categories)
        {
            category.Chapter.Should().NotBeNullOrWhiteSpace();
            category.CategoryName.Should().NotBeNullOrWhiteSpace();
            category.CategoryDescription.Should().NotBeNullOrWhiteSpace();
        }
    }

    [Test]
    public static void Test_RecipeSources_FoundAndValid()
    {
        SourceDatabase db = new();

        db.Recipes.Should().NotBeNullOrEmpty();

        foreach (RecipeInfo recipe in db.Recipes)
        {
            recipe.Chapter.Should().NotBeNullOrEmpty();
            recipe.Category.Should().NotBeNullOrEmpty();
            recipe.Name.Should().NotBeNullOrEmpty();
            recipe.Description.Should().NotBeNullOrEmpty();
            recipe.Source.Should().NotBeNullOrEmpty();
            recipe.RecipeClassName.Should().NotBeNullOrEmpty();
            recipe.CategoryClassName.Should().NotBeNullOrEmpty();
        }

        db.Recipes.Select(x => x.RecipeClassName).Should().OnlyHaveUniqueItems();
        db.Recipes.Select(x => x.Name).Should().OnlyHaveUniqueItems();
        db.Recipes.Select(x => x.Description).Should().OnlyHaveUniqueItems();
        db.Recipes.Select(x => x.ImageUrl).Should().OnlyHaveUniqueItems();
        db.Recipes.Select(x => x.RecipeUrl).Should().OnlyHaveUniqueItems();
    }
}
