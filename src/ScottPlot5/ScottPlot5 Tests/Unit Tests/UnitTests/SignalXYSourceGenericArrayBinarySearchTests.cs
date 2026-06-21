using ScottPlot.DataSources;

namespace ScottPlotTests.UnitTests;

internal class SignalXYSourceGenericArrayBinarySearchTests
{
    [TestCase(-0.25, 0)]
    [TestCase(0, 0)]
    [TestCase(0.25, 1)]
    [TestCase(1, 1)]
    [TestCase(1.25, 2)]
    [TestCase(1.5, 2)]
    [TestCase(1.75, 2)]
    [TestCase(3.75, 4)]
    [TestCase(4, 4)]
    [TestCase(4.25, 4)]
    public void Test_SignalXYSourceGenericArray_GetIndex_UsesDoubleInsertionIndex(double x, int expectedIndex)
    {
        int[] xs = [0, 1, 2, 3, 4];
        double[] ys = [0, 0, 0, 0, 0];
        SignalXYSourceGenericArray<int, double> source = new(xs, ys);

        int index = source.GetIndex(x);

        Assert.That(index, Is.EqualTo(expectedIndex));
    }

    [Test]
    public void Test_SignalXYSourceGenericArray_GetIndex_UsesDoubleInsertionIndexForIntegralTypes()
    {
        AssertInsertionIndex<sbyte>([-3, -1, 1, 3], 0.25, 2);
        AssertInsertionIndex<byte>([0, 2, 4, 6], 2.25, 2);
        AssertInsertionIndex<short>([-10, -5, 0, 5], -4.75, 2);
        AssertInsertionIndex<ushort>([0, 10, 20, 30], 19.75, 2);
        AssertInsertionIndex<int>([-10, -5, 0, 5], -4.75, 2);
        AssertInsertionIndex<uint>([0, 10, 20, 30], 19.75, 2);
        AssertInsertionIndex<long>([-10, -5, 0, 5], -4.75, 2);
        AssertInsertionIndex<ulong>([0, 10, 20, 30], 19.75, 2);
    }

    [TestCase(1.75, 2)]
    [TestCase(2, 2)]
    [TestCase(4.25, 5)]
    [TestCase(5, 5)]
    [TestCase(5.25, 5)]
    public void Test_SignalXYSourceGenericArray_GetIndex_LimitsResultsToIndexRange(double x, int expectedIndex)
    {
        int[] xs = [0, 1, 2, 3, 4, 5, 6];
        double[] ys = [0, 0, 0, 0, 0, 0, 0];
        SignalXYSourceGenericArray<int, double> source = new(xs, ys);
        IndexRange range = new(2, 5);

        int index = source.GetIndex(x, range);

        Assert.That(index, Is.EqualTo(expectedIndex));
    }

    [TestCase(109.5, 1)]
    [TestCase(110, 1)]
    [TestCase(110.5, 2)]
    [TestCase(149.5, 5)]
    [TestCase(150, 5)]
    [TestCase(150.5, 5)]
    public void Test_SignalXYSourceGenericArray_GetIndex_HandlesOffsetAndScale(double x, int expectedIndex)
    {
        int[] xs = [0, 1, 2, 3, 4, 5];
        double[] ys = [0, 0, 0, 0, 0, 0];
        SignalXYSourceGenericArray<int, double> source = new(xs, ys)
        {
            XOffset = 100,
            XScale = 10,
        };

        int index = source.GetIndex(x);

        Assert.That(index, Is.EqualTo(expectedIndex));
    }

    private static void AssertInsertionIndex<TX>(TX[] xs, double x, int expectedIndex)
    {
        double[] ys = new double[xs.Length];
        SignalXYSourceGenericArray<TX, double> source = new(xs, ys);

        int index = source.GetIndex(x);

        Assert.That(index, Is.EqualTo(expectedIndex));
    }
}
