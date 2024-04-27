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
}
