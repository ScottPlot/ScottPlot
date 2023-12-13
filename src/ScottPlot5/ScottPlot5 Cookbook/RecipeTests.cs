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
        IEnumerable<WebRecipe> recipes = Query.GetWebRecipesByCategory().SelectMany(x => x.Value);

        recipes.Should().NotBeNullOrEmpty();

        foreach (WebRecipe recipe in recipes)
        {
            recipe.Chapter.Should().NotBeNullOrEmpty();
            recipe.Category.Should().NotBeNullOrEmpty();
            recipe.Name.Should().NotBeNullOrEmpty();
            recipe.Description.Should().NotBeNullOrEmpty();
            recipe.Source.Should().NotBeNullOrEmpty();
            recipe.ClassName.Should().NotBeNullOrEmpty();
        }

        recipes.Select(x => x.ImageUrl).Should().OnlyHaveUniqueItems();
        recipes.Select(x => x.RecipeUrl).Should().OnlyHaveUniqueItems();
    }

    [Test]
    public static void Test_Recipes_HaveUniqueClassNames()
    {
        HashSet<string> classNames = new();

        foreach (WebRecipe recipe in Query.GetWebRecipesByCategory().SelectMany(x => x.Value))
        {
            if (classNames.Contains(recipe.ClassName))
            {
                Assert.Fail($"The '{recipe.Category}' class '{recipe.ClassName}' must be renamed to something unique.");
            }

            classNames.Add(recipe.ClassName);
        }
    }
}
