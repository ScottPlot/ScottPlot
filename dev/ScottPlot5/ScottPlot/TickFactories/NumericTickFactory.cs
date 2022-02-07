using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.TickFactories;

public class NumericTickFactory : ITickFactory
{
    public Tick[] GenerateTicks(PlotInfo info, Edge edge)
    {
        return edge switch
        {
            Edge.Bottom => GetEvenlySpacedTicks(info.AxisLimits.XMin, info.AxisLimits.XMax, 25, Edge.Bottom),
            Edge.Left => GetEvenlySpacedTicks(info.AxisLimits.YMin, info.AxisLimits.YMax, 50, Edge.Left),
            _ => throw new NotImplementedException(),
        };
    }

    private Tick[] GetEvenlySpacedTicks(double min, double max, double count, Edge edge)
    {
        List<Tick> ticks = new();

        double span = max - min;
        for (int i = 0; i <= count; i++)
        {
            double frac = i / count;
            double position = min + frac * span;
            Tick tick = new(position, edge);

            if (i % 5 == 0)
            {
                tick.Label = $"{position:N2}";
                tick.IsMajor = true;
            }
            else
            {
                tick.TickLength = 2;
            }

            ticks.Add(tick);
        }

        return ticks.ToArray();
    }
}