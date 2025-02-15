namespace ScottPlotTests.UnitTests.PrimitiveTests;

public class AngleTests
{
    [Test]
    public void Test_Angle_Equality()
    {
        Angle.FromDegrees(111).Should().BeEquivalentTo(Angle.FromDegrees(111));
        Angle.FromDegrees(111).Should().NotBeEquivalentTo(Angle.FromDegrees(222));
        Angle.FromRadians(1.11).Should().BeEquivalentTo(Angle.FromRadians(1.11));
        Angle.FromRadians(1.11).Should().NotBeEquivalentTo(Angle.FromRadians(2.22));
    }

    [Test]
    [TestCase([0, 0])]
    [TestCase([90, Math.PI / 2])]
    [TestCase([-90, -Math.PI / 2])]
    [TestCase([180, Math.PI])]
    [TestCase([-180, -Math.PI])]
    [TestCase([360, Math.PI * 2])]
    [TestCase([-360, -Math.PI * 2])]
    public void Test_Angle_DegreesToRadians(double degrees, double radians)
    {
        Angle.FromDegrees(degrees).Should().BeEquivalentTo(Angle.FromRadians(radians));
        Angle.FromRadians(radians).Should().BeEquivalentTo(Angle.FromDegrees(degrees));
    }

    [Test]
    public void Test_Angle_Normalization()
    {
        Angle.FromDegrees(375).Normalized.Should().BeEquivalentTo(Angle.FromDegrees(15));
    }

    [Test]
    public void Test_Angle_Multiply()
    {
        (Angle.FromDegrees(10) * 2).Should().BeEquivalentTo(Angle.FromDegrees(20));
        (Angle.FromDegrees(-10) * 2).Should().BeEquivalentTo(Angle.FromDegrees(-20));
        (Angle.FromDegrees(100) * 5).Should().BeEquivalentTo(Angle.FromDegrees(500));
        (Angle.FromDegrees(-100) * 5).Should().BeEquivalentTo(Angle.FromDegrees(-500));
    }

    [Test]
    public void Test_Angle_Divide()
    {
        (Angle.FromDegrees(20) / 2).Should().BeEquivalentTo(Angle.FromDegrees(10));
        (Angle.FromDegrees(-20) / 2).Should().BeEquivalentTo(Angle.FromDegrees(-10));
        (Angle.FromDegrees(500) / 5).Should().BeEquivalentTo(Angle.FromDegrees(100));
        (Angle.FromDegrees(-500) / 5).Should().BeEquivalentTo(Angle.FromDegrees(-100));
    }

    [Test]
    public void Test_Angle_Modulus()
    {
        (Angle.FromDegrees(100) % 90).Should().BeEquivalentTo(Angle.FromDegrees(10));
        (Angle.FromDegrees(-100) % 90).Should().BeEquivalentTo(Angle.FromDegrees(-10));
    }

    [Test]
    public void Test_Angle_FromFraction()
    {
        (Angle.FromFraction(0)).Should().BeEquivalentTo(Angle.FromDegrees(0));
        (Angle.FromFraction(0.25)).Should().BeEquivalentTo(Angle.FromDegrees(90));
        (Angle.FromFraction(-0.25)).Should().BeEquivalentTo(Angle.FromDegrees(-90));
        (Angle.FromFraction(2)).Should().BeEquivalentTo(Angle.FromDegrees(720));
        (Angle.FromFraction(0.25, clockwise: true)).Should().BeEquivalentTo(Angle.FromDegrees(-90));
        (Angle.FromFraction(0.25, Angle.FromDegrees(90))).Should().BeEquivalentTo(Angle.FromDegrees(180));
    }
}
