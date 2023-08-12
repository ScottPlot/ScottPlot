namespace ScottPlotTests.RenderTests;

internal class RenderManagerTests
{
    [Test]
    public void Test_RenderManager_Output()
    {
        Plot plt = new();
        plt.Add.Signal(Generate.Sin(51));
        plt.Add.Signal(Generate.Cos(51));
        _ = plt.GetImageBytes(400, 300);

        var timedActions = plt.RenderManager.LastRender.TimedActions;
        timedActions.Should().NotBeNull();
        timedActions.Should().NotBeEmpty();

        foreach ((string action, TimeSpan elapsed) in timedActions)
        {
            elapsed.TotalMilliseconds.Should().BeGreaterThan(0);
            Console.WriteLine($"{elapsed.TotalMilliseconds} ms \t {action.Split(".").Last()}");
        }

        Console.WriteLine($"{plt.RenderManager.LastRender.Elapsed.TotalMilliseconds} ms \t TOTAL");
    }
}
