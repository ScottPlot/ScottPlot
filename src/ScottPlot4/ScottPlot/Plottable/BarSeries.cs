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

    public LegendItem[] GetLegendItems() => LegendItem.None;

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
            if (bar.Value < 0)
            {
                (top, _) = (bottom, top);
                height = -height;
            }
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
            if (bar.Value < 0)
            {
                left = right;
                width = -width;
            }
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
                bool drawBelow = bar.Value < 0;

                if (bar.IsVertical)
                {
                    using StringFormat sfVert = GDI.StringFormat(HorizontalAlignment.Center, drawBelow ? VerticalAlignment.Upper : VerticalAlignment.Lower);

                    var pos = drawBelow ? rect.Bottom : rect.Top;
                    gfx.DrawString(bar.Label, font, brush, rect.Left + rect.Width / 2, pos, sfVert);
                }
                else
                {
                    using StringFormat sfHoriz = GDI.StringFormat(drawBelow ? HorizontalAlignment.Right : HorizontalAlignment.Left, VerticalAlignment.Middle);

                    var pos = drawBelow ? rect.Left : rect.Right;
                    gfx.DrawString(bar.Label, font, brush, pos, rect.Top + rect.Height / 2, sfHoriz);
                }
            }
        }
    }
}
