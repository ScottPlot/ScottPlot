using System.Linq;
using System;

namespace ScottPlotTests.RenderTests.Figure;

public class Tests
{
    [Test]
    public void Test_Generate_Consecutive()
    {
        double[] values = ScottPlot.Generate.Consecutive(10);
        values.Should().NotBeNullOrEmpty();
        values.Should().HaveCount(10);
    }

    [Test]
    public void Test_RandomNormal()
    {
        double[] values = Generate.RandomNormal(10);
        Console.WriteLine(string.Join(Environment.NewLine, values.Select(x => x.ToString())));
    }

    [Test]
    public void Test_Range()
    {
        double[] values = Generate.Range(7, 9, 0.5);
        double[] expected = [7, 7.5, 8, 8.5, 9];
        values.Should().BeEquivalentTo(expected);
    }
}
