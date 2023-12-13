using ScottPlotCookbook.Recipes;
using ScottPlotCookbook.Website;

namespace ScottPlotCookbook;

internal class RecipeTests
{
    [Test]
    public void Test_Query_Categories()
    {
        IEnumerable<ICategory> categories = Query.GetCategoryClasses();
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
    public void Test_Query_RecipesByCategory()
    {
        foreach (var kv in Query.GetRecipesByCategory())
        {
            ICategory category = kv.Key;

            foreach (IRecipe recipe in kv.Value)
            {
                Console.WriteLine($"[{category.CategoryName}] {recipe.Name}");
            }
        }
    }

    [Test]
    public static void Test_RecipeSources_FoundAndValid()
    {
        IEnumerable<RecipeInfo> recipes = Query.GetWebRecipesByCategory().SelectMany(x => x.Value);

        recipes.Should().NotBeNullOrEmpty();

        foreach (RecipeInfo recipe in recipes)
        {
            recipe.Chapter.Should().NotBeNullOrEmpty();
            recipe.Category.Should().NotBeNullOrEmpty();
            recipe.Name.Should().NotBeNullOrEmpty();
            recipe.Description.Should().NotBeNullOrEmpty();
            recipe.Source.Should().NotBeNullOrEmpty();
            recipe.RecipeClassName.Should().NotBeNullOrEmpty();
            recipe.CategoryClassName.Should().NotBeNullOrEmpty();
        }

        recipes.Select(x => x.ImageUrl).Should().OnlyHaveUniqueItems();
        recipes.Select(x => x.RecipeUrl).Should().OnlyHaveUniqueItems();
    }

    [Test]
    public static void Test_Recipes_HaveUniqueClassNames()
    {
        HashSet<string> names = new();

        foreach (RecipeInfo recipe in Query.GetWebRecipesByCategory().SelectMany(x => x.Value))
        {
            if (names.Contains(recipe.RecipeClassName))
            {
                Assert.Fail($"The '{recipe.Category}' class '{recipe.RecipeClassName}' must be renamed to something unique.");
            }

            names.Add(recipe.RecipeClassName);
        }
    }

    [Test]
    public static void Test_Recipes_HaveUniqueNames()
    {
        HashSet<string> names = new();

        foreach (RecipeInfo recipe in Query.GetWebRecipesByCategory().SelectMany(x => x.Value))
        {
            if (names.Contains(recipe.Name))
            {
                Assert.Fail($"The '{recipe.Category}' class has a recipe '{recipe.Name}' which must be renamed to something unique.");
            }

            names.Add(recipe.Name);
        }
    }

    [Test]
    public static void Test_ChapterNames_ArePresentForAllRecipes()
    {
        string[] chapterNames = Query.GetChapterNamesInOrder();

        foreach (RecipeInfo recipe in Query.GetWebRecipesByCategory().SelectMany(x => x.Value))
        {
            chapterNames.Should().Contain(recipe.Chapter);
        }
    }
}
