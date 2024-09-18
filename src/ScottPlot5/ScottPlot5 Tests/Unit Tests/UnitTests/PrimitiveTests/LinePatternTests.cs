namespace ScottPlotTests.UnitTests.PrimitiveTests;

public class LinePatternTests
{
    [Test]
    public void Test_EveryLinePattern_HasPathEffect()
    {
        LinePattern[] patterns = new LinePattern[] {
            LinePattern.Solid,
            LinePattern.Dashed,
            LinePattern.DenselyDashed,
            LinePattern.Dotted,
            new LinePattern(new float[] { 10, 3 }, 5)
        };
        foreach (LinePattern pattern in patterns)
        {
            Action act = () => pattern.GetPathEffect();
            act.Should().NotThrow();
        }
    }
}
