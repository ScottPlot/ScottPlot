using System.Globalization;

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

        // use invariant culture for all tests
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    }

    [OneTimeTearDown]
    public void RunAfterAllTests()
    {

    }
}
