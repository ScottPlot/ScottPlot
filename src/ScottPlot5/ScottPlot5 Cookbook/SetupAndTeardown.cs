using ScottPlotCookbook.HtmlPages;

namespace ScottPlotCookbook;

[SetUpFixture]
internal class SetupAndTeardown
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        // delete all old cookbook content
        if (Directory.Exists(Cookbook.OutputFolder))
            Directory.Delete(Cookbook.OutputFolder, true);

        // create a fresh cookbook folder
        Directory.CreateDirectory(Cookbook.OutputFolder);

        // create subfolders for every page
        foreach (RecipePageBase page in Cookbook.GetPages())
        {
            string pageFolder = Path.Combine(Cookbook.OutputFolder, UrlTools.GetPageUrl(page));
            Directory.CreateDirectory(pageFolder);
        }
    }

    [OneTimeTearDown]
    public void RunAfterAllTests()
    {

    }
}
