using NUnit.Framework.Internal;
using ScottPlotCookbook.Recipes;
using ScottPlotCookbook.Website;
using System.Reflection;

namespace ScottPlotCookbook;

internal class RecipeTests
{
    [Test]
    public void Test_Query_Categories_HavePopulatedFields()
    {
        var categories = Query.GetCategories().ToArray();

        categories.Should().NotBeNullOrEmpty();

        foreach (ICategory category in categories)
        {
            category.Chapter.Should().NotBeNullOrWhiteSpace();
            category.CategoryName.Should().NotBeNullOrWhiteSpace();
            category.CategoryDescription.Should().NotBeNullOrWhiteSpace();
        }
    }

    [Test]
    public void Test_Query_Categories_HaveUniqueNames()
    {
        ScottPlot.Testing.DuplicateIdentifier<ICategory> ids = new("category name");

        foreach (ICategory category in Query.GetCategories())
        {
            ids.Add(category.CategoryName, category);
        }

        ids.ShouldHaveNoDuplicates();
    }

    [Test]
    public void Test_Query_Categories_HaveUniqueDescriptions()
    {
        ScottPlot.Testing.DuplicateIdentifier<ICategory> ids = new("category description");

        foreach (ICategory category in Query.GetCategories())
        {
            ids.Add(category.CategoryDescription, category);
        }

        ids.ShouldHaveNoDuplicates();
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

    [Test]
    public static void Test_ChaptersList_HasAllChapters()
    {
        var orderedChapterNames = Query.GetChapterNamesInOrder();
        var recipeChapterNames = Query.GetCategories().Select(x => x.Chapter).Distinct();

        foreach (string chapter in recipeChapterNames)
        {
            orderedChapterNames.Should().Contain(chapter);
        }
    }

    [Test]
    public static void Test_Recipes_HaveTestAttribute()
    {
        var recipeTypes = Assembly.GetAssembly(typeof(IRecipe))!
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(RecipeBase)))
            .Where(x => !x.IsAbstract);

        foreach (Type recipeType in recipeTypes)
        {
            bool hasTestAttribute = recipeType.Methods()
                .Where(x => x.Name == "Execute")
                .Where(x => !x.IsAbstract)
                .Single()
                .GetCustomAttributes<TestAttribute>(inherit: false)
                .Any();

            if (!hasTestAttribute)
                Assert.Fail($"{recipeType.Name}'s Execute() method is missing the [Test] attribute");
        }
    }

    [Test]
    public static void Test_Recipes_ArePublic()
    {
        var recipeTypes = Assembly.GetAssembly(typeof(IRecipe))!
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(RecipeBase)))
            .Where(x => !x.IsAbstract);

        foreach (Type recipeType in recipeTypes)
        {
            TypeInfo recipeClassInfo = recipeType.GetTypeInfo();
            recipeClassInfo.IsVisible.Should().BeTrue($"{recipeClassInfo.Namespace}.{recipeClassInfo.Name} should be public");
        }
    }
}
