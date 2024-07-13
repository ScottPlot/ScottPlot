using ScottPlot.Interactivity;
using ScottPlot.Interactivity.UserActions;

namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class RightClickContextMenuTests
{
    const int FIGURE_WIDTH = 400;
    const int FIGURE_HEIGHT = 300;
    Pixel FIGURE_CENTER => new(FIGURE_WIDTH / 2, FIGURE_HEIGHT / 2);

    [Test]
    public void Test_RightClickContextMenu_LaunchesMenu()
    {
        MockPlotControl control = new();
        control.Refresh();
        control.ContextMenuLaunchCount.Should().Be(0);

        UserInputProcessor proc = new(control.Plot);

        proc.Process(new RightMouseDown(FIGURE_CENTER));
        proc.Process(new RightMouseUp(FIGURE_CENTER));
        control.ContextMenuLaunchCount.Should().Be(1);

        proc.Process(new RightMouseDown(FIGURE_CENTER));
        proc.Process(new RightMouseUp(FIGURE_CENTER));
        control.ContextMenuLaunchCount.Should().Be(2);
    }

    [Test]
    public void Test_RightClickDrag_DoesNotLaunchMenu()
    {
        MockPlotControl control = new();
        control.Refresh();
        control.ContextMenuLaunchCount.Should().Be(0);

        UserInputProcessor proc = new(control.Plot);

        proc.Process(new RightMouseDown(FIGURE_CENTER));
        proc.Process(new RightMouseUp(FIGURE_CENTER.WithOffset(10, 10)));
        control.ContextMenuLaunchCount.Should().Be(0);

        proc.Process(new RightMouseDown(FIGURE_CENTER));
        proc.Process(new RightMouseUp(FIGURE_CENTER));
        control.ContextMenuLaunchCount.Should().Be(1);
    }
}
