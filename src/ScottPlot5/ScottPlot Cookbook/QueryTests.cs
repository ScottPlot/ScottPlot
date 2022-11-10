using FluentAssertions;
using ScottPlotCookbook.Info;

namespace ScottPlotCookbook;

internal class QueryTests
{
    [Test]
    public void Test_Chapters_Are_Valid()
    {
        List<ChapterInfo> chapters = Query.GetChapters();
        chapters.Should().NotBeEmpty();

        foreach (ChapterInfo chapter in chapters)
        {
            chapter.Name.Should().NotBeNullOrEmpty();
            //chapter.Pages.Should().NotBeEmpty();
        }
    }

    [Test]
    public void Test_Pages_Are_Valid()
    {
        List<PageInfo> pages = Query.GetPages();

        foreach (PageInfo page in pages)
        {
            page.Name.Should().NotBeNullOrEmpty();
            page.Description.Should().NotBeNullOrEmpty();
            page.Recipes.Should().NotBeEmpty();
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
}
