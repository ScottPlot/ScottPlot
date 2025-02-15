using ScottPlot.Interactivity;

namespace ScottPlotTests.InteractivityTests;

internal class KeyboardStateTests
{
    [Test]
    public void Test_KeyboardState_RemembersKey()
    {
        KeyboardState ks = new();
        ks.PressedKeyCount.Should().Be(0);

        ks.Add(StandardKeys.Shift);
        ks.PressedKeyCount.Should().Be(1);
        ks.GetPressedKeyNames.Should().Contain(StandardKeys.Shift.Name);
        ks.IsPressed(StandardKeys.Shift).Should().BeTrue();
    }
}
