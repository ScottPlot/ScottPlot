using System;
using System.Collections.Generic;

namespace ScottPlot;

#nullable enable

public class RandomDataGenerator
{
    readonly Random Rand;

    /// <summary>
    /// Random seed
    /// </summary>
    public RandomDataGenerator()
    {
        Rand = new();
    }

    /// <summary>
    /// Defined seed
    /// </summary>
    public RandomDataGenerator(int seed = 0)
    {
        Rand = new Random(seed);
    }

    public double[] Random(int count, double mult = 1, double offset = 0)
    {
        double[] values = new double[count];
        for (int i = 0; i < count; i++)
            values[i] = Rand.NextDouble() * mult + offset;
        return values;
    }
    
    public double Random(double mult = 1, double offset = 0)
    {
        return Rand.NextDouble() * mult + offset;
    }

    public double[] RandomSin(int count)
    {
        double mult = Math.Pow(2, 1 + Rand.NextDouble() * 10);
        double offset = mult * (Rand.NextDouble() - .5);
        double oscillations = 1 + Rand.NextDouble() * 5;
        double phase = Rand.NextDouble() * Math.PI * 2;
        return Generate.Sin(count, mult, offset, oscillations, phase);
    }

    public double RandomNumber(double min, double max)
    {
        double span = max - min;
        return min + Rand.NextDouble() * span;
    }

    public double[] RandomWalk(int count, double mult = 1, double offset = 0)
    {
        double[] data = new double[count];
        data[0] = offset;
        for (int i = 1; i < data.Length; i++)
            data[i] = data[i - 1] + (Rand.NextDouble() * 2 - 1) * mult;
        return data;
    }

    public double NonZeroRandomDouble()
    {
        double randomValue = Rand.NextDouble();
        return randomValue != 0
            ? randomValue
            : NonZeroRandomDouble();
    }

    public double RandomNormal(double mean = 0, double stdDev = 1)
    {
        double u1 = NonZeroRandomDouble();
        double u2 = NonZeroRandomDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        return mean + stdDev * randStdNormal;
    }
}
