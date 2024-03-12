namespace ScottPlot.Control;

/// <summary>
/// Logic for drawing the shaded region on the plot when the user middle-click-drags to zoom
/// </summary>
public class StandardZoomRectangle : IZoomRectangle
{
    public bool IsVisible { get; set; } = false;

    public Color FillColor = new Color(255, 0, 0).WithAlpha(100);

    public LineStyle LineStyle { get; } = new() { Color = new Color(255, 0, 0).WithAlpha(200) };

    public Pixel MouseDown { get; set; }
    public Pixel MouseUp { get; set; }
    public bool HorizontalSpan { get; set; } = false;
    public bool VerticalSpan { get; set; } = false;

    public void Apply(Plot plot)
    {
        PixelRect dataRect = plot.RenderManager.LastRender.DataRect;

        if (HorizontalSpan == false)
        {
            foreach (IXAxis xAxis in plot.Axes.XAxes)
            {
                double x1 = xAxis.GetCoordinate(MouseDown.X, dataRect);
                double x2 = xAxis.GetCoordinate(MouseUp.X, dataRect);
                double xMin = Math.Min(x1, x2);
                double xMax = Math.Max(x1, x2);
                xAxis.Range.Set(xMin, xMax);
            }
        }

        if (VerticalSpan == false)
        {
            foreach (IYAxis yAxis in plot.Axes.YAxes)
            {
                double y1 = yAxis.GetCoordinate(MouseDown.Y, dataRect);
                double y2 = yAxis.GetCoordinate(MouseUp.Y, dataRect);
                double xMin = Math.Min(y1, y2);
                double xMax = Math.Max(y1, y2);
                yAxis.Range.Set(xMin, xMax);
            }
        }
    }

    public void Render(RenderPack rp)
    {
        SKCanvas canvas = rp.Canvas;
        PixelRect dataRect = rp.DataRect;

        SKRect rect = new(MouseDown.X, MouseDown.Y, MouseUp.X, MouseUp.Y);

        canvas.Save();
        canvas.ClipRect(dataRect.ToSKRect());

        if (HorizontalSpan)
        {
            rect.Left = dataRect.Left;
            rect.Right = dataRect.Right;
        }

        if (VerticalSpan)
        {
            rect.Bottom = dataRect.Bottom;
            rect.Top = dataRect.Top;
        }

        using SKPaint paint = new()
        {
            IsAntialias = true
        };

        paint.Color = FillColor.ToSKColor();
        paint.IsStroke = false;
        canvas.DrawRect(rect, paint);

        paint.Color = LineStyle.Color.ToSKColor();
        paint.StrokeWidth = LineStyle.Width;
        paint.IsStroke = true;
        canvas.DrawRect(rect, paint);

        canvas.Restore();
    }
}
