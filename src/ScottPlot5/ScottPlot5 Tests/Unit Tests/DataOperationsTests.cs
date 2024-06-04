namespace ScottPlotTests;

internal class DataOperationsTests
{
    [Test]
    public void Test_DataOperations_NaNToNull()
    {
        double[,] input =
        {
            { double.NaN, 2, 3, 4 },
            { 5, double.NaN, 7, 8 },
            { 9, 10, double.NaN, 12 },
            { 13, 14, 15, double.NaN }
        };

        double?[,] output = DataOperations.ReplaceNaNWithNull(input);

        for (int y = 0; y < input.GetLength(0); y++)
        {
            for (int x = 0; x < input.GetLength(1); x++)
            {
                if (double.IsNaN(input[y, x]))
                {
                    output[y, x].Should().BeNull();
                }
            }
        }
    }

    [Test]
    public void Test_DataOperations_NullToNaN()
    {
        double?[,] input =
        {
            { null, 2, 3, 4 },
            { 5, null, 7, 8 },
            { 9, 10, null, 12 },
            { 13, 14, 15, null }
        };

        double[,] output = DataOperations.ReplaceNullWithNaN(input);

        for (int y = 0; y < input.GetLength(0); y++)
        {
            for (int x = 0; x < input.GetLength(1); x++)
            {
                if (input[y, x] is null)
                {
                    double.IsNaN(output[y, x]).Should().BeTrue();
                }
            }
        }
    }

    [Test]
    public void Test_DataOperations_ResizeHalf()
    {
        double[,] input =
        {
            { 1, 2, 3, 4 },
            { 5, 6, 7, 8 },
            { 9, 10, 11, 12 },
            { 13, 14, 15, 16 }
        };

        double[,] output = DataOperations.ResizeHalf(input);

        output.GetLength(0).Should().Be(input.GetLength(0) / 2);
        output.GetLength(1).Should().Be(input.GetLength(1) / 2);

        output[0, 0].Should().Be((1 + 2 + 5 + 6) / 4f);
        output[0, 1].Should().Be((3 + 4 + 7 + 8) / 4f);
        output[1, 0].Should().Be((9 + 10 + 13 + 14) / 4f);
        output[1, 1].Should().Be((11 + 12 + 15 + 16) / 4f);
    }
}
