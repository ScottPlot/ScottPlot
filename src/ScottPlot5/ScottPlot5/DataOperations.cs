using System;

namespace ScottPlot;

#nullable enable

public static class DataOperations
{
    public static double[,] ResizeHalf(double[,] values)
    {
        int height = values.GetLength(0);
        int width = values.GetLength(1);

        int heightNew = (int)Math.Floor((double)height / 2);
        int widthNew = (int)Math.Floor((double)width / 2);

        double[,] output = new double[heightNew, widthNew];

        for (int y = 0; y < heightNew; y++)
        {
            for (int x = 0; x < widthNew; x++)
            {
                double sum = 0;
                sum += values[y * 2, x * 2];
                sum += values[y * 2 + 1, x * 2];
                sum += values[y * 2, x * 2 + 1];
                sum += values[y * 2 + 1, x * 2 + 1];
                output[y, x] = sum / 4;
            }
        }

        return output;
    }

    public static double[,] ReplaceNullWithNaN(double?[,] values)
    {
        int height = values.GetLength(0);
        int width = values.GetLength(1);
        double[,] output = new double[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                output[y, x] = values[y, x] ?? double.NaN;
            }
        }

        return output;
    }

    public static double?[,] ReplaceNaNWithNull(double[,] values)
    {
        int height = values.GetLength(0);
        int width = values.GetLength(1);
        double?[,] output = new double?[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                output[y, x] = double.IsNaN(values[y, x]) ? null : values[y, x];
            }
        }

        return output;
    }
}