/* Unmerged change from project 'ScottPlot (net462)'
Before:
namespace ScottPlot.Style;
After:
using ScottPlot;
using ScottPlot;
using ScottPlot.Style;
*/

namespace ScottPlot;

/// <summary>
/// This configuration object (reference type) permanently lives inside objects which require styling.
/// It is recommended to use this object as an init-only property.
/// </summary>
public class MarkerStyle
{
    public bool IsVisible { get; set; }

    public MarkerShape Shape { get; set; }

    /// <summary>
    /// Diameter of the marker (in pixels)
    /// </summary>
    public float Size { get; set; }

    public IMarker MarkerRenderer { get; set; }

    public FillStyle Fill { get; set; } = new() { Color = Colors.Gray };

    public LineStyle Outline { get; set; } = new() { Width = 0 };

    public MarkerStyle() : this(MarkerShape.FilledCircle, 5, Colors.Gray)
    {
    }

    public MarkerStyle(MarkerShape shape, float size) : this(shape, size, Colors.Gray)
    {
    }

    public MarkerStyle(MarkerShape shape, float size, Color color)
    {
        Shape = shape;
        MarkerRenderer = shape.GetRenderer();
        IsVisible = shape != MarkerShape.None;

        Outline.Color = color;
        if (shape.IsOutlined())
        {
            Fill.Color = Colors.Transparent;
            Outline.Width = 2;
        }
        else
        {
            Fill.Color = color;
        }

        Size = size;
    }

    public static MarkerStyle Default => new(MarkerShape.FilledCircle, 5);

    public static MarkerStyle None => new(MarkerShape.None, 0);

    public void Render(SKCanvas canvas, Pixel pixel)
    {
        Render(canvas, new[] { pixel });
    }

    public void Render(SKCanvas canvas, IEnumerable<Pixel> pixels)
    {
        if (!IsVisible)
            return;

        using SKPaint paint = new() { IsAntialias = true };

        foreach (Pixel pixel in pixels)
        {
            MarkerRenderer.Render(canvas, paint, pixel, Size, Fill, Outline);
        }
    }
}
