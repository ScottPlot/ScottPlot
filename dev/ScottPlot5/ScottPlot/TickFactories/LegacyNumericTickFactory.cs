using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ScottPlot.TickFactories;

/// <summary>
/// Mimic numeric tick behavior of ScottPlot4 (niche features omitted)
/// </summary>
internal class LegacyNumericTickFactory : ITickFactory
{
    public Edge Edge { get; private set; }

    public LegacyNumericTickFactory(Edge edge)
    {
        Edge = edge;
    }

    public Tick[] GenerateTicks(PlotConfig info)
    {
        return Edge switch
        {
            Edge.Bottom => RecalculatePositionsAutomaticNumeric(info, Edge.Bottom),
            Edge.Left => RecalculatePositionsAutomaticNumeric(info, Edge.Left),
            Edge.Top => RecalculatePositionsAutomaticNumeric(info, Edge.Top),
            Edge.Right => RecalculatePositionsAutomaticNumeric(info, Edge.Right),
            _ => throw new NotImplementedException($"Unsupported {nameof(Edge)}: {Edge}"),
        };
    }

    private static Tick[] RecalculatePositionsAutomaticNumeric(PlotConfig info, Edge edge)
    {
        float labelWidth = 30;
        float labelHeight = 12;
        float tickDensity = 1.0f;
        double low, high, tickSpacing;
        int MinorTickCount = 5;
        int maxTickCount;

        bool isVertical = (edge == Edge.Left) || (edge == Edge.Right);

        if (isVertical)
        {
            low = info.AxisLimits.YMin - info.UnitsPerPxY; // add an extra pixel to capture the edge tick
            high = info.AxisLimits.YMax + info.UnitsPerPxY; // add an extra pixel to capture the edge tick
            maxTickCount = (int)(info.DataRect.Height / labelHeight * tickDensity);
            tickSpacing = GetIdealTickSpacing(low, high, maxTickCount);
        }
        else
        {
            low = info.AxisLimits.XMin - info.UnitsPerPxX; // add an extra pixel to capture the edge tick
            high = info.AxisLimits.XMax + info.UnitsPerPxX; // add an extra pixel to capture the edge tick
            maxTickCount = (int)(info.DataRect.Width / labelWidth * tickDensity);
            tickSpacing = GetIdealTickSpacing(low, high, maxTickCount);
        }

        double firstTickOffset = low % tickSpacing;
        int tickCount = (int)((high - low) / tickSpacing) + 2;
        tickCount = Math.Min(1000, tickCount);
        tickCount = Math.Max(1, tickCount);

        double[] tickPositionsMajor = Enumerable.Range(0, tickCount)
            .Select(x => low - firstTickOffset + tickSpacing * x)
            .Where(x => low <= x && x <= high)
            .ToArray();

        if (tickPositionsMajor.Length < 2)
        {
            double tickBelow = low - firstTickOffset;
            double firstTick = tickPositionsMajor.Length > 0 ? tickPositionsMajor[0] : tickBelow;
            double nextTick = tickBelow + tickSpacing;
            tickPositionsMajor = new double[] { firstTick, nextTick };
        }

        double[] tickPositionsMinor = MinorFromMajor(tickPositionsMajor, MinorTickCount, low, high);

        string[] tickLabels = GetPrettyTickLabels(tickPositionsMajor);

        List<Tick> ticks = new();

        for (int i = 0; i < tickPositionsMajor.Length; i++)
        {
            Tick tick = new(tickPositionsMajor[i], edge);
            tick.TickMarkLength = 5;
            tick.GridLineWidth = 1;
            tick.Label.Text = tickLabels[i];
            ticks.Add(tick);
        }

        for (int i = 0; i < tickPositionsMinor.Length; i++)
        {
            Tick tick = new(tickPositionsMinor[i], edge);
            tick.TickMarkLength = 3;
            tick.GridLineWidth = 0;
            ticks.Add(tick);
        }

        return ticks.ToArray();
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
        while ((tickCount < maxTickCount) && (tickSpacings.Count < 1000))
        {
            tickSpacings.Add(tickSpacings.Last() / divBy[divisions++ % divBy.Length]);
            tickCount = (int)(range / tickSpacings.Last());
        }

        return tickSpacings[tickSpacings.Count - 3];
    }

    private static string[] GetPrettyTickLabels(double[] positions)
    {
        string[] labels = new string[positions.Length];

        if (positions.Length == 0)
            return (labels);

        for (int i = 0; i < positions.Length; i++)
        {
            labels[i] = FormatLocal(positions[i], CultureInfo.CurrentCulture);
            if (labels[i] == "-0")
                labels[i] = "0";
        }

        return labels;
    }

    private static string FormatLocal(double value, CultureInfo culture)
    {
        // if the number is round or large, use the numeric format
        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#the-numeric-n-format-specifier
        bool isRoundNumber = ((int)value == value);
        bool isLargeNumber = (Math.Abs(value) > 1000);
        if (isRoundNumber || isLargeNumber)
            return value.ToString("N0", culture);

        // otherwise the number is probably small or very precise to use the general format (with slight rounding)
        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#the-general-g-format-specifier
        return Math.Round(value, 10).ToString("G", culture);
    }

    private static double[] MinorFromMajor(double[] majorTicks, double minorTicksPerMajorTick, double lowerLimit, double upperLimit)
    {
        if ((majorTicks == null) || (majorTicks.Length < 2))
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
                if ((minorTickPosition > lowerLimit) && (minorTickPosition < upperLimit))
                    minorTicks.Add(minorTickPosition);
            }
        }

        return minorTicks.ToArray();
    }
}
