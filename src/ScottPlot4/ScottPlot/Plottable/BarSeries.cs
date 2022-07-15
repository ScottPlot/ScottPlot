using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ScottPlot.Drawing;

namespace ScottPlot.Plottable;

/// <summary>
/// This plot type displays a collection of Bar objects, 
/// allowing each Bar to be positioned and styled individually.
/// </summary>
public class BarSeries : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public int XAxisIndex { get; set; } = 0;
    public int YAxisIndex { get; set; } = 0;

    public readonly List<Bar> Bars;

    public int Count => Bars.Count;

    public BarSeries()
    {
        Bars = new List<Bar>();
    }

    public BarSeries(List<Bar> bars)
    {
        Bars = bars;
    }

    public AxisLimits GetAxisLimits()
    {
        if (Bars.Count() == 0)
            return AxisLimits.NoLimits;

        AxisLimits limits = Bars.First().GetLimits();
        foreach (Bar bar in Bars.Skip(1))
            limits = bar.GetLimits().Expand(limits);

        return limits;
    }

    public LegendItem[] GetLegendItems() => Array.Empty<LegendItem>();

    public void ValidateData(bool deep = false) { }

    private RectangleF GetPixelRect(PlotDimensions dims, Bar bar)
    {
        if (bar.IsVertical)
        {
            float left = dims.GetPixelX(bar.Position - bar.Thickness / 2);
            float right = dims.GetPixelX(bar.Position + bar.Thickness / 2);
            float bottom = dims.GetPixelY(bar.ValueBase);
            float top = dims.GetPixelY(bar.Value);
            float width = right - left;
            float height = bottom - top;
            return new RectangleF(left, top, width, height);
        }
        else
        {
            float left = dims.GetPixelX(bar.ValueBase);
            float right = dims.GetPixelX(bar.Value);
            float top = dims.GetPixelY(bar.Position + bar.Thickness / 2);
            float bottom = dims.GetPixelY(bar.Position - bar.Thickness / 2);
            float width = right - left;
            float height = bottom - top;
            return new RectangleF(left, top, width, height);
        }
    }

    /// <summary>
    /// Return the bar located under the given coordinate (or null if no bar is there)
    /// </summary>
    public Bar GetBar(Coordinate coordinate)
    {
        foreach (Bar bar in Bars)
        {
            if (bar.GetLimits().Contains(coordinate))
                return bar;
        }

        return null;
    }

    public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
        if (IsVisible == false || Bars.Count() == 0)
            return;

        using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);
        using Brush brush = GDI.Brush(Color.Black);
        using Pen pen = GDI.Pen(Color.Black);
        using StringFormat sfVert = GDI.StringFormat(Alignment.LowerCenter);
        using StringFormat sfHoriz = GDI.StringFormat(Alignment.MiddleLeft);

        foreach (Bar bar in Bars)
        {
            RectangleF rect = GetPixelRect(dims, bar);

            // fill
            ((SolidBrush)brush).Color = bar.FillColor;
            gfx.FillRectangle(brush, rect);

            // outline
            if (bar.LineWidth > 0)
            {
                pen.Color = bar.LineColor;
                pen.Width = bar.LineWidth;
                gfx.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
            }

            // text label
            if (!string.IsNullOrWhiteSpace(bar.Label))
            {
                using var font = GDI.Font(bar.Font);
                ((SolidBrush)brush).Color = bar.Font.Color;

                if (bar.IsVertical)
                {
                    gfx.DrawString(bar.Label, font, brush, rect.Left + rect.Width / 2, rect.Top, sfVert);
                }
                else
                {
                    gfx.DrawString(bar.Label, font, brush, rect.Right, rect.Top + rect.Height / 2, sfHoriz);
                }
            }
        }
    }
}
