using FluentAssertions;

namespace ScottPlotCookbook;

internal class CookbookTests
{
    [Test]
    public void Test_Recipes_Found()
    {
        List<IRecipe> recipes = Cookbook.GetRecipes();
        recipes.Should().NotBeNull();
        recipes.Should().NotBeEmpty();
        recipes.ForEach(x => TestContext.WriteLine(x));
    }

    [Test]
    public void Test_Pages_Found()
    {
        List<RecipePageBase> pages = Cookbook.GetPages();
        pages.Should().NotBeNull();
        pages.Should().NotBeEmpty();
        pages.ForEach(x => TestContext.WriteLine(x));
    }

    [Test]
    public void Test_Chapters_Found()
    {
        List<Chapter> chapters = Cookbook.GetChapters();
        chapters.Should().NotBeNull();
        chapters.Should().NotBeEmpty();
        chapters.ForEach(x => TestContext.WriteLine(x));
    }

    [Test]
    public void Test_RecipeDescriptions_ShouldEndWithPeriod()
    {
        foreach (IRecipe recipe in Cookbook.GetRecipes())
        {
            try
            {
                recipe.Description.Should().EndWith(".", "Cookbook recipe descriptions must end with a period");
            }
            catch (Exception)
            {
                Console.WriteLine(recipe.ToString());
                throw;
            }
        }
    }

    [Test]
    public void Test_ChapterDescription_ShouldNotEndWithPeriod()
    {
        foreach (RecipePageBase page in Cookbook.GetPages())
        {
            try
            {
                page.PageDetails.PageDescription.Should().NotEndWith(".", "Cookbook chatper descriptions should not end with a period");
            }
            catch (Exception)
            {
                Console.WriteLine(page.ToString());
                throw;
            }
        }
    }

    [Test]
    public void Test_PageNames_AreUnique()
    {
        string[] pageNames = Cookbook.GetPages().Select(x => x.PageDetails.PageName).ToArray();
        HashSet<string> uniqueNames = new(pageNames);

        pageNames.Length.Should().Be(uniqueNames.Count, "Cookbook page names must be universally unique");
    }

    [Test]
    public void Test_RecipeNames_AreUniqueWithinPages()
    {
        foreach (RecipePageBase page in Cookbook.GetPages())
        {
            string[] recipeNames = page.GetRecipes().Select(x => x.Name).ToArray();
            HashSet<string> uniqueNames = new(recipeNames);

            recipeNames.Length.Should().Be(uniqueNames.Count, "Recipe names must all be unique within the same page");
        }
    }
}
