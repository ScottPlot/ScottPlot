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
    }

    [OneTimeTearDown]
    public void RunAfterAllTests()
    {

    }
}
