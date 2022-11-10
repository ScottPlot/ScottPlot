namespace ScottPlotCookbook;

[SetUpFixture]
internal class SetupAndTeardown
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        // delete all old cookbook content
        if (Directory.Exists(CookbookGenerator.OutputFolder))
            Directory.Delete(CookbookGenerator.OutputFolder, true);

        // create a fresh cookbook folder
        Directory.CreateDirectory(CookbookGenerator.OutputFolder);

        // create subfolders for every page
        foreach (RecipePageBase page in Cookbook.GetPages())
        {
            string pageFolder = Path.Combine(CookbookGenerator.OutputFolder, Html.GetPageUrl(page));
            Directory.CreateDirectory(pageFolder);
        }
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {

    }
}
