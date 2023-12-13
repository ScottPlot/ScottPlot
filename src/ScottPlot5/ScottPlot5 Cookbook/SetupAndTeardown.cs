namespace ScottPlotCookbook;

[SetUpFixture]
internal class SetupAndTeardown
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        // delete all old cookbook content
        if (Directory.Exists(Paths.OutputFolder))
            Directory.Delete(Paths.OutputFolder, true);

        // create a fresh cookbook folder
        Directory.CreateDirectory(Paths.OutputFolder);
    }

    [OneTimeTearDown]
    public void RunAfterAllTests()
    {

    }
}
