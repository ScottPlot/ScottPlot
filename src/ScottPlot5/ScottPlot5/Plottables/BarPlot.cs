﻿using ScottPlot.Interfaces;

namespace ScottPlot.Plottables;

/// <summary>
/// Holds a collection of individually styled bars
/// </summary>
public class BarPlot : IPlottable
{
    public string Label { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public IEnumerable<Bar> Bars { get; set; } // TODO: bars data source

    /// <summary>
    /// Apply a fill color to all bars
    /// </summary>
    public Color Color
    {
        set
        {
            foreach (Bar bar in Bars)
            {
                bar.FillColor = value;
            }
        }
    }

    /// <summary>
    /// Define orientation for all bars
    /// </summary>
    public bool Horizontal
    {
        set
        {
            foreach (Bar bar in Bars)
            {
                bar.Orientation = value
                    ? Orientation.Horizontal
                    : Orientation.Vertical;
            }
        }
    }

    public BarPlot(Bar bar)
    {
        Bars = new Bar[] { bar };
    }

    public BarPlot(IEnumerable<Bar> bars)
    {
        Bars = bars;
    }

    public IEnumerable<LegendItem> LegendItems
    {
        get
        {
            if (!Bars.Any())
            {
                return LegendItem.None;
            }

            LegendItem item = new()
            {
                Label = Label,
                FillColor = Bars.First().FillColor,
            };

            return LegendItem.Single(item);
        }
    }

    public AxisLimits GetAxisLimits()
    {
        ExpandingAxisLimits limits = new();

        foreach (Bar bar in Bars)
        {
            limits.Expand(bar.AxisLimits);
        }

        return limits.AxisLimits;
    }

    public void Render(RenderPack rp)
    {
        using var paint = new SKPaint();

        foreach (Bar bar in Bars)
        {
            bar.Render(rp, Axes, paint);
        }
    }
}
