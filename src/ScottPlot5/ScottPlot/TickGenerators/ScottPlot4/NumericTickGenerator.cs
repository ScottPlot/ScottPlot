using System.Globalization;

namespace ScottPlot.TickGenerators.ScottPlot4;

internal class NumericTickGenerator : ITickGenerator
{
    private readonly bool IsVertical;

    public NumericTickGenerator(bool isVertical)
    {
        IsVertical = isVertical;
    }

    public Tick[] GenerateTicks(double min, double max, float edgeSize)
    {
        return GenerateTicks(min, max, edgeSize, 12, 12);
    }


    public Tick[] GenerateTicks(double min, double max, float edgeSize, float labelWidthEstimate, float labelHeightEstimate, int depth = 0)
    {
        double span = max - min;
        double unitsPerPx = span / edgeSize;

        float labelSize = IsVertical ? labelHeightEstimate : labelWidthEstimate;
        float tickDensity = 1.0f;
        int targetTickCount = (int)(edgeSize / labelSize * tickDensity);
        double tickSpacing = GetIdealTickSpacing(min, max, targetTickCount);

        double firstTickOffset = min % tickSpacing;
        int tickCount = (int)(span / tickSpacing) + 2;
        tickCount = Math.Min(1000, tickCount);
        tickCount = Math.Max(1, tickCount);

        double[] majorTickPositions = Enumerable.Range(0, tickCount)
            .Select(x => min - firstTickOffset + tickSpacing * x)
            .Where(x => min <= x && x <= max)
            .ToArray();

        if (majorTickPositions.Length < 2)
        {
            double tickBelow = min - firstTickOffset;
            double firstTick = majorTickPositions.Length > 0 ? majorTickPositions[0] : tickBelow;
            double nextTick = tickBelow + tickSpacing;
            majorTickPositions = new double[] { firstTick, nextTick };
        }

        string[] majorTickLabels = majorTickPositions.Select(position => GetPrettyTickLabel(position)).ToArray();

        (float Width, float Height) maxTickBounds = new();
        using var paint = new SkiaSharp.SKPaint();
        for (int i = 0; i < majorTickLabels.Length; i++)
        {
            PixelSize tickSize = Drawing.MeasureString(majorTickLabels[i], paint);
            maxTickBounds.Width = Math.Max(maxTickBounds.Width, tickSize.Width);
            maxTickBounds.Height = Math.Max(maxTickBounds.Height, tickSize.Height);
        }

        if (maxTickBounds.Width > labelWidthEstimate || maxTickBounds.Height > labelHeightEstimate)
        {
            return GenerateTicks(min, max, edgeSize, Math.Max(labelWidthEstimate, maxTickBounds.Width), Math.Max(labelHeightEstimate, maxTickBounds.Height), depth + 1);
        }

        Tick[] majorTicks = majorTickPositions
            .Select((position, i) => Tick.Major(position, majorTickLabels[i]))
            .ToArray();

        int minorTicksPerMajorTick = 5;
        double[] minorTickPositions = MinorFromMajor(majorTickPositions, minorTicksPerMajorTick, min, max);
        Tick[] minorTicks = minorTickPositions
            .Select(position => Tick.Minor(position))
            .ToArray();

        System.Diagnostics.Debug.WriteLine("Depth {0}", depth);
        return majorTicks.Concat(minorTicks).ToArray();
    }

    private static double GetIdealTickSpacing(double low, double high, int maxTickCount)
    {
        int radix = 10;
        double range = high - low;
        int exponent = (int)Math.Log(range, radix);
        double initialSpace = Math.Pow(radix, exponent);
        List<double> tickSpacings = new() { initialSpace, initialSpace, initialSpace };

        double[] divBy;
        if (radix == 10)
            divBy = new double[] { 2, 2, 2.5 }; // 10, 5, 2.5, 1
        else if (radix == 16)
            divBy = new double[] { 2, 2, 2, 2 }; // 16, 8, 4, 2, 1
        else
            throw new ArgumentException($"radix {radix} is not supported");

        int divisions = 0;
        int tickCount = 0;
        while (tickCount < maxTickCount && tickSpacings.Count < 1000)
        {
            tickSpacings.Add(tickSpacings.Last() / divBy[divisions++ % divBy.Length]);
            tickCount = (int)(range / tickSpacings.Last());
        }

        return tickSpacings[tickSpacings.Count - 3];
    }

    private static double[] MinorFromMajor(double[] majorTicks, double minorTicksPerMajorTick, double lowerLimit, double upperLimit)
    {
        if (majorTicks == null || majorTicks.Length < 2)
            return new double[] { };

        double majorTickSpacing = majorTicks[1] - majorTicks[0];
        double minorTickSpacing = majorTickSpacing / minorTicksPerMajorTick;

        List<double> majorTicksWithPadding = new()
        {
            majorTicks[0] - majorTickSpacing
        };
        majorTicksWithPadding.AddRange(majorTicks);

        List<double> minorTicks = new();
        foreach (var majorTickPosition in majorTicksWithPadding)
        {
            for (int i = 1; i < minorTicksPerMajorTick; i++)
            {
                double minorTickPosition = majorTickPosition + minorTickSpacing * i;
                if (minorTickPosition > lowerLimit && minorTickPosition < upperLimit)
                    minorTicks.Add(minorTickPosition);
            }
        }

        return minorTicks.ToArray();
    }

    private static string GetPrettyTickLabel(double position)
    {
        string label = FormatLocal(position, CultureInfo.CurrentCulture);
        return (label == "-0") ? "0" : label;
    }

    private static string FormatLocal(double value, CultureInfo culture)
    {
        // if the number is round or large, use the numeric format
        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#the-numeric-n-format-specifier
        bool isRoundNumber = (int)value == value;
        bool isLargeNumber = Math.Abs(value) > 1000;
        if (isRoundNumber || isLargeNumber)
            return value.ToString("N0", culture);

        // otherwise the number is probably small or very precise to use the general format (with slight rounding)
        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#the-general-g-format-specifier
        return Math.Round(value, 10).ToString("G", culture);
    }
}
