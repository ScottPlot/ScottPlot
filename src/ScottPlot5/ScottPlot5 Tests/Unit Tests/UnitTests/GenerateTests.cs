namespace ScottPlotTests.UnitTests;

internal class GenerateTests
{
    [Test]
    public void Test_Weekdays()
    {
        DateTime[] dates = Generate.ConsecutiveWeekdays(100);
        foreach (DateTime dt in dates)
        {
            dt.DayOfWeek.Should().NotBe(DayOfWeek.Saturday);
            dt.DayOfWeek.Should().NotBe(DayOfWeek.Sunday);
        }
    }


    [Test]
    public void Test_RandomNumbers()
    {
        double[] nums = Generate.RandomNumbers(100, 34, 38);
        foreach (var n in nums)
        {
            n.Should().BeGreaterOrEqualTo(34);
            n.Should().BeLessOrEqualTo(38);
        }
    }
}
