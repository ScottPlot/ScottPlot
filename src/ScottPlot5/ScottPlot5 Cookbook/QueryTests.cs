namespace ScottPlotCookbook;

internal class QueryTests
{
    [Test]
    public void Test_Chapters_HaveNames()
    {
        List<ChapterInfo> chapters = Query.GetChapters();
        chapters.Should().NotBeEmpty();

        foreach (ChapterInfo chapter in chapters)
        {
            chapter.Name.Should().NotBeNullOrEmpty();
        }
    }

    [Test]
    public void Test_Pages_Are_Valid()
    {
        List<CategoryInfo> pages = Query.GetCategories();

        foreach (CategoryInfo page in pages)
        {
            page.Name.Should().NotBeNullOrEmpty();
            page.Description.Should().NotBeNullOrEmpty();
            page.RecipeInfos.Should().NotBeEmpty();
        }
    }

    [Test]
    public void Test_Recipes_Are_Valid()
    {
        List<RecipeInfo> recipes = Query.GetRecipes();

        foreach (RecipeInfo recipe in recipes)
        {
            recipe.Name.Should().NotBeNullOrEmpty();
            recipe.Description.Should().NotBeNullOrEmpty();
            //recipe.SourceCode.Should().NotBeNullOrEmpty();
            recipe.Recipe.Should().NotBeNull();
        }
    }

    [Test]
    public void Test_ImageFileNames_AreUnique()
    {
        HashSet<string> seen = new();
        foreach (RecipeInfo recipe in Query.GetRecipes())
        {
            if (seen.Contains(recipe.ImageFilename))
            {
                throw new InvalidOperationException($"duplicate image filename: {recipe.Category} - {recipe.Name}");
            }

            seen.Add(recipe.ImageFilename);
        }
    }
}
