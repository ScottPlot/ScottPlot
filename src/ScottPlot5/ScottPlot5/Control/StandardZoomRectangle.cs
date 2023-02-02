using ScottPlot.Axis;

namespace ScottPlot.Control;

/// <summary>
/// Logic for drawing the shaded region on the plot when the user middle-click-drags to zoom
/// </summary>
public class StandardZoomRectangle : IZoomRectangle
{
    public bool IsVisible { get; set; } = false;

    public Color FillColor = new Color(255, 0, 0).WithAlpha(100);

    public LineStyle LineStyle { get; } = new() { Color = new Color(255, 0, 0).WithAlpha(200) };

    public Pixel MouseDown { get; private set; }
    public Pixel MouseUp { get; private set; }
    public bool HorizontalSpan { get; set; } = false;
    public bool VerticalSpan { get; set; } = false;

    public StandardZoomRectangle()
    {
    }

    public void Update(Pixel mouseDown, Pixel mouseUp)
    {
        MouseDown = mouseDown;
        MouseUp = mouseUp;
        IsVisible = true;
    }

    public void Clear()
    {
        IsVisible = false;
    }

    public void Render(SKCanvas canvas, PixelRect dataRect)
    {
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
