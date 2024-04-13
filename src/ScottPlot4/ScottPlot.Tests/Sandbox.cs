using NUnit.Framework;
using System.Drawing;
using ScottPlotTests;

namespace ScottPlot.Tests;
internal class Sandbox
{
    [Test]
    public void Test_Sandbox()
    {
        Plot plt = new();

        // place bars simulating groups with manually defined colors
        plt.AddBar(-1, 15, 3, Color.Red);
        plt.AddBar(0, 17, 4, Color.Orange);
        plt.AddBar(1, 13, 5, Color.Yellow);

        plt.AddBar(3, 14, 3, Color.Green);
        plt.AddBar(4, 15, 4, Color.Blue);
        var lastBar = plt.AddBar(5, 16, 5, Color.Violet);

        // bars can be further customized individually
        lastBar.HatchStyle = Drawing.HatchStyle.LargeCheckerBoard;
        lastBar.BorderLineWidth = 3;
        lastBar.ErrorCapSize = 0.2;
        lastBar.ErrorLineWidth = 2;
        lastBar.BarWidth = 0.5;

        // use manaual tick labels to name groups
        double[] tickPositions = { 0, 4 };
        string[] tickLabels = { "Group 1", "Group 2" };
        plt.BottomAxis.ManualTickPositions(tickPositions, tickLabels);

        // adjust axis limits so the view starts at zero
        plt.SetAxisLimits(yMin: 0);

        TestTools.SaveFig(plt);
    }
}
