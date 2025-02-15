namespace ScottPlot.Plottables;

/// <summary>
/// This plottable renders an axis line, ticks, and tick labels inside the data area
/// </summary>
public class FloatingAxis : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    LineStyle SpineLineStyle { get; set; } = new(1, Colors.Black);
    TickMarkStyle MajorTickMarkStyle { get; set; } = new() { Color = Colors.Black };
    TickMarkStyle MinorTickMarkStyle { get; set; } = new() { Color = Colors.Black.WithAlpha(.2) };
    LabelStyle TickLabelStyle { get; set; } = LabelStyle.Default;

    float TickLength { get; set; } = 5;
    bool SkipCenterTickLabel { get; set; } = true;

    public bool IsVertical { get; }
    public ITickGenerator TickGenerator { get; }
    public double Position = 0.5;

    public FloatingAxis(IXAxis axis)
    {
        IsVertical = false;
        TickGenerator = axis.TickGenerator;
        TickLabelStyle.Alignment = Alignment.UpperCenter;
    }

    public FloatingAxis(IYAxis axis)
    {
        IsVertical = true;
        TickGenerator = axis.TickGenerator;
        TickLabelStyle.Alignment = Alignment.MiddleRight;
    }

    public virtual void Render(RenderPack rp)
    {
        if (IsVertical)
        {
            RenderVerticalAxis(rp);
        }
        else
        {
            RenderHorizontalAxis(rp);
        }
    }

    public void RenderVerticalAxis(RenderPack rp)
    {
        float x = (float)(rp.DataRect.Width * Position + rp.DataRect.Left);
        PixelLine spineLine = new(x, rp.DataRect.Top, x, rp.DataRect.Bottom);
        Console.WriteLine(spineLine.ToString());
        Drawing.DrawLine(rp.Canvas, rp.Paint, spineLine, SpineLineStyle);

        float x1 = x - TickLength / 2;
        float x2 = x + TickLength / 2;
        foreach (var tick in TickGenerator.Ticks)
        {
            float y = Axes.YAxis.GetPixel(tick.Position, rp.DataRect);
            PixelLine tickLine = new(x1, y, x2, y);
            if (tick.IsMajor)
            {
                MajorTickMarkStyle.Render(rp.Canvas, rp.Paint, tickLine);
                if (SkipCenterTickLabel && (Math.Abs(y - rp.DataRect.VerticalCenter) < 1.5)) continue;
                TickLabelStyle.Render(rp.Canvas, tickLine.Pixel1, rp.Paint, tick.Label);
            }
            else
            {
                MinorTickMarkStyle.Render(rp.Canvas, rp.Paint, tickLine);
            }
        }
    }

    public void RenderHorizontalAxis(RenderPack rp)
    {
        float y = (float)(rp.DataRect.Height * Position + rp.DataRect.Top);
        PixelLine spineLine = new(rp.DataRect.Left, y, rp.DataRect.Right, y);
        Drawing.DrawLine(rp.Canvas, rp.Paint, spineLine, SpineLineStyle);

        float y1 = y - TickLength / 2;
        float y2 = y + TickLength / 2;
        foreach (var tick in TickGenerator.Ticks)
        {
            float x = Axes.XAxis.GetPixel(tick.Position, rp.DataRect);
            PixelLine tickLine = new(x, y1, x, y2);
            if (tick.IsMajor)
            {
                MajorTickMarkStyle.Render(rp.Canvas, rp.Paint, tickLine);
                if (SkipCenterTickLabel && (Math.Abs(x - rp.DataRect.HorizontalCenter) < 1.5)) continue;
                TickLabelStyle.Render(rp.Canvas, tickLine.Pixel2, rp.Paint, tick.Label);
            }
            else
            {
                MinorTickMarkStyle.Render(rp.Canvas, rp.Paint, tickLine);
            }
        }
    }
}
