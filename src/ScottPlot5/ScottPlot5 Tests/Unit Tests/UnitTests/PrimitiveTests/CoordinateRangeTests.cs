namespace ScottPlotTests.UnitTests.PrimitiveTests;

internal class CoordinateRangeTests
{
    [Test]
    public void Test_CoordinateRange_Standard()
    {
        CoordinateRange range = new(2, 4);
        range.Value1.Should().Be(2);
        range.Value2.Should().Be(4);
        range.Min.Should().Be(2);
        range.Max.Should().Be(4);
        range.IsInverted.Should().BeFalse();
    }

    [Test]
    public void Test_CoordinateRange_Inverted()
    {
        CoordinateRange range = new(4, 2);
        range.Value1.Should().Be(4);
        range.Value2.Should().Be(2);
        range.Min.Should().Be(2);
        range.Max.Should().Be(4);
        range.IsInverted.Should().BeTrue();
    }

    [Test]
    public void Test_CoordinateRange_Equality()
    {
        CoordinateRange range1 = new(4, 2);
        CoordinateRange range2 = new(4, 2);
        range1.Should().Be(range2);
    }

    [Test]
    public void Test_CoordinateRange_ValueInequality()
    {
        CoordinateRange range1 = new(4, 2);
        CoordinateRange range2 = new(4, 3);
        range1.Should().NotBe(range2);
    }

    [Test]
    public void Test_CoordinateRange_InversionInequality()
    {
        CoordinateRange range1 = new(4, 2);
        CoordinateRange range2 = new(2, 4);
        range1.Should().NotBe(range2);
    }

    [Test]
    public void Test_CoordinateRange_InfinityEquality()
    {
        CoordinateRange range1 = new(double.NegativeInfinity, double.PositiveInfinity);
        CoordinateRange range2 = new(double.NegativeInfinity, double.PositiveInfinity);
        range1.Should().Be(range2);
    }

    [Test]
    public void Test_CoordinateRange_InfinityInequality()
    {
        CoordinateRange range1 = new(double.NegativeInfinity, double.PositiveInfinity);
        CoordinateRange range2 = new(double.PositiveInfinity, double.NegativeInfinity);
        range1.Should().NotBe(range2);
    }

    [Test]
    public void Test_CoordinateRange_Rectification()
    {
        new CoordinateRange(2, 4).Rectified().Should().Be(new CoordinateRange(2, 4));
        new CoordinateRange(4, 2).Rectified().Should().Be(new CoordinateRange(2, 4));
    }

    [Test]
    public void Test_CoordinateRange_Span()
    {
        new CoordinateRange(2, 10).Span.Should().Be(8);
        new CoordinateRange(-10, 2).Span.Should().Be(12);
        new CoordinateRange(-2, 10).Span.Should().Be(12);

        new CoordinateRange(10, 2).Span.Should().Be(-8);
        new CoordinateRange(2, -10).Span.Should().Be(-12);
        new CoordinateRange(10, -2).Span.Should().Be(-12);
    }

    [Test]
    public void Test_CoordinateRange_Center()
    {
        new CoordinateRange(2, 10).Center.Should().Be(6);
        new CoordinateRange(-10, 2).Center.Should().Be(-4);
        new CoordinateRange(-2, 10).Center.Should().Be(4);

        new CoordinateRange(10, 2).Center.Should().Be(6);
        new CoordinateRange(2, -10).Center.Should().Be(-4);
        new CoordinateRange(10, -2).Center.Should().Be(4);
    }

    [Test]
    public void Test_CoordinateRange_Extrema_Empty()
    {
        CoordinateRange.Extrema(Array.Empty<double>()).Should().Be(CoordinateRange.NoLimits);
    }

    [Test]
    public void Test_CoordinateRange_Extrema_Single_Value()
    {
        CoordinateRange.Extrema(new[] { 3.14159 }).Should().Be(new CoordinateRange(3.14159, 3.14159));
    }

    [Test]
    public void Test_CoordinateRange_Extrema_AllNaN()
    {
        CoordinateRange.Extrema(new[] { double.NaN, double.NaN }).Should().Be(CoordinateRange.NoLimits);
    }

    [Test]
    public void Test_CoordinateRange_Extrema_SomeNaN()
    {
        CoordinateRange.Extrema(new[] { 1.0, double.NaN, 2.0 }).Should().Be(new CoordinateRange(1.0, 2.0));
    }

    [Test]
    public void Test_CoordinateRange_Extrema_Normal()
    {
        CoordinateRange.Extrema(new[] { -1.0, 0.0, 1.0 }).Should().Be(new CoordinateRange(-1.0, 1.0));
    }

    [Test]
    public void Test_CoordinateRange_Length()
    {
        new CoordinateRange(2, 10).Length.Should().Be(8);
        new CoordinateRange(-10, 2).Length.Should().Be(12);
        new CoordinateRange(-2, 10).Length.Should().Be(12);

        new CoordinateRange(10, 2).Length.Should().Be(8);
        new CoordinateRange(2, -10).Length.Should().Be(12);
        new CoordinateRange(10, -2).Length.Should().Be(12);
    }
}
