namespace ScottPlot;

public static class Generate
{
    public static double[] Consecutive(int count)
    {
        double[] result = new double[count];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = i;
        }

        return result;
    }
    public static double[] Sin(int count)
    {
        double[] result = new double[count];

        double step = 2 * Math.PI / count;

        for (int i = 0; i < count; i++)
        {
            result[i] = Math.Sin(i * step);
        }

        return result;
    }

    public static double[] Cos(int count)
    {
        double[] result = new double[count];

        double step = 2 * Math.PI / count;

        for (int i = 0; i < count; i++)
        {
            result[i] = Math.Cos(i * step);
        }

        return result;
    }
}
