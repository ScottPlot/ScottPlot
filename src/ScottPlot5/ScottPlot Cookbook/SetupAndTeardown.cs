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
            string folderName = Html.UrlSafe(page.PageDetails.PageName);
            string folderPath = Path.Combine(CookbookGenerator.OutputFolder, folderName);
            Directory.CreateDirectory(folderPath);
        }
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {

    }
}
