namespace ScottPlot.Tests.UnitTests;

internal class ColorTests
{
    [TestCase("#abcdef", 0xAB, 0xCD, 0xEF, 0xFF)]
    [TestCase("#ABCDEF", 0xAB, 0xCD, 0xEF, 0xFF)]
    [TestCase("#123456", 0x12, 0x34, 0x56, 0xFF)]
    [TestCase("#abcdefaa", 0xAB, 0xCD, 0xEF, 0xAA)]
    [TestCase("#ABCDEFaa", 0xAB, 0xCD, 0xEF, 0xAA)]
    [TestCase("#123456aa", 0x12, 0x34, 0x56, 0xAA)]
    public void TestFromHex(string hex, byte expectedR, byte expectedG, byte expectedB, byte expectedA)
    {
        Color actual = Color.FromHex(hex);
        Assert.That(actual.Red, Is.EqualTo(expectedR));
        Assert.That(actual.Green, Is.EqualTo(expectedG));
        Assert.That(actual.Blue, Is.EqualTo(expectedB));
        Assert.That(actual.Alpha, Is.EqualTo(expectedA));
    }
}
