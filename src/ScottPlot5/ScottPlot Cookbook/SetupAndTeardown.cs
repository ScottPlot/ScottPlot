using ScottPlotCookbook.HtmlPages;

namespace ScottPlotCookbook;

[SetUpFixture]
internal class SetupAndTeardown
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        // delete all old cookbook content
        if (Directory.Exists(HtmlPageGenerator.OutputFolder))
            Directory.Delete(HtmlPageGenerator.OutputFolder, true);

        // create a fresh cookbook folder
        Directory.CreateDirectory(HtmlPageGenerator.OutputFolder);

        // create subfolders for every page
        foreach (RecipePageBase page in Cookbook.GetPages())
        {
            string pageFolder = Path.Combine(HtmlPageGenerator.OutputFolder, UrlTools.GetPageUrl(page));
            Directory.CreateDirectory(pageFolder);
        }
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {

    }
}
