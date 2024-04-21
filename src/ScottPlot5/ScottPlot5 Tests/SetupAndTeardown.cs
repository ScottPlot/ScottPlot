namespace ScottPlotTests;

[SetUpFixture]
internal class SetupAndTeardown
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        // delete old test images
        if (Directory.Exists(Paths.OutputFolder))
            Directory.Delete(Paths.OutputFolder, true);

        // create a fresh output folder
        Directory.CreateDirectory(Paths.OutputFolder);
    }

    [OneTimeTearDown]
    public void RunAfterAllTests()
    {

    }
}
