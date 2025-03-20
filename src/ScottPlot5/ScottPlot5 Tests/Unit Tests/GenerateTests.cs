namespace ScottPlotTests.GenerateTests;

public class GenerateTests
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

    [Test]
    public void Test_RangeRound()
    {
        double[] values = Generate.Range(0, 1, 0.05);
        values.Length.Should().Be(21);
        values[0].Should().Be(0);
        values[1].Should().Be(.05);
        values[2].Should().Be(.10);
        values[^2].Should().Be(.95);
        values[^1].Should().Be(1);
    }

    [Test]
    public void Test_Generate_Consecutive_DateTime()
    {
        var values = Generate.Consecutive(10, new DateTime(2020, 1, 1), TimeSpan.FromDays(1));
        DateTime[] expected = [
            new(2020, 1, 1),
            new(2020, 1, 2),
            new(2020, 1, 3),
            new(2020, 1, 4),
            new(2020, 1, 5),
            new(2020, 1, 6),
            new(2020, 1, 7),
            new(2020, 1, 8),
            new(2020, 1, 9),
            new(2020, 1, 10),
        ];
        values.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Test_Generate_Consecutive_DateTimeOffset()
    {
        var values = Generate.Consecutive(10, new DateTimeOffset(new DateTime(2020, 1, 1), TimeSpan.FromHours(-7)), TimeSpan.FromDays(1));
        DateTimeOffset[] expected = [
            new(new DateTime(2020, 1, 1), TimeSpan.FromHours(-7)),
            new(new DateTime(2020, 1, 2), TimeSpan.FromHours(-7)),
            new(new DateTime(2020, 1, 3), TimeSpan.FromHours(-7)),
            new(new DateTime(2020, 1, 4), TimeSpan.FromHours(-7)),
            new(new DateTime(2020, 1, 5), TimeSpan.FromHours(-7)),
            new(new DateTime(2020, 1, 6), TimeSpan.FromHours(-7)),
            new(new DateTime(2020, 1, 7), TimeSpan.FromHours(-7)),
            new(new DateTime(2020, 1, 8), TimeSpan.FromHours(-7)),
            new(new DateTime(2020, 1, 9), TimeSpan.FromHours(-7)),
            new(new DateTime(2020, 1, 10), TimeSpan.FromHours(-7)),
        ];
        values.Should().BeEquivalentTo(expected);
    }
}
