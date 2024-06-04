using ScottPlot.TickGenerators;
using ScottPlot.TickGenerators.TimeUnits;
using SkiaSharp;
using DateTime = System.DateTime;

namespace ScottPlotTests.UnitTests.TickGenerators;

public class DateTimeFixedIntervalsTests
{
    [Test]
    public void Test_HourlyTicks_GeneratesTicksFromStartTimeOfRange()
    {
        DateTimeFixedInterval gen = new(new Hour());

        DateTime startRange = new(2000, 1, 1, 0, 30, 30);
        DateTime endRange = new(2000, 1, 2, 0, 30, 30);

        CoordinateRange range = new(
            NumericConversion.ToNumber(startRange),
            NumericConversion.ToNumber(endRange)
        );

        gen.Regenerate(range, Edge.Bottom, new PixelLength(1), new SKPaint(), new Label());

        gen.Ticks.Skip(1).First().Position.Should().Be(NumericConversion.ToNumber(startRange.AddHours(1)));
    }

    [Test]
    public void Test_DailyTicksWithHourlyMinor_GeneratesTicksFromStartTimeOfRange()
    {
        DateTimeFixedInterval gen = new(new Day(), 1, new Hour());

        DateTime startRange = new(2000, 1, 1);
        DateTime endRange = startRange.AddDays(2);

        CoordinateRange range = new(
            NumericConversion.ToNumber(startRange),
            NumericConversion.ToNumber(endRange)
        );

        gen.Regenerate(range, Edge.Bottom, new PixelLength(1), new SKPaint(), new Label());

        // 1st major tick is start
        gen.Ticks.First(t => t.IsMajor).Position.Should().Be(NumericConversion.ToNumber(startRange));
        // 2nd major tick is for the next day
        gen.Ticks.Where(t => t.IsMajor).Skip(1).First().Position.Should().Be(NumericConversion.ToNumber(startRange.AddDays(1)));

        // 1st minor tick is 1 hour after start.
        gen.Ticks.First(t => !t.IsMajor).Position.Should().Be(NumericConversion.ToNumber(startRange.AddHours(1)));
    }

    [Test]
    public void Test_2DaysMajor6HoursMinor_GeneratesTicksFromStartTimeOfRange()
    {
        DateTimeFixedInterval gen = new(new Day(), 2, new Hour(), 6);

        DateTime startRange = new(2000, 1, 1);
        DateTime endRange = startRange.AddDays(6);

        CoordinateRange range = new(
            NumericConversion.ToNumber(startRange),
            NumericConversion.ToNumber(endRange)
        );

        gen.Regenerate(range, Edge.Bottom, new PixelLength(1), new SKPaint(), new Label());

        // 1st major tick is start
        gen.Ticks.First(t => t.IsMajor).Position.Should().Be(NumericConversion.ToNumber(startRange));
        // 2nd major tick is for 2 days from start
        gen.Ticks.Where(t => t.IsMajor).Skip(1).First().Position.Should().Be(NumericConversion.ToNumber(startRange.AddDays(2)));

        // 1st minor tick is 6 hours after start.
        gen.Ticks.First(t => !t.IsMajor).Position.Should().Be(NumericConversion.ToNumber(startRange.AddHours(6)));
    }

    [Test]
    public void Test_6HoursMajor1HourMinorStartTicksAtMidnight_GeneratesTicksAtExpectedPoints()
    {
        DateTimeFixedInterval gen = new(new Hour(), 6, new Hour(), 1,
            dt => new DateTime(dt.Year, dt.Month, dt.Day));

        // Make start range not coincident with any tick markers
        DateTime startRange = new(2000, 1, 1, 2, 15, 0);
        DateTime endRange = startRange.AddDays(1);

        CoordinateRange range = new(
            NumericConversion.ToNumber(startRange),
            NumericConversion.ToNumber(endRange)
        );

        gen.Regenerate(range, Edge.Bottom, new PixelLength(1), new SKPaint(), new Label());

        // 1st minor tick is 3am, 45minutes after range start.
        gen.Ticks.First(t => !t.IsMajor).Position.Should().Be(NumericConversion.ToNumber(startRange.AddMinutes(45)));

        // 1st major tick is 6am, 3hrs 45min after range start
        gen.Ticks.First(t => t.IsMajor).Position.Should().Be(NumericConversion.ToNumber(startRange.AddHours(3).AddMinutes(45)));
    }
}
