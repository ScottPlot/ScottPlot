using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ScottPlot.Drawing;

namespace ScottPlot.Plottable;

public class BarSeries : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public int XAxisIndex { get; set; } = 0;
    public int YAxisIndex { get; set; } = 0;

    public readonly List<Bar> Bars = new();

    public BarSeries(IEnumerable<Bar> bars)
    {
        Bars.AddRange(bars);
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
            throw new NotImplementedException("horizontal bars not yet supported");
        }
    }

    /// <summary>
    /// Return the bar intersected by the given coordinate (or null if no bar is there)
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
                using var sf = GDI.StringFormat(Alignment.LowerCenter);
                ((SolidBrush)brush).Color = bar.Font.Color;
                gfx.DrawString(bar.Label, font, brush, rect.Left + rect.Width / 2, rect.Top, sf);
            }
        }
    }
}
