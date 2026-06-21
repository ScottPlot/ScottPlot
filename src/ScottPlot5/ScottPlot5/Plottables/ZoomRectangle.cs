namespace ScottPlot.Plottables;

/// <summary>
/// The shaded region on the plot when the user middle-click-drags to zoom
/// </summary>
public class ZoomRectangle(Plot plot) : IZoomRectangle
{
    public bool IsVisible { get; set; } = false;

    public FillStyle FillStyle { get; set; } = new()
    {
        Color = new Color(255, 0, 0).WithAlpha(100),
        AntiAlias = false,
    };

    public LineStyle LineStyle { get; set; } = new()
    {
        Color = new Color(255, 0, 0).WithAlpha(200),
        Width = 1,
        AntiAlias = false,
    };

    public Pixel MouseDown { get; set; }
    public Pixel MouseUp { get; set; }
    public bool HorizontalSpan { get; set; } = false;
    public bool VerticalSpan { get; set; } = false;
    public Plot Plot { get; } = plot;

    public void Apply(IXAxis xAxis)
    {
        if (HorizontalSpan == true)
            return;

        PixelRect dataRect = Plot.RenderManager.LastRender.DataRect;
        double x1 = xAxis.GetCoordinate(MouseDown.X, dataRect);
        double x2 = xAxis.GetCoordinate(MouseUp.X, dataRect);
        double xMin = Math.Min(x1, x2);
        double xMax = Math.Max(x1, x2);
        CoordinateRange xRange = xAxis.IsInverted() ? new(xMax, xMin) : new(xMin, xMax);
        xAxis.Range.Set(xRange);
    }

    public void Apply(IYAxis yAxis)
    {
        if (VerticalSpan == true)
            return;

        PixelRect dataRect = Plot.RenderManager.LastRender.DataRect;
        double y1 = yAxis.GetCoordinate(MouseDown.Y, dataRect);
        double y2 = yAxis.GetCoordinate(MouseUp.Y, dataRect);
        double yMin = Math.Min(y1, y2);
        double yMax = Math.Max(y1, y2);
        CoordinateRange yRange = yAxis.IsInverted() ? new(yMax, yMin) : new(yMin, yMax);
        yAxis.Range.Set(yRange);
    }

    public void Render(RenderPack rp)
    {
        SKCanvas canvas = rp.Canvas;
        PixelRect dataRect = rp.DataRect;

        SKRect rect = new(MouseDown.X, MouseDown.Y, MouseUp.X, MouseUp.Y);

        rp.CanvasState.Save();
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

        Drawing.FillRectangle(canvas, rect.ToPixelRect(), rp.Paint, FillStyle);
        Drawing.DrawRectangle(canvas, rect.ToPixelRect(), rp.Paint, LineStyle);

        rp.CanvasState.Restore();
    }
}
